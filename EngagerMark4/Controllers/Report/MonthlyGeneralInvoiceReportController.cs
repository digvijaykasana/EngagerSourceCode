using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.PDFs;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
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
    public class MonthlyGeneralInvoiceReportController : BaseController<WorkOrderCri, WorkOrder, IWorkOrderService>
    {
        IMonthlyGeneralInvoiceReportRepository _repository;

        // GET: MonthlyGeneralInvoiceReport
        public MonthlyGeneralInvoiceReportController(IWorkOrderService service,
            IMonthlyGeneralInvoiceReportRepository repository) : base(service)
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
        public ActionResult Index(MonthlyGeneralInvoiceReportCri aCri)
        {
            try
            {
                var viewModels = this._repository.GetReport(aCri);
                MonthlyGeneralInvoiceReportPDF pdf = new MonthlyGeneralInvoiceReportPDF
                {
                    Month = aCri.Month,
                    Year = aCri.Year,
                    Invoices = viewModels.ToList()
                };

                switch (aCri.GeneratedReportFormat)
                {
                    case ApplicationCore.Cris.BaseCri.ReportFormat.Excel:
                        ExcelProcessor<MonthlyGeneralInvoiceReportViewModel> processor = new ExcelProcessor<MonthlyGeneralInvoiceReportViewModel>();
                        string path = processor.ExportToExcel(pdf.Invoices, Server.MapPath(FileConfig.ExportExcelMonthlyGeneralInvoiceReport), KeyUtil.GenerateKey());
                        return base.File(path, CONTENT_DISPOSITION, $"{pdf.MonthStr}_Daily_Summary_Job_By_Company.xls");
                    case ApplicationCore.Cris.BaseCri.ReportFormat.PDF:
                        return base.File(base.GeneratePDF<MonthlyGeneralInvoiceReportPDF>(pdf, FileConfig.MonthlyGeneralInvoiceReport, $"{pdf.MonthStr}_Monthly_Invoice_Report"), CONTENT_DISPOSITION, $"{pdf.MonthStr}_Monthly_Invoice_Report.pdf");
                    default:
                        return base.File(base.GeneratePDF<MonthlyGeneralInvoiceReportPDF>(pdf, FileConfig.MonthlyGeneralInvoiceReport, $"{pdf.MonthStr}_Monthly_Invoice_Report"), CONTENT_DISPOSITION, $"{pdf.MonthStr}_Monthly_Invoice_Report.pdf");
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}