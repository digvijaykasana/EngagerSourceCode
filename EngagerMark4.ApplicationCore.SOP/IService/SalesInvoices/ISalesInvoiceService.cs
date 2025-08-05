using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.Excels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices
{
    public interface ISalesInvoiceService : IBaseService<SalesInvoiceCri, SalesInvoice>
    {
        Task<IEnumerable<SalesInvoice>> GetByInvoiceNo(string invoiceNo);

        Task PreparePriceListFromTransferVoucher(WorkOrder workOrder, List<Price> prices);

        Task PreparePriceListForMeetingService(WorkOrder workOrder, List<Price> prices);

        Task PreparePriceList(WorkOrder workOrder, List<Price> prices);

        Task<Int64> UpdatePriceList(SalesInvoice invoice, List<Price> prices);
    }
}
