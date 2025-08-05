using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.ViewModels
{
    public class BilledOrderListInvoiceViewModel
    {
        public SalesInvoiceSummary  salesInvoiceSummary { get; set; }

        public decimal DiscountPercent { get; set; }

        public decimal DiscountAmt { get; set; }

    }
}
