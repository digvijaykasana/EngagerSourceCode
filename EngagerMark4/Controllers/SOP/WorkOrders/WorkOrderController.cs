using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.Job.IService;
using EngagerMark4.ApplicationCore.Dummy.Entities.MeetingServices;
using EngagerMark4.ApplicationCore.Dummy.IService.MeetingServices;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IService.Jobs;
using EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Exceptions;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Controllers.Permissions;
using EngagerMark4.Hubs.Services;
using EngagerMark4.Infrasturcture.MobilePushNotifications.FCM;
using EngagerMark4.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;

namespace EngagerMark4.Controllers.SOP.WorkOrders
{
    public class WorkOrderController : BaseController<WorkOrderCri, WorkOrder, IWorkOrderService>
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

        List<ServiceJob> serviceJobs;

        public WorkOrderController(IWorkOrderService service,
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
            FCMSender fcmSender) : base(service)
        {
            _defaultColumn = "PickUpdateDate";
            _defaultOrderBy = BaseCri.EntityOrderBy.Dsc.ToString();
            _defaultDataType = BaseCri.DataType.DateTime.ToString();
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
        }

        #region Load References For List Page

        protected override WorkOrderCri GetCri()
        {
            var cri = new WorkOrderCri();
            //cri.IsPagination = true;
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

            ViewBag.CurrentId = String.IsNullOrEmpty(Request["CurrentId"]) ? "0" : Request["CurrentId"];

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

            //Drivers
            cri.Drivers = drivers;
            long driverId = 0;
            Int64.TryParse(drivers, out driverId);
            cri.DriverId = driverId;

            //Customers
            Int64 customerId = 0;
            if (!string.IsNullOrEmpty(customerIdStr))
            {
                Int64.TryParse(customerIdStr, out customerId);
            }
            cri.CustomerId = customerId;

            //Vessels
            Int64 vesselId = 0;
            if (!string.IsNullOrEmpty(vesselIdStr))
            {
                Int64.TryParse(vesselIdStr, out vesselId);
            }
            cri.VesselId = vesselId;

            //Status
            int statusInt = 0;
            if (!string.IsNullOrEmpty(status))
            {
                Int32.TryParse(status, out statusInt);
            }
            cri.Status = (EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders.WorkOrder.OrderStatus)statusInt;

            //From Date
            if (!string.IsNullOrEmpty(fromDate))
                cri.FromDate = Util.ConvertStringToDateTime(fromDate, DateConfig.CULTURE);

            //To Date
            if (!string.IsNullOrEmpty(toDate))
                cri.ToDate = Util.ConvertStringToDateTime(toDate, DateConfig.CULTURE);

            //Sales Invoice Summary No
            cri.SalesInvoiceSummaryNo = invoiceNo;

            //Invoice Date
            cri.InvoiceDate = invoiceDate;

            return cri;
        }

        protected async override Task<IEnumerable<WorkOrder>> GetData(WorkOrderCri cri)
        {
            //if (string.IsNullOrEmpty(cri.SalesInvoiceSummaryNo))
            //{
            IEnumerable<WorkOrder> workOrders = await base.GetData(cri);
            //
            return workOrders;
            //}
            //else
            //{
            //    return await this._service.GetByInvoiceNo(cri.SalesInvoiceSummaryNo);
            //}
        }

