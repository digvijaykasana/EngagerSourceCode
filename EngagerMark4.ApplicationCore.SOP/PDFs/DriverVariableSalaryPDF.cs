using EngagerMark4.ApplicationCore.SOP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.PDFs
{
    public class DriverVariableSalaryPDF
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

        public string YearStr
        {
            get
            {
                return Year.ToString().Substring(2);
            }
        }

        public string MonthStr
        {
            get
            {
                var dateTime = new DateTime(Year, Month, 1);
                return dateTime.ToString("MMM");
            }
        }

        public int MonthInDays
        {
            get
            {
                return DateTime.DaysInMonth(Year, Month);
            }
        }

        public List<DriverVariableSalaryReportViewModel> Models
        {
            get;
            set;
        }
    }
}
