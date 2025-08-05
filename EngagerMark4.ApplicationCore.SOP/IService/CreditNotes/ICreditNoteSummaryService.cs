using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.PDFs;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IService.CreditNotes
{
    public interface ICreditNoteSummaryService : IBaseService<CreditNoteCri, CreditNoteSummary>
    {
        CreditNoteSummaryPDF GetDetailsReport(CreditNoteCri aCri);

        IEnumerable<SalesInvoiceSummary> GetAlreadySavedSalesInvoiceSummaries(CreditNoteCri aCri);

        IEnumerable<CreditNoteSummaryDetails> GetCreditNoteSummariesBySalesInvoiceIds(List<Int64> salesInvoiceSummaryIds);
    }
}
