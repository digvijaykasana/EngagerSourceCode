using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.Job.IService;
using EngagerMark4.ApplicationCore.Dummy.IService.MeetingServices;
using EngagerMark4.ApplicationCore.ReportViewModels;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IService.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IService.Jobs;
using EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using EngagerMark4.Common;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Controllers.Api;
using EngagerMark4.Controllers.SOP.WorkOrders;
using EngagerMark4.DocumentProcessor;
using EngagerMark4.Hubs.Services;
using EngagerMark4.Infrasturcture.MobilePushNotifications.FCM;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;

namespace EngagerMark4.Controllers.SOP.BilledOrders
{
    public class BilledOrdersController : BaseController<WorkOrderCri, WorkOrder, IWorkOrderService>
    {
        IRolePermissionService _rolePermissionService;
        ICustomerService _customerService;
        ICommonConfigurationService _configurationService;
        IUserService _userService;
        ILocationService _locationService;
        IVehicleService _vehicelService;
        IMeetingServiceService _meetingService;
        IChecklistTemplateService _checklistTemplateService;
        ICheckListService _checkListService;
        IServiceJobService _serviceJobService;
        ISalesInvoiceService _salesInvoiceService;
        INotificationRepository _notificationRepository;
        ISalesInvoiceSummaryService _salesInvoiceSummaryService;
        IHotelService _hotelService;
        FCMSender _fmcSender;
        ISystemSettingRepository _systemSettingRepository;
        ICompanyService _companyService;
        IGSTService _gstService;

        ICreditNoteService _creditNoteService;

        List<Audit> audits = new List<Audit>();


        public BilledOrdersController(IWorkOrderService service,
            IRolePermissionService rolePermissionService,
            ICustomerService customerService,
            ICommonConfigurationService configurationService,
            IUserService userService,
            ILocationService locationService,
            IVehicleService vehicleService,
            IMeetingServiceService meetingService,
            IChecklistTemplateService checklistTemplateService,
            ICheckListService checkListService,
            IServiceJobService serviceJobService,
            ISalesInvoiceService salesInvoiceService,
            INotificationRepository notificationRepository,
            ISalesInvoiceSummaryService salesInvoiceSummaryService,
            IHotelService hotelService,
            ISystemSettingRepository systemSettingRepository,
            FCMSender fcmSender,
            ICompanyService companyService,
            IGSTService gstService,
            ICreditNoteService creditNoteService) : base(service)
        {
            _defaultColumn = "ReferenceNoNumber";
            _defaultOrderBy = BaseCri.EntityOrderBy.Dsc.ToString();
            _defaultDataType = BaseCri.DataType.Int64.ToString();
            this._rolePermissionService = rolePermissionService;
            this._customerService = customerService;
            this._configurationService = configurationService;
            this._userService = userService;
            this._locationService = locationService;
            this._vehicelService = vehicleService;
            this._meetingService = meetingService;
            this._checklistTemplateService = checklistTemplateService;
            this._checkListService = checkListService;
            this._serviceJobService = serviceJobService;
            this._salesInvoiceService = salesInvoiceService;
            this._notificationRepository = notificationRepository;
            this._salesInvoiceSummaryService = salesInvoiceSummaryService;
            this._hotelService = hotelService;
            this._fmcSender = fcmSender;
            this._systemSettingRepository = systemSettingRepository;
            this._companyService = companyService;
            this._gstService = gstService;
            this._creditNoteService = creditNoteService;
        }

        #region Load References

