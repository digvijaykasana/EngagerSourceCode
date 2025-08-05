using EngagerMark4.ApplicationCore.Entities.Application;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EngagerMark4.Hubs.Services
{
    public class DashboardHubService
    {
        public void NotifyUpdatesForWorkOrder(Notification notification)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<DashboardHub>();

            if(hubContext!=null)
            {
                hubContext.Clients.All.updateWorkOrder(notification);
            }
        }
    }
}