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
using EngagerMark4.ApplicationCore.Dummy.IService.MeetingServices;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IService.Jobs;
using EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Controllers.Users;
using EngagerMark4.Hubs.Services;
using EngagerMark4.Infrasturcture.MobilePushNotifications.FCM;
using EngagerMark4.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;

namespace EngagerMark4.Controllers.SOP.CustomerOrders
{
    public class CustomerOrderController : BaseController<WorkOrderCri, WorkOrder, IWorkOrderService>
    {
        IRolePermissionService _rolePermissionService;
        ICustomerService _customerService;
        IUserCustomerService _userCustomerService;
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

        public CustomerOrderController(IWorkOrderService service,
            IRolePermissionService rolePermissionService,
            ICustomerService customerService,
            IUserCustomerService userCustomerService,
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
            _defaultColumn = "ReferenceNoNumber";
            _defaultOrderBy = BaseCri.EntityOrderBy.Dsc.ToString();
            _defaultDataType = BaseCri.DataType.Int64.ToString();
            this._rolePermissionService = rolePermissionService;
            this._customerService = customerService;
            this._userCustomerService = userCustomerService;
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

        public bool IsFirstTimeSubmission = false;

        #region Load References For List Page

        protected override WorkOrderCri GetCri()
        {
            String currentCustomer = GetAgentCustomerId().ToString();

            ViewBag.CurrentCustomer = currentCustomer;

            var cri = base.GetCri();

            if (cri == null)
                cri = new WorkOrderCri();
            cri.Includes = new List<string>();
            cri.Includes.Add("Customer");

            var customerIdStr = currentCustomer.ToString();
            var vesselIdStr = Request["Vessels"];
            var fromDate = Request["FromDate"];
            var toDate = Request["ToDate"];
            var status = Request["Status"];
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
            ViewBag.orderPage = orderPage;
            ViewBag.orderColumn = orderColumn;
            ViewBag.orderOrderBy = orderOrderBy;
            ViewBag.orderDataType = orderDataType;

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

            return cri;
        }

        protected async override Task<IPagedList<WorkOrder>> GetEntities(WorkOrderCri aCri)
        {
            try
            {
                var cri = GetCri();

                var cri2 = GetOrderBy(cri);

                var entities = await _service.GetOrdersForAgents(cri2);

                var workOrders = new List<WorkOrder>();

                if (entities != null)
                {
                    workOrders = entities.ToList();

                    foreach (WorkOrder woOrder in workOrders)
                    {
                        if (woOrder.Status >= WorkOrder.OrderStatus.Submitted)
                        {
                            woOrder.Status = WorkOrder.OrderStatus.Verified;
                        }
                    }
                }
                else
                {
                    entities = new List<WorkOrder>();
                }

                string userId = User.Identity.GetUserId();
                var currentAgentUser = _userService.GetByApplicatioNId(userId);

                if (currentAgentUser != null && _rolePermissionService.HasPermission(nameof(AgentSeeOwnOrdersController), userId))
                {
                    var customerOrders = workOrders.Where(x => x.AgentId.HasValue && x.AgentId.Value == currentAgentUser.Id).OrderByDescending(x => x.RefereneceNo).ToList();
                    if (_rolePermissionService.HasPermission(nameof(AgentSeeCompanyOrdersController), userId))
                    {
                        customerOrders.AddRange(workOrders.Where(x => !x.AgentId.HasValue || x.AgentId.Value != currentAgentUser.Id).ToList());
                    }
                    //var pagedList = await GetDisplayName(customerOrders);
                    return customerOrders.ToPagedList(aCri.CurrentPage, aCri.NoOfPage);
                }

                return workOrders.ToPagedList(aCri.CurrentPage, aCri.NoOfPage);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }

        protected async override Task LoadReferencesForList(WorkOrderCri aCri)
        {
            ViewBag.CurrentCustomer = GetAgentCustomerId().ToString();

            var customerId = Request["Customers"];
            var vesselId = Request["Vessels"];
            var customers = (await this._customerService.GetByCri(null)).OrderBy(x => x.Name);
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
            ViewBag.Customers = new SelectList(customers, "Id", "Name", customerId);

            //Vessels
            CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            configurationCri.Includes = new List<string>();
            configurationCri.Includes.Add("ConfigurationGroup");
            var vessels = (await _configurationService.GetByCri(configurationCri)).Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.VesselId.ToString())).OrderBy(x => x.Name);
            ViewBag.Vessels = new SelectList(vessels, "Id", "Name", vesselId);

            //From Date - To Date
            string fromDate = Request["FromDate"];
            string toDate = Request["ToDate"];
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            //Status
            var status = Request["Status"];
            List<CommonConfiguration> Statuses = WorkOrderCri.GetOrderStatusesForAgents();
            ViewBag.Status = new SelectList(Statuses, "Id", "Name", status);
        }

