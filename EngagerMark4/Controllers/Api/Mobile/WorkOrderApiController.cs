using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.DTOs;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.ReportViewModels;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs;
using EngagerMark4.ApplicationCore.SOP.IService.Jobs;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Hubs.Services;
using EngagerMark4.Infrasturcture.MobilePushNotifications.FCM;
using EngagerMark4.Models;
using EngagerMark4.MvcFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;

namespace EngagerMark4.Controllers.Api.Account
{
    public class WorkOrderApiController : BaseApiController
    {
        IDriverDailyReportRepository _repository;
        IUserService _userService;
        IWorkOrderService _workOrderService;
        IServiceJobService _serviceJobService;
        ILocationService _locationService;
        IUserVehicleService _userVehicleService;
        ICommonConfigurationService _commonConfigurationService;
        INotificationRepository _notificationRepository;
        ISystemSettingRepository _systemSettingRepository;

        //PCR2021
        IWorkOrderPassengerService _workOrderPassengerService;
        IVehicleService _vehicleService;

        FCMSender _fcmSender;
        Notification notification;

        public WorkOrderApiController(IDriverDailyReportRepository repository,
            IUserService userService,
            IWorkOrderService workOrderService,
            IServiceJobService serviceJobService,
            ILocationService locationService,
            IUserVehicleService userVehicleService,
            ICommonConfigurationService commonConfigurationService,
            INotificationRepository notificationRepository,
            ISystemSettingRepository systemSettingRepository,
            IWorkOrderPassengerService workOrderPassengerService,
            IVehicleService vehicleService,
            FCMSender fcmSender)
        {
            this._repository = repository;
            this._userService = userService;
            this._workOrderService = workOrderService;
            this._serviceJobService = serviceJobService;
            this._locationService = locationService;
            this._userVehicleService = userVehicleService;
            this._commonConfigurationService = commonConfigurationService;
            this._notificationRepository = notificationRepository;
            this._systemSettingRepository = systemSettingRepository;

            //PCR2021
            this._workOrderPassengerService = workOrderPassengerService;
            this._vehicleService = vehicleService;

            this._fcmSender = fcmSender;

        }


        #region Load References

        [MobileApi]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> GetOrderByServiceJobId(Int64 serviceJobId, Int64 userId)
        {
            var workOrders = _repository.GetDriverDailyReportForMobile(new ApplicationCore.Cris.Users.DriverDailyReportCri
            {
                Date = TimeUtil.GetLocalTime(),
                ToDate = TimeUtil.GetLocalTime(),
                DriverId = userId,
                ServiceJobId = serviceJobId
            }).ToList();

            var entity = workOrders.FirstOrDefault();

            WorkOrder workOrder = await _workOrderService.GetById(entity.WorkOrderId);

            foreach (WorkOrderLocation location in workOrder.WorkOrderLocationList)
            {
                if (location.Type.ToString() == "AdditionalStop")
                {
                    if (location.HotelId == null)
                    {
                        entity.AdditionalStops = entity.AdditionalStops + " " + location.Location.Name + ",";
                    }
                    else
                    {
                        entity.AdditionalStops = " " + entity.AdditionalStops + location.Hotel + ",";
                    }
                }

            }

            entity.PickUpPointDesc = workOrder.GetPickUpPointDesc();

            entity.DropOffPointDesc = workOrder.GetDropOffPointDesc();

            entity.AdditionalStops = entity.AdditionalStops.TrimEnd(',').Trim();

            var passengers = workOrder.WorkOrderPassengerList.Where(x => x.Vehicle.VehicleNo == entity.VehicleNo).ToList();

            if (entity.NoOfPax == null)
            {
                entity.NoOfPax = 0;
            }

            WorkOrderPassengerDTO passengerDTO;

            foreach (var passenger in passengers)
            {
                entity.NoOfPax += passenger.NoOfPax;

                //PCR2021
                passengerDTO = new WorkOrderPassengerDTO()
                {
                    Id = passenger.Id,
                    Name = passenger.Name
                };


                if (passenger.IsSigned == WorkOrderPassenger.SignStatus.NotSigned)
                {
                    if (passenger.InCharge)
                    {
                        passengerDTO.Name += " (IC)";

                        entity.WorkOrderPassengerList.Add(passengerDTO);

                    }
                    else
                    {
                        if (entity.TFRequireAllPassSignatures && passenger.ServiceJobId == serviceJobId)
                        {
                            entity.WorkOrderPassengerList.Add(passengerDTO);
                        }
                    }
                }
            }

            var inCharge = passengers.FirstOrDefault(x => x.InCharge);

            if (inCharge != null)
            {
                inCharge = inCharge == null ? new WorkOrderPassenger() : inCharge;

                entity.InChargePassenger = inCharge.Name;

                entity.RankId = inCharge.RankId;

                if (inCharge.RankId != null)
                {
                    var rankEntity = await _commonConfigurationService.GetById(inCharge.RankId);

                    entity.RankStr = rankEntity == null ? "N.A." : rankEntity.Name;
                }
            }
            entity.FlightNo = workOrder.GetFlightNo();

            entity.BoardTypeStr = "N.A.";

            if (workOrder.BoardTypeId != null)
            {
                var boardTypeEntity = await _commonConfigurationService.GetById(workOrder.BoardTypeId);

                if (boardTypeEntity != null)
                {
                    entity.BoardTypeStr = boardTypeEntity.Name.ToString();
                }
            }

            return Json(entity, JsonRequestBehavior.AllowGet);

        }

