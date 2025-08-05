using EngagerMark4.ApplicationCore.Cris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Cris
{
    public class InvoiceSummaryReportCri : BaseCri
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
