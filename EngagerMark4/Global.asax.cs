using EngagerMark4.App_Start;
using EngagerMark4.Common.Configs;
using EngagerMark4.DocumentProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Timers;
using EngagerMark4.Infrasturcture.EFContext;
using System.Threading.Tasks;

namespace EngagerMark4
{
    public class MvcApplication : System.Web.HttpApplication
    {

        public Timer removeCancelledWorkOrdersTimer;

        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AsposeConfig.RegisterLicense(Server);

            #region Timers

            //PCR2021 - Remove cancelled work orders
            int removeCancelledWorkOrdersMilliSeconds = 300000; //300 secs | 5 mins;          

            removeCancelledWorkOrdersTimer = new Timer(removeCancelledWorkOrdersMilliSeconds);
            removeCancelledWorkOrdersTimer.Elapsed +=  OnRemoveCancelledWorkOrders;
            removeCancelledWorkOrdersTimer.Enabled = true;

            #endregion
        }

        /// <summary>
        /// Prevent back button after logout
        /// https://stackoverflow.com/questions/19315742/after-logout-if-browser-back-button-press-then-it-go-back-last-screen
        /// </summary>
        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

        //PCR2021 - 
        private static void OnRemoveCancelledWorkOrders(Object source, ElapsedEventArgs e)
        {
            try
            {
                var isTimerOn = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["RemovedCancelledWorkOrders"]);

                if (isTimerOn)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        RemoveCancelledWorkOrders.Start(db);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