        [MobileApi]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> GetOrderByWorkOrderId(Int64 workOrderId, Int64 userId)
        {
            var workOrders = _repository.GetDriverDailyReportForMobile(new ApplicationCore.Cris.Users.DriverDailyReportCri
            {
                WorkOrderId = workOrderId,
                DriverId = userId
            }).ToList();

            if (workOrders != null && workOrders.Count > 0)
            {
                var entity = workOrders.FirstOrDefault();

                WorkOrder workOrder = await _workOrderService.GetById(entity.WorkOrderId);

                foreach (WorkOrderLocation location in workOrder.WorkOrderLocationList)
                {
                    if (location.Type.ToString() == "AdditionalStop")
                    {
                        if (location.HotelId == null)
                        {
                            entity.AdditionalStops = entity.AdditionalStops + " " + location.Location.Name + ",";
                        }
                        else
                        {
                            entity.AdditionalStops = " " + entity.AdditionalStops + location.Hotel + ",";
                        }
                    }

                }

                entity.PickUpPointDesc = workOrder.GetPickUpPointDesc();

                entity.DropOffPointDesc = workOrder.GetDropOffPointDesc();

                entity.AdditionalStops = entity.AdditionalStops.TrimEnd(',').Trim();

                var passengers = workOrder.WorkOrderPassengerList.Where(x => x.Vehicle.VehicleNo == entity.VehicleNo).ToList();

                if (entity.NoOfPax == null)
                {
                    entity.NoOfPax = 0;
                }

                foreach (var passenger in passengers)
                {
                    entity.NoOfPax += passenger.NoOfPax;
                }

                var inCharge = passengers.FirstOrDefault(x => x.InCharge);

                if (inCharge != null)
                {
                    inCharge = inCharge == null ? new WorkOrderPassenger() : inCharge;

                    entity.InChargePassenger = inCharge.Name;

                    entity.RankId = inCharge.RankId;

                    if (inCharge.RankId != null)
                    {
                        var rankEntity = await _commonConfigurationService.GetById(inCharge.RankId);

                        entity.RankStr = rankEntity == null ? "N.A." : rankEntity.Name;
                    }
                }
                entity.FlightNo = workOrder.GetFlightNo();

                entity.BoardTypeStr = "N.A.";

                if (workOrder.BoardTypeId != null)
                {
                    var boardTypeEntity = await _commonConfigurationService.GetById(workOrder.BoardTypeId);

                    if (boardTypeEntity != null)
                    {
                        entity.BoardTypeStr = boardTypeEntity.Name.ToString();
                    }
                }

                return Json(entity, JsonRequestBehavior.AllowGet);

            }
            else
            {
                DriverDailyReportViewModel entity = new DriverDailyReportViewModel();

                return Json(entity, JsonRequestBehavior.AllowGet);
            }
        }

