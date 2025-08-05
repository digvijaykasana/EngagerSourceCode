using Aspose.Cells;
using EngagerMark4.ApplicationCore.Excels;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.IService.Application;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.Excels;
using EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.PDFs;
using EngagerMark4.Common.Configs;
using EngagerMark4.DocumentProcessor;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers
{
    public class TestController : BaseController<CreditNoteCri, CreditNote,ICreditNoteService>
    {
        ISMTPService _service;
        ISalesInvoiceRepository _salesInvoiceRepository;
        ICreditNoteSummaryRepository _creditNoteSummaryRepository;
        ICompanyService companyService;

        ApplicationDbContext db;

        public TestController(ICreditNoteService service, ISMTPService smtpservice,
            ISalesInvoiceRepository salesInvoiceRepository,
            ICreditNoteSummaryRepository creditNoteSummaryRepository,
            ICompanyService companyService,
            ApplicationDbContext db) : base(service)
        {
            this._service = smtpservice;
            this._salesInvoiceRepository = salesInvoiceRepository;
            this._creditNoteSummaryRepository = creditNoteSummaryRepository;
            this.db = db;
            this.companyService = companyService;
        }

        [AllowAnonymous]
        public async Task<ActionResult> GetSMTP()
        {
            var smtp = (await _service.GetByCri(null)).FirstOrDefault();

            return Json(smtp, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult ImportExcel()
        {
            ExcelProcessor<ExcelTest> excelProcessor = new ExcelProcessor<ExcelTest>();
            var objs =  excelProcessor.ImportFromExcel(@"D:\Test_1.xls");
            return Json(objs, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult TestQuery()
        {
            List<long> lists = new List<long>();
            lists.Add(1);
            lists.Add(2);
            return Json(_salesInvoiceRepository.GetData(lists.ToArray()), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult DownloadExcel2()
        {
            ExcelProcessor<InvoiceExcel> excelProcessor = new ExcelProcessor<InvoiceExcel>();

            var path = Server.MapPath(FileConfig.EXPORT_EXCEL_INVOICES);

            List<long> ids = new List<long>();
            ids.Add(1);
            ids.Add(2);

            var fullPath = excelProcessor.ExportToExcel(_salesInvoiceRepository.GetData(ids.ToArray()), path, Guid.NewGuid().ToString(), false);

            return base.File(fullPath, "content-disposition", "Test.xls");
        }

        [AllowAnonymous]
        public ActionResult DownloadExceL()
        {
            var path = Server.MapPath(FileConfig.EXPORT_EXCEL);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Instantiating a Workbook object
            Workbook workbook = new Workbook();

            // Obtaining the reference of the first worksheet
            Worksheet worksheet = workbook.Worksheets[0];

            //// Adding a string value to the cell
            //worksheet.Cells["A1"].PutValue("Hello World");
            worksheet.Cells[0, 0].PutValue("Hellow World");

            worksheet.Cells[0, 1].PutValue("Fuck");

            //// Adding a double value to the cell
            //worksheet.Cells["A2"].PutValue(20.5);

            //// Adding an integer  value to the cell
            //worksheet.Cells["A3"].PutValue(15);

            //// Adding a boolean value to the cell
            //worksheet.Cells["A4"].PutValue(true);

            //// Adding a date/time value to the cell
            //worksheet.Cells["A5"].PutValue(DateTime.Now);

            
            // Setting the display format of the date
            Style style = worksheet.Cells["A5"].GetStyle();
            style.Number = 15;
            worksheet.Cells["A5"].SetStyle(style);

            // Saving the Excel file
            workbook.Save(path + "output.out.xls");

            var filePath = path + "output.out.xls";

            return base.File(filePath, "content-disposition", "fact.xls");
        }

        [AllowAnonymous]
        public async Task<ActionResult> TestGroupBy()
        {
            var company = db.Companies.FirstOrDefault();

            var list = this._creditNoteSummaryRepository.GetSummaryViewModel(null).ToList();

            var pdf = new CreditNoteSummaryPDF { CreditNotes = list.ToList() };

            pdf.HeaderLogo = company.ReportHeaderLogo;

            pdf.CalculateNoOfPage();

            var path = GeneratePDF<CreditNoteSummaryPDF>(pdf, FileConfig.CREDIT_NOTE_SUMMARY_REPORT, "CN.pdf");

            return Json(this._creditNoteSummaryRepository.GetSummaryViewModel(null), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult OpenPDF()
        {
            return View();
        }

        [AllowAnonymous]
        public FileResult GetReport()
        {
            string ReportURL = @"D:\Users\fxuser\Documents\Temperature Monitoring Log";
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, "application/pdf");
        }
    }
}