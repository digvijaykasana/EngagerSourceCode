using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.ViewModels
{
    public class CreditNoteDetailsReportViewModel
    {
        public decimal GSTPercent
        {
            get;
            set;
        }

        public string InvoiceNo
        {
            get;
            set;
        }

        public DateTime InvoiceDate
        {
            get;
            set;
        }

        public string InvoiceDateStr
        {
            get
            {
                return Util.ConvertDateToString(InvoiceDate, DateConfig.CULTURE);
            }
        }

        public string VesselName
        {
            get;
            set;
        }

        public decimal TotalAmount
        {
            get;
            set;
        }
    }
}