        public async override Task<ActionResult> List(WorkOrderCri aCri)
        {
            // ViewBag.OrderBy = aCri.OrderBy;

            _column = Request["Column"];
            _orderBy = Request["OrderBy"];
            _dataType = Request["DataType"];

            _column = string.IsNullOrEmpty(_column) ? _defaultColumn : _column;
            _orderBy = string.IsNullOrEmpty(_orderBy) ? _defaultOrderBy : _orderBy;
            _dataType = string.IsNullOrEmpty(_dataType) ? _defaultDataType : _dataType;

            ViewBag.Column = _column;
            ViewBag.OrderBy = _orderBy;
            ViewBag.DataType = _dataType;

            cri = aCri;

            var entities = await GetEntities(aCri);

            var entity = RouteData.Values["controller"].ToString().Replace("Controller", string.Empty);

            Audit audit = new Audit
            {
                Description = $"{entity} list page {aCri.CurrentPage} accessed",
                StartProcessingTime = TimeUtil.GetLocalTime(),
                Type = Audit.AuditType.Normal
            };

            audits.Add(audit);

            return PartialView(entities);
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
            //var status = "100";
            //var statusInt = WorkOrder.OrderStatus.Billed;
            var invoiceNo = Request["SalesInvoiceSummaryNo"];
            var startingInvoiceNo = Request["StartingSalesInvoiceSummaryNo"];
            var endingInvoiceNo = Request["EndingSalesInvoiceSummaryNo"];
            var isSearchByRange = Request["isSearchByRange"];
            var invoiceDate = Request["InvoiceDate"];
            var drivers = Request["DriverId"];
            var orderPage = Request["CurrentPage"];
            var orderColumn = Request["Column"];
            var orderOrderBy = Request["OrderBy"];
            var orderDataType = Request["DataType"];
            ViewBag.CurrentId = Request["CurrentId"];

            ViewBag.CustomerId = customerIdStr;
            ViewBag.VesselId = vesselIdStr;


            //if (String.IsNullOrEmpty(fromDate))
            //{
            //    fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
            //}

            ViewBag.FromDate = fromDate;

            //if (String.IsNullOrEmpty(toDate))
            //{
            //    toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");
            //}

            ViewBag.ToDate = toDate;
            ViewBag.Status = status;
            ViewBag.SalesInvoiceSummaryNo = invoiceNo;
            ViewBag.StartingSalesInvoiceSummaryNo = startingInvoiceNo;
            ViewBag.EndingSalesInvoiceSummaryNo = endingInvoiceNo;
            ViewBag.isSearchByRange = isSearchByRange;
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


            cri.InvoiceDate = invoiceDate;

            cri.SalesInvoiceSummaryNo = invoiceNo;

            cri.SearchByRange = Convert.ToBoolean(isSearchByRange);

            if (cri.SearchByRange)
            {
                cri.SalesInvoiceSummaryStartingNo = startingInvoiceNo;

                if (!String.IsNullOrEmpty(cri.SalesInvoiceSummaryStartingNo))
                {
                    var tempArr = cri.SalesInvoiceSummaryStartingNo.Split('/');

                    cri.StartingRefYearMonth = Convert.ToInt32(tempArr[0] + tempArr[1]);
                    cri.StartingRefSerial = Convert.ToInt32(tempArr[2]);
                }

                cri.SalesInvoiceSummaryEndingNo = endingInvoiceNo;

                if (!String.IsNullOrEmpty(cri.SalesInvoiceSummaryEndingNo))
                {
                    var tempArr = cri.SalesInvoiceSummaryEndingNo.Split('/');

                    cri.EndingRefYearMonth = Convert.ToInt32(tempArr[0] + tempArr[1]);
                    cri.EndingRefSerial = Convert.ToInt32(tempArr[2]);
                }

            }

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
            var invoiceNo = Request["SalesInvoiceSummaryNo"];
            var startingInvoiceNo = Request["StartingSalesInvoiceSummaryNo"];
            var endingInvoiceNo = Request["SalesInvoiceSummaryNo"];
            var isSearchByRange = Request["isSearchByRange"];

            var customers = (await this._customerService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.Customers = new SelectList(customers, "Id", "Name", customerId);
            CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            configurationCri.Includes = new List<string>();
            configurationCri.Includes.Add("ConfigurationGroup");
            var vessels = (await _configurationService.GetByCri(configurationCri)).Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.VesselId.ToString())).OrderBy(x => x.Name);
            ViewBag.Vessels = new SelectList(vessels, "Id", "Name", vesselId);

