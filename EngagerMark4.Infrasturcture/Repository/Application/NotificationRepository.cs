using EngagerMark4.ApplicationCore.Cris.Application;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.Repository.Application
{
    public class NotificationRepository : GenericRepository<ApplicationDbContext, NotificationCri, Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public async Task Acknowledge(long notificationId)
        {
            var notification = dbSet.FirstOrDefault(x => x.Id == notificationId);

            if (notification != null)
                notification.Acknowledge = true;

            await this.context.SaveChangesAsync();
        }

        public async override Task<IEnumerable<Notification>> GetByCri(NotificationCri cri)
        {
            return context.Notifications.AsNoTracking().OrderByDescending(x => x.Id).Take(30).AsEnumerable();
        }

        public IEnumerable<Notification> GetModifiedNotificationsByWorkOrderId(long woId)
        {
            return context.Notifications.AsNoTracking().OrderByDescending(x => x.Id).Where(x => x.ReferenceId == woId && x.Type == Notification.NotificationType.WorkOrderUpdated).AsEnumerable();
        }

        public IEnumerable<Notification> GetNotificationsForDriver(long UserId)
        {
            var result = context.Notifications.AsNoTracking().OrderByDescending(x => x.Created).Where(x => x.NotifiedUserId == UserId && x.Acknowledge == false);

            return result;
        }

        public bool HasPendingNotificationByWorkOrder(Int64 woId)
        {
            var notifications = context.Notifications.AsNoTracking().Where(x => x.ReferenceId == woId && x.Type == Notification.NotificationType.WorkOrderUpdated && x.Acknowledge == false);

            return notifications.Count() > 0 ? true : false;
        }
    }
}
