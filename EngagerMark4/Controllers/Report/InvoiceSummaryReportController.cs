using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.PDFs;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.PDFs;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.DocumentProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Report
{
    public class InvoiceSummaryReportController : BaseController<SalesInvoiceCri, SalesInvoice, ISalesInvoiceService>
    {
        IInvoiceSummaryReportRepository _reportRepository;

        // GET: InvoiceSummaryReport
        public InvoiceSummaryReportController(ISalesInvoiceService service,
            IInvoiceSummaryReportRepository repository) : base(service)
        {
            this._reportRepository = repository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(InvoiceSummaryReportCri aCri)
        {


            try
            {
                var viewModels = this._reportRepository.getInvoices(aCri);

                List<InvoiceSummaryReportViewModel> invoices = new List<InvoiceSummaryReportViewModel>();

                foreach (var invoice in viewModels)
                {
                    var tempInvoice = invoices.Where(x => x.ReferenceNo.Equals(invoice.ReferenceNo)).FirstOrDefault();

                    if (tempInvoice == null)
                    {
                        tempInvoice = invoice;
                        invoices.Add(tempInvoice);
                    }
                    else
                    {
                        invoices.Remove(tempInvoice);
                        tempInvoice.TotalAmt += invoice.TotalAmt;
                        tempInvoice.GSTAmount += invoice.GSTAmount;
                        tempInvoice.InvoiceTotalNetAmount += invoice.InvoiceTotalNetAmount;
                        tempInvoice.cnTotalAmt += invoice.cnTotalAmt;
                        tempInvoice.cnGSTAmount += invoice.cnGSTAmount;
                        tempInvoice.cnGrandTotalAmount += invoice.cnGrandTotalAmount;
                        invoices.Add(tempInvoice);
                    }
                }

                InvoiceSummaryReportPDF pdf = new InvoiceSummaryReportPDF
                {
                    FromDateStr = aCri.FromDate,
                    ToDateStr = aCri.ToDate,
                    Invoices = invoices.ToList()
                };

                switch (aCri.GeneratedReportFormat)
                {
                    case ApplicationCore.Cris.BaseCri.ReportFormat.Excel:
                        ExcelProcessor<InvoiceSummaryReportViewModel> processor = new ExcelProcessor<InvoiceSummaryReportViewModel>();
                        string path = processor.ExportToExcel(pdf.Invoices, Server.MapPath(FileConfig.ExportExcelInvoiceSummary), KeyUtil.GenerateKey());
                        return base.File(path, CONTENT_DISPOSITION, $"Invoice Summary Report_{pdf.FromDateStr}_{pdf.ToDateStr}.xls");
                    case ApplicationCore.Cris.BaseCri.ReportFormat.PDF:
                        return base.File(base.GeneratePDF<InvoiceSummaryReportPDF>(pdf, FileConfig.DAILY_SUMMARY_JOB_BY_DRIVER, $"Invoice Summary Report_{pdf.FromDateStr}_{pdf.ToDateStr}"), CONTENT_DISPOSITION, $"Invoice Summary Report_{pdf.FromDateStr}_{pdf.ToDateStr}.pdf");
                    default:
                        return base.File(base.GeneratePDF<InvoiceSummaryReportPDF>(pdf, FileConfig.DAILY_SUMMARY_JOB_BY_DRIVER, $"Invoice Summary Report_{pdf.FromDateStr}_{pdf.ToDateStr}"), CONTENT_DISPOSITION, $"Invoice Summary Report_{pdf.FromDateStr}_{pdf.ToDateStr}.pdf");
                }

            }
            catch(Exception ex)
            {
                return null;
            }         
        }

    }
}