        #endregion

        #region Load References For Details Page

        protected async override Task LoadReferences(WorkOrder entity)
        {
            if (entity == null) entity = new WorkOrder();

            //Return URL - Current Id
            ViewBag.ReturnUrl = Request["returnUrl"];
            ViewBag.CurrentId = String.IsNullOrEmpty(Request["CurrentId"]) ? "0" : Request["CurrentId"];

            #region Work Order

            //Getting Agent Customer Id
            string currentCustomerId = GetAgentCustomerId().ToString();
            ViewBag.AgentCustomerId = currentCustomerId;

            //Getting Current Agent Id
            Int64 userId = _userService.GetByApplicatioNId(User.Identity.GetUserId()).Id;
            ViewBag.CurrentUserId = userId;

            //Getting all configuration data
            CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            configurationCri.Includes = new List<string>();
            configurationCri.Includes.Add("ConfigurationGroup");
            var configurations = await _configurationService.GetByCri(configurationCri);

            //Customers
            var customers = (await _customerService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.Customers = new SelectList(customers.OrderBy(x => x.Name), "Id", "Name", entity.CustomerId);

            //Agents
            long customerID = 0;
            if (!string.IsNullOrEmpty(currentCustomerId))
            {
                customerID = Convert.ToInt64(currentCustomerId);
            }
            List<EngagerMark4.ApplicationCore.Entities.Users.User> agents = (await _userService.GetByCustomerId(entity.CustomerId == null ? customerID : entity.CustomerId.Value)).ToList();
            if (entity.AgentId == null || entity.AgentId == 0) entity.AgentId = userId;
            ViewBag.Agents = new SelectList(agents.OrderBy(x => x.LastName).ThenBy(x => x.FirstName), "Id", "Name", entity.AgentId);

            //Vessels
            Customer dbcustomer = null;
            if (entity.Id > 0)
                await _customerService.GetById(Convert.ToInt64(currentCustomerId));
            if (dbcustomer == null)
            {
                List<CommonConfiguration> vessels = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.VesselId.ToString()) && x.Id == entity.VesselId).ToList();
                ViewBag.VesselId = new SelectList(vessels, "Id", "Name", entity.VesselId);

                //Vessels for Meeting Service
                ViewBag.Vessels = new SelectList(configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.VesselId.ToString())).OrderBy(x => x.Name), "Id", "Name");
            }
            else
            {
                ViewBag.VesselId = new SelectList(dbcustomer.VesselList, "VesselId", "Vessel", entity.VesselId);

                //Vessels for Meeting Service
                ViewBag.Vessels = new SelectList(dbcustomer.VesselList, "VesselId", "Vessel", entity.VesselId);
            }

            //Board Types
            List<CommonConfiguration> boardTypes = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.BoardType.ToString())).ToList();
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
            List<CommonConfiguration> ranks = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.Rank.ToString())).ToList();
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

            #endregion

            #region Work Order Meeting Services

            //Meeting Services
            var meetingServices = (await _meetingService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.MeetingServices = new SelectList(meetingServices, "Id", "Name");

            //Meeting Service Passengers
            ViewBag.MeetingServicePassengers = new SelectList(entity.WorkOrderPassengerList == null ? new List<WorkOrderPassenger>() : entity.WorkOrderPassengerList, "Name", "Name");

            #endregion

            #region Permissions

            //Permissions
            ViewBag.HasPermissionForCustomerOrderLocation = _rolePermissionService.HasPermission(nameof(CustomerOrderLocationController), User.Identity.GetUserId());
            ViewBag.HasPermissionForCustomerOrderMeetingService = _rolePermissionService.HasPermission(nameof(CustomerOrderMeetingServiceController), User.Identity.GetUserId());
            ViewBag.HasPermissionForCustomerOrderPassenger = _rolePermissionService.HasPermission(nameof(CustomerOrderPassengerController), User.Identity.GetUserId());

            #endregion
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

        public async override Task<ActionResult> Details(Int64 id = 0)
        {
            WorkOrder entity = new WorkOrder();
            try
            {
                if (id != 0)
                    entity = await _service.GetById(id);

                await LoadReferences(entity);

                var entityStr = RouteData.Values["controller"].ToString().Replace("Controller", string.Empty);
                audits.Add(new Audit
                {
                    Description = $"{entityStr} {entity.ToString()} accessed",
                    StartProcessingTime = TimeUtil.GetLocalTime(),
                    Type = Audit.AuditType.Normal
                });

                if(entity.AgentId.HasValue && entity.AgentId.Value > 0)
                {
                    string userId = User.Identity.GetUserId();
                    var currentAgentUser = _userService.GetByApplicatioNId(userId);

                    bool isAllowedToModifyJob = false;

                    if(currentAgentUser != null && _rolePermissionService.HasPermission(nameof(AgentSeeOwnOrdersController), userId))
                    {
                        if (entity.AgentId.Value == currentAgentUser.Id) isAllowedToModifyJob = true;
                    }

                    if (_rolePermissionService.HasPermission(nameof(AgentSeeOwnOrdersController), userId)) isAllowedToModifyJob = true;

                    ViewBag.isAllowedToModifyJob = isAllowedToModifyJob;
                }

                return View(entity);
            }
            catch (Exception ex)
            {
                return View(entity);
            }
        }

        #endregion

        #region Save Work Order

        [HttpPost]
        public JsonResult CheckForSimilarOrders(string PickUpDateBinding, string PickUpTimeBinding, Int64 CustomerId, Int64 VesselId, List<SimliarOrderLocationViewModel> Locations, Int64 currentId)
        {
            DateTime PickUpDate = Util.ConvertStringToDateTime(PickUpDateBinding + " " + PickUpTimeBinding, DateConfig.CULTURE);

            List<WorkOrderLocation> locationList = new List<WorkOrderLocation>();

            if(Locations != null)
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

            if (resultLst.Count == 1)
            {
                if (resultLst.FirstOrDefault().Id == currentId)
                {
                    return Json("NoSimilarOrder");
                }
            }

            foreach (var workOrder in resultLst)
            {
                if (currentId != 0)
                {
                    if (workOrder.Id != currentId)
                    {
                        message += workOrder.RefereneceNo + ", ";
                    }
                }
                else
                {
                    message += workOrder.RefereneceNo + ", ";
                }
            }

            message = message.Remove(message.Length - 2);

            return Json(message);

            //string modifiedPickUpDate = PickUpDate.ToString();
            //string fromTimeStr = PickUpDate.AddHours(-1).ToString();
            //string toTimeStr = PickUpDate.AddHours(1).ToString();
            //return Json("pickupdate : " + PickUpDateBinding + " " + PickUpTimeBinding + ", modified : " + modifiedPickUpDate.ToString() + ", from : " + fromTimeStr + ", to : " + toTimeStr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async override Task<ActionResult> Details(WorkOrder aEntity)
        {   
            LoadAccessInfo();

            var returnUrl = Request["ReturnUrl"];

            try
            {
                ValidateEntity(aEntity);

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

                    }

                    if (aEntity.Status == WorkOrder.OrderStatus.Ordered) IsFirstTimeSubmission = true;

                    if (aEntity.PreviousStatus == WorkOrder.OrderStatus.Ordered) IsFirstTimeSubmission = false;

                    aEntity.isFromOps = true;

                    await SaveEntity(aEntity);

                    AfterSaveMessage();

                    await PushNotification(aEntity);

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

                    //ViewBag.ReturnUrl = returnUrl;                    

                    //ViewBag.CurrentId = aEntity.Id;

                    //if (string.IsNullOrEmpty(returnUrl))
                    //    return RedirectToAction(nameof(Index));
                    //else
                    //    return Redirect(returnUrl);
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                ModelState.AddModelError("", "Cannot insert dupliate record!");
            }
            catch(Exception ex) { 
            
            }

            TempData["color"] = ApplicationConfig.MESSAGE_SAVE_COLOR;
            TempData["message"] = ApplicationConfig.MESSAGE_SAVE_MESSAGE;
            return RedirectToAction("Details", new { id = aEntity.Id, returnUrl = returnUrl  });
        }

        #endregion

        #region Push Notifications

        public async override Task PushNotification(WorkOrder aWorkOrder)
        {
            var notifiedDay = await _systemSettingRepository.GetWorkOrderNotifiedDay();
            var todayDate = TimeUtil.GetLocalTime();
            var notifiedDate = TimeUtil.GetLocalTime().AddDays(notifiedDay);
            if (IsFirstTimeSubmission)
            {
                #region Push Notify to Web Dashboard
                var raiser = _userService.GetByApplicatioNId(aWorkOrder.ModifiedBy);
                var customer = aWorkOrder.CustomerId == null ? new Customer() : await this._customerService.GetById(aWorkOrder.CustomerId);
                Notification notification = new Notification
                {
                    Description = $"New Work Order {aWorkOrder.RefereneceNo} raised by {raiser.Name}.<br/>Customer: {customer.Name} <br/>Pick up Point and Time:{aWorkOrder.PickUpPoint} ({aWorkOrder.PickUpDateOnlyStr} {aWorkOrder.PickUpdateTimeStr})",
                    ReferenceId = aWorkOrder.Id,
                    Type = Notification.NotificationType.WorkOrderByAgent,
                };

                this._notificationRepository.Save(notification);
                this._notificationRepository.SaveChanges();
                var dashboardHubService = new DashboardHubService();
                dashboardHubService.NotifyUpdatesForWorkOrder(notification);
                #endregion
            }
            else
            {
                var raiser = _userService.GetByApplicatioNId(aWorkOrder.ModifiedBy);
                var customer = aWorkOrder.CustomerId == null ? new Customer() : await this._customerService.GetById(aWorkOrder.CustomerId);

                Notification notification = new Notification
                {
                    ReferenceId = aWorkOrder.Id,
                    Type = Notification.NotificationType.WorkOrder,
                };

                if (aWorkOrder.Status == WorkOrder.OrderStatus.Ordered)
                {
                    notification.Description = $"Work Order {aWorkOrder.RefereneceNo} Modified by {raiser.Name}.<br/>Customer: {customer.Name} <br/>Pick up Point and Time:{aWorkOrder.PickUpPoint} ({aWorkOrder.PickUpDateOnlyStr} {aWorkOrder.PickUpdateTimeStr})";
                }

                if (aWorkOrder.Status == WorkOrder.OrderStatus.Cancelled)
                {
                    notification.Description = $"Work Order {aWorkOrder.RefereneceNo} Cancelled by {raiser.Name}.<br/>Customer: {customer.Name} <br/>Pick up Point and Time:{aWorkOrder.PickUpPoint} ({aWorkOrder.PickUpDateOnlyStr} {aWorkOrder.PickUpdateTimeStr})";
                }

                this._notificationRepository.Save(notification);
                this._notificationRepository.SaveChanges();
                var dashboardHubService = new DashboardHubService();
                dashboardHubService.NotifyUpdatesForWorkOrder(notification);
            }
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

        #region Common

        public Int64 GetAgentCustomerId()
        {
            User user = _userService.GetByApplicatioNId(User.Identity.GetUserId());

            Int64 currentCustomerId = _userCustomerService.GetByUserId(user.Id).FirstOrDefault().CustomerId;

            return currentCustomerId;
        }

        #endregion
    }
}