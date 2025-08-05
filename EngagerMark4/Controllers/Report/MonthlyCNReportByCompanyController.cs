using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IService.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.PDFs;
using EngagerMark4.Common;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Report
{
    public class MonthlyCNReportByCompanyController : BaseController<CreditNoteCri, CreditNote, ICreditNoteService>
    {

        IMonthlyCreditNoteReportRepository _repository;
        ICustomerService _customerService;
        ICompanyService _companyService;

        // GET: MonthlyInvoiceReportByCompany
        public MonthlyCNReportByCompanyController(ICreditNoteService service,
                                                  IMonthlyCreditNoteReportRepository repository,
                                                  ICustomerService customerService,
                                                  ICompanyService companyService) : base(service)
        {
            this._repository = repository;
            this._customerService = customerService;
            this._companyService = companyService;
        }

        protected async override Task LoadReferencesForList(CreditNoteCri aCri)
        {
            var customers = (await this._customerService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.CustomerId = new SelectList(customers, "Id", "Name");

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
        public async Task<ActionResult> Index(MonthlyCreditNoteCri aCri)
        {
            try
            {
                var creditNotes = this._repository.getCreditNotes(aCri);

                if (creditNotes == null || creditNotes.Count() <= 0)
                {
                    TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                    TempData["message"] = "No Credit Notes generated during this month.";
                    return RedirectToAction(nameof(Index));
                }

                CreditNote tempCN = new CreditNote();

                tempCN = creditNotes.FirstOrDefault();

                var customer = await _customerService.GetById(tempCN.CustomerId);

                var company = await _companyService.GetById(GlobalVariable.COMPANY_ID);
                if (company == null) company = new ApplicationCore.Entities.Company();

                MonthlyCreditNoteReportPDF cnPDF = new MonthlyCreditNoteReportPDF();

                cnPDF.LastDate = aCri.GetDateTime().AddMonths(1).AddDays(-1);
                cnPDF.Customer = customer.Name;
                cnPDF.Address = customer.Address;
                cnPDF.HeaderLogo = company.ReportHeaderLogo;

                cnPDF.TotalAmount = 0;
                
                if(creditNotes.Count > 0)
                {
                    foreach (var creditNote in creditNotes)
                    {
                        cnPDF.TotalAmount += creditNote.SubTotal;
                    }
                }
                
                return base.File(base.GeneratePDF<MonthlyCreditNoteReportPDF>(cnPDF, FileConfig.CREDIT_NOTE_MONTHLY_REPORT, $"Monthly Credit Note Report_{customer.Acronym.ToString()}_{cnPDF.LastDate.ToString("MMM")}_{cnPDF.LastDate.Year.ToString()}"), CONTENT_DISPOSITION, $"Monthly Credit Note_{customer.Acronym.ToString()}_{cnPDF.LastDate.ToString("MMM")}_{cnPDF.LastDate.Year.ToString()}.pdf");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Cannot insert dupliate record!");
                return View(aCri);
            }
        }
    }
}