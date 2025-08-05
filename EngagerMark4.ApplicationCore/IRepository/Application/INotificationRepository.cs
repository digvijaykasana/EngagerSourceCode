using EngagerMark4.ApplicationCore.Cris.Application;
using EngagerMark4.ApplicationCore.Entities.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IRepository.Application
{
    public interface INotificationRepository : IBaseRepository<NotificationCri, Notification>
    {
        Task Acknowledge(long notificationId);

        IEnumerable<Notification> GetNotificationsForDriver(Int64 UserId);

        IEnumerable<Notification> GetModifiedNotificationsByWorkOrderId(Int64 woId);

        bool HasPendingNotificationByWorkOrder(Int64 woId);
    }
}
