using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.ApplicationCore.Common;
using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.ReportViewModels;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.Common;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.DocumentProcessor;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.SOP.SalesInvoice
{
    public class GenerateInvoiceController : BaseController<WorkOrderCri, WorkOrder, IWorkOrderService>
    {
        IRolePermissionService _rolePermissionService;
        ICustomerService _customerService;
        ICommonConfigurationService _commonConfigService;
        ISalesInvoiceSummaryService _salesInvoiceSummaryService;
        IGSTService _gstService;
        ISalesInvoiceService _salesInvoiceService;
        IWorkOrderService workOrderService;
        IUserService _userService;
        ICompanyService companyService;
        ApplicationDbContext db;

        public GenerateInvoiceController(IWorkOrderService service,
            IRolePermissionService rolePermissionService,
            ICustomerService customerService,
            ICommonConfigurationService commonConfigService,
            ISalesInvoiceSummaryService salesInvoiceSummaryService,
            IGSTService gstService,
            ISalesInvoiceService salesInvoiceService,
            IWorkOrderService workOrderService,
            ICompanyService companyService,
            IUserService userService,
            ApplicationDbContext db) : base(service)
        {
            _defaultColumn = "ReferenceNoNumber";
            _defaultOrderBy = BaseCri.EntityOrderBy.Dsc.ToString();
            _defaultDataType = BaseCri.DataType.Int64.ToString();
            this._rolePermissionService = rolePermissionService;
            this._customerService = customerService;
            this._commonConfigService = commonConfigService;
            this._salesInvoiceSummaryService = salesInvoiceSummaryService;
            this._gstService = gstService;
            this._salesInvoiceService = salesInvoiceService;
            this.workOrderService = workOrderService;
            this.companyService = companyService;
            this._userService = userService;
            this.db = db;
        }

        protected override WorkOrderCri GetCri()
        {
            var cri = base.GetCri();

            if (cri == null)
                cri = new WorkOrderCri();
            cri.Includes = new List<string>();
            cri.Includes.Add("Customer");

            var customerIdStr = Request["Customers"];
            var vesselIdStr = Request["Vessels"];
            var fromDate = Request["FromDate"];
            var toDate = Request["ToDate"];
            var status = Request["Status"];
            var invoiceNo = Request["SalesInvoiceSummaryNo"];
            var dnNo = Request["DNNo"];
            var invoiceDate = Request["InvoiceDate"];
            var drivers = Request["DriverId"];
            var returnUrl = Request["returnUrl"];
            var orderPage = Request["CurrentPage"];
            var orderColumn = Request["Column"];
            var orderOrderBy = Request["OrderBy"];
            var orderDataType = Request["DataType"];
            ViewBag.CurrentId = Request["CurrentId"];
            ViewBag.ReturnUrl = returnUrl;

            ViewBag.CustomerId = customerIdStr;
            ViewBag.VesselId = vesselIdStr;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.Status = status;
            ViewBag.SalesInvoiceSummaryNo = invoiceNo;
            ViewBag.DNNo = dnNo;
            ViewBag.InvoiceDate = invoiceDate;
            ViewBag.DriverId = drivers;
            ViewBag.orderPage = orderPage;
            ViewBag.orderColumn = orderColumn;
            ViewBag.orderOrderBy = orderOrderBy;
            ViewBag.orderDataType = orderDataType;

            #region Customer
            Int64 customerId = 0;
            if (!string.IsNullOrEmpty(customerIdStr))
            {
                Int64.TryParse(customerIdStr, out customerId);
            }
            cri.CustomerId = customerId;
            #endregion

            #region Vessel
            Int64 vesselId = 0;
            if (!string.IsNullOrEmpty(vesselIdStr))
            {
                Int64.TryParse(vesselIdStr, out vesselId);
            }
            cri.VesselId = vesselId;
            #endregion

            int statusInt = 0;
            if (!string.IsNullOrEmpty(status))
            {
                Int32.TryParse(status, out statusInt);
            }

            cri.Status = (EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders.WorkOrder.OrderStatus)statusInt;

            if (!string.IsNullOrEmpty(fromDate))
                cri.FromDate = Util.ConvertStringToDateTime(fromDate, DateConfig.CULTURE);

            if (!string.IsNullOrEmpty(toDate))
                cri.ToDate = Util.ConvertStringToDateTime(toDate, DateConfig.CULTURE);
            cri.SalesInvoiceSummaryNo = invoiceNo;
            cri.DNNo = dnNo;
            cri.InvoiceDate = invoiceDate;

            cri.Drivers = drivers;
            long driverId = 0;
            Int64.TryParse(drivers, out driverId);
            cri.DriverId = driverId;

            return cri;
        }

        protected async override Task LoadReferencesForList(WorkOrderCri aCri)
        {
            var customerId = Request["Customers"];
            var vesselId = Request["Vessels"];
            var dnNo = Request["DNNo"];
            var invoiceDate = Request["InvoiceDate"];
            var returnUrl = Request["returnUrl"];
            var orderPage = Request["CurrentPage"];
            string orderColumn = Request["Column"];
            string orderOrderBy = Request["OrderBy"];
            string orderDataType = Request["DataType"];
            string currentId = Request["CurrentId"];

            ViewBag.ReturnUrl = returnUrl;

            ViewBag.orderPage = orderPage;
            ViewBag.orderColumn = orderColumn;
            ViewBag.orderOrderBy = orderOrderBy;
            ViewBag.orderDataType = orderDataType;
            ViewBag.CurrentId = currentId;

            var customers = (await this._customerService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.Customers = new SelectList(customers, "Id", "Name", customerId);

            //if(customerId != null && customer > 0)
            //{
            //    var firstCustomer = await _customerService.GetById(customers.FirstOrDefault().Id);

            //    ViewBag.Vessels = new SelectList(firstCustomer.VesselList.OrderBy(x => x.Vessel), "VesselId", "Vessel", vesselId);
            //}
            //else
            //{

            //}

            CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            configurationCri.Includes = new List<string>();
            configurationCri.Includes.Add("ConfigurationGroup");
            var vessels = (await _commonConfigService.GetByCri(configurationCri)).Where(x => x.ConfigurationGroup.Code.Equals(GeneralConfig.ConfigurationGrpCodes.VesselId.ToString())).OrderBy(x => x.Name);
            ViewBag.Vessels = new SelectList(vessels, "Id", "Name", vesselId);


            var drivers = _userService.GetDrivers();
            ViewBag.DriverId = new SelectList(drivers, "Id", "Name", aCri.DriverId);
            string fromDate = Request["FromDate"];
            string toDate = Request["ToDate"];

            if (fromDate == null && toDate == null)
            {
                if (!string.IsNullOrEmpty(invoiceDate))
                {
                    DateTime invoiceDateTime = Util.ConvertStringToDateTime(invoiceDate, DateConfig.CULTURE);

                    DateTime startDate = new DateTime(invoiceDateTime.Year, invoiceDateTime.Month, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                    fromDate = Util.ConvertDateToString(startDate, DateConfig.CULTURE);
                    toDate = Util.ConvertDateToString(endDate, DateConfig.CULTURE);
                }
                else
                {
                    DateTime endDate = TimeUtil.GetLocalTime();
                    DateTime startDate = new DateTime(endDate.Year, endDate.Month, 1);

                    fromDate = Util.ConvertDateToString(startDate, DateConfig.CULTURE);
                    toDate = Util.ConvertDateToString(endDate, DateConfig.CULTURE);
                }
            }


            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.Status = new SelectList(WorkOrderCri.GetOrderStatusesForAccounting(), "Id", "Name");

            ViewBag.InvoiceDate = invoiceDate;

            var salesInvoiceSummary = await _salesInvoiceSummaryService.GetByInvoiceNo(aCri.SalesInvoiceSummaryNo);
            if (salesInvoiceSummary != null)
            {
                aCri.CustomerId = Convert.ToInt64(customerId);
                aCri.VesselId = Convert.ToInt64(vesselId);
                aCri.DNNo = salesInvoiceSummary.DNNo;
                aCri.InvoiceDate = salesInvoiceSummary.InvoiceDateStr;
                aCri.IncludeTax = salesInvoiceSummary.IsTaxable;
                aCri.AttnStr = salesInvoiceSummary.AttnStr;
            }

            aCri.NoOfPage = 400;
        }

        protected async override Task<IEnumerable<WorkOrder>> GetData(WorkOrderCri cri)
        {
            cri.IsComeFromAccount = true;
            ViewBag.SalesInvoiceNo = cri.SalesInvoiceSummaryNo;

            //if (string.IsNullOrEmpty(cri.SalesInvoiceSummaryNo))
            //{
            IEnumerable<WorkOrder> workOrders = await base.GetData(cri);
            //if (cri.DriverId != 0)
            //{
            //    var driver = await _userService.GetById(cri.DriverId);
            //    if (driver != null)
            //    {
            //        workOrders = workOrders.Where(x => x.Drivers.ToLower().Contains(driver.Name.ToLower()));
            //    }
            //}
            return workOrders;
            //}
            //return await base.GetData(cri);
            //else
            //{
            //    return await this._service.GetByInvoiceNo(cri.SalesInvoiceSummaryNo);
            //}
        }

        protected async override Task<IPagedList<WorkOrder>> GetEntities(WorkOrderCri aCri)
        {
            try
            {
                aCri.NoOfPage = 400;

                cri = GetCri();

                var cri2 = GetOrderBy(cri);

                var entities = await GetData(cri2);

                return entities.ToPagedList(aCri.CurrentPage, aCri.NoOfPage);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }

        //Saving Invoice Data Only without Download Invoice
        public async Task<ActionResult> GenerateInvoice(Int64[] workOrderIds, WorkOrderCri aCri)
        {
            try

            {
                //Check if there are work orders selected
                if (workOrderIds == null || workOrderIds.Count() == 0)
                {
                    if (String.IsNullOrEmpty(aCri.SalesInvoiceSummaryNo))
                    {
                        TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                        TempData["message"] = "There is no work order selected!";
                        return RedirectToAction(nameof(Index), new { Customers = aCri.CustomerId, Vessels = aCri.VesselId, FromDate = aCri.FromDate, ToDate = aCri.ToDate, Status = aCri.Status, SalesInvoiceSummaryNo = aCri.SalesInvoiceSummaryNo, DNNo = aCri.DNNo, InvoiceDate = aCri.InvoiceDate });
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(aCri.SalesInvoiceSummaryNo))
                        {
                            await this.workOrderService.RemoveUnselectedOrdersFromInvoice(workOrderIds, aCri.SalesInvoiceSummaryNo);

                            //Get Existing Invoice Data As No Tracking
                            var dbsalesInvoice = await _salesInvoiceSummaryService.GetByInvoiceNo(aCri.SalesInvoiceSummaryNo);

                            if (dbsalesInvoice != null)
                            {
                                SalesInvoiceSummary salesInvoiceSummary = new SalesInvoiceSummary();

                                //Inserting DN No
                                salesInvoiceSummary.DNNo = aCri.DNNo;

                                //Inserting Invoice Date if custom date is inserted
                                salesInvoiceSummary.InvoiceDate = string.IsNullOrEmpty(aCri.InvoiceDate) ? TimeUtil.GetLocalTime() : Util.ConvertStringToDateTime(aCri.InvoiceDate, DateConfig.CULTURE);

                                //Inserting Ccustomer and Vessel Name
                                var customer = await _customerService.GetHeaderOnly(dbsalesInvoice.CustomerId);

                                salesInvoiceSummary.CustomerName = customer.Name;
                                salesInvoiceSummary.CustomerId = customer.Id;

                                salesInvoiceSummary.VesselId = dbsalesInvoice.VesselId;
                                salesInvoiceSummary.VesselName = dbsalesInvoice.VesselName;

                                //Inserting 'Is Taxable Invoice' value
                                salesInvoiceSummary.IsTaxable = aCri.IncludeTax;

                                //Inserting 'ATTN' value
                                salesInvoiceSummary.AttnStr = aCri.AttnStr;

                                salesInvoiceSummary.Details = new List<SalesInvoiceSummaryDetails>();

                                salesInvoiceSummary.Id = dbsalesInvoice.Id;

                                salesInvoiceSummary.ReferenceNo = dbsalesInvoice.ReferenceNo;

                                //Inserting RefNum Int Field
                                salesInvoiceSummary.YearMonthNo = dbsalesInvoice.YearMonthNo;
                                salesInvoiceSummary.SerialNo = dbsalesInvoice.SerialNo;

                                //Inserting Sales Invoice Summary Status
                                salesInvoiceSummary.Status = dbsalesInvoice.Status;

                                await _salesInvoiceSummaryService.Save(salesInvoiceSummary);
                            }
                        }
                    }
                }
                else
                {

                    #region Validation
                    List<WorkOrder> duplicateInvoiceWorkOrders = new List<WorkOrder>();

                    duplicateInvoiceWorkOrders = await workOrderService.GetByIds(workOrderIds);

                    //Check if selected work orders have more than more than one invoice number
                    if (duplicateInvoiceWorkOrders.Select(x => x.ShortText2).Distinct().Count() > 1)
                    {
                        //Check if one work order have any invoice details yet
                        if (duplicateInvoiceWorkOrders.Select(x => x.ShortText2).Distinct().Count() == 2 && duplicateInvoiceWorkOrders.Distinct().Select(x => x.ShortText2 == null).Count() > 0)
                        {
                            //Check if Sales Invoice Summary text field is filled or not
                            if (String.IsNullOrEmpty(aCri.SalesInvoiceSummaryNo))
                            {
                                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                                TempData["message"] = "Not allowed to generate new invoice for work orders with existing invoices.";

                                return RedirectToAction(nameof(Index), new { Customers = aCri.CustomerId, Vessels = aCri.VesselId, FromDate = aCri.FromDate, ToDate = aCri.ToDate, Status = aCri.Status, SalesInvoiceSummaryNo = aCri.SalesInvoiceSummaryNo, DNNo = aCri.DNNo, InvoiceDate = aCri.InvoiceDate });
                            }
                        }
                        else
                        {
                            TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                            TempData["message"] = "Not allowed to select multiple work orders with different invoice numbers!";
                            return RedirectToAction(nameof(Index), new { Customers = aCri.CustomerId, Vessels = aCri.VesselId, FromDate = aCri.FromDate, ToDate = aCri.ToDate, Status = aCri.Status, SalesInvoiceSummaryNo = aCri.SalesInvoiceSummaryNo, DNNo = aCri.DNNo, InvoiceDate = aCri.InvoiceDate });
                        }
                    }

                    //Check if selected workorders are from different customers
                    if (duplicateInvoiceWorkOrders.Select(x => x.CustomerId).Distinct().Count() > 1)
                    {
                        TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                        TempData["message"] = "Not allowed to select work orders with different customers!";
                        return RedirectToAction(nameof(Index), new { Customers = aCri.CustomerId, Vessels = aCri.VesselId, FromDate = aCri.FromDate, ToDate = aCri.ToDate, Status = aCri.Status, SalesInvoiceSummaryNo = aCri.SalesInvoiceSummaryNo, DNNo = aCri.DNNo, InvoiceDate = aCri.InvoiceDate });
                    }

                    //Check if selected workorders are from different vessels
                    if (duplicateInvoiceWorkOrders.Select(x => x.VesselId).Distinct().Count() > 1)
                    {
                        TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                        TempData["message"] = "Not allowed to select work orders with different vessels!";
                        return RedirectToAction(nameof(Index), new { Customers = aCri.CustomerId, Vessels = aCri.VesselId, FromDate = aCri.FromDate, ToDate = aCri.ToDate, Status = aCri.Status, SalesInvoiceSummaryNo = aCri.SalesInvoiceSummaryNo, DNNo = aCri.DNNo, InvoiceDate = aCri.InvoiceDate });
                    }
                    #endregion

                    #region Prepare GST List
                    //Get GSTs
                    var gsts = (await _gstService.GetByCri(null)).OrderBy(x => x.Name);

                    if (gsts == null || gsts.ToList().Count == 0)
                    {
                        TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                        TempData["message"] = "No gst defined yet!";
                        return RedirectToAction(nameof(Index), new { Customers = aCri.CustomerId, Vessels = aCri.VesselId, FromDate = aCri.FromDate, ToDate = aCri.ToDate, Status = aCri.Status, SalesInvoiceSummaryNo = aCri.SalesInvoiceSummaryNo });
                    }

                    #endregion

                    #region Prepare Invoice Data
                    List<EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice> saleInvoices = new List<ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice>();
                    List<WorkOrder> workOrders = new List<WorkOrder>();

                    if (!String.IsNullOrEmpty(aCri.SalesInvoiceSummaryNo))
                    {
                        await this.workOrderService.RemoveUnselectedOrdersFromInvoice(workOrderIds, aCri.SalesInvoiceSummaryNo);
                    }

                    //Get Corresponding Sales Invoice Details based on Work Order Ids and getting Corresponding Work Orders

                    foreach (var workOrderId in workOrderIds)
                    {
                        var workOrder = await this.workOrderService.GetById(workOrderId);

                        if (workOrder == null) continue;

                        if (workOrder.CustomerId == null) continue;

                        workOrders.Add(workOrder);

                        workOrder.Customer = await this._customerService.GetHeaderOnly(workOrder.CustomerId.Value);
                    }

                    if (workOrders != null && workOrders.Count > 0)
                    {
                        workOrders = workOrders.OrderBy(x => x.PickUpdateDate).ToList();

                        foreach (var workOrder in workOrders)
                        {
                            List<Price> prices = new List<Price>();

                            if (workOrder.InvoiceId == null)
                            {

                                //Generate Invoicing Details and Credit Note Details if there is no Invoice yet for the Work Order

                                EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice saleInvoice = new ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice();

                                await _salesInvoiceService.PreparePriceListFromTransferVoucher(workOrder, prices);
                                await _salesInvoiceService.PreparePriceListForMeetingService(workOrder, prices);
                                await _salesInvoiceService.PreparePriceList(workOrder, prices);

                                saleInvoice.GenerateInvoiceDetail(workOrder, prices, gsts.Where(x => x.isDefault).FirstOrDefault());
                                saleInvoices.Add(saleInvoice);
                                saleInvoice.WorkOrderId = workOrder.Id;
                                saleInvoice.InvoiceDate = string.IsNullOrEmpty(aCri.InvoiceDate) ? TimeUtil.GetLocalTime() : Util.ConvertStringToDateTime(aCri.InvoiceDate, DateConfig.CULTURE);
                                await _salesInvoiceService.Save(saleInvoice);
                            }
                            else
                            {
                                //Adding the Invoicing Detail to the details list if the Invoicing Detail exists

                                var salesInvoice = await _salesInvoiceService.GetById(workOrder.InvoiceId.Value);

                                saleInvoices.Add(salesInvoice);
                            }
                        }
                    }

                    #endregion

                    #region PDF Report Generation

                    //Creating Sales Invoice Summary Entity

                    SalesInvoiceSummary salesInvoiceSummary = new SalesInvoiceSummary();

                    //Inserting DN No
                    salesInvoiceSummary.DNNo = aCri.DNNo;

                    //Inserting Invoice Date if custom date is inserted
                    salesInvoiceSummary.InvoiceDate = string.IsNullOrEmpty(aCri.InvoiceDate) ? TimeUtil.GetLocalTime() : Util.ConvertStringToDateTime(aCri.InvoiceDate, DateConfig.CULTURE);

                    //Inserting Ccustomer and Vessel Name
                    var customer = await _customerService.GetHeaderOnly(saleInvoices[0].CustomerId);

                    salesInvoiceSummary.CustomerName = customer.Name;
                    salesInvoiceSummary.CustomerId = customer.Id;

                    salesInvoiceSummary.VesselId = saleInvoices[0].VesselId.Value;
                    salesInvoiceSummary.VesselName = saleInvoices[0].VesselName;

                    //Inserting 'Is Taxable Invoice' value
                    salesInvoiceSummary.IsTaxable = aCri.IncludeTax;

                    //Inserting 'ATTN' value
                    salesInvoiceSummary.AttnStr = aCri.AttnStr;

                    //Start Setting up Invoice Report ViewModel for Invoice PDF

                    InvoiceReportViewModel report = new InvoiceReportViewModel();
                    report.IsTaxInvoice = aCri.IncludeTax;

                    report.Date = salesInvoiceSummary.InvoiceDate.Value;

                    var company = await companyService.GetById(GlobalVariable.COMPANY_ID);
                    if (company == null) company = new ApplicationCore.Entities.Company();

                    report.HeaderLogo = company.ReportHeaderLogo;
                    report.Customer = customer.Name;

                    int count = 0;

                    try
                    {
                        //Add Invoice Details to Sales Invoice Summary

                        foreach (var salesInvoice in saleInvoices)
                        {
                            //Add Sales Invoice Summary Details

                            SalesInvoiceSummaryDetails salesInvoiceSummaryDetails = new SalesInvoiceSummaryDetails();
                            salesInvoiceSummaryDetails.SalesInvoiceId = salesInvoice.Id;

                            var workOrderTemp = await workOrderService.GetById(salesInvoice.WorkOrderId);
                            if (workOrderTemp != null) salesInvoiceSummaryDetails.CreditNoteId = workOrderTemp.CreditNoteId.Value;

                            salesInvoiceSummaryDetails.WorkOrderId = salesInvoice.WorkOrderId.Value;
                            var workOrder = workOrderTemp == null ? workOrders[count++] : workOrderTemp;
                            salesInvoiceSummaryDetails.WorkOrderId = workOrder.Id;
                            salesInvoiceSummary.Details.Add(salesInvoiceSummaryDetails);

                            var workOrderPassenger = workOrder.WorkOrderPassengerList.FirstOrDefault(x => x.InCharge == true);
                            if (workOrderPassenger == null) workOrderPassenger = new ApplicationCore.SOP.Entities.WorkOrders.WorkOrderPassenger();

                            report.Address = salesInvoice.CompanyAddress;
                            report.Vessel = salesInvoice.VesselName;
                            salesInvoice.WorkOrder = workOrder;

                            var gst = await _gstService.GetById(salesInvoice.GSTId);
                            report.TaxDescription = gst.GSTPercent.ToString();
                            List<EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice> salesInvoices = new List<EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice>();
                            //salesInvoices.Add(salesInvoice);
                            report.TotalAmount += salesInvoice.TotalAmt;
                            report.TaxAmount += salesInvoice.GSTAmount;
                            report.TotalNonTaxableAmount += salesInvoice.TotalNonTaxable;
                            report.GrandTotal += salesInvoice.TotalNetAmount;

                            InvoiceDetailsReportViewModel details = new InvoiceDetailsReportViewModel
                            {
                                WorkOrderNo = salesInvoice.WorkOrderNo + " " + workOrderPassenger.Name + " (" + workOrderPassenger.Rank + ") x" + workOrderPassenger.NoOfPax.ToString(),
                                IsHeader = true,
                            };

                            if (salesInvoice.WorkOrder.PickUpdateDate != null)
                                details.InvoiceDate = salesInvoice.WorkOrder.PickUpdateDate.Value;

                            report.Details.Add(details);

                            int currentIndex = 0;

                            foreach (var invoiceDetails in salesInvoice.Details)
                            {
                                InvoiceDetailsReportViewModel invDetail = new InvoiceDetailsReportViewModel
                                {
                                    Code = invoiceDetails.Code,
                                    Description = invoiceDetails.Description,
                                    Qty = invoiceDetails.Qty,
                                    Price = invoiceDetails.Price,
                                    TotalAmount = invoiceDetails.TotalAmt,
                                    IsTaxable = invoiceDetails.Taxable
                                };

                                //if (currentIndex == 0)
                                //{
                                //    if (details.InvoiceDate != null)
                                //        invDetail.Code += " @ " + (details.InvoiceDate.ToString("HH:mm")).Replace(":", "") + " Hr";
                                //}
                                if (report.IsTaxInvoice == false && invoiceDetails.GSTEssentials)
                                {
                                    invDetail.TotalAmount += invDetail.TotalAmount * (report.TaxPercent / 100);
                                }
                                report.Details.Add(invDetail);

                                currentIndex++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                    if (!string.IsNullOrEmpty(aCri.DNNo))
                    {
                        InvoiceDetailsReportViewModel dnNo = new InvoiceDetailsReportViewModel
                        {
                            WorkOrderNo = aCri.DNNo,
                            IsHeader = true,
                            IsDNNo = true
                        };
                        report.Details.Add(dnNo);
                    }

                    var dbsalesInvoice = await _salesInvoiceSummaryService.GetByInvoiceNo(aCri.SalesInvoiceSummaryNo);
                    if (dbsalesInvoice != null)
                    {
                        //Generating Already Existed Invoices

                        report.InvoiceNo = dbsalesInvoice.ReferenceNo;

                        salesInvoiceSummary.Id = dbsalesInvoice.Id;

                        salesInvoiceSummary.ReferenceNo = report.InvoiceNo;

                        //Inserting Year, Month and Serial No based on Reference No.
                        salesInvoiceSummary.YearMonthNo = dbsalesInvoice.YearMonthNo;
                        salesInvoiceSummary.SerialNo = dbsalesInvoice.SerialNo;

                        //Inserting Sales Invoice Summary Status
                        salesInvoiceSummary.Status = dbsalesInvoice.Status;

                        await _salesInvoiceSummaryService.Save(salesInvoiceSummary);
                    }
                    else
                    {
                        //Generating New Invoice

                        await _salesInvoiceSummaryService.Save(salesInvoiceSummary);

                        report.SetInvoiceNo(new SerialNoRepository<SalesInvoiceReportSerialNo>(db).GetSerialNoByMonth(salesInvoiceSummary.Id, report.Date));

                        salesInvoiceSummary.ReferenceNo = report.InvoiceNo;

                        //Split the ReferenceNo to YearNo, MonthNo and SerialNo
                        var tempArr = salesInvoiceSummary.ReferenceNo.Split('/');

                        salesInvoiceSummary.YearMonthNo = Convert.ToInt32(tempArr[0] + tempArr[1]);
                        salesInvoiceSummary.SerialNo = Convert.ToInt32(tempArr[2]);

                        await db.SaveChangesAsync();
                    }
                    report.CalculateNoOfPage();

                    //Setting Invoice Number of Corresponding Work Orders
                    int index = 0;

                    workOrders = workOrders.OrderBy(x => x.Created).ToList();

                    foreach (var workOrder in workOrders)
                    {
                        workOrder.SummaryInvoiceNo = report.InvoiceNo;

                        var insalesInvoice = saleInvoices[index++];

                        insalesInvoice.InvoiceNo = report.InvoiceNo;

                        if (dbsalesInvoice != null)
                        {
                            workOrder.SummaryInvoiceId = dbsalesInvoice.Id;

                            insalesInvoice.SummaryInvoiceId = dbsalesInvoice.Id;
                        }
                        else
                        {
                            workOrder.SummaryInvoiceId = salesInvoiceSummary.Id;

                            insalesInvoice.SummaryInvoiceId = salesInvoiceSummary.Id;
                        }
                    }

                    await db.SaveChangesAsync();


                    #endregion
                }


                if (String.IsNullOrEmpty(aCri.SalesInvoiceSummaryNo))
                {
                    TempData["color"] = ApplicationConfig.MESSAGE_SAVE_COLOR;
                    TempData["message"] = ApplicationConfig.MESSAGE_SAVE_MESSAGE;
                    return RedirectToAction(nameof(Index), "InvoiceSummary");
                }
                else
                {
                    TempData["color"] = ApplicationConfig.MESSAGE_SAVE_COLOR;
                    TempData["message"] = ApplicationConfig.MESSAGE_SAVE_MESSAGE;
                    return RedirectToAction(nameof(Index), new { Customers = aCri.CustomerId, Vessels = aCri.VesselId, FromDate = aCri.FromDate, ToDate = aCri.ToDate, Status = aCri.Status, SalesInvoiceSummaryNo = aCri.SalesInvoiceSummaryNo, DNNo = aCri.DNNo, InvoiceDate = aCri.InvoiceDate });
                }

            }
            catch (Exception ex)
            {
                TempData["color"] = ApplicationConfig.MESSAGE_SAVE_COLOR;
                TempData["message"] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Index), "InvoiceSummary");

            }
        }

        //Downloading Invoice without any Saving
        public async Task<ActionResult> DownloadInvoice(Int64[] workOrderIds, WorkOrderCri aCri)
        {
            #region Validation
            //Check if there are work orders selected
            if (workOrderIds == null)
            {
                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                TempData["message"] = "There is no work order under this invoice!";
                return RedirectToAction(nameof(Index), new { Customers = aCri.CustomerId, Vessels = aCri.VesselId, FromDate = aCri.FromDate, ToDate = aCri.ToDate, Status = aCri.Status, SalesInvoiceSummaryNo = aCri.SalesInvoiceSummaryNo, DNNo = aCri.DNNo, InvoiceDate = aCri.InvoiceDate });
            }

            List<WorkOrder> duplicateInvoiceWorkOrders = new List<WorkOrder>();

            duplicateInvoiceWorkOrders = await workOrderService.GetByIds(workOrderIds);

            //Check if selected work orders have more than more than one invoice number
            if (duplicateInvoiceWorkOrders.Select(x => x.ShortText2).Distinct().Count() > 1)
            {
                //Check if one work order have no invoice details yet
                if (duplicateInvoiceWorkOrders.Select(x => x.ShortText2).Distinct().Count() == 2 && duplicateInvoiceWorkOrders.Distinct().Select(x => x.ShortText2 == null).Count() > 0)
                {
                    //Check if Sales Invoice Summary text field is filled or not
                    if (String.IsNullOrEmpty(aCri.SalesInvoiceSummaryNo))
                    {
                        TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                        TempData["message"] = "Please key in the invoice number to generate an invoice with that number!";
                        return RedirectToAction(nameof(Index), new { Customers = aCri.CustomerId, Vessels = aCri.VesselId, FromDate = aCri.FromDate, ToDate = aCri.ToDate, Status = aCri.Status, SalesInvoiceSummaryNo = aCri.SalesInvoiceSummaryNo, DNNo = aCri.DNNo, InvoiceDate = aCri.InvoiceDate });
                    }
                }
                else
                {
                    TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                    TempData["message"] = "Not allowed to select multiple work orders with different invoice numbers!";
                    return RedirectToAction(nameof(Index), new { Customers = aCri.CustomerId, Vessels = aCri.VesselId, FromDate = aCri.FromDate, ToDate = aCri.ToDate, Status = aCri.Status, SalesInvoiceSummaryNo = aCri.SalesInvoiceSummaryNo, DNNo = aCri.DNNo, InvoiceDate = aCri.InvoiceDate });
                }
            }

            //Check if selected workorders are from different customers
            if (duplicateInvoiceWorkOrders.Select(x => x.CustomerId).Distinct().Count() > 1)
            {
                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                TempData["message"] = "Not allowed to select work orders with different customers!";
                return RedirectToAction(nameof(Index), new { Customers = aCri.CustomerId, Vessels = aCri.VesselId, FromDate = aCri.FromDate, ToDate = aCri.ToDate, Status = aCri.Status, SalesInvoiceSummaryNo = aCri.SalesInvoiceSummaryNo, DNNo = aCri.DNNo, InvoiceDate = aCri.InvoiceDate });
            }

            //Check if selected workorders are from different vessels
            if (duplicateInvoiceWorkOrders.Select(x => x.VesselId).Distinct().Count() > 1)
            {
                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                TempData["message"] = "Not allowed to select work orders with different vessels!";
                return RedirectToAction(nameof(Index), new { Customers = aCri.CustomerId, Vessels = aCri.VesselId, FromDate = aCri.FromDate, ToDate = aCri.ToDate, Status = aCri.Status, SalesInvoiceSummaryNo = aCri.SalesInvoiceSummaryNo, DNNo = aCri.DNNo, InvoiceDate = aCri.InvoiceDate });
            }

            #endregion

            #region Prepare GST List
            //Get GSTs
            var gsts = (await _gstService.GetByCri(null)).OrderBy(x => x.Name);

            if (gsts == null || gsts.ToList().Count == 0)
            {
                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                TempData["message"] = "No gst defined yet!";
                return RedirectToAction(nameof(Index), new { Customers = aCri.CustomerId, Vessels = aCri.VesselId, FromDate = aCri.FromDate, ToDate = aCri.ToDate, Status = aCri.Status, SalesInvoiceSummaryNo = aCri.SalesInvoiceSummaryNo });
            }
            #endregion

            #region Prepare Invoice Data
            List<EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice> saleInvoices = new List<ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice>();
            List<WorkOrder> workOrders = new List<WorkOrder>();



            //Get Corresponding Sales Invoice Details based on Work Order Ids and getting Corresponding Work Orders

            foreach (var workOrderId in workOrderIds)
            {
                var workOrder = await this.workOrderService.GetById(workOrderId);

                if (workOrder == null) continue;

                if (workOrder.CustomerId == null) continue;

                workOrders.Add(workOrder);

                workOrder.Customer = await this._customerService.GetHeaderOnly(workOrder.CustomerId.Value);
            }

            if (workOrders != null && workOrders.Count > 0)
            {
                workOrders = workOrders.OrderBy(x => x.PickUpdateDate).ToList();

                foreach (var workOrder in workOrders)
                {
                    List<Price> prices = new List<Price>();

                    if (workOrder.InvoiceId != null)
                    {
                        var salesInvoice = await _salesInvoiceService.GetById(workOrder.InvoiceId.Value);

                        if (salesInvoice != null)
                        {
                            saleInvoices.Add(salesInvoice);
                        }

                    }
                }
            }

            #endregion

            #region PDF Report Generation

            //Start Setting up Invoice Report ViewModel for Invoice PDF

            InvoiceReportViewModel report = new InvoiceReportViewModel();
            report.IsTaxInvoice = aCri.IncludeTax;
            report.AttnStr = aCri.AttnStr;

            SalesInvoiceSummary salesInvoiceSummary = new SalesInvoiceSummary();

            //Inserting Invoice Date if custom date is inserted
            salesInvoiceSummary.InvoiceDate = string.IsNullOrEmpty(aCri.InvoiceDate) ? TimeUtil.GetLocalTime() : Util.ConvertStringToDateTime(aCri.InvoiceDate, DateConfig.CULTURE);

            report.Date = salesInvoiceSummary.InvoiceDate.Value;


            var company = await companyService.GetById(GlobalVariable.COMPANY_ID);
            if (company == null) company = new ApplicationCore.Entities.Company();
            report.HeaderLogo = company.ReportHeaderLogo;

            var customer = await _customerService.GetHeaderOnly(saleInvoices[0].CustomerId);

            report.Customer = customer.Name;


            salesInvoiceSummary.CustomerName = customer.Name;
            salesInvoiceSummary.CustomerId = customer.Id;
            salesInvoiceSummary.VesselId = saleInvoices.FirstOrDefault().VesselId.Value;
            salesInvoiceSummary.VesselName = saleInvoices.FirstOrDefault().VesselName;

            int count = 0;


            try
            {
                //Add Invoice Details to Sales Invoice Summary

                foreach (var salesInvoice in saleInvoices)
                {
                    //Add Sales Invoice Summary Details

                    SalesInvoiceSummaryDetails salesInvoiceSummaryDetails = new SalesInvoiceSummaryDetails();
                    salesInvoiceSummaryDetails.SalesInvoiceId = salesInvoice.Id;

                    var workOrderTemp = await workOrderService.GetById(salesInvoice.WorkOrderId);
                    if (workOrderTemp != null) salesInvoiceSummaryDetails.CreditNoteId = workOrderTemp.CreditNoteId.Value;

                    salesInvoiceSummaryDetails.WorkOrderId = salesInvoice.WorkOrderId.Value;
                    var workOrder = workOrderTemp == null ? workOrders[count++] : workOrderTemp;
                    salesInvoiceSummaryDetails.WorkOrderId = workOrder.Id;
                    salesInvoiceSummary.Details.Add(salesInvoiceSummaryDetails);

                    var workOrderPassenger = workOrder.WorkOrderPassengerList.FirstOrDefault(x => x.InCharge == true);
                    if (workOrderPassenger == null) workOrderPassenger = new ApplicationCore.SOP.Entities.WorkOrders.WorkOrderPassenger();

                    report.Address = salesInvoice.CompanyAddress;
                    report.Vessel = salesInvoice.VesselName;
                    salesInvoice.WorkOrder = workOrder;

                    var gst = gsts.Where(x => x.Id == salesInvoice.GSTId).FirstOrDefault();

                    if (gst != null) report.TaxDescription = gst.GSTPercent.ToString();
                    List<EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice> salesInvoices = new List<EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice>();

                    report.TotalAmount += salesInvoice.TotalAmt;
                    report.TaxAmount += salesInvoice.GSTAmount;
                    report.TotalNonTaxableAmount += salesInvoice.TotalNonTaxable;
                    report.GrandTotal += salesInvoice.TotalNetAmount;

                    InvoiceDetailsReportViewModel details = new InvoiceDetailsReportViewModel
                    {
                        WorkOrderNo = salesInvoice.WorkOrderNo + " " + workOrderPassenger.Name + " (" + workOrderPassenger.Rank + ") x" + workOrderPassenger.NoOfPax.ToString(),
                        IsHeader = true,
                    };

                    if (salesInvoice.WorkOrder.PickUpdateDate != null)
                        details.InvoiceDate = salesInvoice.WorkOrder.PickUpdateDate.Value;

                    report.Details.Add(details);

                    int currentIndex = 0;

                    foreach (var invoiceDetails in salesInvoice.Details.OrderBy(x => x.SortOrder))
                    {
                        InvoiceDetailsReportViewModel invDetail = new InvoiceDetailsReportViewModel
                        {
                            Code = invoiceDetails.Code,
                            Description = invoiceDetails.Description,
                            Qty = invoiceDetails.Qty,
                            Price = invoiceDetails.Price,
                            TotalAmount = invoiceDetails.TotalAmt,
                            IsTaxable = invoiceDetails.Taxable
                        };

                        //if (currentIndex == 0)
                        //{
                        //    if (details.InvoiceDate != null)
                        //        invDetail.Code += " @ " + (details.InvoiceDate.ToString("HH:mm")).Replace(":", "") + " Hr";
                        //}
                        if (report.IsTaxInvoice == false && invoiceDetails.GSTEssentials)
                        {
                            invDetail.TotalAmount += invDetail.TotalAmount * (report.TaxPercent / 100);
                        }
                        report.Details.Add(invDetail);

                        currentIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            //Include Vessel in front of company name if necessary
            if (aCri.IncludeVesselInFrontOfCompanyName) report.needsVesselNameInFrontCompanyName = true;

            if (!string.IsNullOrEmpty(aCri.DNNo))
            {
                InvoiceDetailsReportViewModel dnNo = new InvoiceDetailsReportViewModel
                {
                    WorkOrderNo = aCri.DNNo,
                    IsHeader = true,
                    IsDNNo = true
                };
                report.Details.Add(dnNo);
            }

            var dbsalesInvoice = await _salesInvoiceSummaryService.GetByInvoiceNo(aCri.SalesInvoiceSummaryNo);
            if (dbsalesInvoice != null)
            {
                //Generating Already Existed Invoices

                report.InvoiceNo = dbsalesInvoice.ReferenceNo;

                salesInvoiceSummary.Id = dbsalesInvoice.Id;

                salesInvoiceSummary.ReferenceNo = report.InvoiceNo;

                //await _salesInvoiceSummaryService.Save(salesInvoiceSummary);
            }

            //Calculate Number of lines
            report.CalculateTotalDetailsLines();

            //Calculate Number of pages
            report.CalculateNoOfPage();

            //Setting Invoice Number of Corresponding Work Orders
            int index = 0;

            workOrders = workOrders.OrderBy(x => x.Created).ToList();

            foreach (var workOrder in workOrders)
            {
                workOrder.SummaryInvoiceNo = report.InvoiceNo;

                var insalesInvoice = saleInvoices[index++];

                insalesInvoice.InvoiceNo = report.InvoiceNo;

                if (dbsalesInvoice != null)
                {
                    workOrder.SummaryInvoiceId = dbsalesInvoice.Id;

                    insalesInvoice.SummaryInvoiceId = dbsalesInvoice.Id;
                }
                else
                {
                    workOrder.SummaryInvoiceId = salesInvoiceSummary.Id;

                    insalesInvoice.SummaryInvoiceId = salesInvoiceSummary.Id;
                }
            }

            //await db.SaveChangesAsync();

            if (aCri.IsExcel)
            {
                var excelTemplatePath = Server.MapPath("~/App_Data/Templates/Excels/Invoice.xlsx");
                var excelFilePath = new ExcelProcessor<InvoiceDetailsReportViewModel>().GenerateInvoice(report, excelTemplatePath, Server.MapPath(FileConfig.EXPORT_EXCEL_INVOICES));
                return base.File(excelFilePath, CONTENT_DISPOSITION, "INV_" + report.InvoiceNo.Replace('/', '_') + ".xlsx");
            }
            else
            {
                var pdfFilePath = base.GeneratePDF<InvoiceReportViewModel>(report, FileConfig.INVOICES, "Inv_" + report.InvoiceNo.Replace('/', '_'));
                PDFProcessor pDFProcessor = new PDFProcessor();
                var excelPath = pDFProcessor.ConvertToExcel(pdfFilePath, Server.MapPath(FileConfig.EXPORT_EXCEL_INVOICES));
                return base.File(pdfFilePath, CONTENT_DISPOSITION, "INV_" + report.InvoiceNo.Replace('/', '_') + ".pdf");
            }

            #endregion
        }

        #region Obsolete Generate Invoice - Aung Ye Kaung - 20190430
        //public async Task<ActionResult> GenerateInvoice(Int64[] workOrderIds, WorkOrderCri aCri)
        //{
        //    #region Validation
        //    //Check if there are work orders selected
        //    if (workOrderIds == null)
        //    {
        //        TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
        //        TempData["message"] = "There is no work order selected!";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    List<WorkOrder> duplicateInvoiceWorkOrders = new List<WorkOrder>();

        //    duplicateInvoiceWorkOrders = await workOrderService.GetByIds(workOrderIds);

        //    //Check if selected work orders have more than more than one invoice number
        //    if (duplicateInvoiceWorkOrders.Select(x => x.ShortText2).Distinct().Count() > 1)
        //    {
        //        //Check if one work order have no invoice details yet
        //        if (duplicateInvoiceWorkOrders.Select(x => x.ShortText2).Distinct().Count() == 2 && duplicateInvoiceWorkOrders.Distinct().Select(x => x.ShortText2 == null).Count() > 0)
        //        {
        //            //Check if Sales Invoice Summary text field is filled or not
        //            if (String.IsNullOrEmpty(aCri.SalesInvoiceSummaryNo))
        //            {
        //                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
        //                TempData["message"] = "Please key in the invoice number to generate an invoice with that number!";
        //                return RedirectToAction(nameof(Index));
        //            }
        //        }
        //        else
        //        {
        //            TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
        //            TempData["message"] = "Not allowed to select multiple work orders with different invoice numbers!";
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }

        //    //Check if selected workorders are from different customers
        //    if (duplicateInvoiceWorkOrders.Select(x => x.CustomerId).Distinct().Count() > 1)
        //    {
        //        TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
        //        TempData["message"] = "Not allowed to select work orders with different customers!";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    //Check if selected workorders are from different vessels
        //    if (duplicateInvoiceWorkOrders.Select(x => x.VesselId).Distinct().Count() > 1)
        //    {
        //        TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
        //        TempData["message"] = "Not allowed to select work orders with different vessels!";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    #endregion

        //    #region Prepare GST List
        //    //Get GSTs
        //    var gsts = (await _gstService.GetByCri(null)).OrderBy(x => x.Name);

        //    if (gsts == null || gsts.ToList().Count == 0)
        //    {
        //        TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
        //        TempData["message"] = "No gst defined yet!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    #endregion

        //    #region Prepare Invoice Data
        //    List<EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice> saleInvoices = new List<ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice>();
        //    List<WorkOrder> workOrders = new List<WorkOrder>();


        //    //Get Corresponding Sales Invoice Details based on Work Order Ids and getting Corresponding Work Orders

        //    foreach (var workOrderId in workOrderIds)
        //    {
        //        var workOrder = await this.workOrderService.GetById(workOrderId);

        //        if (workOrder == null) continue;

        //        if (workOrder.CustomerId == null) continue;

        //        workOrders.Add(workOrder);

        //        workOrder.Customer = await this._customerService.GetHeaderOnly(workOrder.CustomerId.Value);
        //    }

        //    if (workOrders != null && workOrders.Count > 0)
        //    {
        //        workOrders = workOrders.OrderBy(x => x.PickUpdateDate).ToList();

        //        foreach (var workOrder in workOrders)
        //        {
        //            List<Price> prices = new List<Price>();

        //            if (workOrder.InvoiceId == null)
        //            {
        //                //Generate Invoicing Details and Credit Note Details if there is no Invoice yet for the Work Order

        //                EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice saleInvoice = new ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice();

        //                await _salesInvoiceService.PreparePriceListFromTransferVoucher(workOrder, prices);
        //                await _salesInvoiceService.PreparePriceListForMeetingService(workOrder, prices);
        //                await _salesInvoiceService.PreparePriceList(workOrder, prices);

        //                saleInvoice.GenerateInvoice(workOrder, prices, gsts.FirstOrDefault());
        //                saleInvoices.Add(saleInvoice);
        //                saleInvoice.WorkOrderId = workOrder.Id;
        //                await _salesInvoiceService.Save(saleInvoice);
        //            }
        //            else
        //            {
        //                //Adding the Invoicing Detail to the details list if the Invoicing Detail exists

        //                var salesInvoice = await _salesInvoiceService.GetById(workOrder.InvoiceId.Value);

        //                if (salesInvoice != null)
        //                {

        //                    await _salesInvoiceService.PreparePriceListFromTransferVoucher(workOrder, prices);
        //                    await _salesInvoiceService.PreparePriceListForMeetingService(workOrder, prices);
        //                    await _salesInvoiceService.PreparePriceList(workOrder, prices);
        //                    Int64 invoiceId = await _salesInvoiceService.UpdatePriceList(salesInvoice, prices);

        //                    if (invoiceId != 0)
        //                    {
        //                        salesInvoice = await _salesInvoiceService.GetById(invoiceId);
        //                    }
        //                }

        //                //if (salesInvoice.Details.Count == 0)
        //                //{
        //                //    await _salesInvoiceService.PreparePriceListFromTransferVoucher(workOrder, prices);
        //                //    await _salesInvoiceService.PreparePriceListForMeetingService(workOrder, prices);
        //                //    await _salesInvoiceService.PreparePriceList(workOrder, prices);
        //                //    Int64 invoiceId = await _salesInvoiceService.UpdatePriceList(salesInvoice, prices);

        //                //    salesInvoice = await _salesInvoiceService.GetById(invoiceId);
        //                //}

        //                saleInvoices.Add(salesInvoice);
        //            }
        //        }
        //    }

        //    #endregion

        //    #region PDF Report Generation

        //    //Start Setting up Invoice Report ViewModel for Invoice PDF

        //    InvoiceReportViewModel report = new InvoiceReportViewModel();
        //    report.IsTaxInvoice = aCri.IncludeTax;

        //    SalesInvoiceSummary salesInvoiceSummary = new SalesInvoiceSummary();

        //    //Inserting Invoice Date if custom date is inserted
        //    salesInvoiceSummary.InvoiceDate = string.IsNullOrEmpty(aCri.InvoiceDate) ? TimeUtil.GetLocalTime() : Util.ConvertStringToDateTime(aCri.InvoiceDate, DateConfig.CULTURE);

        //    report.Date = salesInvoiceSummary.InvoiceDate.Value;


        //    var company = await companyService.GetById(GlobalVariable.COMPANY_ID);
        //    if (company == null) company = new ApplicationCore.Entities.Company();

        //    var customer = await _customerService.GetHeaderOnly(saleInvoices[0].CustomerId);
        //    int count = 0;

        //    salesInvoiceSummary.CustomerName = customer.Name;
        //    salesInvoiceSummary.CustomerId = customer.Id;


        //    salesInvoiceSummary.VesselId = saleInvoices.FirstOrDefault().VesselId.Value;
        //    salesInvoiceSummary.VesselName = saleInvoices.FirstOrDefault().VesselName;



        //    report.HeaderLogo = company.ReportHeaderLogo;
        //    report.Customer = customer.Name;



        //    try
        //    {
        //        //Add Invoice Details to Sales Invoice Summary

        //        foreach (var salesInvoice in saleInvoices)
        //        {
        //            //Add Sales Invoice Summary Details

        //            SalesInvoiceSummaryDetails salesInvoiceSummaryDetails = new SalesInvoiceSummaryDetails();
        //            salesInvoiceSummaryDetails.SalesInvoiceId = salesInvoice.Id;

        //            var workOrderTemp = await workOrderService.GetById(salesInvoice.WorkOrderId);
        //            if (workOrderTemp != null) salesInvoiceSummaryDetails.CreditNoteId = workOrderTemp.CreditNoteId.Value;

        //            salesInvoiceSummaryDetails.WorkOrderId = salesInvoice.WorkOrderId.Value;
        //            var workOrder = workOrderTemp == null ? workOrders[count++] : workOrderTemp;
        //            salesInvoiceSummaryDetails.WorkOrderId = workOrder.Id;
        //            salesInvoiceSummary.Details.Add(salesInvoiceSummaryDetails);

        //            var workOrderPassenger = workOrder.WorkOrderPassengerList.FirstOrDefault(x => x.InCharge == true);
        //            if (workOrderPassenger == null) workOrderPassenger = new ApplicationCore.SOP.Entities.WorkOrders.WorkOrderPassenger();

        //            report.Address = salesInvoice.CompanyAddress;
        //            report.Vessel = salesInvoice.VesselName;
        //            salesInvoice.WorkOrder = workOrder;

        //            var gst = await _gstService.GetById(salesInvoice.GSTId);
        //            report.TaxDescription = gst.GSTPercent.ToString();
        //            List<EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice> salesInvoices = new List<EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice>();
        //            //salesInvoices.Add(salesInvoice);
        //            report.TotalAmount += salesInvoice.TotalAmt;
        //            report.TaxAmount += salesInvoice.GSTAmount;
        //            report.TotalNonTaxableAmount += salesInvoice.TotalNonTaxable;
        //            report.GrandTotal += salesInvoice.TotalNetAmount;

        //            InvoiceDetailsReportViewModel details = new InvoiceDetailsReportViewModel
        //            {
        //                WorkOrderNo = salesInvoice.WorkOrderNo + " " + workOrderPassenger.Name + " (" + workOrderPassenger.Rank + ") x" + workOrderPassenger.NoOfPax.ToString(),
        //                IsHeader = true,
        //            };

        //            if (salesInvoice.WorkOrder.PickUpdateDate != null)
        //                details.InvoiceDate = salesInvoice.WorkOrder.PickUpdateDate.Value;

        //            report.Details.Add(details);

        //            int currentIndex = 0;

        //            foreach (var invoiceDetails in salesInvoice.Details)
        //            {
        //                InvoiceDetailsReportViewModel invDetail = new InvoiceDetailsReportViewModel
        //                {
        //                    Code = invoiceDetails.Code,
        //                    Description = invoiceDetails.Description,
        //                    Qty = invoiceDetails.Qty,
        //                    Price = invoiceDetails.Price,
        //                    TotalAmount = invoiceDetails.TotalAmt
        //                };

        //                if (currentIndex == 0)
        //                {
        //                    if (details.InvoiceDate != null)
        //                        invDetail.Code += " @ " + (details.InvoiceDate.ToString("HH:mm")).Replace(":", "") + " Hr";
        //                }
        //                if (report.IsTaxInvoice == false && invoiceDetails.GSTEssentials)
        //                {
        //                    invDetail.TotalAmount += invDetail.TotalAmount * (report.TaxPercent / 100);
        //                }
        //                report.Details.Add(invDetail);

        //                currentIndex++;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.Message;
        //    }
        //    if (!string.IsNullOrEmpty(aCri.DNNo))
        //    {
        //        InvoiceDetailsReportViewModel dnNo = new InvoiceDetailsReportViewModel
        //        {
        //            WorkOrderNo = aCri.DNNo,
        //            IsHeader = true,
        //            IsDNNo = true
        //        };
        //        report.Details.Add(dnNo);
        //    }

        //    var dbsalesInvoice = await _salesInvoiceSummaryService.GetByInvoiceNo(aCri.SalesInvoiceSummaryNo);
        //    if (dbsalesInvoice != null)
        //    {
        //        //Generating Already Existed Invoices

        //        report.InvoiceNo = dbsalesInvoice.ReferenceNo;

        //        salesInvoiceSummary.Id = dbsalesInvoice.Id;

        //        salesInvoiceSummary.ReferenceNo = report.InvoiceNo;

        //        await _salesInvoiceSummaryService.Save(salesInvoiceSummary);
        //    }
        //    else
        //    {
        //        //Generating New Invoice

        //        salesInvoiceSummary.DNNo = aCri.DNNo;

        //        await _salesInvoiceSummaryService.Save(salesInvoiceSummary);

        //        report.SetInvoiceNo(new SerialNoRepository<SalesInvoiceReportSerialNo>(db).GetSerialNoByMonth(salesInvoiceSummary.Id, report.Date));

        //        salesInvoiceSummary.ReferenceNo = report.InvoiceNo;

        //        await db.SaveChangesAsync();
        //    }
        //    report.CalculateNoOfPage();

        //    //Setting Invoice Number of Corresponding Work Orders
        //    int index = 0;

        //    workOrders = workOrders.OrderBy(x => x.Created).ToList();

        //    foreach (var workOrder in workOrders)
        //    {
        //        workOrder.SummaryInvoiceNo = report.InvoiceNo;

        //        var insalesInvoice = saleInvoices[index++];

        //        insalesInvoice.InvoiceNo = report.InvoiceNo;

        //        if (dbsalesInvoice != null)
        //        {
        //            workOrder.SummaryInvoiceId = dbsalesInvoice.Id.ToString();

        //            insalesInvoice.SummaryInvoiceId = dbsalesInvoice.Id.ToString();
        //        }
        //        else
        //        {
        //            workOrder.SummaryInvoiceId = salesInvoiceSummary.Id.ToString();

        //            insalesInvoice.SummaryInvoiceId = salesInvoiceSummary.Id.ToString();
        //        }
        //    }

        //    await db.SaveChangesAsync();

        //    var pdfFilePath = base.GeneratePDF<InvoiceReportViewModel>(report, FileConfig.INVOICES, "Inv_" + report.InvoiceNo.Replace('/', '_'));

        //    PDFProcessor pDFProcessor = new PDFProcessor();
        //    var excelPath = pDFProcessor.ConvertToExcel(pdfFilePath, Server.MapPath(FileConfig.EXPORT_EXCEL_INVOICES));
        //    return base.File(pdfFilePath, CONTENT_DISPOSITION, "INV_" + report.InvoiceNo.Replace('/', '_') + ".pdf");
        //    #endregion

        //}
        #endregion
    }
}