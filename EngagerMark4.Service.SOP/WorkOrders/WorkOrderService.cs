using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IRepository.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EngagerMark4.ApplicationCore.IRepository.Application;
using System.Data.Entity.Infrastructure;
using EngagerMark4.Common.Exceptions;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.SOP.IRepository.WorkOrder;
using EngagerMark4.Common.Utilities;
using EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IRepository.Jobs;
using EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs;
using EngagerMark4.ApplicationCore.Job.IRepository;
using EngagerMark4.ApplicationCore.SOP.ViewModels;

namespace EngagerMark4.Service.SOP.WorkOrders
{
    public class WorkOrderService : AbstractService<IWorkOrderRepository, WorkOrderCri, WorkOrder>, IWorkOrderService
    {
        ApplicationDbContext _context;

        INotificationRepository _notificationRepository;
        IWorkOrderHistoryRepository _workOrderHistoryRepository;
        ISalesInvoiceRepository _salesInvoiceRepository;
        IServiceJobRepository _serviceJobRepository;
        ICreditNoteRepository _creditNoteRepository;
        ISalesInvoiceSummaryRepository _invoiceSummaryRepository;
        IWorkOrderPassengerRepository _workOrderPassengerRepository;
        IServiceJobChecklistItemRepository _sjChecklistItemRepository;
        ICheckListRepository _checklistRepository;

        public WorkOrderService(IWorkOrderRepository repository,
                                ApplicationDbContext context, 
                                INotificationRepository notificationRepository,
                                IWorkOrderHistoryRepository workOrderHistoryRepository,
                                ISalesInvoiceRepository salesInvoiceRepository,
                                IServiceJobRepository serviceJobRepository,
                                ICreditNoteRepository creditNoteRepository,
                                ISalesInvoiceSummaryRepository invoiceSummaryRepository,
                                IWorkOrderPassengerRepository workOrderPassengerRepository,
                                IServiceJobChecklistItemRepository sjChecklistItemRepository,
                                ICheckListRepository checkListRepository) : base(repository)
        {
            this._context = context;
            this._notificationRepository = notificationRepository;
            this._workOrderHistoryRepository = workOrderHistoryRepository;
            this._salesInvoiceRepository = salesInvoiceRepository;
            this._serviceJobRepository = serviceJobRepository;
            this._creditNoteRepository = creditNoteRepository;
            this._invoiceSummaryRepository = invoiceSummaryRepository;
            this._workOrderPassengerRepository = workOrderPassengerRepository;
            this._sjChecklistItemRepository = sjChecklistItemRepository;
            this._checklistRepository = checkListRepository;
        }

        #region Load References

        public Task<IEnumerable<WorkOrder>> GetByInvoiceNo(string invoiceNo)
        {
            return this.repository.GetByInvoiceNo(invoiceNo);
        }

        public async Task<List<WorkOrder>> GetByIds(long[] workOrderIds)
        {
            return await this.repository.GetByIds(workOrderIds);
        }

        public async Task<WorkOrder> GetByCreditNoteId(long creditNoteId)
        {
            return await this.repository.GetByCreditNoteId(creditNoteId);
        }

        public async Task<List<WorkOrderPassenger>> GetNonInchargePassengers(long workOrderId, long vehicleId)
        {
            return await this.repository.GetNonInchargePassengers(workOrderId, vehicleId);
        }


        public async Task<List<WorkOrderPassenger>> GetNonInchargePassengersByServiceJobId(long workOrderId, long serviceJobId)
        {
            return await this.repository.GetNonInchargePassengersByServiceJobId(workOrderId, serviceJobId);

        }

        public List<WorkOrder> GetSimliarOrders(DateTime pickupDate, long CustomerId, long VesselId, List<WorkOrderLocation> locations)
        {
            return repository.GetSimliarOrders(pickupDate, CustomerId, VesselId, locations);
        }

        public async Task<IEnumerable<WorkOrder>> GetByInvoiceDateCri(WorkOrderCri cri)
        {
            return await this.repository.GetByInvoiceDateCri(cri);
        }

        //PCR2021
        public async Task<IEnumerable<BilledOrderListInvoiceViewModel>> GetDataForBilledOrderList(WorkOrderCri cri)
        {
            return await this.repository.GetDataForBilledOrderList(cri);
        }

