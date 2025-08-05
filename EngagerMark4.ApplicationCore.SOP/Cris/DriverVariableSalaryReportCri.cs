using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Cris
{
    public class DriverVariableSalaryReportCri : BaseCri
    {

        public DateTime? Date
        {
            get;
            set;
        }

        public string DateStr
        {
            get
            {
                return Date == null ? string.Empty : Util.ConvertDateToString(Date.Value, DateConfig.CULTURE);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Date = Util.ConvertStringToDateTime(value, DateConfig.CULTURE);
                }
            }
        }

        public string ReportFromDateStr
        {
            get
            {
                return Date == null ? string.Empty : Util.ConvertDateToFormattedString(Date.Value, DateConfig.US_CULTURE, "ddMMM");
            }
        }

        public DateTime? ToDate
        {
            get;
            set;
        }

        public string ToDateStr
        {
            get
            {
                return ToDate == null ? string.Empty : Util.ConvertDateToString(ToDate.Value, DateConfig.CULTURE);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ToDate = Util.ConvertStringToDateTime(value, DateConfig.CULTURE);
                }
            }
        }
        public string ReportToDateStr
        {
            get
            {
                return ToDate == null ? string.Empty : Util.ConvertDateToFormattedString(ToDate.Value, DateConfig.US_CULTURE, "ddMMM");
            }
        }

        //OBSOLETE - Using From Date and To Date
        public int Month
        {
            get;
            set;
        }

        //OBSOLETE - Using From Date and To Date
        public int Year
        {
            get;
            set;
        }

        //OBSOLETE - Using From Date and To Date
        public DateTime GetFromDateTime()
        {
            return new DateTime(Year, Month, 1);
        }
    }
}
