using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.Excels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices
{
    public interface ISalesInvoiceRepository : IBaseRepository<SalesInvoiceCri, SalesInvoice>
    {
        List<InvoiceExcel> GetData(Int64[] ids);

        Task<IEnumerable<SalesInvoice>> GetByInvoiceNo(string invoiceNo);

        long? SaveVersionedInvoice(SalesInvoice entity);

        void UpdateVersions(long? workOrderId, long? VersionNumber);

        long? GetInvoiceDetailVesselId(Int64 Id);

        Task<bool> UpdateVesselIdAndVesselName(long VesselId, string VesselName, long? Id);
    }
}
