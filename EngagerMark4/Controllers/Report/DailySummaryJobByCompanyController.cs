using EngagerMark4.ApplicationCore.Cris.Jobs;
using EngagerMark4.ApplicationCore.ReportViewModels;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using EngagerMark4.Common.Utilities;
using EngagerMark4.ApplicationCore.PDFs;
using EngagerMark4.Common.Configs;
using EngagerMark4.DocumentProcessor;

namespace EngagerMark4.Controllers.Report
{
    public class DailySummaryJobByCompanyController : BaseController<WorkOrderCri, WorkOrder, IWorkOrderService>
    {
        IDailySummaryJobByCompanyRepository _repository;

        public DailySummaryJobByCompanyController(IWorkOrderService service,
            IDailySummaryJobByCompanyRepository repository) : base(service)
        {
            this._repository = repository;
        }

        protected async override Task LoadReferencesForList(WorkOrderCri aCri)
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
        public ActionResult Index(DailySummaryJobByCompanyCri aCri)
        {
            var viewModels = this._repository.GetReport(aCri);
            DailySummaryJobCompanyPDF pdf = new DailySummaryJobCompanyPDF
            {
                Month = aCri.Month,
                Year = aCri.Year,
                Jobs = viewModels.ToList()
            };

            switch (aCri.GeneratedReportFormat)
            {
                case ApplicationCore.Cris.BaseCri.ReportFormat.Excel:
                    ExcelProcessor<DailySummaryJobByCompanyViewModel> processor = new ExcelProcessor<DailySummaryJobByCompanyViewModel>();
                    string path = processor.ExportToExcel(pdf.Jobs, Server.MapPath(FileConfig.ExportExcelDailySummaryJobByCompany), KeyUtil.GenerateKey());
                    return base.File(path, CONTENT_DISPOSITION, $"{pdf.MonthStr}_Daily_Summary_Job_By_Company.xls");
                case ApplicationCore.Cris.BaseCri.ReportFormat.PDF:
                    return base.File(base.GeneratePDF<DailySummaryJobCompanyPDF>(pdf, FileConfig.DAILY_SUMMARY_JOB_BY_COMPANY, $"{pdf.MonthStr}_Daily_Summary_Job_By_Company"), CONTENT_DISPOSITION, $"{pdf.MonthStr}_Daily_Summary_Job_By_Company.pdf");
                default:
                    return base.File(base.GeneratePDF<DailySummaryJobCompanyPDF>(pdf, FileConfig.DAILY_SUMMARY_JOB_BY_COMPANY, $"{pdf.MonthStr}_Daily_Summary_Job_By_Company"), CONTENT_DISPOSITION, $"{pdf.MonthStr}_Daily_Summary_Job_By_Company.pdf");
            }
            
        }
    }
}