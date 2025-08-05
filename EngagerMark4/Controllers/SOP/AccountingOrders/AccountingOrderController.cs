using EngagerMark4.ApplicationCore.Account.IService;
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
using System.Web;
using System.Web.Mvc;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;

namespace EngagerMark4.Controllers.SOP.AccountingOrders
{
    public class AccountingOrderController : BaseController<WorkOrderCri, WorkOrder, IWorkOrderService>
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

        public AccountingOrderController(IWorkOrderService service,
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
            var invoiceDate = Request["InvoiceDate"];
            var drivers = Request["DriverId"];
            var orderPage = Request["CurrentPage"];
            var orderColumn = Request["Column"];
            var orderOrderBy = Request["OrderBy"];
            var orderDataType = Request["DataType"];
            ViewBag.CurrentId = Request["CurrentId"];

            ViewBag.CustomerId = customerIdStr;
            ViewBag.VesselId = vesselIdStr;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.Status = status;
            ViewBag.SalesInvoiceSummaryNo = invoiceNo;
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
            var orderPage = Request["CurrentPage"];
            string orderColumn = Request["Column"];
            string orderOrderBy = Request["OrderBy"];
            string orderDataType = Request["DataType"];
            string currentId = Request["CurrentId"];

            ViewBag.orderPage = orderPage;
            ViewBag.orderColumn = orderColumn;
            ViewBag.orderOrderBy = orderOrderBy;
            ViewBag.orderDataType = orderDataType;
            ViewBag.CurrentId = currentId;

            var customers = (await this._customerService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.Customers = new SelectList(customers, "Id", "Name", customerId);
            CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            configurationCri.Includes = new List<string>();
            configurationCri.Includes.Add("ConfigurationGroup");
            var vessels = (await _commonConfigService.GetByCri(configurationCri)).Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.VesselId.ToString())).OrderBy(x => x.Name);
            ViewBag.Vessels = new SelectList(vessels, "Id", "Name", vesselId);
            var drivers = _userService.GetDrivers();
            ViewBag.DriverId = new SelectList(drivers, "Id", "Name", aCri.DriverId);
            string fromDate = Request["FromDate"];
            string toDate = Request["ToDate"];
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.Status = new SelectList(WorkOrderCri.GetOrderStatusesForAccounting(), "Id", "Name");
            var salesInvoiceSummary = await _salesInvoiceSummaryService.GetByInvoiceNo(aCri.SalesInvoiceSummaryNo);
            if (salesInvoiceSummary != null)
            {
                aCri.DNNo = salesInvoiceSummary.DNNo;
                aCri.InvoiceDate = salesInvoiceSummary.InvoiceDateStr;
            }
        }

        protected async override Task<IEnumerable<WorkOrder>> GetData(WorkOrderCri cri)
        {
            cri.IsComeFromAccount = true;
            ViewBag.SalesInvoiceNo = cri.SalesInvoiceSummaryNo;

            //if (string.IsNullOrEmpty(cri.SalesInvoiceSummaryNo))
            //{
                IEnumerable<WorkOrder> workOrders = await base.GetData(cri);
                if (cri.DriverId != 0)
                {
                    var driver = await _userService.GetById(cri.DriverId);
                    if (driver != null)
                    {
                        workOrders = workOrders.Where(x => x.Drivers.ToLower().Contains(driver.Name.ToLower()));
                    }
                }
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
                cri = GetCri();

                var cri2 = GetOrderBy(cri);

                var entities = await GetData(cri2);

                aCri.NoOfPage = 200;

                return entities.ToPagedList(aCri.CurrentPage, aCri.NoOfPage);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }

        public async Task<ActionResult> GenerateInvoice(Int64[] workOrderIds, WorkOrderCri aCri)
        {
            #region Validation
            //Check if there are work orders selected
            if (workOrderIds == null)
            {
                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                TempData["message"] = "There is no work order selected!";
                return RedirectToAction(nameof(Index));
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
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                    TempData["message"] = "Not allowed to select multiple work orders with different invoice numbers!";
                    return RedirectToAction(nameof(Index));
                }
            }

            //Check if selected workorders are from different customers
            if (duplicateInvoiceWorkOrders.Select(x => x.CustomerId).Distinct().Count() > 1)
            {
                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                TempData["message"] = "Not allowed to select work orders with different customers!";
                return RedirectToAction(nameof(Index));
            }

            //Check if selected workorders are from different vessels
            if (duplicateInvoiceWorkOrders.Select(x => x.VesselId).Distinct().Count() > 1)
            {
                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                TempData["message"] = "Not allowed to select work orders with different vessels!";
                return RedirectToAction(nameof(Index));
            }

            #endregion

            #region Prepare GST List
            //Get GSTs
            var gsts = (await _gstService.GetByCri(null)).OrderBy(x => x.Name);

            if (gsts == null || gsts.ToList().Count == 0)
            {
                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                TempData["message"] = "No gst defined yet!";
                return RedirectToAction(nameof(Index));
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

            if(workOrders != null && workOrders.Count > 0)
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

                        saleInvoice.GenerateInvoiceDetail(workOrder, prices, gsts.FirstOrDefault());
                        saleInvoices.Add(saleInvoice);
                        saleInvoice.WorkOrderId = workOrder.Id;
                        await _salesInvoiceService.Save(saleInvoice);
                    }
                    else
                    {
                        //Adding the Invoicing Detail to the details list if the Invoicing Detail exists

                        var salesInvoice = await _salesInvoiceService.GetById(workOrder.InvoiceId.Value);

                        if (salesInvoice != null)
                        {

                            await _salesInvoiceService.PreparePriceListFromTransferVoucher(workOrder, prices);
                            await _salesInvoiceService.PreparePriceListForMeetingService(workOrder, prices);
                            await _salesInvoiceService.PreparePriceList(workOrder, prices);
                            Int64 invoiceId = await _salesInvoiceService.UpdatePriceList(salesInvoice, prices);

                            if (invoiceId != 0)
                            {
                                salesInvoice = await _salesInvoiceService.GetById(invoiceId);
                            }
                        }

                        //if (salesInvoice.Details.Count == 0)
                        //{
                        //    await _salesInvoiceService.PreparePriceListFromTransferVoucher(workOrder, prices);
                        //    await _salesInvoiceService.PreparePriceListForMeetingService(workOrder, prices);
                        //    await _salesInvoiceService.PreparePriceList(workOrder, prices);
                        //    Int64 invoiceId = await _salesInvoiceService.UpdatePriceList(salesInvoice, prices);

                        //    salesInvoice = await _salesInvoiceService.GetById(invoiceId);
                        //}

                        saleInvoices.Add(salesInvoice);
                    }
                }
            }            

            #endregion

            #region PDF Report Generation

            //Start Setting up Invoice Report ViewModel for Invoice PDF

            InvoiceReportViewModel report = new InvoiceReportViewModel();
            report.IsTaxInvoice = aCri.IncludeTax;

            SalesInvoiceSummary salesInvoiceSummary = new SalesInvoiceSummary();

            //Inserting Invoice Date if custom date is inserted
            salesInvoiceSummary.InvoiceDate = string.IsNullOrEmpty(aCri.InvoiceDate) ? TimeUtil.GetLocalTime() : Util.ConvertStringToDateTime(aCri.InvoiceDate, DateConfig.CULTURE);
            if(saleInvoices != null && saleInvoices.Count > 0) salesInvoiceSummary.CustomerId = saleInvoices.FirstOrDefault().CustomerId;
            report.Date = salesInvoiceSummary.InvoiceDate.Value;


            var company = await companyService.GetById(GlobalVariable.COMPANY_ID);
            if (company == null) company = new ApplicationCore.Entities.Company();

            var customer = await _customerService.GetHeaderOnly(saleInvoices[0].CustomerId);
            int count = 0;

            salesInvoiceSummary.CustomerName = customer.Name;

            report.HeaderLogo = company.ReportHeaderLogo;
            report.Customer = customer.Name;


            
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
                            TotalAmount = invoiceDetails.TotalAmt
                        };

                        if (currentIndex == 0)
                        {
                            //if (details.InvoiceDate != null)
                            //    invDetail.Code += " @ " + (details.InvoiceDate.ToString("HH:mm")).Replace(":", "") + " Hr";
                        }
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

                await _salesInvoiceSummaryService.Save(salesInvoiceSummary);
            }
            else
            {
                //Generating New Invoice

                salesInvoiceSummary.DNNo = aCri.DNNo;

                await _salesInvoiceSummaryService.Save(salesInvoiceSummary);

                report.SetInvoiceNo(new SerialNoRepository<SalesInvoiceReportSerialNo>(db).GetSerialNoByMonth(salesInvoiceSummary.Id, report.Date));

                salesInvoiceSummary.ReferenceNo = report.InvoiceNo;

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

            var pdfFilePath = base.GeneratePDF<InvoiceReportViewModel>(report, FileConfig.INVOICES, "Inv_" + report.InvoiceNo.Replace('/', '_'));

            PDFProcessor pDFProcessor = new PDFProcessor();
            var excelPath = pDFProcessor.ConvertToExcel(pdfFilePath, Server.MapPath(FileConfig.EXPORT_EXCEL_INVOICES));
            return base.File(pdfFilePath, CONTENT_DISPOSITION, "INV_" + report.InvoiceNo.Replace('/', '_') + ".pdf");
            #endregion

        }

        [HttpPost]
        public async Task<ActionResult> MoveToBill(Int64[] workOrderIds)
        {
            try
            {
                await _service.MovetoBill(workOrderIds);
                return Content("success");
            }
            catch(NotSupportedException ex)
            {
                return Content("noInvoiceException");
            }
            catch(Exception ex)
            {
                return Content("failure: " + ex.Message);
            }
        }
    }
}