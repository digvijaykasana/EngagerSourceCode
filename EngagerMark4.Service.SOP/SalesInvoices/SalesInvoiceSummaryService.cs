using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.SOP.SalesInvoices
{
    public class SalesInvoiceSummaryService : AbstractService<ISalesInvoiceSummaryRepository, SalesInvoiceSummaryCri, SalesInvoiceSummary>, ISalesInvoiceSummaryService
    {
        public SalesInvoiceSummaryService(ISalesInvoiceSummaryRepository repository) : base(repository)
        {
        }

        public bool Get(long vesselId, long invoiceSummaryId)
        {
            throw new NotImplementedException();
        }

        public async Task<SalesInvoiceSummary> GetByInvoiceNo(string aInvoiceNo)
        {
            return await this.repository.GetByInvoiceNo(aInvoiceNo);
        }

        public long GetInvoiceVesselId(long Id)
        {
            var vesselId = this.repository.GetInvoiceVesselId(Id);

            return vesselId.HasValue ? vesselId.Value : 0 ;
        }

        public bool IsWorkOrderUnderInvoice(long woId, long invoiceSummaryId)
        {
            return this.repository.IsWorkOrderUnderInvoice(woId, invoiceSummaryId);
        }
    }
}