        [MobileApi]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> GetOrders(Int64 userId)
        {
            if (!ValidateToken())
            {
                return Content("Invalid Token");
            }

            SetParentCompanyId();

            var workOrders = _repository.GetDriverDailyReportForMobile(new ApplicationCore.Cris.Users.DriverDailyReportCri
            {
                Date = TimeUtil.GetLocalTime(),
                ToDate = TimeUtil.GetLocalTime(),
                DriverId = userId
            }).ToList();

            foreach (DriverDailyReportViewModelMobile entity in workOrders)
            {
                WorkOrder workOrder = await _workOrderService.GetById(entity.WorkOrderId);

                foreach (WorkOrderLocation location in workOrder.WorkOrderLocationList)
                {
                    if (location.Type.ToString() == "AdditionalStop")
                    {
                        string addStopDesc = String.IsNullOrEmpty(location.Description) ? "No Description" : location.Description.ToString();
                        if (location.HotelId == null)
                        {
                            entity.AdditionalStops = entity.AdditionalStops + " " + location.Location.Name + " ~ " + addStopDesc + ",";
                        }
                        else
                        {
                            entity.AdditionalStops = " " + entity.AdditionalStops + location.Hotel + " ~ " + addStopDesc + ",";
                        }
                    }

                }

                entity.PickUpPointDesc = workOrder.GetPickUpPointDesc();

                entity.DropOffPointDesc = workOrder.GetDropOffPointDesc();

                entity.AdditionalStops = entity.AdditionalStops.TrimEnd(',').Trim();

                var passengers = workOrder.WorkOrderPassengerList.Where(x => x.Vehicle.VehicleNo == entity.VehicleNo).ToList();

                if (entity.NoOfPax == null)
                {
                    entity.NoOfPax = 0;
                }

                foreach (var passenger in passengers)
                {
                    if (passenger.InCharge == true)
                    {
                        entity.NoOfPax = passenger.NoOfPax;
                    }
                }

                var inCharge = passengers.FirstOrDefault(x => x.InCharge);

                if (inCharge != null)
                {
                    inCharge = inCharge == null ? new WorkOrderPassenger() : inCharge;

                    entity.InChargePassenger = inCharge.Name;

                    entity.RankId = inCharge.RankId;

                    if (inCharge.RankId != null)
                    {
                        var rankEntity = await _commonConfigurationService.GetById(inCharge.RankId);

                        entity.RankStr = rankEntity == null ? "N.A." : rankEntity.Name;
                    }
                }
                entity.FlightNo = workOrder.GetFlightNo();

                entity.BoardTypeStr = "N.A.";

                if (workOrder.BoardTypeId != null)
                {
                    var boardTypeEntity = await _commonConfigurationService.GetById(workOrder.BoardTypeId);

                    if (boardTypeEntity != null)
                    {
                        entity.BoardTypeStr = boardTypeEntity.Name.ToString();
                    }
                }

            }



            return Json(workOrders, JsonRequestBehavior.AllowGet);
        }

        [MobileApi]
        [AllowAnonymous]
        public async Task<ActionResult> AddPassengers(long workOrderId = 0, long serviceJobId = 0, string token = "", long parentCompanyId = 0)
        {
            if (!ValidateToken())
                return Content("Invalid Access");

            if (workOrderId == 0)
                return Content("No Work Order");

            if (serviceJobId == 0)
                return Content("No Service Job");

            CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            configurationCri.Includes = new List<string>();
            configurationCri.Includes.Add("ConfigurationGroup");
            var configurations = await _commonConfigurationService.GetByCri(configurationCri);
            List<CommonConfiguration> ranks = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.Rank.ToString())).OrderBy(x => x.SerialNo).ToList();
            ViewBag.Ranks = new SelectList(ranks, "Id", "Name");
            ViewBag.WorkOrderId = workOrderId;
            ViewBag.ServiceJobId = serviceJobId;
            WorkOrder workOrder = new WorkOrder();
            var serviceJob = await this._serviceJobService.GetById(serviceJobId);

            if (serviceJob == null) return Content("No Service Job");

            workOrder.WorkOrderPassengerList = await _workOrderService.GetNonInchargePassengersByServiceJobId(workOrderId, serviceJobId);

