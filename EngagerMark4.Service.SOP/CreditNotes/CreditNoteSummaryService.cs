using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IService.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EngagerMark4.ApplicationCore.SOP.PDFs;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;

namespace EngagerMark4.Service.SOP.CreditNotes
{
    public class CreditNoteSummaryService : AbstractService<ICreditNoteSummaryRepository, CreditNoteCri, CreditNoteSummary>, ICreditNoteSummaryService
    {
        public CreditNoteSummaryService(ICreditNoteSummaryRepository repository) : base(repository) 
        {
        }

        public IEnumerable<SalesInvoiceSummary> GetAlreadySavedSalesInvoiceSummaries(CreditNoteCri aCri)
        {
            return this.repository.GetAlreadySavedSalesInvoiceSummaries(aCri);
        }

        public IEnumerable<CreditNoteSummaryDetails> GetCreditNoteSummariesBySalesInvoiceIds(List<Int64> salesInvoiceSummaryIds)
        {
            return this.repository.GetCreditNoteSummariesBySalesInvoiceIds(salesInvoiceSummaryIds);
        }


        public CreditNoteSummaryPDF GetDetailsReport(CreditNoteCri aCri)
        {
            try
            {
                var list = this.repository.GetSummaryViewModel(aCri);

                var pdf = new CreditNoteSummaryPDF { CreditNotes = list.ToList() };

                pdf.CalculateNoOfPage();

                if (list.Any())
                {
                    pdf.GSTPercent = list.First().GSTPercent.HasValue ? list.First().GSTPercent.Value : 0;
                }

                return pdf;
            }
            catch(Exception ex)
            {
                return new CreditNoteSummaryPDF();
            }
        }
    }
}
