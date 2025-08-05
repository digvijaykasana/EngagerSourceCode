using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices
{
    public interface ISalesInvoiceSummaryService : IBaseService<SalesInvoiceSummaryCri, SalesInvoiceSummary>
    {
        Task<SalesInvoiceSummary> GetByInvoiceNo(string aInvoiceNo);

        bool IsWorkOrderUnderInvoice(Int64 woId, Int64 invoiceSummaryId);

        bool Get(Int64 vesselId, Int64 invoiceSummaryId);

        long GetInvoiceVesselId(Int64 Id);
    }
}