        public List<long> GetWorkOrderIdsFromInvoiceIds(long[] invoiceIds)
        {
            return this.repository.GetWorkOrderIdsFromInvoiceIds(invoiceIds);
        }

        public async Task<IEnumerable<WorkOrder>> GetOrdersForAgents(WorkOrderCri cri)
        {
            try
            {
                return await this.repository.GetOrdersForAgents(cri);
            }
            catch(Exception ex)
            {
                return Enumerable.Empty<WorkOrder>().AsQueryable();
            }
        }
        #endregion

        #region Save / Update Work Order

        public async Task UpdatePassengerInCharge(WorkOrder workOrder, string applicationUserId, WorkOrderPassenger inChargePassenger, CommonConfiguration rank)
        {
            workOrder.WorkOrderPassengerList = workOrder.GetPassengers();

            var passengers = workOrder.WorkOrderPassengerList.Where(x => x.VehicleId == inChargePassenger.VehicleId).ToList();

            var inChargePerson = passengers.Where(x => x.Name.ToLower().Trim() == inChargePassenger.Name.ToLower().Trim()).FirstOrDefault();

            if (!(inChargePerson == null))
            {
                var icPerson = await _workOrderPassengerRepository.GetById(inChargePerson.Id);

                if(icPerson != null)
                {
                    icPerson.RankId = rank.Id;
                    icPerson.Rank = rank.Name;
                    icPerson.InCharge = true;

                    _workOrderPassengerRepository.Save(icPerson);
                }

                foreach (WorkOrderPassenger passenger in passengers)
                {
                    if (passenger.Name.ToLower().Trim() != inChargePassenger.Name.ToLower().Trim())
                    {
                        var currentPerson = await _workOrderPassengerRepository.GetById(passenger.Id);

                        currentPerson.InCharge = false;

                        _workOrderPassengerRepository.Save(currentPerson);
                    }
                }
            }
            else
            {
                foreach (WorkOrderPassenger passenger in passengers)
                {
                    var currentPerson = await _workOrderPassengerRepository.GetById(passenger.Id);

                    currentPerson.InCharge = false;

                    _workOrderPassengerRepository.Save(currentPerson);
                }

                _workOrderPassengerRepository.Save(inChargePassenger);
            }

            await _workOrderPassengerRepository.SaveChangesAsync();


            //repository.UpdatePassengerInCharge(workOrder, applicationUserId);

            //await repository.SaveChangesAsync();
        }

        public async Task MovetoBill(long[] workOrderIds)
        {

            List<WorkOrder> workOrderList = new List<WorkOrder>();

            if (workOrderIds != null)
            {
                workOrderList = await this.repository.GetByIds(workOrderIds);

                foreach (var workOrder in workOrderList)
                {
                    var invoice = await _context.SalesInvoices.FirstOrDefaultAsync(x => x.Id == workOrder.InvoiceId);
                    
                    if (invoice == null) throw new NotSupportedException();

                    invoice.Status = EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice.SalesInvoiceStatus.Billed;
                    if (workOrder != null)
                        workOrder.Status = WorkOrder.OrderStatus.Billed;
                }
            }
            await repository.SaveChangesAsync();

            if (workOrderList != null && workOrderList.Count() > 0)
            {
                foreach (var workOrder in workOrderList)
                {
                    WorkOrderHistory woLog = new WorkOrderHistory();

                    woLog.WorkOrderId = workOrder.Id;
                    woLog.WorkOrderNo = workOrder.RefereneceNo;
                    woLog.Vessel = workOrder.VesselName;
                    woLog.PickupDate = workOrder.PickUpdateDate;
                    woLog.PickupPoint = workOrder.PickUpPoint;
                    woLog.DropPoint = workOrder.DropPoint;
                    woLog.StandByDate = workOrder.StandByDate;
                    woLog.WorkOrderStatus = workOrder.Status;

                    woLog.CurrentStateDescription = "Work Order '" + woLog.WorkOrderNo + "' updated with status - " + StringUtil.SplitCamelCase(woLog.WorkOrderStatus.ToString()) + ".";


                    _workOrderHistoryRepository.Save(woLog);

                    await _workOrderHistoryRepository.SaveChangesAsync();
                }
            }
        }

