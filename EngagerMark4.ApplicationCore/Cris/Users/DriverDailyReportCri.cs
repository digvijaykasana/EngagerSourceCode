using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Cris.Users
{
    public class DriverDailyReportCri : BaseCri
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
                if(!string.IsNullOrEmpty(value))
                {
                    Date = Util.ConvertStringToDateTime(value, DateConfig.CULTURE);
                }
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

        public Int64? DriverId
        {
            get;
            set;
        }

        public Int64? WorkOrderId
        {
            get;
            set;
        }

        public Int64? ServiceJobId
        {
            get;
            set;
        }
    }
}