        protected async override Task LoadReferencesForList(WorkOrderCri aCri)
        {
            var customerId = Request["Customers"];
            var vesselId = Request["Vessels"];
            var driverId = Request["Drivers"];
            var orderPage = Request["CurrentPage"];
            string orderColumn = Request["Column"];
            string orderOrderBy = Request["OrderBy"];
            string orderDataType = Request["DataType"];
            string currentId = String.IsNullOrEmpty(Request["CurrentId"]) ? "0" : Request["CurrentId"];

            ViewBag.orderPage = orderPage;
            ViewBag.orderColumn = orderColumn;
            ViewBag.orderOrderBy = orderOrderBy;
            ViewBag.orderDataType = orderDataType;
            ViewBag.CurrentId = currentId;

            //Customers
            var customers = (await this._customerService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.Customers = new SelectList(customers, "Id", "Name", customerId);

            //Vessels
            CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            var vessels = _configurationService.GetVessels();
            ViewBag.Vessels = new SelectList(vessels, "Id", "Name", vesselId);

            //Drivers
            var drivers = _userService.GetDrivers();
            ViewBag.DriverId = new SelectList(drivers, "Id", "Name", aCri.DriverId);

            //From Date - To Date
            string fromDate = Request["FromDate"];
            string toDate = Request["ToDate"];
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            //Status
            var status = Request["Status"];
            List<CommonConfiguration> Statuses = WorkOrderCri.GetOrderStatuses();
            ViewBag.Status = new SelectList(Statuses, "Id", "Name", status);

            //Notifications
            var notifications = await this._notificationRepository.GetByCri(null);
            ViewBag.Notifications = notifications;

            ViewBag.HasPermissionForWONotiAcknowlegement = _rolePermissionService.HasPermission(nameof(AcknowledgeWONotificationsPermissionController), User.Identity.GetUserId());
        }

        #endregion

        #region Load References For Details Page

        protected async override Task LoadReferences(WorkOrder entity)
        {
            try
            {
                if (entity == null) entity = new WorkOrder();

                //Return URL - Current Id
                ViewBag.ReturnUrl = Request["returnUrl"];
                ViewBag.CurrentId = String.IsNullOrEmpty(Request["CurrentId"]) ? "0" : Request["CurrentId"];

                #region Work Order

                entity.ModificationComments = string.Empty;

                //Getting all configuration data
                CommonConfigurationCri configurationCri = new CommonConfigurationCri();
                configurationCri.Includes = new List<string>();
                configurationCri.Includes.Add("ConfigurationGroup");
                var configurations = await _configurationService.GetByCri(configurationCri);

                //Customers
                var customers = (await _customerService.GetByCri(null)).OrderBy(x => x.Name);
                ViewBag.CustomerId = new SelectList(customers.OrderBy(x => x.Name), "Id", "Name", entity.CustomerId);

                //Agents
                List<EngagerMark4.ApplicationCore.Entities.Users.User> agents = (await _userService.GetByCustomerId(entity.CustomerId == null ? 0 : entity.CustomerId.Value)).ToList();
                ViewBag.AgentId = new SelectList(agents.OrderBy(x => x.LastName).ThenBy(x => x.FirstName), "Id", "Name", entity.AgentId);

                //Vessels
                Customer dbcustomer = null;
                if (entity.Id > 0)
                    dbcustomer = await _customerService.GetById(entity.CustomerId);
                if (dbcustomer == null)
                {
                    List<CommonConfiguration> vessels = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.VesselId.ToString()) && x.Id == entity.VesselId).OrderBy(x => x.SerialNo).ToList();
                    ViewBag.VesselId = new SelectList(vessels, "Id", "Name", entity.VesselId);

                    //Vessels for Meeting Service
                    ViewBag.Vessels = new SelectList(new List<CommonConfiguration>(), "Id", "Name");
                    //ViewBag.Vessels = new SelectList(configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.VesselId.ToString())).OrderBy(x => x.Name), "Id", "Name");

                }
                else
                {
                    ViewBag.VesselId = new SelectList(dbcustomer.VesselList, "VesselId", "Vessel", entity.VesselId);

                    //Vessels for Meeting Service
                    ViewBag.Vessels = new SelectList(dbcustomer.VesselList, "VesselId", "Vessel", entity.VesselId);
                }

                //Board Types
                List<CommonConfiguration> boardTypes = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.BoardType.ToString())).OrderBy(x => x.SerialNo).ToList();
                ViewBag.BoardTypeId = new SelectList(boardTypes, "Id", "Name", entity.BoardTypeId);

                #endregion

                #region Work Order Locations

                //Locations
                ViewBag.LocationType = new SelectList(WorkOrderLocation.GetLocationTypes(), "Id", "Name");
                var locations = await this._locationService.GetByCri(null);
                ViewBag.Locations = new SelectList(locations.OrderBy(x => x.Display), "Id", "Display");

                //Hotels
                var hotels = await this._hotelService.GetByCri(null);
                ViewBag.Hotels = new SelectList(hotels.OrderBy(x => x.Display), "Id", "Display");

                #endregion

                #region Work Order Passengers

                //Ranks
                List<CommonConfiguration> ranks = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.Rank.ToString())).OrderBy(x => x.SerialNo).ToList();
                ViewBag.Ranks = new SelectList(ranks, "Id", "Name");

                //Vehicles
                List<CommonConfiguration> userStatuses = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.UserStatus.ToString())).OrderBy(x => x.SerialNo).ToList();
                Int64 userActiveId = userStatuses.Where(x => x.Code.Equals(UserStatusCodes.Active.ToString())).FirstOrDefault().Id;
                var allVehiclesList = (await _vehicelService.GetByCri(null)).OrderBy(x => x.VehicleNo).ToList();

                List<Vehicle> vehicles = new List<Vehicle>();
                var userVehicles = _vehicelService.GetWithDrivers();
                foreach (var vehicle in allVehiclesList)
                {
                    var userVehicleLst = userVehicles.Where(x => x.VehicleId == vehicle.Id && x.User.StatusId == userActiveId).ToList();

                    if (userVehicleLst.Count > 0)
                    {
                        vehicle.ShortText1 = userVehicleLst.FirstOrDefault().User.Name;

                        vehicles.Add(vehicle);
                    }
                }
                ViewBag.Vehicles = new SelectList(vehicles.OrderBy(x => x.Display), "Id", "Display");

                if (entity.Id > 0)
                {
                    var serviceJobs = await _serviceJobService.GetServiceJobsByWorkOrderId(entity.Id);

                    if (serviceJobs.Any() && serviceJobs.Count() > 0)
                    {
                        ViewBag.WorkOrderPassengerServiceJobs = serviceJobs.OrderBy(x => x.ReferenceNo).ToList();
                    }
                    else
                    {
                        ViewBag.WorkOrderPassengerServiceJobs = new List<ServiceJob>();
                    }
                }
                else
                {
                    ViewBag.WorkOrderPassengerServiceJobs = new List<ServiceJob>();
                }

                #endregion

                #region Work Order Meeting Services

                //Meeting Services
                var meetingServices = (await _meetingService.GetByCri(null)).OrderBy(x => x.Name);
                ViewBag.MeetingServices = new SelectList(meetingServices, "Id", "Name");

                //Meeting Service Passengers
                ViewBag.MeetingServicePassengers = new SelectList(entity.WorkOrderPassengerList == null ? new List<WorkOrderPassenger>() : entity.WorkOrderPassengerList, "Name", "Name");

                if (entity.MeetingServiceList.Any())
                {
                    foreach (var meeting in entity.MeetingServiceList)
                    {
                        if (meeting.AirportMeetingServiceId > 0 && meeting.MeetingService == null)
                        {
                            meeting.MeetingService = meetingServices.Where(x => x.Id == meeting.AirportMeetingServiceId).FirstOrDefault();
                        }
                    }
                }

                #endregion

                #region Work Order Service Jobs

                //Drivers
                var staffs = _userService.GetDrivers();
                ViewBag.Staffs = new SelectList(staffs, "Id", "Name");

                //Vehicles
                ViewBag.ServiceJobVehicles = new SelectList(allVehiclesList, "Id", "VehicleNo");

                //Custom Detentions
                var customDetentions = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.CustomDetention.ToString())).OrderBy(x => x.SerialNo).ToList();
                ViewBag.CustomDetentions = new SelectList(customDetentions, "Id", "Name");

                //Checklists
                var checklists = (await _checkListService.GetByCri(null)).OrderBy(x => x.Name).ToList();
                ViewBag.Checklists = checklists;

                #endregion


                #region Notifications

                //Notifications
                if (entity.Id != 0)
                {
                    var notificationList = _notificationRepository.GetModifiedNotificationsByWorkOrderId(entity.Id);

                    entity.WorkOrderNotificationList = new List<Notification>();

                    if (notificationList != null || notificationList.Count() > 0)
                    {
                        entity.WorkOrderNotificationList = notificationList.ToList();
                    }
                }

                #endregion

                #region Permissions

                //Permissions
                ViewBag.HasPermissionForWorkOrderLocation = _rolePermissionService.HasPermission(nameof(WorkOrderLocationController), User.Identity.GetUserId());
                ViewBag.HasPermissionForWorkOrderMeetingService = _rolePermissionService.HasPermission(nameof(WorkOrderMeetingServiceController), User.Identity.GetUserId());
                ViewBag.HasPermissionForWorkOrderPassenger = _rolePermissionService.HasPermission(nameof(WorkOrderPassengerController), User.Identity.GetUserId());
                ViewBag.HasPermissionForWorkOrderFileUpload = _rolePermissionService.HasPermission(nameof(WorkOrderFileUploadController), User.Identity.GetUserId());
                ViewBag.HasPermissionForWorkOrderHistory = _rolePermissionService.HasPermission(nameof(WorkOrderHistoryController), User.Identity.GetUserId());

                #endregion
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

        public async Task<ActionResult> GetMeetingServiceByMeetingServiceId(long meetingServiceId = 0)
        {

            if (meetingServiceId == 0)
            {
                return null;
            }
            var meetingService = await _meetingService.GetById(meetingServiceId);

            if (meetingService != null)
            {
                return Json(meetingService, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
            //
        }

        public async Task<ActionResult> GetPricePerPaxInfo(long meetingServiceId = 0, long noOfPax = 0)
        {
            if (meetingServiceId == 0)
            {
                return null;
            }
            var meetingService = await _meetingService.GetById(meetingServiceId);

            MeetingServiceDetails detail = null;

            if (meetingService.MeetingServiceDetails != null && meetingService.MeetingServiceDetails.Count() > 0)
            {
                foreach (var msDetail in meetingService.MeetingServiceDetails.OrderBy(x => x.MinPax))
                {
                    if (noOfPax >= msDetail.MinPax && noOfPax <= msDetail.MaxPax)
                    {
                        detail = msDetail;
                    }
                }

                int maxPaxRange = meetingService.MeetingServiceDetails.OrderByDescending(x => x.MaxPax).First().MaxPax;

                if (detail == null && noOfPax > maxPaxRange)
                {
                    detail = meetingService.MeetingServiceDetails.OrderByDescending(x => x.MaxPax).First();
                }
            }

            if (detail == null)
            {
                detail = new MeetingServiceDetails();

                detail.NoOfPax = "NA";
                detail.Charges = 0;
                detail.MaxPax = 0;
            }
            //
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        private bool AllAccess = true;
        List<Function> functionList = new List<Function>();
        List<Role> roles = new List<Role>();
        Function currentFunction = new Function();
        List<RolePermissionDetails> rolePermissionsDetails = new List<RolePermissionDetails>();
        List<Audit> audits = new List<Audit>();

        private void LoadAccessInfo()
        {
            this._controller = RouteData.Values["controller"].ToString() + "Controller";
            this.functionList = HttpContext.Items[AppKeyConfig.FUNCTION] as List<Function>;
            if (this.functionList == null) this.functionList = new List<Function>();
            this.roles = HttpContext.Items[AppKeyConfig.ROLE] as List<Role>;
            if (this.roles == null) this.roles = new List<Role>();
            this.currentFunction = this.functionList.FirstOrDefault(x => x.Controller.Equals(_controller));
            if (this.currentFunction == null) this.currentFunction = new Function();
            this.AllAccess = (bool)HttpContext.Items[AppKeyConfig.ALL_ACCESS];
            this.rolePermissionsDetails = HttpContext.Items[AppKeyConfig.DETAILS_PERMISSION] as List<RolePermissionDetails>;
        }

        #endregion

        #region Save Work Order

        [HttpPost]
        public JsonResult CheckForSimilarOrders(string PickUpDateBinding, string PickUpTimeBinding, Int64 CustomerId, Int64 VesselId, List<SimliarOrderLocationViewModel> Locations)
        {
            DateTime PickUpDate = Util.ConvertStringToDateTime(PickUpDateBinding + " " + PickUpTimeBinding, DateConfig.CULTURE);

            List<WorkOrderLocation> locationList = new List<WorkOrderLocation>();

            if (Locations != null)
            {
                foreach (var model in Locations)
                {
                    WorkOrderLocation location = new WorkOrderLocation()
                    {
                        LocationId = model.LocationId,
                        HotelId = model.HotelId,
                        Type = model.LocationType
                    };

                    locationList.Add(location);
                }
            }

            var resultLst = _service.GetSimliarOrders(PickUpDate, CustomerId, VesselId, locationList);

            if (resultLst == null || resultLst.Count == 0)
                return Json("NoSimilarOrder");

            string message = "";

            foreach (var workOrder in resultLst)
            {
                message += workOrder.RefereneceNo + ", ";
            }

            if (!string.IsNullOrEmpty(message))
            {
                message = message.Remove(message.Length - 2);
            }

            return Json(message);
        }

        Dictionary<Int64, Int64> serviceJob_UserId = new Dictionary<long, long>();

        protected async override Task SaveEntity(WorkOrder aEntity)
        {
            try
            {


                if (aEntity.ServiceJobList != null)
                {
                    foreach (var serviceJob in aEntity.ServiceJobList)
                    {
                        serviceJob_UserId[serviceJob.Id] = serviceJob.UserId == null ? 0 : serviceJob.UserId.Value;
                    }
                }

                switch (aEntity.isSavingCopySign)
                {
                    case true:
                        if (aEntity.ServiceJobList.Count > 0)
                        {
                            foreach (ServiceJob service in aEntity.ServiceJobList)
                            {
                                if (service.Id != 0)
                                {
                                    await _serviceJobService.UpdateCopySign(service.Id, service.CopySign);
                                }
                            }
                        }
                        break;
                    case false:
                        if (aEntity.ServiceJobList.Count > 0)
                        {
                            serviceJobs = new List<ServiceJob>();

                            foreach (ServiceJob service in aEntity.ServiceJobList)
                            {
                                if (service.Id != 0)
                                {
                                    serviceJobs.Add(service);
                                }
                            }
                        }

                        try
                        {
                            aEntity.isFromOps = true;
                            await _service.Save(aEntity);
                        }
                        catch (Exception ex)
                        { }
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected override void ValidateEntity(WorkOrder aEntity)
        {
            //Validate if there is any checklist item amount left unfilled
            if (aEntity.Status >= WorkOrder.OrderStatus.Submitted && aEntity.ServiceJobList.Any() && aEntity.ServiceJobList.Count() > 0)
            {
                int unfilledAmountChkItemCount = 0;

                foreach (var serviceJob in aEntity.ServiceJobList.Where(x => x.Status == ServiceJob.ServiceJobStatus.Submitted))
                {
                    if (!string.IsNullOrEmpty(serviceJob.CheckListIds))
                    {
                        var sJCheckListItems = serviceJob.GetChecklistItemList();

                        unfilledAmountChkItemCount = sJCheckListItems.Where(x => x.ChecklistPrice == 0).Count();
                    }

                    if (serviceJob.TripFees == 0) unfilledAmountChkItemCount++;

                    //MS Fees is optional
                    //if (serviceJob.MSFees == 0) unfilledAmountChkItemCount++;
                }

                if (unfilledAmountChkItemCount > 0)
                {
                    ModelState.AddModelError("", "Empty service job fees detected. Fill in the required fields to proceed.");
                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async override Task<ActionResult> Details(WorkOrder aEntity)
        {
            LoadAccessInfo();

            try
            {
                ValidateEntity(aEntity);

                //var errors = ModelState.Values.Where(x => x.Errors.Any()).ToList(); 

                if (ModelState.IsValid)
                {
                    if (aEntity.Id == 0) IsForFirstTime = true;
                    if (IsForFirstTime)
                    {
                        if (AllAccess == false)
                        {
                            var create = this.rolePermissionsDetails.FirstOrDefault(x => x.Type == RolePermissionDetails.PermissionType.Create);
                            if (create == null)
                            {
                                ModelState.AddModelError("", "No permission to create record!");
                                await LoadReferences(aEntity);
                                return View(aEntity);
                            }
                        }

                    }
                    else
                    {
                        if (AllAccess == false)
                        {
                            var update = this.rolePermissionsDetails.FirstOrDefault(x => x.Type == RolePermissionDetails.PermissionType.Edit);
                            if (update == null)
                            {
                                ModelState.AddModelError("", "No permission to update record!");
                                await LoadReferences(aEntity);
                                return View(aEntity);
                            }
                        }

                        if (aEntity.SalesInvoiceSummaryId != 0)
                        {
                            var isOrderExistUnderInvoice = _salesInvoiceSummaryService.IsWorkOrderUnderInvoice(aEntity.Id, aEntity.SalesInvoiceSummaryId);

                            if (!isOrderExistUnderInvoice)
                            {
                                ModelState.AddModelError("", "Data conflict between current work order and its corresponding invoice data. Refresh the page and try again.");

                                var entity = await _service.GetById(aEntity.Id);
                                await LoadReferences(entity);
                                return View(entity);
                            }
                            else
                            {
                                var invoiceVesselId = _salesInvoiceSummaryService.GetInvoiceVesselId(aEntity.SalesInvoiceSummaryId);

                                if (invoiceVesselId != aEntity.VesselId)
                                {
                                    ModelState.AddModelError("", "Vessel cannot be changed due to data integrity conflict between work order and its related invoice data.");
                                    var entity = await _service.GetById(aEntity.Id);
                                    await LoadReferences(entity);
                                    return View(entity);
                                }
                            }
                        }

                    }


                    await SaveEntity(aEntity);

                    AfterSaveMessage();

                    await PushNotification(aEntity);

                    _currentEntityId = aEntity.Id;

                    var returnUrl = Request["ReturnUrl"];

                    if (IsForFirstTime)
                    {
                        var entityStr = RouteData.Values["controller"].ToString().Replace("Controller", string.Empty);
                        audits.Add(new Audit
                        {
                            Description = $"{entityStr} {aEntity.ToString()} created",
                            StartProcessingTime = TimeUtil.GetLocalTime(),
                            Type = Audit.AuditType.Normal
                        });
                    }
                    else
                    {
                        var entityStr = RouteData.Values["controller"].ToString().Replace("Controller", string.Empty);
                        audits.Add(new Audit
                        {
                            Description = $"{entityStr} {aEntity.ToString()} updated",
                            StartProcessingTime = TimeUtil.GetLocalTime(),
                            Type = Audit.AuditType.Normal
                        });
                    }

                    return DetailRedirect(returnUrl);
                }
            }
            catch (CannotAddException ex)
            {
                ModelState.AddModelError("", "Wrong Data Inserted! Record cannot be saved!");
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                ModelState.AddModelError("", "Cannot insert dupliate record!");
            }
            await LoadReferences(aEntity);
            return View(aEntity);
        }

        #endregion

        #region Push Notifications and Dashboard

        public ActionResult ChangeData(string text)
        {
            var dashboardHubService = new DashboardHubService();
            //dashboardHubService.NotifyUpdatesForWorkOrder(text);
            return Content("Sent Ok");
        }

        public async override Task PushNotification(WorkOrder aWorkOrder)
        {
            var notifiedDay = await _systemSettingRepository.GetWorkOrderNotifiedDay();
            var todayDate = TimeUtil.GetLocalTime();
            var notifiedDate = TimeUtil.GetLocalTime().AddDays(notifiedDay);

            TimeSpan ts = new TimeSpan(0, 0, 0);
            DateTime fromDate = todayDate.Date + ts;

            TimeSpan newts = new TimeSpan(23, 59, 59);
            DateTime toDate = notifiedDate.Date + newts;

            if (IsForFirstTime)
            {
                #region Push Notify to Android
                if (aWorkOrder.Status >= WorkOrder.OrderStatus.Assigned)
                {
                    if (aWorkOrder.PickUpdateDate != null && aWorkOrder.PickUpdateDate >= fromDate && aWorkOrder.PickUpdateDate <= toDate)
                    {
                        var serviceJobList = aWorkOrder.ServiceJobList.Where(x => x.Delete == false).ToList();

                        if (!(serviceJobList.Count == 0 || serviceJobList == null))
                        {
                            foreach (var serviceJob in aWorkOrder.ServiceJobList.Where(x => x.Delete == false))
                            {
                                var user = await _userService.GetById(serviceJob.UserId);
                                if (user != null)
                                    await this._fmcSender.Send(user.FCMId, "New Work Order Created", $"{aWorkOrder.RefereneceNo} - {aWorkOrder.PickUpDateStr} {aWorkOrder.PickUpdateTimeStr} - {aWorkOrder.PickUpPoint}");

                                if (serviceJob.Id != 0)
                                {
                                    await _serviceJobService.UpdateCreatedNotificationSentStatus(serviceJob.Id);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Push Notify to Web Dashboard
                var rasier = aWorkOrder.AgentId == null ? _userService.GetByApplicatioNId(aWorkOrder.CreatedBy) : await _userService.GetById(aWorkOrder.AgentId.Value);
                var customer = aWorkOrder.CustomerId == null ? new Customer() : await this._customerService.GetById(aWorkOrder.CustomerId);
                Notification notification = new Notification
                {
                    Description = $"New Work Order {aWorkOrder.RefereneceNo} raised by {rasier.Name}.<br/>Customer: {customer.Name} <br/>Pick up Point and Time: {aWorkOrder.PickUpPoint} ({aWorkOrder.PickUpDateOnlyStr} {aWorkOrder.PickUpdateTimeStr})",
                    ReferenceId = aWorkOrder.Id,
                    Acknowledge = true,
                    Type = Notification.NotificationType.WorkOrder,
                };

                this._notificationRepository.Save(notification);
                this._notificationRepository.SaveChanges();
                var dashboardHubService = new DashboardHubService();
                dashboardHubService.NotifyUpdatesForWorkOrder(notification);
                #endregion
            }
            else
            {
                #region Push Notification to Android when job is Updated

                if (aWorkOrder.Status >= WorkOrder.OrderStatus.Assigned && aWorkOrder.Status < WorkOrder.OrderStatus.Submitted)
                {
                    if (aWorkOrder.PickUpdateDate != null && aWorkOrder.PickUpdateDate >= fromDate && aWorkOrder.PickUpdateDate <= toDate)
                    {
                        var serviceJobList = aWorkOrder.ServiceJobList.Where(x => x.Delete == false).ToList();

                        ServiceJobCri sJCri = new ServiceJobCri()
                        {
                            WorkOrderId = aWorkOrder.Id
                        };

                        //var serviceJobList = await _serviceJobService.GetByCri(sJCri);

                        if (!(serviceJobList == null || serviceJobList.Count() == 0))
                        {
                            foreach (var serviceJob in serviceJobList)
                            {
                                //var inSJ = serviceJobs.FirstOrDefault(x => x.Id == serviceJob.Id);

                                var user = await _userService.GetById(serviceJob.UserId);

                                DateTime tempDate = new DateTime();

                                if (!serviceJob.IsCreatedNotiSent)
                                {
                                    if (user != null && !String.IsNullOrEmpty(user.FCMId))
                                        await this._fmcSender.Send(user.FCMId, "New Work Order Created", $"{aWorkOrder.RefereneceNo} - {aWorkOrder.PickUpDateStr} {aWorkOrder.PickUpdateTimeStr} - {aWorkOrder.PickUpPoint}");
                                    if (serviceJob.Id != 0)
                                    {
                                        await _serviceJobService.UpdateCreatedNotificationSentStatus(serviceJob.Id);
                                    }
                                }
                                else
                                {
                                    if (!String.IsNullOrEmpty(aWorkOrder.ModificationComments))
                                    {
                                        if (user != null)
                                        {
                                            Notification notification = new Notification
                                            {
                                                Description = $"Work Order Updated - {aWorkOrder.RefereneceNo} - Pickup Time: {aWorkOrder.PickUpDateStr}",
                                                DetailedDescription = $"Admin Comments : {aWorkOrder.ModificationComments}",
                                                ReferenceId = aWorkOrder.Id,
                                                NotifiedUserId = user.Id,
                                                NotifiedUserName = user.LastName + " " + user.FirstName,
                                                Acknowledge = false,
                                                Type = Notification.NotificationType.WorkOrderUpdated,
                                            };
                                            aWorkOrder.HasPendingNotification = true;
                                            this._notificationRepository.Save(notification);
                                            this._notificationRepository.SaveChanges();

                                            await this._fmcSender.Send(user.FCMId, "Work Order Updated", $"{aWorkOrder.RefereneceNo} - Pickup Time: {aWorkOrder.PickUpDateStr} - {aWorkOrder.PickUpPoint}");

                                        }
                                    }
                                }
                            }
                        }

                        #region Notify Deleted service jobs
                        var deletedServiceJobList = aWorkOrder.ServiceJobList.Where(x => x.Delete == true).ToList();

                        if (deletedServiceJobList != null)
                        {
                            foreach (var deletedServiceJob in deletedServiceJobList)
                            {
                                var removeduser = await _userService.GetById(serviceJob_UserId[deletedServiceJob.Id]);

                                Notification notification = new Notification
                                {
                                    Description = $"Work Order Cancelled - {aWorkOrder.RefereneceNo} - Pickup Time: {aWorkOrder.PickUpDateStr}",
                                    //DetailedDescription = $"Admin Comments : {aWorkOrder.ModificationComments}",
                                    ReferenceId = aWorkOrder.Id,
                                    NotifiedUserId = removeduser.Id,
                                    NotifiedUserName = removeduser.LastName + " " + removeduser.FirstName,
                                    Acknowledge = false,
                                    Type = Notification.NotificationType.WorkOrderUpdated,
                                };

                                aWorkOrder.HasPendingNotification = true;

                                this._notificationRepository.Save(notification);
                                this._notificationRepository.SaveChanges();

                                await this._fmcSender.Send(removeduser.FCMId, "Work Order Cancelled", $"{aWorkOrder.RefereneceNo} - {aWorkOrder.PickUpDateStr} {aWorkOrder.PickUpdateTimeStr} - {aWorkOrder.PickUpPoint}");
                            }
                        }
                        #endregion
                        //this._serviceJobService.SaveChanges();
                    }
                }
                #endregion

                #region Push Notification to Android when job is Cancelled
                else if (aWorkOrder.Status == WorkOrder.OrderStatus.Cancelled)
                {
                    if (aWorkOrder.PickUpdateDate != null && aWorkOrder.PickUpdateDate >= fromDate && aWorkOrder.PickUpdateDate <= toDate)
                    {
                        foreach (var serviceJob in aWorkOrder.ServiceJobList.Where(x => x.Delete == false))
                        {
                            var user = await _userService.GetById(serviceJob.UserId);

                            if (user != null)
                            {
                                if (!String.IsNullOrEmpty(aWorkOrder.ModificationComments))
                                {

                                    Notification notification = new Notification
                                    {
                                        Description = $"Work Order Cancelled - {aWorkOrder.RefereneceNo} - Pickup Time: {aWorkOrder.PickUpDateStr}",
                                        DetailedDescription = $"Admin Comments : {aWorkOrder.ModificationComments}",
                                        ReferenceId = aWorkOrder.Id,
                                        NotifiedUserId = user.Id,
                                        NotifiedUserName = user.LastName + " " + user.FirstName,
                                        Acknowledge = false,
                                        Type = Notification.NotificationType.WorkOrderUpdated,
                                    };

                                    this._notificationRepository.Save(notification);
                                    this._notificationRepository.SaveChanges();

                                    await this._fmcSender.Send(user.FCMId, "Work Order Cancelled", $"{aWorkOrder.RefereneceNo} - {aWorkOrder.PickUpDateStr} {aWorkOrder.PickUpdateTimeStr} - {aWorkOrder.PickUpPoint}");

                                }
                            }
                        }
                    }
                }
                #endregion
            }
        }

        [HttpPost]
        public async Task<ActionResult> Acknowledge(long notificationId)
        {
            await _notificationRepository.Acknowledge(notificationId);

            return Content("success");
        }

        #endregion

        #region Download Transfer Voucher

        public async Task<ActionResult> DownloadVoucher(Int64 serviceJobId)
        {
            var serviceJob = await this._serviceJobService.GetById(serviceJobId);

            if (serviceJob == null) return HttpNotFound();

            return base.File(base.GeneratePDF<ServiceJob>(serviceJob, FileConfig.SERVICE_JOBS, serviceJob.ReferenceNo), CONTENT_DISPOSITION, serviceJob.ReferenceNo + ".pdf");
        }

        #endregion
    }
}