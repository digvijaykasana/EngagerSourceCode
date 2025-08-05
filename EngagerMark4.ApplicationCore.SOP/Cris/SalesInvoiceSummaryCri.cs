using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Cris
{
    public class SalesInvoiceSummaryCri : BaseCri
    {
        public string SearchValue
        {
            get;
            set;
        }

        public Int64 VesselId
        {
            get;
            set;
        }

        public DateTime? FromDate
        {
            get;
            set;
        }

        public DateTime? ToDate
        {
            get;
            set;
        }

        public Int64 CustomerId
        {
            get;
            set;
        }
    }
}
