using EngagerMark4.ApplicationCore.SOP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.PDFs
{
    public class InvoiceSummaryReportPDF
    {
        public string FromDateStr
        {
            get;
            set;
        }

        public string ToDateStr
        {
            get;
            set;
        }

        public List<InvoiceSummaryReportViewModel> Invoices
        {
            get;
            set;
        }
    }
}
