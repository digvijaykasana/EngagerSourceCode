using EngagerMark4.ApplicationCore.ReportViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.PDFs
{
    public class DailySummaryJobCompanyPDF
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

        public string MonthStr
        {
            get
            {
                var dateTime = new DateTime(Year, Month, 1);
                return dateTime.ToString("MMMM");
            }
        }

        public int MonthInDays
        {
            get
            {
                return DateTime.DaysInMonth(Year, Month);
            }
        }

        public List<DailySummaryJobByCompanyViewModel> Jobs
        {
            get;
            set;
        }
    }
}
