using EngagerMark4.ApplicationCore.DTOs;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.Common.Configs;
using EngagerMark4.Controllers.Api.Account;
using EngagerMark4.MvcFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api.Mobile
{
    public class NotificationApiController : BaseApiController
    {
        INotificationRepository _notificationRepository;
        IWorkOrderService _workOrderService;

        public NotificationApiController(INotificationRepository notificationRepository, IWorkOrderService workOrderService)
        {
            
            this._notificationRepository = notificationRepository;
            this._workOrderService = workOrderService;
        }

        [MobileApi]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetNotificationCount(Int64 userId)
        {
            if (!ValidateToken())
            {
                return Content("Invalid Token");
            }

            SetParentCompanyId();


            var notificationList = _notificationRepository.GetNotificationsForDriver(userId).ToList();


            if (notificationList != null && notificationList.Count() > 0)
            {

                General result = new General()
                {
                    Id = _successId,
                    Value = notificationList.Count().ToString()
                };
                

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                General result = new General()
                {
                    Id = 0,
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [MobileApi]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetNotificationList(Int64 userId)
        {
            if (!ValidateToken())
            {
                return Content("Invalid Token");
            }

            SetParentCompanyId();


            var notificationList = _notificationRepository.GetNotificationsForDriver(userId).ToList();

            List<NotificationDTO> resultLst = new List<NotificationDTO>();

            if(notificationList != null && notificationList.Count() > 0)
            {
               foreach(var item in notificationList)
                {
                    NotificationDTO model = new NotificationDTO()
                    {
                        Id = item.Id,
                        Description = item.Description,
                        DetailedDescription = item.DetailedDescription,
                        ReferenceId = item.ReferenceId
                    };

                    resultLst.Add(model);
                }
            }

            return Json(resultLst, JsonRequestBehavior.AllowGet);
        }

        [MobileApi]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AcknowledgeNotification(Int64 NotificationId)
        {
            if (!ValidateToken())
            {
                return Content("Invalid Token");
            }

            SetParentCompanyId();

            try
            {
                var notification = await _notificationRepository.GetById(NotificationId);

                notification.Acknowledge = true;

                _notificationRepository.Save(notification);

                await _notificationRepository.SaveChangesAsync();

                var workOrder = await _workOrderService.GetById(notification.ReferenceId);

                if (workOrder != null)
                {
                    workOrder.HasPendingNotification = _notificationRepository.HasPendingNotificationByWorkOrder(workOrder.Id);

                    await _workOrderService.Save(workOrder);
                }

                General result = new General()
                {
                    Id = _successId,
                    Value = "success"

                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                General result = new General()
                {
                    Id = 0,
                    Value = "failure"
                };

                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }
    }
}