        public async Task MoveToWithAccounts(long[] workOrderIds)
        {
            try
            {
                List<WorkOrder> workOrderList = new List<WorkOrder>();

                if (workOrderIds != null)
                {
                    workOrderList = await this.repository.GetByIds(workOrderIds);

                    foreach (var workOrder in workOrderList)
                    {
                        var invoice = await _context.SalesInvoices.FirstOrDefaultAsync(x => x.Id == workOrder.InvoiceId);
                        if (invoice != null)
                            invoice.Status = EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice.SalesInvoiceStatus.Draft;
                        if (workOrder != null)
                            workOrder.Status = WorkOrder.OrderStatus.With_Accounts;
                    }
                }
                await repository.SaveChangesAsync();

                if(workOrderList != null && workOrderList.Count() > 0)
                {
                    foreach( var workOrder in workOrderList)
                    {
                        WorkOrderHistory woLog = new WorkOrderHistory();

                        woLog.WorkOrderId = workOrder.Id;
                        woLog.WorkOrderNo = workOrder.RefereneceNo;
                        woLog.Vessel = workOrder.VesselName;
                        woLog.PickupDate = workOrder.PickUpdateDate;
                        woLog.PickupPoint = workOrder.PickUpPoint;
                        woLog.DropPoint = workOrder.DropPoint;
                        woLog.StandByDate = workOrder.StandByDate;
                        woLog.WorkOrderStatus = workOrder.Status;

                        woLog.CurrentStateDescription = "Work Order '" + woLog.WorkOrderNo + "' updated with status - " + StringUtil.SplitCamelCase(woLog.WorkOrderStatus.ToString()) + ".";


                        _workOrderHistoryRepository.Save(woLog);

                        await _workOrderHistoryRepository.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async override Task<long> Save(WorkOrder entity)
        {
            bool IsNewWorkOrder = false;

            if (entity.Id == 0)
            {
                if (entity.CustomerId == null)
                {
                    throw new CannotAddException("Customer Field is Required.");
                }

                IsNewWorkOrder = true;
            }

            entity.GenerateDateFromString();

            bool needToGeneralSerialNo = entity.Id == 0;

            this.repository.Save(entity);

            await this.repository.SaveChangesAsync();

            if (needToGeneralSerialNo)
            {
                SerialNoRepository<WorkOrderSerialNo> serialNoRepoitory = new SerialNoRepository<WorkOrderSerialNo>(_context);

                entity.RefereneceNo = entity.GetWorkOrderNo(serialNoRepoitory.GetSerialNoByMonth(entity.Id, entity.WorkOrderDate));

                await this.repository.SaveChangesAsync();
            }

            await SaveServiceJobChecklistItem(entity);
            await SaveAuditHistoryForWorkOrder(entity,IsNewWorkOrder);

            if(entity.InvoiceId.HasValue && entity.CreditNoteId.HasValue)
            {
                var invoiceVesselId = _salesInvoiceRepository.GetInvoiceDetailVesselId(entity.InvoiceId.Value);

                if(invoiceVesselId.HasValue && entity.InvoiceId.Value != invoiceVesselId)
                {
                    var invResult = await _salesInvoiceRepository.UpdateVesselIdAndVesselName(entity.VesselId.Value, entity.VesselName, entity.InvoiceId.Value);

                    if(invResult)
                    {
                        var creditNoteVesselId = _creditNoteRepository.GetCreditNoteDetailVesselId(entity.CreditNoteId.Value);

                        if(creditNoteVesselId.HasValue && entity.CreditNoteId.Value != creditNoteVesselId)
                        {
                            var cnResult = await _creditNoteRepository.UpdateVesselIdAndVesselName(entity.VesselId.Value, entity.VesselName, entity.CreditNoteId.Value);
                        }
                    }
                }
            }

            return entity.Id;
        }

        //PCR2011 - OBSOLETE
        public async Task SavePassengers(long workOrderId, long vehicleId, List<WorkOrderPassenger> passengers)
        {
            var workOrder = await this.repository.GetById(workOrderId);

            await this.repository.SavePassengers(workOrderId, vehicleId, passengers);

            if(workOrder != null)
            {
                await this.SaveAuditHistoryForWorkOrder(workOrder, false);
            }
        }

        public async Task RemoveUnselectedOrdersFromInvoice(Int64[] workOrderIds, string invoiceSummaryNo)
        {
            try
            {

                var workOrders = await this.repository.GetByInvoiceNo(invoiceSummaryNo);
                var invoiceDetails = await this._salesInvoiceRepository.GetByInvoiceNo(invoiceSummaryNo);
                var currentInvoiceSummary = await this._invoiceSummaryRepository.GetByInvoiceNo(invoiceSummaryNo);

                if (workOrders != null && workOrders.Count() > 0)
                {
                    List<WorkOrder> excludedWorkOrders = new List<WorkOrder>();

                    if (workOrderIds == null || workOrderIds.Count() == 0)
                    {
                        excludedWorkOrders = workOrders.ToList();
                    }
                    else
                    {
                        excludedWorkOrders = workOrders.Where(x => !workOrderIds.Contains(x.Id)).ToList();
                    }

                    if (excludedWorkOrders != null && excludedWorkOrders.Count() > 0)
                    {
                        var invoiceId = excludedWorkOrders.FirstOrDefault().SalesInvoiceSummaryId;

                        foreach (var order in excludedWorkOrders)
                        {
                            var currentWorkOrder = await repository.GetById(order.Id);

                            currentWorkOrder.ShortText2 = null;
                            currentWorkOrder.ShortText3 = null;
                            currentWorkOrder.SalesInvoiceSummaryId = 0;

                            this.repository.Save(currentWorkOrder);
                        }

                        if (currentInvoiceSummary != null)
                        {
                            this._invoiceSummaryRepository.RemoveInvoiceSummaryDetailsByWorkOrderId(currentInvoiceSummary.Id, excludedWorkOrders.Select(x => x.Id).ToList());
                        }

                        await this.repository.SaveChangesAsync();

                    }
                }

                if (invoiceDetails != null && invoiceDetails.Count() > 0)
                {

                    List<SalesInvoice> excludedInvoiceDetails = new List<SalesInvoice>();

                    if(workOrderIds == null || workOrderIds.Count() == 0)
                    {
                        excludedInvoiceDetails = invoiceDetails.ToList();
                    }
                    else
                    {
                        excludedInvoiceDetails = invoiceDetails.Where(x => x.WorkOrderId.HasValue && !workOrderIds.Contains(x.WorkOrderId.Value)).ToList();
                    }

                    if (excludedInvoiceDetails != null && excludedInvoiceDetails.Count() > 0)
                    {
                        foreach (var invDetail in excludedInvoiceDetails)
                        {
                            invDetail.ShortText1 = null;
                            invDetail.ShortText2 = null;
                            invDetail.SalesInvoiceSummaryId = 0;


                            _salesInvoiceRepository.Save(invDetail);
                        }

                        await _salesInvoiceRepository.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<Int64> SaveAuditHistoryForWorkOrder(WorkOrder workOrder, bool IsNewEntity)
        {
            try
            {
                var checklistQuery = await _checklistRepository.GetByCri(new EngagerMark4.ApplicationCore.Job.Cris.CheckListCri());

                var checklists = checklistQuery.ToList();

                workOrder.ServiceJobList = new List<EngagerMark4.ApplicationCore.SOP.Entities.Jobs.ServiceJob>();
                var jobs = await _serviceJobRepository.GetServiceJobsByWorkOrderId(workOrder.Id);
                workOrder.ServiceJobList = jobs.ToList();

                WorkOrderHistory woLog = new WorkOrderHistory();

                woLog.WorkOrderId = workOrder.Id;
                woLog.WorkOrderNo = workOrder.RefereneceNo;
                woLog.Vessel = workOrder.VesselName;
                woLog.PickupDate = workOrder.PickUpdateDate;
                woLog.PickupPoint = workOrder.PickUpPoint;
                woLog.DropPoint = workOrder.DropPoint;
                woLog.StandByDate = workOrder.StandByDate;
                woLog.WorkOrderStatus = workOrder.Status;

                if (workOrder.ServiceJobList != null && workOrder.ServiceJobList.Count > 0)
                {
                    woLog.ServiceJobOverallStatus = string.Empty;
                    woLog.ChecklistValues = string.Empty;
                    woLog.ChecklistValueStr = string.Empty;
                    woLog.TripFeesVal = string.Empty;
                    woLog.TripFeesValStr = string.Empty;
                    woLog.MSFeesVal = string.Empty;
                    woLog.MSFeesValStr = string.Empty;

                    foreach (var serviceJob in workOrder.ServiceJobList)
                    {
                        woLog.ServiceJobOverallStatus += serviceJob.ReferenceNo + " - " + serviceJob.User.LastName + " " + serviceJob.User.FirstName + " - " + serviceJob.Status.ToString() + ". ";

                        if (!string.IsNullOrEmpty(serviceJob.CheckListIds))
                        {
                            woLog.ChecklistValues += serviceJob.CheckListIds;

                            var sjChecklistItems = serviceJob.GetChecklistItemList();

                            if (sjChecklistItems.Any() && sjChecklistItems.Count > 0)
                            {
                                woLog.ChecklistValueStr += serviceJob.ReferenceNo + " : ";

                                foreach (var chkItem in sjChecklistItems)
                                {
                                    var currentChecklist = checklists.Where(x => x.Id == chkItem.ChecklistId).FirstOrDefault();

                                    if (currentChecklist != null)
                                    {
                                        woLog.ChecklistValueStr += currentChecklist.Name + " - " + chkItem.ChecklistPrice.ToString("0.00") + "; ";
                                    }
                                }
                            }

                            woLog.ChecklistValueStr += "|";
                        }

                        if (serviceJob.TripFees > 0)
                        {
                            woLog.TripFeesVal += serviceJob.Id + "," + serviceJob.TripFees.ToString() + "|";
                            woLog.TripFeesValStr += serviceJob.ReferenceNo + " : Trip Fees - " + serviceJob.TripFees.ToString("0.00") + "; ";
                        }

                        if (serviceJob.MSFees > 0)
                        {
                            woLog.MSFeesVal += serviceJob.Id + "," + serviceJob.MSFees.ToString() + "|";
                            woLog.MSFeesValStr += serviceJob.ReferenceNo + " : MS Fees - " + serviceJob.MSFees.ToString("0.00") + "; ";
                        }
                    }
                }

                if (woLog.WorkOrderStatus == WorkOrder.OrderStatus.Cancelled)
                {
                    woLog.CurrentStateDescription = "Work Order '" + woLog.WorkOrderNo + "' cancelled.";
                }
                else
                {
                    WorkOrderHistoryCri aCri = new WorkOrderHistoryCri();
                    aCri.NumberCris = new Dictionary<string, EngagerMark4.ApplicationCore.Cris.IntValue>();
                    aCri.NumberCris["WorkOrderId"] = new EngagerMark4.ApplicationCore.Cris.IntValue { ComparisonOperator = EngagerMark4.ApplicationCore.Cris.BaseCri.NumberComparisonOperator.Equal, Value = woLog.WorkOrderId };

                    var workOrders = await _workOrderHistoryRepository.GetByCri(aCri);

                    if (workOrders == null || workOrders.Count() == 0)
                    {
                        if (IsNewEntity)
                        {
                            woLog.CurrentStateDescription = "Work Order '" + woLog.WorkOrderNo + "' created with status - " + StringUtil.SplitCamelCase(woLog.WorkOrderStatus.ToString()) + ".";

                            woLog.ChangeDescription = string.Empty;

                            if (!String.IsNullOrEmpty(woLog.Vessel))
                            {
                                woLog.ChangeDescription += "Vessel added : '" + woLog.Vessel + "'.|";
                            }

                            if (woLog.PickupDate.HasValue)
                            {
                                woLog.ChangeDescription += "Pickup Date added : '" + woLog.PickupDate.Value.ToString("dd/MM/yyyy HH:mm") + "'.|";
                            }

                            if (woLog.StandByDate.HasValue)
                            {
                                woLog.ChangeDescription += "Standby Date added : '" + woLog.StandByDate.Value.ToString("dd/MM/yyyy HH:mm") + "'.|";
                            }

                            if (!String.IsNullOrEmpty(woLog.PickupPoint))
                            {
                                woLog.ChangeDescription += "Pickup Point added : '" + woLog.PickupPoint + "'.|";
                            }

                            if (!String.IsNullOrEmpty(woLog.DropPoint))
                            {
                                woLog.ChangeDescription += "Drop Point added : '" + woLog.DropPoint + "'.|";
                            }

                            if (!string.IsNullOrEmpty(woLog.ServiceJobOverallStatus))
                            {
                                woLog.ChangeDescription += woLog.ServiceJobOverallStatus + "|";
                            }
                        }
                        else
                        {
                            woLog.CurrentStateDescription = "Work Order '" + woLog.WorkOrderNo + "' updated with status - " + StringUtil.SplitCamelCase(woLog.WorkOrderStatus.ToString()) + ".";

                            woLog.ChangeDescription = string.Empty;

                            if (!String.IsNullOrEmpty(woLog.Vessel))
                            {
                                woLog.ChangeDescription += "Vessel : '" + woLog.Vessel + "'.|";
                            }

                            if (woLog.PickupDate.HasValue)
                            {
                                woLog.ChangeDescription += "Pickup Date : '" + woLog.PickupDate.Value.ToString("dd/MM/yyyy HH:mm") + "'.|";
                            }

                            if (woLog.StandByDate.HasValue)
                            {
                                woLog.ChangeDescription += "Standby Date : '" + woLog.StandByDate.Value.ToString("dd/MM/yyyy HH:mm") + "'.|";
                            }

                            if (!String.IsNullOrEmpty(woLog.PickupPoint))
                            {
                                woLog.ChangeDescription += "Pickup Point : '" + woLog.PickupPoint + "'.|";
                            }

                            if (!String.IsNullOrEmpty(woLog.DropPoint))
                            {
                                woLog.ChangeDescription += "Drop Point : '" + woLog.DropPoint + "'.|";
                            }

                            if (!string.IsNullOrEmpty(woLog.ServiceJobOverallStatus))
                            {
                                woLog.ChangeDescription += woLog.ServiceJobOverallStatus + "|";
                            }
                        }
                    }
                    else
                    {
                        woLog.CurrentStateDescription = "Work Order '" + woLog.WorkOrderNo + "' updated with status - " + woLog.WorkOrderStatus.ToString() + ".";

                        woLog.ChangeDescription = "";

                        var previousWorkOrder = workOrders.OrderByDescending(x => x.Id).FirstOrDefault();

                        if (previousWorkOrder.Vessel != woLog.Vessel)
                        {
                            if (String.IsNullOrEmpty(previousWorkOrder.Vessel))
                            {
                                woLog.ChangeDescription += "Vessel added : '" + woLog.Vessel + "'.|";
                            }
                            else
                            {
                                woLog.ChangeDescription += "Vessel changed : '" + previousWorkOrder.Vessel + "' to '" + woLog.Vessel + "'.|";
                            }
                        }

                        if (previousWorkOrder.PickupDate != woLog.PickupDate)
                        {
                            string previousPickupDateString = previousWorkOrder.PickupDate.HasValue ? previousWorkOrder.PickupDate.Value.ToString("dd/MM/yyyy HH:mm") : "";
                            string currentPickupDateString = woLog.PickupDate.HasValue ? woLog.PickupDate.Value.ToString("dd/MM/yyyy HH:mm") : "";

                            if (String.IsNullOrEmpty(previousPickupDateString))
                            {
                                woLog.ChangeDescription += "Pickup Date added : '" + currentPickupDateString + "'.|";
                            }
                            else
                            {
                                woLog.ChangeDescription += "Pickup Date changed : '" + previousPickupDateString + "' to '" + currentPickupDateString + "'.|";
                            }
                        }

                        if (previousWorkOrder.StandByDate != woLog.StandByDate)
                        {
                            string previousStandByDateString = previousWorkOrder.StandByDate.HasValue ? previousWorkOrder.StandByDate.Value.ToString("dd/MM/yyyy HH:mm") : "";
                            string currentStandByDateString = woLog.StandByDate.HasValue ? woLog.StandByDate.Value.ToString("dd/MM/yyyy HH:mm") : "";

                            if (String.IsNullOrEmpty(previousStandByDateString))
                            {
                                woLog.ChangeDescription += "Standby Date added : '" + currentStandByDateString + "'.|";
                            }
                            else
                            {
                                woLog.ChangeDescription += "Standby Date changed : '" + previousStandByDateString + "' to '" + currentStandByDateString + "'.|";
                            }
                        }

                        if (previousWorkOrder.PickupPoint != woLog.PickupPoint)
                        {
                            if (String.IsNullOrEmpty(previousWorkOrder.PickupPoint))
                            {
                                woLog.ChangeDescription += "Pickup Point added : '" + woLog.PickupPoint + "'.|";
                            }
                            else
                            {
                                woLog.ChangeDescription += "Pickup Point changed : '" + previousWorkOrder.PickupPoint + "' to '" + woLog.PickupPoint + "'.|";
                            }
                        }

                        if (previousWorkOrder.DropPoint != woLog.DropPoint)
                        {
                            if (String.IsNullOrEmpty(previousWorkOrder.DropPoint))
                            {
                                woLog.ChangeDescription += "Drop Point added : '" + woLog.DropPoint + "'.|";
                            }
                            else
                            {
                                woLog.ChangeDescription += "Drop Point changed : '" + previousWorkOrder.DropPoint + "' to '" + woLog.DropPoint + "'.|";
                            }
                        }

                        if (previousWorkOrder.ServiceJobOverallStatus != woLog.ServiceJobOverallStatus)
                        {
                            if (!string.IsNullOrEmpty(woLog.ServiceJobOverallStatus))
                            {
                                woLog.ChangeDescription += woLog.ServiceJobOverallStatus + "|";
                            }
                        }

                        if (previousWorkOrder.ChecklistValues != woLog.ChecklistValues)
                        {
                            if (!string.IsNullOrEmpty(woLog.ChecklistValues))
                            {
                                woLog.ChangeDescription += woLog.ChecklistValueStr + "|";
                            }

                        }

                        if (previousWorkOrder.TripFeesVal != woLog.TripFeesVal)
                        {
                            if (!string.IsNullOrEmpty(woLog.TripFeesValStr))
                            {
                                woLog.ChangeDescription += woLog.TripFeesValStr + "|";
                            }
                        }

                        if (previousWorkOrder.MSFeesVal != woLog.MSFeesVal)
                        {
                            if (!string.IsNullOrEmpty(woLog.MSFeesValStr))
                            {
                                woLog.ChangeDescription += woLog.MSFeesValStr + "|";
                            }
                        }
                    }
                }

                _workOrderHistoryRepository.Save(woLog);

                await _workOrderHistoryRepository.SaveChangesAsync();

                return 0;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        //PCR2021
        public async Task SaveServiceJobChecklistItem(WorkOrder workOrder)
        {
            try
            {

                int savingCount = 0;

                workOrder.ServiceJobList = new List<EngagerMark4.ApplicationCore.SOP.Entities.Jobs.ServiceJob>();
                var jobs = await _serviceJobRepository.GetServiceJobsByWorkOrderId(workOrder.Id);
                workOrder.ServiceJobList = jobs.ToList();

                foreach (var job in jobs)
                {
                    ServiceJobChecklistItemCri aCri = new ServiceJobChecklistItemCri();
                    aCri.NumberCris = new Dictionary<string, EngagerMark4.ApplicationCore.Cris.IntValue>();
                    aCri.NumberCris["ServiceJobId"] = new EngagerMark4.ApplicationCore.Cris.IntValue { ComparisonOperator = EngagerMark4.ApplicationCore.Cris.BaseCri.NumberComparisonOperator.Equal, Value = job.Id };

                    var existingChkListItemList = await _sjChecklistItemRepository.GetByCri(aCri);

                    List<ServiceJobChecklistItem> chklistItemList = new List<ServiceJobChecklistItem>();

                    if (!string.IsNullOrEmpty(job.CheckListIds)) chklistItemList = job.GetChecklistItemList();

                    foreach (var chklistItem in chklistItemList)
                    {
                        var isExisting = existingChkListItemList.Where(x => x.ChecklistId == chklistItem.ChecklistId).Any();

                        if (isExisting)
                        {
                            var existingItem = await _sjChecklistItemRepository.GetById(existingChkListItemList.Where(x => x.ChecklistId == chklistItem.ChecklistId).First().Id);

                            if (existingItem != null)
                            {
                                existingItem.ChecklistPrice = chklistItem.ChecklistPrice;
                                _sjChecklistItemRepository.Save(existingItem);
                                savingCount++;
                            }
                        }
                        else
                        {
                            _sjChecklistItemRepository.Save(chklistItem);
                            savingCount++;
                        }
                    }
                }

                if (savingCount > 0)
                {
                    await _sjChecklistItemRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}
