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
    public class DailyJobByCompanyApiController : Controller
    {
        IDailySummaryJobByCompanyRepository _repository;

        public DailyJobByCompanyApiController(IDailySummaryJobByCompanyRepository repository)
        {
            this._repository = repository;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetData(DailySummaryJobByCompanyCri aCri)
        {
            var viewModels = this._repository.GetReport(aCri);

            int daysInMonth = DateTime.DaysInMonth(aCri.Year, aCri.Month);

            AngularChartViewModel viewModel = new AngularChartViewModel();

            List<string> labels = new List<string>();
            // Prepare Labels
            for (int i = 1; i <= daysInMonth; i++)
            {
                labels.Add(i.ToString());
            }
            viewModel.Labels = labels.ToArray();

            List<string> series = new List<string>();
            Dictionary<string, decimal[]> companyPoints = new Dictionary<string, decimal[]>();
            // Prepare Series
            foreach(var companyData in viewModels)
            {
                series.Add(companyData.CustomerName);
                List<decimal> points = new List<decimal>();
                for(int i = 1;i<=daysInMonth;i++)
                {
                    switch (i)
                    {
                        case 1:
                            points.Add(companyData._1);
                            break;
                        case 2:
                            points.Add(companyData._2);
                            break;
                        case 3:
                            points.Add(companyData._3);
                            break;
                        case 4:
                            points.Add(companyData._4);
                            break;
                        case 5:
                            points.Add(companyData._5);
                            break;
                        case 6:
                            points.Add(companyData._6);
                            break;
                        case 7:
                            points.Add(companyData._7);
                            break;
                        case 8:
                            points.Add(companyData._8);
                            break;
                        case 9:
                            points.Add(companyData._9);
                            break;
                        case 10:
                            points.Add(companyData._10);
                            break;
                        case 11:
                            points.Add(companyData._11);
                            break;
                        case 12:
                            points.Add(companyData._12);
                            break;
                        case 13:
                            points.Add(companyData._13);
                            break;
                        case 14:
                            points.Add(companyData._14);
                            break;
                        case 15:
                            points.Add(companyData._15);
                            break;
                        case 16:
                            points.Add(companyData._16);
                            break;
                        case 17:
                            points.Add(companyData._17);
                            break;
                        case 18:
                            points.Add(companyData._18);
                            break;
                        case 19:
                            points.Add(companyData._19);
                            break;
                        case 20:
                            points.Add(companyData._20);
                            break;
                        case 21:
                            points.Add(companyData._21);
                            break;
                        case 22:
                            points.Add(companyData._22);
                            break;
                        case 23:
                            points.Add(companyData._23);
                            break;
                        case 24:
                            points.Add(companyData._24);
                            break;
                        case 25:
                            points.Add(companyData._25);
                            break;
                        case 26:
                            points.Add(companyData._26);
                            break;
                        case 27:
                            points.Add(companyData._27);
                            break;
                        case 28:
                            points.Add(companyData._28);
                            break;
                        case 29:
                            points.Add(companyData._29);
                            break;
                        case 30:
                            points.Add(companyData._30);
                            break;
                        case 31:
                            points.Add(companyData._31);
                            break;
                        default:
                            points.Add(companyData._1);
                            break;
                    }
                }
                companyPoints[companyData.CustomerName] = points.ToArray();
            }
            viewModel.Series = series.ToArray();
            viewModel.Points = companyPoints;

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
    }
}