            var drivers = _userService.GetDrivers();
            ViewBag.DriverId = new SelectList(drivers, "Id", "Name", aCri.DriverId);

            string fromDate = Request["FromDate"];
            string toDate = Request["ToDate"];

            if (String.IsNullOrEmpty(fromDate))
            {
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
            }

            ViewBag.FromDate = fromDate;

            if (String.IsNullOrEmpty(toDate))
            {
                toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");
            }

            ViewBag.ToDate = toDate;
            ViewBag.Status = new SelectList(WorkOrderCri.GetOrderStatusesForBilled(), "Id", "Name");
            //ViewBag.ActionThings = new SelectList(WorkOr.)
            var salesInvoiceSummary = await _salesInvoiceSummaryService.GetByInvoiceNo(aCri.SalesInvoiceSummaryNo);
            if (salesInvoiceSummary != null)
            {
                aCri.DNNo = salesInvoiceSummary.DNNo;
            }

            ViewBag.SalesInvoiceSummaryNo = invoiceNo;
            ViewBag.StartingSalesInvoiceSummaryNo = startingInvoiceNo;
            ViewBag.EndingSalesInvoiceSummaryNo = endingInvoiceNo;
            ViewBag.isSearchByRange = isSearchByRange;


            if (!String.IsNullOrEmpty(aCri.SalesInvoiceSummaryStartingNo))
            {
                var tempArr = aCri.SalesInvoiceSummaryStartingNo.Split('/');

                aCri.StartingRefYearMonth = Convert.ToInt32(tempArr[0] + tempArr[1]);
                aCri.StartingRefSerial = Convert.ToInt32(tempArr[2]);
            }

            if (!String.IsNullOrEmpty(aCri.SalesInvoiceSummaryEndingNo))
            {
                var tempArr = aCri.SalesInvoiceSummaryEndingNo.Split('/');

                aCri.EndingRefYearMonth = Convert.ToInt32(tempArr[0] + tempArr[1]);
                aCri.EndingRefSerial = Convert.ToInt32(tempArr[2]);
            }

            aCri.NoOfPage = 200;
        }

        protected async Task<IEnumerable<BilledOrderListInvoiceViewModel>> GetData(WorkOrderCri cri)
        {
            cri.IsComeFromAccount = true;

            IEnumerable<BilledOrderListInvoiceViewModel> invoices = await _service.GetDataForBilledOrderList(cri);

            return invoices;
        }

        protected async Task<List<BilledOrderListInvoiceViewModel>> GetEntities(WorkOrderCri aCri)
        {
            try
            {
                cri = GetCri();

                var cri2 = GetOrderBy(cri);

                var entities = await GetData(cri2);

                aCri.NoOfPage = 200;

                var invoices = entities.ToList();

                return invoices.ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }

        protected async override Task LoadReferences(WorkOrder entity)
        {
            if (entity == null) entity = new WorkOrder();

            CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            configurationCri.Includes = new List<string>();
            configurationCri.Includes.Add("ConfigurationGroup");
            var configurations = await _configurationService.GetByCri(configurationCri);

            var customers = (await _customerService.GetByCri(null)).OrderBy(x => x.Name);

            ViewBag.CustomerId = new SelectList(customers.OrderBy(x => x.Name), "Id", "Name", entity.CustomerId);

            var customer = customers.FirstOrDefault(x => x.Id == entity.CustomerId);

            List<EngagerMark4.ApplicationCore.Entities.Users.User> agents = (await _userService.GetByCustomerId(entity.CustomerId == null ? 0 : entity.CustomerId.Value)).ToList();
            ViewBag.AgentId = new SelectList(agents.OrderBy(x => x.LastName).ThenBy(x => x.FirstName), "Id", "Name", entity.AgentId);

            //
            Customer dbcustomer = null;
            if (entity.Id > 0)
                await _customerService.GetById(entity.CustomerId);
            if (dbcustomer == null)
            {
                List<CommonConfiguration> vessels = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.VesselId.ToString()) && x.Id == entity.VesselId).ToList();
                ViewBag.VesselId = new SelectList(vessels, "Id", "Name", entity.VesselId);
            }
            else
            {
                ViewBag.VesselId = new SelectList(dbcustomer.VesselList, "VesselId", "Vessel", entity.VesselId);
            }

