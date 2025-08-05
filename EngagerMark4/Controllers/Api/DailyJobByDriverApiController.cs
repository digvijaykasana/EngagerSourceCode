using EngagerMark4.ApplicationCore.Cris.Jobs;
using EngagerMark4.ApplicationCore.ReportViewModels;
using EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api
{
    public class DailyJobByDriverApiController : Controller
    {
        IDailySummaryJobByDriverRepository _repository;

        public DailyJobByDriverApiController(IDailySummaryJobByDriverRepository repository)
        {
            this._repository = repository;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetData(DailySummaryJobByDriverCri aCri)
        {
            var viewModels = this._repository.GetReports(aCri);

            var daysInMonth = DateTime.DaysInMonth(aCri.Year, aCri.Month);

            AngularChartViewModel viewModel = new AngularChartViewModel();

            List<string> labels = new List<string>();

            // Prepare Labels
            for(int i = 1;i<= daysInMonth;i++)
            {
                labels.Add(i.ToString());
            }
            viewModel.Labels = labels.ToArray();

            List<string> series = new List<string>();
            Dictionary<string, decimal[]> driverPoints = new Dictionary<string, decimal[]>();

            // Prepare Series and Point
            foreach(var driverData in viewModels)
            {
                series.Add(driverData.DriverName);
                List<decimal> points = new List<decimal>();
                for (int i = 1; i <= daysInMonth; i++)
                {
                    switch (i)
                    {
                        case 1:
                            points.Add(driverData._1);
                            break;
                        case 2:
                            points.Add(driverData._2);
                            break;
                        case 3:
                            points.Add(driverData._3);
                            break;
                        case 4:
                            points.Add(driverData._4);
                            break;
                        case 5:
                            points.Add(driverData._5);
                            break;
                        case 6:
                            points.Add(driverData._6);
                            break;
                        case 7:
                            points.Add(driverData._7);
                            break;
                        case 8:
                            points.Add(driverData._8);
                            break;
                        case 9:
                            points.Add(driverData._9);
                            break;
                        case 10:
                            points.Add(driverData._10);
                            break;
                        case 11:
                            points.Add(driverData._11);
                            break;
                        case 12:
                            points.Add(driverData._12);
                            break;
                        case 13:
                            points.Add(driverData._13);
                            break;
                        case 14:
                            points.Add(driverData._14);
                            break;
                        case 15:
                            points.Add(driverData._15);
                            break;
                        case 16:
                            points.Add(driverData._16);
                            break;
                        case 17:
                            points.Add(driverData._17);
                            break;
                        case 18:
                            points.Add(driverData._18);
                            break;
                        case 19:
                            points.Add(driverData._19);
                            break;
                        case 20:
                            points.Add(driverData._20);
                            break;
                        case 21:
                            points.Add(driverData._21);
                            break;
                        case 22:
                            points.Add(driverData._22);
                            break;
                        case 23:
                            points.Add(driverData._23);
                            break;
                        case 24:
                            points.Add(driverData._24);
                            break;
                        case 25:
                            points.Add(driverData._25);
                            break;
                        case 26:
                            points.Add(driverData._26);
                            break;
                        case 27:
                            points.Add(driverData._27);
                            break;
                        case 28:
                            points.Add(driverData._28);
                            break;
                        case 29:
                            points.Add(driverData._29);
                            break;
                        case 30:
                            points.Add(driverData._30);
                            break;
                        case 31:
                            points.Add(driverData._31);
                            break;
                        default:
                            points.Add(driverData._1);
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(driverData.DriverName))
                    driverPoints[driverData.DriverName] = points.ToArray();
            }
            viewModel.Series = series.ToArray();
            viewModel.Points = driverPoints;

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
    }
}