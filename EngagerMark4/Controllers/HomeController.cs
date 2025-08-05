using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.ApplicationCore.ReportViewModels;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Hubs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers
{
    public class HomeController : Controller
    {
        INotificationRepository _repository;
        public HomeController(INotificationRepository repository)
        {
            this._repository = repository;
        }

        public async Task<ActionResult> Index(HomeDashboardCri aCri)
        {
            if (aCri.Month == 0) aCri.Month = TimeUtil.GetLocalTime().Month;
            if (aCri.Year == 0) aCri.Year = TimeUtil.GetLocalTime().Year;
            ViewBag.ChartTypeStr = AngularChartViewModel.LINECHART;

            ViewBag.Month = new SelectList(TimeUtil.GetMonths(), "Id", "Value", aCri.Month);
            ViewBag.Year = new SelectList(TimeUtil.GetYears(), "Id", "Value", aCri.Year);
            var notifications = await this._repository.GetByCri(null);
            ViewBag.Notifications = notifications;
            return View(aCri);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult ChangeData(string text)
        {
            var dashboardHubService = new DashboardHubService();
            //dashboardHubService.NotifyUpdatesForWorkOrder(text);
            return Content("Sent Ok");
        }
    }
}