using EngagerMark4.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Common.Utilities
{
    /// <summary>
    /// FUJI XEROX INTERNAL USE ONLY<<RESTRICTED>>
    /// Disclose to : PSTG T&T Team
    /// Protected until:
    /// Author: Kyaw Min Htut
    /// Prepared on: 2-12-2015
    /// </summary>
    public class TimeUtil
    {
        /// <summary>
        /// Get hours of a day. This hours are to be used in notification settings
        /// Created by: Kyaw Min Htut
        /// Created date: 31-03-2015
        /// </summary>
        /// <returns></returns>
        //public static List<Time> GetTimes()
        //{
        //    List<Time> times = new List<Time>();

        //    Time _600AM = new Time();
        //    _600AM.Name = "6:00 AM";
        //    times.Add(_600AM);

        //    Time _630AM = new Time();
        //    _630AM.Name = "6:30 AM";
        //    times.Add(_630AM);

        //    Time _700AM = new Time();
        //    _700AM.Name = "7:00 AM";
        //    times.Add(_700AM);

        //    Time _730AM = new Time();
        //    _730AM.Name = "7:30 AM";
        //    times.Add(_730AM);

        //    Time _800AM = new Time();
        //    _800AM.Name = "8:00 AM";
        //    times.Add(_800AM);

        //    Time _830AM = new Time();
        //    _830AM.Name = "8:30 AM";
        //    times.Add(_830AM);

        //    Time _900AM = new Time();
        //    _900AM.Name = "9:00 AM";
        //    times.Add(_900AM);

        //    Time _930AM = new Time();
        //    _930AM.Name = "9:30 AM";
        //    times.Add(_930AM);

        //    Time _1000AM = new Time();
        //    _1000AM.Name = "10:00 AM";
        //    times.Add(_1000AM);

        //    Time _1030AM = new Time();
        //    _1030AM.Name = "10:30 AM";
        //    times.Add(_1030AM);

        //    Time _1100AM = new Time();
        //    _1100AM.Name = "11:00 AM";
        //    times.Add(_1100AM);

        //    Time _1130AM = new Time();
        //    _1130AM.Name = "11:30 AM";
        //    times.Add(_1130AM);

        //    Time _1200PM = new Time();
        //    _1200PM.Name = "12:00 PM";
        //    times.Add(_1200PM);

        //    Time _1230PM = new Time();
        //    _1230PM.Name = "12:30 PM";
        //    times.Add(_1230PM);

        //    Time _100PM = new Time();
        //    _100PM.Name = "1:00 PM";
        //    times.Add(_100PM);

        //    Time _130PM = new Time();
        //    _130PM.Name = "1:30 PM";
        //    times.Add(_130PM);

        //    Time _200PM = new Time();
        //    _200PM.Name = "2:00 PM";
        //    times.Add(_200PM);

        //    Time _230PM = new Time();
        //    _230PM.Name = "2:30 PM";
        //    times.Add(_230PM);

        //    Time _300PM = new Time();
        //    _300PM.Name = "3:00 PM";
        //    times.Add(_300PM);

        //    Time _330PM = new Time();
        //    _330PM.Name = "3:30 PM";
        //    times.Add(_330PM);

        //    Time _400PM = new Time();
        //    _400PM.Name = "4:00 PM";
        //    times.Add(_400PM);

        //    Time _430PM = new Time();
        //    _430PM.Name = "4:30 PM";
        //    times.Add(_430PM);

        //    Time _500PM = new Time();
        //    _500PM.Name = "5:00 PM";
        //    times.Add(_500PM);

        //    Time _530PM = new Time();
        //    _530PM.Name = "5:30 PM";
        //    times.Add(_530PM);

        //    Time _600PM = new Time();
        //    _600PM.Name = "6:00 PM";
        //    times.Add(_600PM);

        //    Time _630PM = new Time();
        //    _630PM.Name = "6:30 PM";
        //    times.Add(_630PM);

        //    Time _700PM = new Time();
        //    _700PM.Name = "7:00 PM";
        //    times.Add(_700PM);

        //    Time _730PM = new Time();
        //    _730PM.Name = "7:30 PM";
        //    times.Add(_730PM);

        //    Time _800PM = new Time();
        //    _800PM.Name = "8:00 PM";
        //    times.Add(_800PM);

        //    Time _830PM = new Time();
        //    _830PM.Name = "8:30 PM";
        //    times.Add(_830PM);

        //    Time _900PM = new Time();
        //    _900PM.Name = "9:00 PM";
        //    times.Add(_900PM);

        //    Time _930PM = new Time();
        //    _930PM.Name = "9:30 PM";
        //    times.Add(_930PM);

        //    Time _1000PM = new Time();
        //    _1000PM.Name = "10:00 PM";
        //    times.Add(_1000PM);

        //    Time _1030PM = new Time();
        //    _1030PM.Name = "10:30 PM";
        //    times.Add(_1030PM);

        //    Time _1100PM = new Time();
        //    _1100PM.Name = "11:00 PM";
        //    times.Add(_1100PM);

        //    Time _1130PM = new Time();
        //    _1130PM.Name = "11:30 PM";
        //    times.Add(_1130PM);

        //    Time _1200AM = new Time();
        //    _1200AM.Name = "12:00 AM";
        //    times.Add(_1200AM);

        //    Time _1230AM = new Time();
        //    _1230AM.Name = "12:30 AM";
        //    times.Add(_1230AM);

        //    Time _100AM = new Time();
        //    _100AM.Name = "1:00 AM";
        //    times.Add(_100AM);

        //    Time _130AM = new Time();
        //    _130AM.Name = "1:30 AM";
        //    times.Add(_130AM);

        //    Time _200AM = new Time();
        //    _200AM.Name = "2:00 AM";
        //    times.Add(_200AM);

        //    Time _230AM = new Time();
        //    _230AM.Name = "2:30 AM";
        //    times.Add(_230AM);

        //    Time _300AM = new Time();
        //    _300AM.Name = "3:00 AM";
        //    times.Add(_300AM);

        //    Time _330AM = new Time();
        //    _330AM.Name = "3:30 AM";
        //    times.Add(_330AM);

        //    Time _400AM = new Time();
        //    _400AM.Name = "4:00 AM";
        //    times.Add(_400AM);

        //    Time _430AM = new Time();
        //    _430AM.Name = "4:30 AM";
        //    times.Add(_430AM);

        //    Time _500AM = new Time();
        //    _500AM.Name = "5:00 AM";
        //    times.Add(_500AM);

        //    Time _530AM = new Time();
        //    _530AM.Name = "5:30 AM";
        //    times.Add(_530AM);

        //    return times;
        //}

        //public static List<Time> GetHours()
        //{
        //    List<Time> hours = new List<Time>();

        //    for (int i = 1; i <= 12; i++)
        //    {
        //        Time hour = new Time();
        //        hour.Name = i.ToString();
        //        hours.Add(hour);
        //    }

        //    return hours;
        //}

        //public static List<Time> GetMinutes()
        //{
        //    List<Time> minutes = new List<Time>();

        //    for (int i = 0; i < 60; i++)
        //    {
        //        Time minute = new Time();
        //        minute.Name = i.ToString("00");
        //        minutes.Add(minute);
        //    }

        //    return minutes;
        //}

        //public static List<Time> GetAMorPM()
        //{
        //    List<Time> am_pm = new List<Time>();

        //    Time am = new Time();
        //    am.Name = "AM";
        //    am_pm.Add(am);

        //    Time pm = new Time();
        //    pm.Name = "PM";
        //    am_pm.Add(pm);

        //    return am_pm;
        //}

        public static DateTime GetLocalTime()
        {
            int timePlus = 480;
            try
            {
                timePlus = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[AppSettingKey.TIME_ZONE_MINUTES]);
            }
            catch
            {
            }
            return DateTime.Now.ToUniversalTime().AddMinutes(timePlus);
        }

        public static string ConvertTo24HrsFormat(DateTime? dateTime)
        {
            return dateTime != null ? dateTime.Value.ToString("HH:mm") : "";
        }

        public static string ConvertToDateTime24HrsFormat(DateTime? dateTime)
        {
            return dateTime != null ? dateTime.Value.ToString("dd/MM/yyyy HH:mm") : "";
        }

        public static List<General> GetMonths()
        {
            List<General> months = new List<General>();

            General jan = new General
            {
                Id = 1,
                Value = "Jan",
            };
            months.Add(jan);

            General feb = new General
            {
                Id = 2,
                Value = "Feb",
            };
            months.Add(feb);

            General mar = new General
            {
                Id = 3,
                Value = "Mar",
            };
            months.Add(mar);

            General apr = new General
            {
                Id = 4,
                Value = "Apr"
            };
            months.Add(apr);

            General may = new General
            {
                Id = 5,
                Value = "May"
            };
            months.Add(may);

            General jun = new General
            {
                Id = 6,
                Value = "Jun"
            };
            months.Add(jun);

            General jul = new General
            {
                Id = 7,
                Value = "Jul"
            };
            months.Add(jul);

            General aug = new General
            {
                Id = 8,
                Value = "Aug"
            };
            months.Add(aug);

            General sep = new General
            {
                Id = 9,
                Value = "Sep"
            };
            months.Add(sep);

            General oct = new General
            {
                Id = 10,
                Value = "Oct"
            };
            months.Add(oct);

            General nov = new General
            {
                Id = 11,
                Value = "Nov"
            };
            months.Add(nov);

            General dec = new General
            {
                Id = 12,
                Value = "Dec"
            };
            months.Add(dec);

            return months;


            return months;
        }

        public static List<General> GetYears()
        {
            List<General> years = new List<General>();

            for(int i = 2015;i<=2100;i++)
            {
                General year = new General
                {
                    Id = i,
                    Value = i.ToString()
                };
                years.Add(year);
            }

            return years;
        }
    }
}
