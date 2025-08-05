using EngagerMark4.ApplicationCore.Cris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Cris
{
    public class MonthlyGeneralInvoiceReportCri : BaseCri
    {
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

        public DateTime GetFromDateTime()
        {
            return new DateTime(Year, Month, 1);
        }
    }
}
