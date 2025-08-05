using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Cris.Jobs
{
    public class DailySummaryJobByDriverCri : BaseCri
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

        public DateTime GetDateTime()
        {
            return new DateTime(Year, Month, 1);
        }
    }
}
