using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.ReportViewModels
{
    public class DriverDailyPDFByDay
    {
        public string DriverName
        {
            get;
            set;
        }

        public string DriverNRIC
        {
            get;
            set;
        }

        public string FromDate
        {
            get;
            set;
        }

        public string ToDate
        {
            get;
            set;
        }


        public List<DriverDailyReportByDayViewModel> Models
        {
            get;
            set;
        }
    }
}
