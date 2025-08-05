using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace EngagerMark4.Hubs
{
    public class DashboardHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}