            List<CommonConfiguration> boardTypes = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.BoardType.ToString())).ToList();
            ViewBag.BoardTypeId = new SelectList(boardTypes, "Id", "Name", entity.BoardTypeId);

            ViewBag.LocationType = new SelectList(WorkOrderLocation.GetLocationTypes(), "Id", "Name");
            var locations = await this._locationService.GetByCri(null);
            ViewBag.Locations = new SelectList(locations.OrderBy(x => x.Display), "Id", "Display");
            var hotels = await this._hotelService.GetByCri(null);
            ViewBag.Hotels = new SelectList(hotels.OrderBy(x => x.Display), "Id", "Display");

            List<CommonConfiguration> ranks = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.Rank.ToString())).ToList();
            ViewBag.Ranks = new SelectList(ranks, "Id", "Name");

            var vehicles = (await _vehicelService.GetByCri(null)).OrderBy(x => x.VehicleNo).ToList();
            var userVehicles = _vehicelService.GetWithDrivers();
            foreach (var vehicle in vehicles)
            {
                var userVehicle = userVehicles.FirstOrDefault(x => x.VehicleId == vehicle.Id);
                if (userVehicle != null)
                    vehicle.ShortText1 = userVehicle.User.Name;
            }
            ViewBag.Vehicles = new SelectList(vehicles.OrderBy(x => x.Display), "Id", "Display");

            var meetingServices = (await _meetingService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.MeetingServices = new SelectList(meetingServices, "Id", "Name");

            ViewBag.Vessels = new SelectList(configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.VesselId.ToString())).OrderBy(x => x.Name), "Id", "Name");

            var staffs = _userService.GetDrivers();
            ViewBag.Staffs = new SelectList(staffs, "Id", "Name");
            ViewBag.ServiceJobVehicles = new SelectList(vehicles, "Id", "VehicleNo");
            var customDetentions = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.CustomDetention.ToString())).ToList();
            ViewBag.CustomDetentions = new SelectList(customDetentions, "Id", "Name");
            var checklists = (await _checkListService.GetByCri(null)).OrderBy(x => x.Name).ToList();
            ViewBag.Checklists = checklists;

            ViewBag.HasPermissionForBilledOrderLocation = _rolePermissionService.HasPermission(nameof(BilledOrderLocationController), User.Identity.GetUserId());
            ViewBag.HasPermissionForBilledOrderMeetingService = _rolePermissionService.HasPermission(nameof(BilledOrderMeetingServiceController), User.Identity.GetUserId());
            ViewBag.HasPermissionForBilledOrderPassenger = _rolePermissionService.HasPermission(nameof(BilledOrderPassengerController), User.Identity.GetUserId());
            ViewBag.HasPermissionForBilledOrderFileUpload = _rolePermissionService.HasPermission(nameof(BilledOrderFileUploadController), User.Identity.GetUserId());
            ViewBag.HasPermissionForMoveToBilled = _rolePermissionService.HasPermission(nameof(MoveToBilledController), User.Identity.GetUserId());
        }

        #endregion

        #region Save Data

        [HttpPost]
        public async Task<ActionResult> MoveToWithAccounts(Int64[] invoiceIds)
        {
            try
            {

                var workOrderIds = _service.GetWorkOrderIdsFromInvoiceIds(invoiceIds);

                if (workOrderIds == null || workOrderIds.Count == 0) return Content("failure");

                long[] woIds = new long[workOrderIds.Count];

                for(int i =0; i < workOrderIds.Count; i++ )
                {
                    woIds[i] = workOrderIds[i];
                }

                await _service.MoveToWithAccounts(woIds);
                return Content("success");
            }
            catch (Exception ex)
            {
                return Content("failure: " + ex.Message);
            }
        }

        #endregion

        #region Download Files
        public async Task<ActionResult> DownloadVoucher(Int64 serviceJobId)
        {
            var serviceJob = await this._serviceJobService.GetById(serviceJobId);

            if (serviceJob == null) return HttpNotFound();

            return base.File(base.GeneratePDF<ServiceJob>(serviceJob, FileConfig.SERVICE_JOBS, serviceJob.ReferenceNo), CONTENT_DISPOSITION, serviceJob.ReferenceNo + ".pdf");
        }

        //Download Taxable Invoice
        public async Task<ActionResult> DownloadInvoice(string invoiceSummaryNo)
        {
            #region Prepare GST List
            //Get GSTs
            var gsts = (await _gstService.GetByCri(null)).OrderBy(x => x.Name);
            #endregion

            #region Prepare Invoice Data
            List<EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice> saleInvoices = new List<ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice>();
            List<WorkOrder> workOrders = new List<WorkOrder>();
            List<Int64> workOrderIds = new List<long>();

            //Get Work Order Id No.
            var tempWorkOrders = await this._service.GetByInvoiceNo(invoiceSummaryNo);

            if (tempWorkOrders != null && tempWorkOrders.Count() > 0)
            {
                foreach (var workOrder in tempWorkOrders)
                {
                    workOrderIds.Add(workOrder.Id);
                }

                //Get Corresponding Sales Invoice Details based on Work Order Ids and getting Corresponding Work Orders
                foreach (var workOrderId in workOrderIds)
                {
                    var workOrder = await this._service.GetById(workOrderId);

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
            }

            #endregion

            #region PDF Report Generation

            //Start Setting up Invoice Report ViewModel for Invoice PDF

            InvoiceReportViewModel report = new InvoiceReportViewModel();

            SalesInvoiceSummary salesInvoiceSummary = await _salesInvoiceSummaryService.GetByInvoiceNo(invoiceSummaryNo);

            if (salesInvoiceSummary != null)
            {
                report.Date = salesInvoiceSummary.InvoiceDate.Value;

                //Set Taxable
                report.IsTaxInvoice = salesInvoiceSummary.IsTaxable;

                //Set Attn
                report.AttnStr = salesInvoiceSummary.AttnStr;
            }

            var company = await _companyService.GetById(GlobalVariable.COMPANY_ID);
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

                    var workOrderTemp = await _service.GetById(salesInvoice.WorkOrderId);
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
            report.needsVesselNameInFrontCompanyName = true;

            if (!string.IsNullOrEmpty(salesInvoiceSummary.DNNo))
            {
                InvoiceDetailsReportViewModel dnNo = new InvoiceDetailsReportViewModel
                {
                    WorkOrderNo = salesInvoiceSummary.DNNo,
                    IsHeader = true,
                    IsDNNo = true
                };
                report.Details.Add(dnNo);
            }

            var dbsalesInvoice = await _salesInvoiceSummaryService.GetByInvoiceNo(salesInvoiceSummary.ReferenceNo);
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


            var pdfFilePath = base.GeneratePDF<InvoiceReportViewModel>(report, FileConfig.INVOICES, "Inv_" + report.InvoiceNo.Replace('/', '_'));

            byte[] FileBytes = System.IO.File.ReadAllBytes(pdfFilePath);
            return File(FileBytes, "application/pdf");

            //return base.File(pdfFilePath, CONTENT_DISPOSITION, "INV_" + report.InvoiceNo.Replace('/', '_') + ".pdf");

            #endregion
        }

        //Download Credit Note - PCR2021 - P3
        public async Task<ActionResult> DownloadCreditNote(string invoiceSummaryNo)
        {
            var workOrders = await _service.GetByInvoiceNo(invoiceSummaryNo);

            if (workOrders == null || workOrders.Count() == 0) return null;

            List<Int64> creditNoteDetailIds = new List<long>();

            foreach(var order in workOrders)
            {
                if(order.CreditNoteId.HasValue && order.CreditNoteId > 0)
                {
                    creditNoteDetailIds.Add(order.CreditNoteId.Value);
                }
            }

            if (creditNoteDetailIds == null) return null;

            List<CreditNote> creditNoteDetailList = new List<CreditNote>();

            foreach (var creditNoteDetailId in creditNoteDetailIds)
            {
                var creditNoteDetailResult = await this._creditNoteService.GetById(creditNoteDetailId);

                if (creditNoteDetailResult != null)
                {
                    creditNoteDetailList.Add(creditNoteDetailResult);
                }
            }

            var invoiceSummary = await _salesInvoiceSummaryService.GetByInvoiceNo(invoiceSummaryNo);

            CreditNoteReportViewModel report = new CreditNoteReportViewModel();

            if (creditNoteDetailList != null && creditNoteDetailList.Count() > 0)
            {
                var firstCNDetail = creditNoteDetailList.FirstOrDefault();
                report.IsTaxCN = invoiceSummary == null ? false : invoiceSummary.IsTaxable;
                report.CNNo = invoiceSummaryNo;

                var company = await _companyService.GetById(GlobalVariable.COMPANY_ID);
                if (company == null) company = new ApplicationCore.Entities.Company();
                report.HeaderLogo = company.ReportHeaderLogo;

                var customer = await _customerService.GetById(firstCNDetail.CustomerId);
                report.Customer = customer.Name;

                report.Vessel = firstCNDetail.VesselName;
                report.Address = firstCNDetail.CompanyAddress;
                report.TaxDescription = firstCNDetail.GSTPercent.ToString();

                var GSTPercentage = firstCNDetail.GSTPercent;

                foreach (var creditNoteDetailEntity in creditNoteDetailList)
                {
                    report.TotalAmount += creditNoteDetailEntity.SubTotal;
                    //report.TaxAmount += creditNote.GSTAmount;
                    //report.GrandTotal += creditNote.GrandTotal;
                    report.InvoiceTotal += creditNoteDetailEntity.InvoiceTotalAmount;
                }

                //Calculate Tax Amount Ad Hoc 
                report.TaxAmount = Math.Round((report.TotalAmount * GSTPercentage / 100), 2, MidpointRounding.AwayFromZero);

                report.GrandTotal = Math.Round(report.TotalAmount + report.TaxAmount, 2, MidpointRounding.AwayFromZero);

                var salesInvoiceSummary = await _salesInvoiceSummaryService.GetByInvoiceNo(invoiceSummaryNo);

                report.Date = salesInvoiceSummary == null ? TimeUtil.GetLocalTime() : Util.ConvertStringToDateTime(salesInvoiceSummary.InvoiceDateStr, DateConfig.CULTURE);

                EngagerMark4.ApplicationCore.ReportViewModels.CreditNoteDetailsReportViewModel creditNoteDetail = new EngagerMark4.ApplicationCore.ReportViewModels.CreditNoteDetailsReportViewModel
                {
                    Description = firstCNDetail.Details.FirstOrDefault().Description,
                    TotalAmount = report.TotalAmount
                };

                report.Details.Add(creditNoteDetail);

                report.needsVesselNameInFrontCompanyName = true; //default true for Billed order generation

            }


            var pdfFilePath = base.GeneratePDF<CreditNoteReportViewModel>(report, FileConfig.CREDITNOTES, "CN_" + report.CNNo.Replace('/', '_'));

            byte[] FileBytes = System.IO.File.ReadAllBytes(pdfFilePath);
            return File(FileBytes, "application/pdf");
        }
        #endregion

        #region Push Notifications

        public async override Task PushNotification(WorkOrder aWorkOrder)
        {
            var notifiedDay = await _systemSettingRepository.GetWorkOrderNotifiedDay();
            var todayDate = TimeUtil.GetLocalTime();
            var notifiedDate = TimeUtil.GetLocalTime().AddDays(notifiedDay);


        }

        #endregion

    }
}