            return View(workOrder);
        }

        [MobileApi]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> GetRanks()
        {
            if (!ValidateToken())
            {
                return Content("Invalid Token");
            }

            SetParentCompanyId();

            CommonConfigurationCri cri = new CommonConfigurationCri();
            cri.Includes = new List<string>();
            cri.Includes.Add("ConfigurationGroup");
            var configurations = await _commonConfigurationService.GetByCri(cri);

            List<CommonConfiguration> rankConfigs = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.Rank.ToString())).OrderBy(x => x.Name).ToList();

            List<RankMobileViewModel> ranks = new List<RankMobileViewModel>();

            foreach (CommonConfiguration rank in rankConfigs)
            {
                RankMobileViewModel obj = new RankMobileViewModel
                {
                    Id = rank.Id,
                    Code = rank.Code,
                    Name = rank.Name
                };
                ranks.Add(obj);
            }

            return Json(ranks, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Save Data

        public async Task PushNotification(Int64 serviceJobId, string reason, string standbyDate, string standbyTime)
        {
            var notifiedDay = await _systemSettingRepository.GetWorkOrderNotifiedDay();
            var todayDate = TimeUtil.GetLocalTime();
            var notifiedDate = TimeUtil.GetLocalTime().AddDays(notifiedDay);

            #region Push Notify to Web Dashboard
            var serviceJob = await _serviceJobService.GetById(serviceJobId);

            var workOrder = await _workOrderService.GetById(serviceJob.WorkOrderId);

            var userId = _userVehicleService.GetUserIdByVehicleId(serviceJob.VehicleId);

            var driver = _userService.GetByUserId(userId);

            //if(reason != "")
            //{
            //    notification = new Notification
            //    {
            //        Description = $"Service Job for WorkOrder : {workOrder.RefereneceNo} has been ended by {driver.LastName} {driver.FirstName}. " +
            //      $"<br/>Instead of specifying the standby date and time, driver has chosen the following reason: <br/> " +
            //      $"[ <b> {reason} </b> ]",
            //        ReferenceId = workOrder.Id,
            //        Type = Notification.NotificationType.WorkOrder,
            //    };
            //}

            if (standbyDate != "" && standbyTime != "")
            {
                notification = new Notification
                {
                    Description = $"Service Job for WorkOrder : {workOrder.RefereneceNo} has been ended by {driver.LastName} {driver.FirstName}. " +
                  $"<br/>Driver has specified the following as Standby Date and Time: <br/> " +
                  $"[ <b> {standbyDate} - {standbyTime} </b> ]",
                    ReferenceId = workOrder.Id,
                    Type = Notification.NotificationType.DriverSubmission,
                };
            }

            this._notificationRepository.Save(notification);
            this._notificationRepository.SaveChanges();
            var dashboardHubService = new DashboardHubService();
            dashboardHubService.NotifyUpdatesForWorkOrder(notification);
            #endregion
        }

        [MobileApi]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AcknowledgeByDriver(Int64 serviceJobId)
        {
            try
            {
                if (!ValidateToken())
                {
                    return Content("Invalid Token");
                }

                SetParentCompanyId();

                //Get Service Job With no tracking
                var serviceJob = await _serviceJobService.GetById(serviceJobId);

                if (serviceJob.UserId != null && serviceJob.UserId != 0)
                {
                    await SetCurrentUserId(serviceJob.UserId.Value, _userService);
                }

                if (serviceJob != null)
                {
                    if (serviceJob.Status < ServiceJob.ServiceJobStatus.Scheduled)
                    {
                        //Get Work Order
                        var workOrder = await _workOrderService.GetById(serviceJob.WorkOrderId.Value);

                        if (workOrder != null && workOrder.ServiceJobList.Count() > 0)
                        {
                            int count = 0;

                            foreach (ServiceJob job in workOrder.ServiceJobList)
                            {
                                //Set Service Job Status
                                if (job.Id == serviceJob.Id)
                                {
                                    job.Status = ServiceJob.ServiceJobStatus.Scheduled;
                                }

                                if (job.Status >= ServiceJob.ServiceJobStatus.Scheduled)
                                {
                                    count++;
                                }
                                else
                                {
                                    if (job.Id == serviceJobId)
                                    {
                                        count++;
                                    }
                                }
                            }

                            //Set Work Order Status if all Service Jobs have Scheduled status
                            if (count == workOrder.ServiceJobList.Where(x => x.Delete == false).Count())
                            {
                                if (workOrder != null)
                                    workOrder.Status = WorkOrder.OrderStatus.Scheduled;
                            }

                        }

                        await _workOrderService.Save(workOrder);
                    }
                }
                else
                {
                    return HttpNotFound();
                }

                List<General> results = new List<General>();
                General result = new General
                {
                    Id = _successId,
                    Value = "Acknowledged successfully!"
                };
                results.Add(result);

                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [MobileApi]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> BeginTask(Int64 serviceJobId, double longitude, double latitude)
        {
            try
            {
                if (!ValidateToken())
                {
                    return Content("Invalid Token");
                }

                SetParentCompanyId();

                //Get Service Job With no tracking
                var serviceJob = await _serviceJobService.GetById(serviceJobId);

                if (serviceJob.UserId != null && serviceJob.UserId != 0)
                {
                    await SetCurrentUserId(serviceJob.UserId.Value, _userService);
                }

                if (serviceJob != null)
                {
                    if (serviceJob.Status < ServiceJob.ServiceJobStatus.In_Progress)
                    {
                        //Get Work Order
                        var workOrder = await _workOrderService.GetById(serviceJob.WorkOrderId.Value);

                        if (workOrder != null && workOrder.ServiceJobList.Count() > 0)
                        {
                            int count = 0;

                            foreach (ServiceJob job in workOrder.ServiceJobList)
                            {
                                //Update Service Job Status and Details
                                if (job.Id == serviceJob.Id)
                                {
                                    job.Status = ServiceJob.ServiceJobStatus.In_Progress;
                                    var address = GoogleMapUtil.GenerateAddressFromLongAndLati(longitude, latitude);
                                    job.StartExecutionTime = TimeUtil.GetLocalTime();
                                    job.StartExecutionPlace = address;
                                }

                                if (job.Status >= ServiceJob.ServiceJobStatus.In_Progress)
                                {
                                    count++;
                                }
                                else
                                {
                                    if (job.Id == serviceJobId)
                                    {
                                        count++;
                                    }
                                }
                            }

                            //Set Work Order Status if all Service Jobs have In Progress status
                            if (count == workOrder.ServiceJobList.Count())
                            {
                                if (workOrder != null)
                                    workOrder.Status = WorkOrder.OrderStatus.In_Progress;
                            }

                        }

                        await _workOrderService.Save(workOrder);
                    }
                }
                else
                {
                    return HttpNotFound();
                }

                List<General> results = new List<General>();
                General result = new General
                {
                    Id = _successId,
                    Value = "Begin Task successfully!"
                };
                results.Add(result);

                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [MobileApi]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> EndTask(Int64 serviceJobId, double longitude, double latitude, Int64 userId = 0, string signatureName = "", Int64? rankId = 0, int pickupPax = 0,
            string standByDate = "", string standByTime = "", string reason = "")
        {
            try
            {
                if (!ValidateToken())
                {
                    return Content("Invalid Token");
                }

                SetParentCompanyId();

                //Get Service Job with no tracking
                var serviceJob = await _serviceJobService.GetById(serviceJobId);

                if (userId != 0)
                {
                    await SetCurrentUserId(userId, _userService);
                }

                if (serviceJob != null)
                {
                    if (serviceJob.Status < ServiceJob.ServiceJobStatus.Pending_Sign)
                    {
                        //Get Driver
                        var user = await _userService.GetById(userId);
                        if (user == null) user = new ApplicationCore.Entities.Users.User();

                        //Get Work Order
                        WorkOrder workOrder = await _workOrderService.GetById(serviceJob.WorkOrderId);

                        //Get Rank
                        CommonConfiguration rank = await _commonConfigurationService.GetById(rankId);

                        //Update Passenger In Charge
                        WorkOrderPassenger inChargePerson = new WorkOrderPassenger()
                        {
                            Name = signatureName,
                            WorkOrderId = workOrder.Id,
                            InCharge = true,
                            RankId = rankId,
                            Rank = rank.Name,
                            VehicleId = serviceJob.VehicleId,
                            NoOfPax = pickupPax
                        };

                        var inChargePassenger = workOrder.WorkOrderPassengerList.Where(x => x.Name.ToLower().Trim() == inChargePerson.Name.ToLower().Trim() && x.VehicleId == serviceJob.VehicleId).FirstOrDefault();

                        if (!(inChargePassenger == null))
                        {
                            foreach (WorkOrderPassenger passenger in workOrder.WorkOrderPassengerList.Where(x => x.VehicleId == serviceJob.VehicleId))
                            {
                                if (passenger.Name.ToLower().Trim() != inChargePassenger.Name.ToLower().Trim())
                                {
                                    passenger.InCharge = false;
                                }
                                else
                                {
                                    passenger.RankId = rank.Id;
                                    passenger.Rank = rank.Name;
                                    passenger.InCharge = true;
                                }
                            }
                        }
                        else
                        {
                            foreach (WorkOrderPassenger passenger in workOrder.WorkOrderPassengerList.Where(x => x.VehicleId == serviceJob.VehicleId))
                            {
                                passenger.InCharge = false;
                            }

                            workOrder.WorkOrderPassengerList.Add(inChargePerson);
                        }

                        //await _workOrderService.UpdatePassengerInCharge(workOrder, user.ApplicationUserId, inChargePerson, rank);
                        //var updatedWorkOrder = await _workOrderService.GetById(workOrder.Id);

                        if (workOrder != null && workOrder.ServiceJobList.Count() > 0)
                        {
                            int count = 0;

                            foreach (ServiceJob job in workOrder.ServiceJobList)
                            {
                                //Update Service Job Status and Details
                                if (job.Id == serviceJob.Id)
                                {
                                    job.SignatureName = signatureName;

                                    var address = GoogleMapUtil.GenerateAddressFromLongAndLati(longitude, latitude);

                                    job.EndExecutionTime = TimeUtil.GetLocalTime();
                                    job.EndExecutionPlace = address;
                                    job.PickUpPax = pickupPax;

                                    job.Status = ServiceJob.ServiceJobStatus.Pending_Sign;

                                    if (!(standByDate == "" && standByTime == ""))
                                    {
                                        job.LongText1 = standByDate + " - " + standByTime;

                                        /*** Modified - Kaung [ 25-06-2018 ]
                                        workOrder.StandByDateBinding = standByDate;
                                        workOrder.StandByTimeBinding = standByTime;
                                        workOrder.SetStandByDateTime();
                                        **/
                                    }

                                    if (!(reason == ""))
                                    {
                                        job.LongText1 = reason;
                                    }
                                }

                                if (job.Status >= ServiceJob.ServiceJobStatus.In_Progress)
                                {
                                    count++;
                                }
                                else
                                {
                                    if (job.Id == serviceJobId)
                                    {
                                        count++;
                                    }
                                }
                            }

                            //Set Work Order Status if all Service Jobs have In Progress Status
                            if (count == workOrder.ServiceJobList.Count())
                            {
                                if (workOrder != null)
                                    workOrder.Status = WorkOrder.OrderStatus.In_Progress;
                            }
                        }

                        await _workOrderService.Save(workOrder);

                        if ((standByDate != "" && standByTime != "") && serviceJobId != 0)
                        {
                            var task = Task.Run(async () => { await PushNotification(serviceJobId, reason, standByDate, standByTime); });
                            task.Wait();
                        }
                    }
                }
                else
                {
                    return HttpNotFound();
                }

                List<General> results = new List<General>();
                General result = new General
                {
                    Id = _successId,
                    Value = "End Task successfully!"
                };
                results.Add(result);

                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [MobileApi]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SubmitTask(Int64 serviceJobId,
            string checkListIds = "",
            string additionStops = "",
            string disposals = "",
            string waitingTime = "",
            string meetingServiceIds = "",
            Int64 customDetentionId = 0,
            string remark = "",
            int pickupPax = 0, 
            decimal tripFees = 0,
            bool isMSIncluded= false)
        {
            try
            {
                if (!ValidateToken())
                {
                    return Content("Invalid Token");
                }

                SetParentCompanyId();

                var serviceJob = await _serviceJobService.GetById(serviceJobId);

                if (serviceJob.UserId != null && serviceJob.UserId != 0)
                {
                    await SetCurrentUserId(serviceJob.UserId.Value, _userService);
                }

                if (serviceJob != null)
                {
                    if (serviceJob.Status < ServiceJob.ServiceJobStatus.Submitted)
                    {
                        //Get Work Order
                        var workorder = await _workOrderService.GetById(serviceJob.WorkOrderId.Value);

                        if (workorder != null && workorder.ServiceJobList.Count() > 0)
                        {
                            workorder.PickUpPax = 0;

                            int count = 0;

                            foreach (var job in workorder.ServiceJobList)
                            {
                                //Update Service Job Status and Details
                                if(job.Id == serviceJob.Id)
                                {
                                    job.CheckListIds = checkListIds;
                                    job.AdditionalStops = additionStops;
                                    job.Disposals = disposals;
                                    job.WaitingTime = waitingTime;
                                    job.MeetingServiceIds = meetingServiceIds;
                                    job.CustomDetentionId = customDetentionId;
                                    job.DriverRemark = remark;
                                    job.Status = ServiceJob.ServiceJobStatus.Submitted;

                                    job.TripFees = tripFees;

                                    if (isMSIncluded)
                                    {
                                        //Load Driver Default Meeting Service Fee
                                        CommonConfigurationCri cri = new CommonConfigurationCri();
                                        cri.Includes = new List<string>();
                                        cri.Includes.Add("ConfigurationGroup");
                                        var configs = await _commonConfigurationService.GetByCri(cri);
                                        var defaultFee = configs.Where(x => x.ConfigurationGroup.Code == ConfigurationGrpCodes.DriverMeetingServiceFee.ToString()).FirstOrDefault();

                                        if (defaultFee != null)
                                        {
                                            var isDecimal = decimal.TryParse(defaultFee.Name, out decimal defaultFeeVal);

                                            if (isDecimal)
                                            {
                                                job.MSFees = defaultFeeVal;
                                            }
                                        }
                                    }
                                }

                                if (job.Status == ServiceJob.ServiceJobStatus.Submitted)
                                {
                                    count++;

                                    workorder.PickUpPax += job.PickUpPax;
                                }
                                else
                                {
                                    if (job.Id == serviceJobId)
                                    {
                                        count++;
                                    }
                                }
                            }

                            //Set Work Order Status if all Service Jobs have Submitted status
                            if (count == workorder.ServiceJobList.Count())
                                workorder.Status = ApplicationCore.SOP.Entities.WorkOrders.WorkOrder.OrderStatus.Submitted;

                            await _workOrderService.Save(workorder);
                        }
                    }
                }
                else
                {
                    return HttpNotFound();
                }

                List<General> values = new List<General>
            {
                new General{ Id = 200, Value="Submitted successfully!"}
            };

                return Json(values, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [MobileApi]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> AddPassengers(long workOrderId = 0, long serviceJobId = 0, List<WorkOrderPassenger> WorkOrderPassengerList = null)
        {
            var serviceJob = await _serviceJobService.GetById(serviceJobId);
            if (serviceJob == null)
                return Content("No success Job");

            var workOrder = await _workOrderService.GetById(workOrderId);
            var vehicleId = serviceJob.VehicleId == null ? 0 : serviceJob.VehicleId.Value;
            var vehicle = await _vehicleService.GetById(vehicleId);

            try
            {
                if(vehicle != null)
                {
                    foreach (var passenger in WorkOrderPassengerList)
                    {
                        if(passenger.Delete)
                        {
                            if(passenger.Id > 0)
                            {
                                var currentPass = await _workOrderPassengerService.GetById(passenger.Id);

                                    if(currentPass!= null)
                                {
                                    await _workOrderPassengerService.Delete(currentPass);
                                }
                            }
                        }
                        else
                        {
                            passenger.NoOfPax = 1;
                            passenger.VehicleId = vehicleId;
                            passenger.Vehicle = vehicle;
                            passenger.WorkOrderId = workOrderId;
                            passenger.WorkOrder = workOrder;
                            passenger.ServiceJobId = serviceJobId;

                            await _workOrderPassengerService.Save(passenger);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
            }

            if (workOrder != null)
            {
                await _workOrderService.SaveAuditHistoryForWorkOrder(workOrder, false);
            }

            //await this._workOrderService.SavePassengers(workOrderId, vehicleId, WorkOrderPassengerList);

            return Content("SaveSuccess");
        }

        #endregion

    }
}