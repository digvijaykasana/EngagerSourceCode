using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.PDFs;
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
    public class MonthlyInvoiceReportByCompanyController : BaseController<SalesInvoiceCri, SalesInvoice, ISalesInvoiceService>
    {
        IMonthlyInvoiceReportRepository _reportRepository;
        ICustomerService _customerService;
        ICompanyService _companyService;

        // GET: MonthlyInvoiceReportByCompany
        public MonthlyInvoiceReportByCompanyController(ISalesInvoiceService service,
                                                       IMonthlyInvoiceReportRepository repository,
                                                       ICustomerService customerService,
                                                       ICompanyService companyService) : base(service)
        {
            this._reportRepository = repository;
            this._customerService = customerService;
            this._companyService = companyService;
        }

        protected async override Task LoadReferencesForList(SalesInvoiceCri aCri)
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
        public async Task<ActionResult> Index(MonthlyInvoiceReportCri aCri)
        {
            try
            {
                var viewModels = this._reportRepository.getInvoices(aCri);

                if (viewModels == null || viewModels.Count() <= 0)
                {
                    TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                    TempData["message"] = "No Invoices generated during this month.";
                    return RedirectToAction(nameof(Index));
                }

                var customer = await _customerService.GetById(aCri.CustomerId);

                List<MonthlyInvoiceReportByCompanyViewModel> invoices = new List<MonthlyInvoiceReportByCompanyViewModel>();

                foreach(var invoice in viewModels)
                {
                    var tempInvoice = invoices.Where(x => x.ReferenceNo.Equals(invoice.ReferenceNo)).FirstOrDefault();

                    if(tempInvoice == null)
                    {
                        tempInvoice = invoice;
                        invoices.Add(tempInvoice);
                    }
                    else
                    {
                        invoices.Remove(tempInvoice);
                        tempInvoice.InvoiceTotalNetAmount += invoice.InvoiceTotalNetAmount;
                        invoices.Add(tempInvoice);
                    }
                }

                //GET COMPANY
                var company = await _companyService.GetById(GlobalVariable.COMPANY_ID);
                if (company == null) company = new ApplicationCore.Entities.Company();

                MonthlyInvoiceReportPDF pdf = new MonthlyInvoiceReportPDF
                {
                    Customer = customer.Name,
                    Address = customer.Address,
                    LastDate = aCri.GetDateTime().AddMonths(1).AddDays(-1),
                    Invoices = invoices.ToList(),
                    HeaderLogo = company.ReportHeaderLogo
                };

                return base.File(base.GeneratePDF<MonthlyInvoiceReportPDF>(pdf, FileConfig.INVOICE_MONTHLY_REPORT, $"Monthly Invoice Report_{customer.Acronym.ToString()}_{pdf.LastDate.ToString("MMM")}_{pdf.LastDate.Year.ToString()}"), CONTENT_DISPOSITION, $"Monthly Invoice Report_{customer.Acronym.ToString()}_{pdf.LastDate.ToString("MMM")}_{pdf.LastDate.Year.ToString()}.pdf");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}