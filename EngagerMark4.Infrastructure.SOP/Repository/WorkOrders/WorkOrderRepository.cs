using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IRepository.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using EngagerMark4.Common;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Infrastructure.SOP.TempQueryModels;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.ExpressionBuilders;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.SOP.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkOrderRepository : GenericRepository<ApplicationDbContext, WorkOrderCri, WorkOrder>, IWorkOrderRepository
    {
        IRolePermissionRepository _rolePermissionRepository;
        IUserRepository _userRepository;

        public WorkOrderRepository(ApplicationDbContext aContext,
            IRolePermissionRepository rolePermissionRepository, IUserRepository userRepository) : base(aContext)
        {
            this._rolePermissionRepository = rolePermissionRepository;
            this._userRepository = userRepository;
        }

        #region Save / Update Work Data

        public void UpdatePassengerInCharge(WorkOrder entity, string applicationUserId)
        {
            //base.Save(entity);       
            context.userId = applicationUserId;

            foreach (var detail in entity.WorkOrderPassengerList)
            {
                if (detail.Id == 0 && detail.Delete == false)
                {
                    //context.Entry(detail).Property(x => x.CreatedBy).CurrentValue = applicationUserId;
                    context.WorkOrderPassengers.Add(detail);
                    continue;
                }

                if (detail.Id != 0 && detail.Delete)
                {
                    context.Entry(detail).State = System.Data.Entity.EntityState.Deleted;
                    continue;
                }

                if (detail.Id != 0)
                {
                    context.Entry(detail).State = System.Data.Entity.EntityState.Modified;
                    //context.Entry(detail).Property(x => x.ModifiedBy).CurrentValue = applicationUserId;
                    context.Entry(detail).Property(x => x.Created).IsModified = false;
                    context.Entry(detail).Property(x => x.CreatedBy).IsModified = false;
                }
            }
        }

        public override void Save(WorkOrder model)
        {
            var vessel = context.CommonConfigurations.AsNoTracking().FirstOrDefault(x => x.Id == model.VesselId);

            if (vessel != null) model.VesselName = vessel.Name;

            var customer = context.Customers.AsNoTracking().FirstOrDefault(x => x.Id == model.CustomerId);

            if (customer != null) model.CustomerCompanyName = customer.Name;

            if (this.dbSet == null)
                return;
            if (model.Id == 0)
            {
                model.Created = TimeUtil.GetLocalTime();
                model.Modified = TimeUtil.GetLocalTime();
                model.CreatedBy = GetCurrentUserId();
                model.CreatedByName = GetUserName();
                this.dbSet.Add(model);
            }
            else
            {
                this.dbSet.Attach(model);
                this.context.Entry(model).State = EntityState.Modified;
                this.context.Entry(model).Property(x => x.Created).IsModified = false;
                this.context.Entry(model).Property(x => x.CreatedBy).IsModified = false;
                this.context.Entry(model).Property(x => x.CreatedByName).IsModified = false;
                model.Modified = TimeUtil.GetLocalTime();
                model.ModifiedBy = GetCurrentUserId();
                model.ModifiedByName = GetUserName();

                if (model.isFromOps)
                {
                    this.context.Entry(model).Property(x => x.ShortText2).IsModified = false;
                    this.context.Entry(model).Property(x => x.ShortText3).IsModified = false;
                    this.context.Entry(model).Property(x => x.SalesInvoiceSummaryId).IsModified = false;
                }
            }

            //var hasPermissionForLocation = _rolePermissionRepository.HasPermission("WorkOrderLocationController", HttpContext.Current.User.Identity.GetUserId());

            //if (hasPermissionForLocation)
            //{
            if (model.Id != 0)
            {
                foreach (var detail in context.WorkOrderLocations.Where(x => x.WorkOrderId == model.Id))
                {
                    context.WorkOrderLocations.Remove(detail);
                }
            }

            foreach (var detail in model.GetLocations())
            {
                if (detail.Type == WorkOrderLocation.LocationType.PickUp)
                {
                    var location = context.Locations.AsNoTracking().FirstOrDefault(x => x.Id == detail.LocationId);
                    if (location != null)
                    {
                        model.PickUpPoint = location.Display;
                        if (detail.HotelId != null && !string.IsNullOrEmpty(detail.Hotel))
                        {
                            model.PickUpPoint += detail.Hotel;
                        }
                    }
                    else
                        model.PickUpPoint = detail.Description;
                }
                if (detail.Type == WorkOrderLocation.LocationType.DropOff)
                {
                    var location = context.Locations.AsNoTracking().FirstOrDefault(x => x.Id == detail.LocationId);
                    if (location != null)
                    {
                        model.DropPoint = location.Display;
                        if (detail.HotelId != null && !string.IsNullOrEmpty(detail.Hotel))
                        {
                            model.DropPoint += detail.Hotel;
                        }
                    }
                    else
                        model.DropPoint = detail.Description;
                }
                context.WorkOrderLocations.Add(detail);
            }
            //}

            //var hasPermissionForPassenger = _rolePermissionRepository.HasPermission("WorkOrderPassengerController", HttpContext.Current.User.Identity.GetUserId());

            //if(hasPermissionForPassenger)
            //{

            model.NoOfPax = 0;

            //if (model.Id != 0)
            //{
            //    foreach (var detail in context.WorkOrderPassengers.Where(x => x.WorkOrderId == model.Id))
            //    {
            //        context.WorkOrderPassengers.Remove(detail);
            //    }
            //}

            foreach (var passenger in model.GetPassengers())
            {
                if (passenger.Id > 0 && passenger.Delete)
                {
                    var currentPassenger = context.WorkOrderPassengers.Where(x => x.Id == passenger.Id).FirstOrDefault();

                    if (currentPassenger != null)
                    {
                        context.WorkOrderPassengers.Remove(currentPassenger);
                    }
                }
                else
                {
                    if (!passenger.Delete)
                    {
                        model.NoOfPax = model.NoOfPax + passenger.NoOfPax;

                        if (passenger.Id == 0 && passenger.Delete == false)
                        {
                            context.WorkOrderPassengers.Add(passenger);
                            context.SaveChanges();
                            continue;
                        }

                        if (passenger.Id != 0)
                        {
                            context.Entry(passenger).State = EntityState.Modified;
                            context.Entry(passenger).Property(x => x.Created).IsModified = false;
                            context.Entry(passenger).Property(x => x.CreatedBy).IsModified = false;

                            if (passenger.VehicleId > 0 && model.Status >= WorkOrder.OrderStatus.Submitted)
                            {
                                context.Entry(passenger).Property(x => x.VehicleId).IsModified = false;
                            }
                        }

                    }
                }
            }

            //}

            //var hasPermissionForMeetingService = _rolePermissionRepository.HasPermission("WorkOrderMeetingServiceController", HttpContext.Current.User.Identity.GetUserId());
            //if(hasPermissionForMeetingService)
            //{
            if (model.Id != 0)
            {
                foreach (var detail in context.WorkOrderMeetingServices.Where(x => x.WorkOrderId == model.Id))
                {
                    context.WorkOrderMeetingServices.Remove(detail);
                }
            }

            foreach (var detail in model.GetMeetingServices())
            {
                context.WorkOrderMeetingServices.Add(detail);
            }
            //}

            model.Drivers = string.Empty;

            foreach (var serviceJob in model.GetServiceJobs())
            {
                var driver = context.EngagerUsers.AsNoTracking().FirstOrDefault(x => x.Id == serviceJob.UserId);

                var vehicle = context.Vehicles.AsNoTracking().FirstOrDefault(x => x.Id == serviceJob.VehicleId);

                if (driver != null && serviceJob.Delete == false)
                    model.Drivers += driver.Name + " - " + vehicle.VehicleNo + ", ";


                if (serviceJob.Id == 0 && serviceJob.Delete == false)
                {
                    context.ServiceJobs.Add(serviceJob);
                    context.SaveChanges();
                    SerialNoRepository<ServiceJobSerialNo> serialNoRepository = new SerialNoRepository<ServiceJobSerialNo>(context);
                    serviceJob.ReferenceNo = serviceJob.GetSJno(serialNoRepository.GetSerialNoByMonth(serviceJob.Id, serviceJob.Created));
                    context.SaveChanges();
                    continue;
                }

                if (serviceJob.Id != 0 && serviceJob.Delete)
                {
                    context.Entry(serviceJob).State = EntityState.Deleted;
                    continue;
                }

                if (serviceJob.Id != 0)
                {
                    context.Entry(serviceJob).State = EntityState.Modified;
                    //context.Entry(serviceJob).Property(x => x.StartExecutionTime).IsModified = false;
                    //context.Entry(serviceJob).Property(x => x.EndExecutionTime).IsModified = false;
                    //context.Entry(serviceJob).Property(x => x.StartExecutionPlace).IsModified = false;
                    //context.Entry(serviceJob).Property(x => x.EndExecutionPlace).IsModified = false;
                    //context.Entry(serviceJob).Property(x => x.ReferenceNo).IsModified = false;
                    context.Entry(serviceJob).Property(x => x.Created).IsModified = false;
                    context.Entry(serviceJob).Property(x => x.CreatedBy).IsModified = false;
                    //context.Entry(serviceJob).Property(x => x.DriverRemark).IsModified = false;
                    //context.Entry(serviceJob).Property(x => x.SignatureId).IsModified = false;
                    //context.Entry(serviceJob).Property(x => x.ShortText9).IsModified = false;
                    //context.Entry(serviceJob).Property(x => x.Status).IsModified = false;
                }
            }

            model.Drivers = model.Drivers.TrimEnd(new char[] { ',', ' ' });
        }

        //PCR2011 - OBSOLETE
        public async Task SavePassengers(long workOrderId, long vehicleId, List<WorkOrderPassenger> passengers)
        {
            foreach (var passenger in this.context.WorkOrderPassengers.Where(x => x.WorkOrderId == workOrderId && x.VehicleId == vehicleId && x.InCharge == false))
            {
                context.WorkOrderPassengers.Remove(passenger);
            }

            foreach (var toSavePassenger in passengers.Where(x => x.Delete == false))
            {
                toSavePassenger.Vehicle = null;
                toSavePassenger.WorkOrder = null;
                toSavePassenger.VehicleId = vehicleId;
                toSavePassenger.WorkOrderId = workOrderId;
                toSavePassenger.NoOfPax = 1;
                context.WorkOrderPassengers.Add(toSavePassenger);
            }

            await context.SaveChangesAsync();
        }

        #endregion

        #region Load References

        public async override Task<IEnumerable<WorkOrder>> GetByCri(WorkOrderCri cri)
        {
            var queryableData = context.WorkOrders.AsNoTracking().Where(x => x.ParentCompanyId == GlobalVariable.COMPANY_ID);

            if (cri == null) cri = new WorkOrderCri();

            if (!string.IsNullOrEmpty(cri.SalesInvoiceSummaryNo))
            {
                queryableData = queryableData.Where(x => x.ShortText2.ToLower().Trim().Contains(cri.SalesInvoiceSummaryNo.ToLower().Trim()));

                //var invoice = context.SalesInvoiceSummaries.AsNoTracking().FirstOrDefault(x => x.ReferenceNo.Equals(cri.SalesInvoiceSummaryNo));
                //if (invoice == null) return new List<WorkOrder>();
                //var workOrderIdList = new List<Int64>();
                //foreach (var detail in context.SalesInvoiceSummaryDetails.AsNoTracking().Where(x => x.SalesInvoiceSummaryId == invoice.Id))
                //{
                //    workOrderIdList.Add(detail.WorkOrderId);
                //}
                //var workOrderIds = workOrderIdList.ToArray();

                //queryableData = queryableData.Where(x => workOrderIds.Contains(x.Id));
            }

            if (cri.CustomerId > 0)
                queryableData = queryableData.Where(x => x.CustomerId == cri.CustomerId);

            if (cri.VesselId > 0)
                queryableData = queryableData.Where(x => x.VesselId == cri.VesselId);

            if ((cri.Status > 0 || cri.Status == WorkOrder.OrderStatus.Cancelled) && string.IsNullOrEmpty(cri.SalesInvoiceSummaryNo))
            {
                queryableData = queryableData.Where(x => x.Status == cri.Status);
            }
            else if (cri.Status != WorkOrder.OrderStatus.Cancelled)
            {
                queryableData = queryableData.Where(x => x.Status != WorkOrder.OrderStatus.Cancelled);
            }
            else if ((cri.Status == 0 && cri.IsComeFromAccount))
            {
                queryableData = queryableData.Where(x => x.Status >= WorkOrder.OrderStatus.With_Accounts);
            }

            if (cri.FromDate != null)
            {
                TimeSpan ts = new TimeSpan(0, 0, 0);
                cri.FromDate = cri.FromDate.Value.Date + ts;
                queryableData = queryableData.Where(x => x.PickUpdateDate >= cri.FromDate);
            }


            if (cri.ToDate != null)
            {
                TimeSpan newts = new TimeSpan(23, 59, 59);
                cri.ToDate = cri.ToDate.Value.Date + newts;
                //cri.ToDate = cri.ToDate.Value.AddDays(1);
                queryableData = queryableData.Where(x => x.PickUpdateDate <= cri.ToDate);
            }

            if (cri.DriverId != 0)
            {
                var driver = context.EngagerUsers.AsNoTracking().Where(x => x.Id == cri.DriverId).AsNoTracking().FirstOrDefault();

                if (driver != null)
                {
                    queryableData = queryableData.Where(x => x.Drivers.ToLower().Contains(driver.Name.ToLower()));

                }
            }


            if (cri != null && cri.OrderBys != null)
            {
                foreach (var columnName in cri.OrderBys.Keys)
                {
                    var value = cri.OrderBys[columnName];
                    var dataType = value.Keys.FirstOrDefault();
                    var orderType = value.Values.FirstOrDefault();

                    switch (orderType)
                    {
                        case BaseCri.EntityOrderBy.Asc:
                            switch (dataType)
                            {
                                case BaseCri.DataType.String:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                                    break;
                                case BaseCri.DataType.Int64:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpressionInt64<WorkOrder>(columnName));
                                    break;
                                case BaseCri.DataType.DateTime:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<WorkOrder, DateTime>(columnName));
                                    break;
                                default:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                                    break;
                            }
                            break;
                        case BaseCri.EntityOrderBy.Dsc:
                            switch (dataType)
                            {
                                case BaseCri.DataType.String:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                                    break;
                                case BaseCri.DataType.Int64:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpressionInt64<WorkOrder>(columnName));
                                    break;
                                case BaseCri.DataType.DateTime:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<WorkOrder, DateTime>(columnName));
                                    break;
                                default:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                                    break;
                            }
                            break;
                        default:
                            queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                            break;
                    }
                }
            }
            if (cri != null && cri.IsPagination)
                queryableData = queryableData.Skip(cri.NoOfPage * (cri.CurrentPage - 1)).Take(cri.NoOfPage);

            //queryableData = queryableData.

            //foreach ( var order in queryableData)
            //{
            //    User creator =   _userRepository.GetByUserName(order.CreatedByName);

            //    if(!(creator == null))
            //    {
            //        order.CreatedByName = creator.FirstName + " " + creator.LastName;
            //    }
            //}

            //var result = await queryableData.ToListAsync();

            return queryableData;
        }

        public async override Task<WorkOrder> GetById(object id)
        {
            try
            {
                var workOrder = await base.GetById(id);

                workOrder.WorkOrderLocationList = context.WorkOrderLocations.Include(c => c.Location).AsNoTracking().Where(x => x.WorkOrderId == workOrder.Id).ToList();

                foreach (var location in workOrder.WorkOrderLocationList)
                {
                    if (location.Location == null) location.Location = new ApplicationCore.Entities.Configurations.Location { PostalCode = "TBA", Code = "TBA", Name = "TBA" };
                }

                workOrder.WorkOrderPassengerList = context.WorkOrderPassengers.Include(c => c.Vehicle).AsNoTracking().Where(x => x.WorkOrderId == workOrder.Id).ToList();

                foreach (var passenger in workOrder.WorkOrderPassengerList)
                {
                    if (passenger.Vehicle == null) passenger.Vehicle = new ApplicationCore.Entities.Configurations.Vehicle { VehicleNo = "N/A" };
                }

                workOrder.MeetingServiceList = context.WorkOrderMeetingServices.Include(c => c.MeetingService).AsNoTracking().Where(x => x.WorkOrderId == workOrder.Id).ToList();

                workOrder.ServiceJobList = context.ServiceJobs.Include(c => c.Vehicle).Include(x => x.User).AsNoTracking().Where(x => x.WorkOrderId == workOrder.Id).ToList();

                workOrder.WorkOrderHistoryList = new List<WorkOrderHistory>();

                workOrder.WorkOrderHistoryList = context.WorkOrderHistories.AsNoTracking().Where(x => x.WorkOrderId == workOrder.Id).OrderBy(x => x.Id).ToList();

                return workOrder;
            }
            catch (Exception ex)
            {
                return new WorkOrder();
            }
        }

        public async Task<IEnumerable<WorkOrder>> GetByInvoiceNo(string invoiceNo)
        {
            //var invoice = context.SalesInvoiceSummaries.AsNoTracking().FirstOrDefault(x => x.ReferenceNo.Equals(invoiceNo));

            //if (invoice == null) return new List<WorkOrder>();
            //var workOrderIdList = new List<Int64>();

            //foreach (var detail in context.SalesInvoiceSummaryDetails.AsNoTracking().Where(x => x.SalesInvoiceSummaryId == invoice.Id))
            //{
            //    workOrderIdList.Add(detail.WorkOrderId);
            //}
            //var workOrderIds = workOrderIdList.ToArray();

            var workOrders = context.WorkOrders.Where(x => x.ParentCompanyId == GlobalVariable.COMPANY_ID);

            workOrders = workOrders.Where(x => x.ShortText2 == invoiceNo); //ShortText2 = Invoice Number

            return workOrders.OrderByDescending(x => x.InvoiceNo);
        }

        public async Task<List<WorkOrder>> GetByIds(long[] workOrderIds)
        {
            return await this.context.WorkOrders.Where(x => workOrderIds.Contains(x.Id)).ToListAsync();
        }

        public async Task<WorkOrder> GetByCreditNoteId(long creditNoteId)
        {
            return await this.context.WorkOrders.FirstOrDefaultAsync(x => x.CreditNoteId == creditNoteId);
        }

        public async Task<List<WorkOrderPassenger>> GetNonInchargePassengers(long workOrderId, long vehicleId)
        {
            return await context.WorkOrderPassengers.Include(x => x.Vehicle).AsNoTracking().Where(x => x.WorkOrderId == workOrderId && x.VehicleId == vehicleId && x.InCharge == false).ToListAsync();
        }

        public async Task<List<WorkOrderPassenger>> GetNonInchargePassengersByServiceJobId(long workOrderId, long serviceJobId)
        {
            return await context.WorkOrderPassengers.Include(x => x.Vehicle).AsNoTracking().Where(x => x.WorkOrderId == workOrderId && x.ServiceJobId == serviceJobId && x.InCharge == false).ToListAsync();
        }

        public List<WorkOrder> GetSimliarOrders(DateTime pickupDate, long CustomerId, long VesselId, List<WorkOrderLocation> locations)
        {
            try
            {
                //pickupDate = pickupDate.ToLocalTime();

                DateTime fromTime = pickupDate.AddHours(-1);

                DateTime toTime = pickupDate.AddHours(1);

                var resultLst = context.WorkOrders.Where(x => x.CustomerId == CustomerId && x.VesselId == VesselId && (x.PickUpdateDate >= fromTime && x.PickUpdateDate <= toTime)).ToList();

                List<WorkOrder> orders = new List<WorkOrder>();

                foreach (var workOrder in resultLst)
                {
                    var locationLst = context.WorkOrderLocations.Where(x => x.WorkOrderId == workOrder.Id).ToList();

                    if (locationLst != null && locationLst.Count > 0)
                    {
                        var pickUpPoint = locationLst.Where(x => x.Type == WorkOrderLocation.LocationType.PickUp).FirstOrDefault();

                        var pickUpCri = locations.Where(x => x.Type == WorkOrderLocation.LocationType.PickUp).FirstOrDefault();

                        if (pickUpPoint == null || pickUpCri == null) continue;

                        if (pickUpPoint.HotelId != null && pickUpPoint.HotelId != pickUpCri.HotelId) continue;

                        if (pickUpPoint.LocationId != pickUpCri.LocationId) continue;

                        var dropOffPoint = locationLst.Where(x => x.Type == WorkOrderLocation.LocationType.DropOff).FirstOrDefault();

                        var dropOffCri = locations.Where(x => x.Type == WorkOrderLocation.LocationType.DropOff).FirstOrDefault();

                        if (dropOffPoint == null || dropOffCri == null) continue;

                        if (dropOffPoint.HotelId != null && dropOffPoint.HotelId != dropOffCri.HotelId) continue;

                        if (dropOffPoint.LocationId != dropOffCri.LocationId) continue;

                        orders.Add(workOrder);
                    }
                }

                return orders;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<WorkOrder>> GetByInvoiceDateCri(WorkOrderCri cri)
        {
            try
            {


                var queryableData = context.WorkOrders.AsNoTracking().Where(x => x.ParentCompanyId == GlobalVariable.COMPANY_ID);

                var tempQuery = from workOrder in queryableData
                                join salesInvoiceSummary in context.SalesInvoiceSummaries.AsNoTracking() on workOrder.SalesInvoiceSummaryId equals salesInvoiceSummary.Id
                                select new WorkOrderTempQuery
                                {
                                    WorkOrder = workOrder,
                                    InvoiceDate = salesInvoiceSummary.InvoiceDate,
                                    YearMonthNo = salesInvoiceSummary.Id1,
                                    SerialNo = salesInvoiceSummary.Id2
                                };

                if (cri == null) cri = new WorkOrderCri();

                if (!string.IsNullOrEmpty(cri.SalesInvoiceSummaryNo))
                {
                    tempQuery = tempQuery.Where(x => x.WorkOrder.ShortText2.ToLower().Trim().Contains(cri.SalesInvoiceSummaryNo.ToLower().Trim()));

                    //var invoice = context.SalesInvoiceSummaries.AsNoTracking().FirstOrDefault(x => x.ReferenceNo.Equals(cri.SalesInvoiceSummaryNo));
                    //if (invoice == null) return new List<WorkOrder>();
                    //var workOrderIdList = new List<Int64>();
                    //foreach (var detail in context.SalesInvoiceSummaryDetails.AsNoTracking().Where(x => x.SalesInvoiceSummaryId == invoice.Id))
                    //{
                    //    workOrderIdList.Add(detail.WorkOrderId);
                    //}
                    //var workOrderIds = workOrderIdList.ToArray();

                    //queryableData = queryableData.Where(x => workOrderIds.Contains(x.Id));
                }

                if (cri.CustomerId > 0)
                    tempQuery = tempQuery.Where(x => x.WorkOrder.CustomerId == cri.CustomerId);

                if (cri.VesselId > 0)
                    tempQuery = tempQuery.Where(x => x.WorkOrder.VesselId == cri.VesselId);

                if ((cri.Status > 0 || cri.Status == WorkOrder.OrderStatus.Cancelled) && string.IsNullOrEmpty(cri.SalesInvoiceSummaryNo))
                {
                    tempQuery = tempQuery.Where(x => x.WorkOrder.Status == cri.Status);
                }

                if (cri.Status != WorkOrder.OrderStatus.Cancelled)
                    tempQuery = tempQuery.Where(x => x.WorkOrder.Status != WorkOrder.OrderStatus.Cancelled);

                if (cri.Status == 0 && cri.IsComeFromAccount)
                    tempQuery = tempQuery.Where(x => x.WorkOrder.Status >= WorkOrder.OrderStatus.With_Accounts);

                if (cri.FromDate != null)
                {
                    TimeSpan ts = new TimeSpan(0, 0, 0);
                    cri.FromDate = cri.FromDate.Value.Date + ts;
                    tempQuery = tempQuery.Where(x => x.InvoiceDate >= cri.FromDate);
                }


                if (cri.ToDate != null)
                {
                    TimeSpan newts = new TimeSpan(23, 59, 59);
                    cri.ToDate = cri.ToDate.Value.Date + newts;
                    //cri.ToDate = cri.ToDate.Value.AddDays(1);
                    tempQuery = tempQuery.Where(x => x.InvoiceDate <= cri.ToDate);
                }

                if (cri.SearchByRange)
                {
                    if (cri.StartingRefYearMonth > 0 && cri.StartingRefSerial > 0)
                    {
                        tempQuery = tempQuery.Where(x => x.YearMonthNo >= cri.StartingRefYearMonth && x.SerialNo >= cri.StartingRefSerial);
                    }

                    if (cri.EndingRefYearMonth > 0 && cri.EndingRefSerial > 0)
                    {
                        tempQuery = tempQuery.Where(x => x.YearMonthNo <= cri.EndingRefYearMonth && x.SerialNo <= cri.EndingRefSerial);
                    }
                }

                queryableData = tempQuery.Select(x => x.WorkOrder);

                if (cri != null && cri.OrderBys != null)
                {
                    foreach (var columnName in cri.OrderBys.Keys)
                    {
                        var value = cri.OrderBys[columnName];
                        var dataType = value.Keys.FirstOrDefault();
                        var orderType = value.Values.FirstOrDefault();

                        switch (orderType)
                        {
                            case BaseCri.EntityOrderBy.Asc:
                                switch (dataType)
                                {
                                    case BaseCri.DataType.String:
                                        queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                                        break;
                                    case BaseCri.DataType.Int64:
                                        queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpressionInt64<WorkOrder>(columnName));
                                        break;
                                    case BaseCri.DataType.DateTime:
                                        queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<WorkOrder, DateTime>(columnName));
                                        break;
                                    default:
                                        queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                                        break;
                                }
                                break;
                            case BaseCri.EntityOrderBy.Dsc:
                                switch (dataType)
                                {
                                    case BaseCri.DataType.String:
                                        queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                                        break;
                                    case BaseCri.DataType.Int64:
                                        queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpressionInt64<WorkOrder>(columnName));
                                        break;
                                    case BaseCri.DataType.DateTime:
                                        queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<WorkOrder, DateTime>(columnName));
                                        break;
                                    default:
                                        queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                                        break;
                                }
                                break;
                            default:
                                queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                                break;
                        }
                    }
                }
                if (cri != null && cri.IsPagination)
                    queryableData = queryableData.Skip(cri.NoOfPage * (cri.CurrentPage - 1)).Take(cri.NoOfPage);

                return queryableData;

            }
            catch (Exception ex)
            {
                return new List<WorkOrder>();
            }
        }

        //PCR2021
        public async Task<IEnumerable<BilledOrderListInvoiceViewModel>> GetDataForBilledOrderList(WorkOrderCri cri)
        {
            try
            {
                //Id3 = Status --> 1: Billed;
                var query = context.SalesInvoiceSummaries.AsNoTracking()
                                    .Where(x => x.Id3 == 1 &&
                                                x.ParentCompanyId == GlobalVariable.COMPANY_ID);

                if (cri == null) cri = new WorkOrderCri();

                if (!string.IsNullOrEmpty(cri.SalesInvoiceSummaryNo))
                {
                    query = query.Where(x => x.ReferenceNo.ToLower().Trim().Contains(cri.SalesInvoiceSummaryNo.ToLower().Trim()));
                }

                if (cri.CustomerId > 0)
                {
                    query = query.Where(x => x.CustomerId == cri.CustomerId);
                }

                if (cri.VesselId > 0)
                {
                    query = query.Where(x => x.VesselId == cri.VesselId);
                }

                if (cri.FromDate != null)
                {
                    TimeSpan ts = new TimeSpan(0, 0, 0);
                    cri.FromDate = cri.FromDate.Value.Date + ts;
                    query = query.Where(x => x.InvoiceDate >= cri.FromDate);
                }

                if (cri.ToDate != null)
                {
                    TimeSpan newts = new TimeSpan(23, 59, 59);
                    cri.ToDate = cri.ToDate.Value.Date + newts;
                    //cri.ToDate = cri.ToDate.Value.AddDays(1);
                    query = query.Where(x => x.InvoiceDate <= cri.ToDate);
                }

                if (cri.SearchByRange)
                {
                    if (cri.StartingRefYearMonth > 0 && cri.StartingRefSerial > 0)
                    {
                        query = query.Where(x => x.Id1 >= cri.StartingRefYearMonth && x.Id2 >= cri.StartingRefSerial);
                    }

                    if (cri.EndingRefYearMonth > 0 && cri.EndingRefSerial > 0)
                    {
                        query = query.Where(x => x.Id1 <= cri.EndingRefYearMonth && x.Id2 <= cri.EndingRefSerial);
                    }
                }

                var queryResult = from invoice in query
                                  join customer in context.Customers.AsNoTracking() on invoice.CustomerId equals customer.Id
                                  select new BilledOrderListInvoiceViewModel()
                                  {
                                      salesInvoiceSummary = invoice,
                                      DiscountAmt = customer.DiscountAmt,
                                      DiscountPercent = customer.DiscountPercent
                                  };

                return queryResult;
            }
            catch (Exception ex)
            {
                return new List<BilledOrderListInvoiceViewModel>();
            }
        }

        //PCR2021
        public List<long> GetWorkOrderIdsFromInvoiceIds(long[] invoiceIds)
        {
            try
            {
                var query = from workOrder in this.context.WorkOrders.AsNoTracking()
                            join invoiceSummaryDetail in this.context.SalesInvoiceSummaryDetails.AsNoTracking() on workOrder.Id equals invoiceSummaryDetail.WorkOrderId
                            join invoiceSummary in this.context.SalesInvoiceSummaries.AsNoTracking() on invoiceSummaryDetail.SalesInvoiceSummaryId equals invoiceSummary.Id
                            where invoiceIds.Contains(invoiceSummary.Id)
                            select workOrder.Id;

                return query.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<WorkOrder>> GetOrdersForAgents(WorkOrderCri cri)
        {
            var queryableData = context.WorkOrders.Include(w => w.Customer).AsNoTracking().Where(x => x.ParentCompanyId == GlobalVariable.COMPANY_ID);

            if (cri == null) cri = new WorkOrderCri();

            if (cri.CustomerId > 0)
                queryableData = queryableData.Where(x => x.CustomerId == cri.CustomerId);

            if (cri.VesselId > 0)
                queryableData = queryableData.Where(x => x.VesselId == cri.VesselId);

            if (cri.Status == WorkOrder.OrderStatus.Verified)
            {
                queryableData = queryableData.Where(x => x.Status >= WorkOrder.OrderStatus.Verified);
            }
            else if ((cri.Status > 0 || cri.Status == WorkOrder.OrderStatus.Cancelled) && string.IsNullOrEmpty(cri.SalesInvoiceSummaryNo))
            {
                queryableData = queryableData.Where(x => x.Status == cri.Status);
            }

            if (cri.Status != WorkOrder.OrderStatus.Cancelled)
                queryableData = queryableData.Where(x => x.Status != WorkOrder.OrderStatus.Cancelled);

            if (cri.Status == 0 && cri.IsComeFromAccount)
                queryableData = queryableData.Where(x => x.Status >= WorkOrder.OrderStatus.With_Accounts);

            //DateTime currentDateTime = DateTime.Now;

            //DateTime dateTimeLimit = currentDateTime.AddDays(-3);

            if (cri.FromDate != null)
            {
                //if (cri.FromDate.Value.Date < dateTimeLimit.Date)
                //{
                //    cri.FromDate = dateTimeLimit;
                //}

                queryableData = queryableData.Where(x => DbFunctions.TruncateTime(x.PickUpdateDate.Value) >= DbFunctions.TruncateTime(cri.FromDate.Value));
            }
            //else
            //{
            //    cri.FromDate = dateTimeLimit;

            //    queryableData = queryableData.Where(x => DbFunctions.TruncateTime(x.PickUpdateDate.Value) >= DbFunctions.TruncateTime(cri.FromDate.Value));
            //}


            if (cri.ToDate != null)
            {
                //TimeSpan newts = new TimeSpan(23, 59, 59);
                //cri.ToDate = cri.ToDate.Value.Date + newts;
                //cri.ToDate = cri.ToDate.Value.AddDays(1);
                queryableData = queryableData.Where(x => DbFunctions.TruncateTime(x.PickUpdateDate.Value) <= DbFunctions.TruncateTime(cri.ToDate.Value));
            }


            if (cri != null && cri.OrderBys != null)
            {
                foreach (var columnName in cri.OrderBys.Keys)
                {
                    var value = cri.OrderBys[columnName];
                    var dataType = value.Keys.FirstOrDefault();
                    var orderType = value.Values.FirstOrDefault();

                    switch (orderType)
                    {
                        case BaseCri.EntityOrderBy.Asc:
                            switch (dataType)
                            {
                                case BaseCri.DataType.String:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                                    break;
                                case BaseCri.DataType.Int64:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpressionInt64<WorkOrder>(columnName));
                                    break;
                                case BaseCri.DataType.DateTime:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<WorkOrder, DateTime>(columnName));
                                    break;
                                default:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                                    break;
                            }
                            break;
                        case BaseCri.EntityOrderBy.Dsc:
                            switch (dataType)
                            {
                                case BaseCri.DataType.String:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                                    break;
                                case BaseCri.DataType.Int64:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpressionInt64<WorkOrder>(columnName));
                                    break;
                                case BaseCri.DataType.DateTime:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<WorkOrder, DateTime>(columnName));
                                    break;
                                default:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                                    break;
                            }
                            break;
                        default:
                            queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<WorkOrder>(columnName, dataType));
                            break;
                    }
                }
            }
            if (cri != null && cri.IsPagination)
                queryableData = queryableData.Skip(cri.NoOfPage * (cri.CurrentPage - 1)).Take(cri.NoOfPage);

            //queryableData = queryableData.

            //foreach ( var order in queryableData)
            //{
            //    User creator =   _userRepository.GetByUserName(order.CreatedByName);

            //    if(!(creator == null))
            //    {
            //        order.CreatedByName = creator.FirstName + " " + creator.LastName;
            //    }
            //}

            //var result = await queryableData.ToListAsync();

            return queryableData;
        }

        #endregion

    }
}
