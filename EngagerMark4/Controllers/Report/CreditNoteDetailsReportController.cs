using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IService.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
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
    public class CreditNoteDetailsReportController : BaseController<CreditNoteCri,CreditNote, ICreditNoteService>
    {
        ICustomerService _customerService;
        ICompanyService _companyService;

        public CreditNoteDetailsReportController(ICreditNoteService service,
            ICustomerService customerService,
            ICompanyService companyService) : base(service)
        {
            this._customerService = customerService;
            this._companyService = companyService;
        }

        protected async override Task LoadReferencesForList(CreditNoteCri aCri)
        {
            var customers = (await this._customerService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.CustomerId = new SelectList(customers, "Id", "Name");

            ViewBag.StartDate = Request["StartDate"];
            ViewBag.EndDate = Request["EndDate"];
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(CreditNoteCri aCri)
        {
            try
            {
                if(!aCri.ValidateDateRange())
                {
                    ModelState.AddModelError("", "To date must not be less than from date!");
                    await LoadReferencesForList(aCri);
                    return View(aCri);
                }
                var pdf = await this._service.GetDetailsReport(aCri);

                var company = await _companyService.GetById(GlobalVariable.COMPANY_ID);
                if (company == null) company = new ApplicationCore.Entities.Company();

                pdf.HeaderLogo = company.ReportHeaderLogo;

                var customer = await _customerService.GetById(aCri.CustomerId);
                customer = customer == null ? new ApplicationCore.Customer.Entities.Customer() : customer;
                pdf.Customer = customer.Name;
                pdf.Address = customer.Address;
                pdf.Date = TimeUtil.GetLocalTime();

                return base.File(base.GeneratePDF<CreditNoteReportPDF>(pdf, FileConfig.CREDIT_NOTE_MONTHLY_REPORT, $"CN"), CONTENT_DISPOSITION, $"CN.pdf");
            }
            catch(Exception ex)
            {
                return null;
            }
        }

    }
}