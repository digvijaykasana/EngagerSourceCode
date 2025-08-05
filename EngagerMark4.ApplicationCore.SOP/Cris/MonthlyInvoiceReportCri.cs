using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Cris
{
    public class MonthlyInvoiceReportCri
    {
        public Int64 CustomerId
        {
            get;
            set;
        }

        public int Month
        {
            get;
            set;
        }

        public int Year
        {
            get;
            set;
        }

        public DateTime GetDateTime()
        {
            return new DateTime(Year, Month, 1);
        }
    }
}
