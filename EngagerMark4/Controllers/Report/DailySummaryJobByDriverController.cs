using EngagerMark4.ApplicationCore.Cris.Jobs;
using EngagerMark4.ApplicationCore.PDFs;
using EngagerMark4.ApplicationCore.ReportViewModels;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs;
using EngagerMark4.ApplicationCore.SOP.IService.Jobs;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.DocumentProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Report
{
    public class DailySummaryJobByDriverController : BaseController<ServiceJobCri, ServiceJob, IServiceJobService>
    {
        IDailySummaryJobByDriverRepository _repository;

        public DailySummaryJobByDriverController(IServiceJobService service,
            IDailySummaryJobByDriverRepository repository) : base(service)
        {
            this._repository = repository;
        }

        protected async override Task LoadReferencesForList(ServiceJobCri aCri)
        {
            string monthStr = Request["Month"];
            string yearStr = Request["Year"];

            int month = string.IsNullOrEmpty(monthStr) ? TimeUtil.GetLocalTime().Month : Convert.ToInt32(monthStr);
            int year = string.IsNullOrEmpty(yearStr) ? TimeUtil.GetLocalTime().Year : Convert.ToInt32(yearStr);
            ViewBag.Month = new SelectList(TimeUtil.GetMonths(), "Id", "Value", month);
            ViewBag.Year = new SelectList(TimeUtil.GetYears(), "Id", "Value", year);
            await base.LoadReferencesForList(aCri);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(DailySummaryJobByDriverCri aCri)
        {
            var viewModels = this._repository.GetReports(aCri);

            DailySummaryJobDriverPDF pdf = new DailySummaryJobDriverPDF
            {
                Month = aCri.Month,
                Year = aCri.Year,
                Jobs = viewModels.ToList()
            };

            switch (aCri.GeneratedReportFormat)
            {
                case ApplicationCore.Cris.BaseCri.ReportFormat.Excel:
                    ExcelProcessor<DailySummaryJobByDriverViewModel> processor = new ExcelProcessor<DailySummaryJobByDriverViewModel>();
                    var path = processor.ExportToExcel(pdf.Jobs, Server.MapPath(FileConfig.ExportExcelDailySummaryJobByDriver), KeyUtil.GenerateKey());
                    return base.File(path, CONTENT_DISPOSITION, $"{pdf.MonthStr}_Daily_Summary_Job_By_Driver.xls");
                case ApplicationCore.Cris.BaseCri.ReportFormat.PDF:
                    return base.File(base.GeneratePDF<DailySummaryJobDriverPDF>(pdf, FileConfig.DAILY_SUMMARY_JOB_BY_DRIVER, $"{pdf.MonthStr}_Daily_Summary_Job_By_Driver"), CONTENT_DISPOSITION, $"{pdf.MonthStr}_Daily_Summary_Job_By_Driver.pdf");
                default:
                    return base.File(base.GeneratePDF<DailySummaryJobDriverPDF>(pdf, FileConfig.DAILY_SUMMARY_JOB_BY_DRIVER, $"{pdf.MonthStr}_Daily_Summary_Job_By_Driver"), CONTENT_DISPOSITION, $"{pdf.MonthStr}_Daily_Summary_Job_By_Driver.pdf");
            }
        }
    }
}