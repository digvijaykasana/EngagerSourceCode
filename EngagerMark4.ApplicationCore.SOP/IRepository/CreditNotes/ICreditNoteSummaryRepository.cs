using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes
{
    public interface ICreditNoteSummaryRepository : IBaseRepository<CreditNoteCri, CreditNoteSummary>
    {
        IEnumerable<CreditNoteSummaryViewModel> GetSummaryViewModel(CreditNoteCri aCri);

        IEnumerable<SalesInvoiceSummary> GetAlreadySavedSalesInvoiceSummaries(CreditNoteCri aCri);

        IEnumerable<CreditNoteSummaryDetails> GetCreditNoteSummariesBySalesInvoiceIds(List<Int64> salesInvoiceSummaryIds);
    }
}
