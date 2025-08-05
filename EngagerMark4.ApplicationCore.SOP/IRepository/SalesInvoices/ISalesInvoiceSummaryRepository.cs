using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices
{
    public interface ISalesInvoiceSummaryRepository : IBaseRepository<SalesInvoiceSummaryCri, SalesInvoiceSummary>
    {
        Task<SalesInvoiceSummary> GetByInvoiceNo(string invoiceNo);

        bool IsWorkOrderUnderInvoice(Int64 woId, Int64 invoiceSummaryId);

        long? GetInvoiceVesselId(Int64 Id);

        void RemoveInvoiceSummaryDetailsByWorkOrderId(long id, List<long> workOrderIds);
    }
}
