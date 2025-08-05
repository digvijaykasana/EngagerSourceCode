using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IService.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.PDFs;
using EngagerMark4.Common;
using EngagerMark4.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static EngagerMark4.ApplicationCore.Cris.BaseCri;

namespace EngagerMark4.Controllers.SOP.CreditNotes
{
    public class CreditNoteSummaryController : BaseController<CreditNoteCri, CreditNoteSummary, ICreditNoteSummaryService>
    {
        ISalesInvoiceSummaryService salesInvoiceSummaryService;
        ICustomerService customerService;
        ICompanyService companyService;
        IGSTService gstService;

        public CreditNoteSummaryController(ICreditNoteSummaryService service,
            ISalesInvoiceSummaryService salesInvoiceSummaryService,
            ICompanyService companyService,
            ICustomerService customerService,
            IGSTService gstService) : base(service)
        {
            this._defaultColumn = "ReferenceNo";
            this._defaultOrderBy = EntityOrderBy.Dsc.ToString();
            this.salesInvoiceSummaryService = salesInvoiceSummaryService;
            this.customerService = customerService;
            this.companyService = companyService;
            this.gstService = gstService;
        }

        #region Load References For List Page

        protected async override Task LoadReferencesForList(CreditNoteCri aCri)
        {
            var customers = (await this.customerService.GetByCri(null)).OrderBy(x => x.Name);

            ViewBag.CustomerId = new SelectList(customers, "Id", "Name", aCri.CustomerId);
        }

        protected override CreditNoteCri GetCri()
        {

            cri.Includes = new List<string>();
            cri.Includes.Add("Customer");

            ViewBag.ReferenceNo = cri.ReferenceNo;
            ViewBag.CustomerId = cri.CustomerId;
            ViewBag.FromDate = cri.FromDate;
            ViewBag.ToDate = cri.ToDate;
            ViewBag.Status = cri.Status;

            return cri;
        }

        #endregion

        #region Load References For Details Page

        protected async override Task LoadReferences(CreditNoteSummary entity)
        {
            var customers = (await this.customerService.GetByCri(null)).OrderBy(x => x.Name);
            long customerId = 0;
            Int64.TryParse(Request["customerId"], out customerId);
            if (customerId == 0)
            {
                customerId = entity.Id == 0 ? customers.FirstOrDefault() == null ? 0 : customers.FirstOrDefault().Id : entity.CustomerId;
            }
            ViewBag.CustomerId = new SelectList(customers, "Id", "Name", customerId);

            string FromDate = Request["fromDate"];
            string ToDate = Request["toDate"];

            entity.FromDateStr = FromDate;
            entity.ToDateStr = ToDate;

            var salesInvoiceSummaries = await this.salesInvoiceSummaryService.GetByCri(new SalesInvoiceSummaryCri { CustomerId = customerId, FromDate = entity.FromDate, ToDate = entity.ToDate });

            var creditNoteSummaries = this._service.GetCreditNoteSummariesBySalesInvoiceIds(salesInvoiceSummaries.Select(x => x.Id).ToList());

            foreach (var invoice in salesInvoiceSummaries)
            {
                var creditNoteSummary = creditNoteSummaries.Where(x => x.SalesInvoiceSummaryId == invoice.Id).FirstOrDefault();

                if (creditNoteSummary != null && creditNoteSummary.CreditNoteSummary != null)
                {
                    invoice.CreditNoteSummaryNo = creditNoteSummary.CreditNoteSummary.ReferenceNo;
                }
            }

            var alreadySavedSalesInvoiceSummaries = this._service.GetAlreadySavedSalesInvoiceSummaries(new CreditNoteCri { FromDateTime = entity.FromDate.Value, ToDateTime = entity.ToDate.Value, CustomerId = customerId }).Select(x => x.Id).ToList();

            if (entity.Id == 0)
            {
                salesInvoiceSummaries = salesInvoiceSummaries.Where(x => !alreadySavedSalesInvoiceSummaries.Contains(x.Id));
            }


            ViewBag.SalesInvoiceSummaries = salesInvoiceSummaries;
        }

        #endregion

        #region Save Credit Note Summary

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomDetails(CreditNoteSummary aEntity, Int64[] SalesInvoiceIds)
        {
            if (SalesInvoiceIds == null || SalesInvoiceIds.Count() <= 0)
            {
                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                TempData["message"] = "There is no invoice selected!";
                return RedirectToAction(nameof(Details), new { Id = aEntity.Id });
            }

            aEntity.GenerateDetails(SalesInvoiceIds);

            await this._service.Save(aEntity);

            AfterSaveMessage();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Generate PDF

        public async Task<ActionResult> GeneratePDF(Int64 id)
        {
            var cnSummary = await this._service.GetById(id);

            //var company = (await this.companyService.GetByCri(null)).FirstOrDefault();

            var company = await companyService.GetById(GlobalVariable.COMPANY_ID);
            if (company == null) company = new ApplicationCore.Entities.Company();

            var pdf = this._service.GetDetailsReport(new CreditNoteCri { SalesInvoiceSummaryIds = cnSummary.Details.Select(x => x.SalesInvoiceSummaryId).ToList() });

            //Get GST
            var gsts = (await gstService.GetByCri(null)).OrderBy(x => x.Name);

            if (gsts == null || gsts.ToList().Count == 0)
            {
                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                TempData["message"] = "No gst defined yet!";
                return RedirectToAction(nameof(Index));
            }

            //pdf.GSTPercent = gsts.FirstOrDefault().GSTPercent;

            pdf.Date = cnSummary.CreditNoteDate.Value;

            pdf.CreditNoteNo = cnSummary.ReferenceNo;

            pdf.HeaderLogo = company.ReportHeaderLogo;

            var path = GeneratePDF<CreditNoteSummaryPDF>(pdf, FileConfig.CREDIT_NOTE_SUMMARY_REPORT, "CN.pdf");

            return base.File(path, CONTENT_DISPOSITION, "CNSummary.pdf");
        }


        #endregion

    }
}