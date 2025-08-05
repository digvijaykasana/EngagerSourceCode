using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.ViewModels
{
    public class CreditNoteSummaryViewModel
    {
        public string Customer
        {
            get;
            set;
        }

        public string Address
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
                return InvoiceDate.ToString("dd.MM.yyyy"); //Util.ConvertDateToString(InvoiceDate, DateConfig.CULTURE);
            }
        }

        public string ReferenceNo
        {
            get;
            set;
        }

        public string VesselName
        {
            get;
            set;
        }

        public decimal? SubTotal
        {
            get;
            set;
        }

        public decimal? TotalAmt
        {
            get;
            set;
        }

        public decimal? GSTPercent
        {
            get;
            set;
        }
    }
}
