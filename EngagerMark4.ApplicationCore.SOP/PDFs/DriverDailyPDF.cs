using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.ReportViewModels
{
    public class DriverDailyPDF
    {
        public string CompanyName
        {
            get;
            set;
        }

        public string CompanyLogo
        {
            get; set;
        }

        public string DriverName
        {
            get;
            set;
        }

        public string VehicleNo
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


        public List<DriverDailyReportViewModel> Models
        {
            get;
            set;
        }
    }
}
