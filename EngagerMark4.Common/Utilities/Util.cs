using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace EngagerMark4.Common.Utilities
{
    /// <summary>
    /// FUJI XEROX INTERNAL USE ONLY<<RESTRICTED>>
    /// Disclose to : PSTG T&T Team
    /// Protected until:
    /// Author: Kyaw Min Htut
    /// Prepared on: 2-12-2015
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Specify date and time
        /// Created by: Kyaw Min Htut
        /// Created date: 19-01-2015
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="time"></param>
        /// <returns>DateTime</returns>
        public static DateTime SpecifyDateAndTime(DateTime dateTime, String time)
        {
            String strDateTime = dateTime.ToShortDateString() + " " + time;
            DateTime specifiedDateTime = Convert.ToDateTime(strDateTime);
            return specifiedDateTime;
        }

        /// <summary>
        /// Convert date to YYYY-mm-dd format
        /// Created by: Kyaw Min Htut
        /// Created date: 23-1-2015
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>Date string</returns>
        public static String ConvertDateToString(DateTime dateTime)
        {
            //return dateTime.Year.ToString() + "-" + dateTime.Month.ToString() + "-" + dateTime.Day.ToString();
            return $"{dateTime.Year}-{dateTime.Month}-{dateTime.Day}";
        }

        /// <summary>
        /// Convert date string to date time
        /// Created by: Kyaw Min Htut
        /// Created date: 23-1-2015
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns>DateTime</returns>
        public static DateTime ConvertStringToDateTime(String strDate)
        {
            String[] strs = strDate.Split('-');
            int year = Convert.ToInt32(strs[0]);
            int month = Convert.ToInt32(strs[1]);
            int day = Convert.ToInt32(strs[2]);
            return new DateTime(year, month, day);
        }

        /// <summary>
        /// Deprecated
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDateTime1(string strDate)
        {
            return Convert.ToDateTime(strDate);
        }

        /// <summary>
        /// Convert date string to DateTime according to Culture
        /// Created by: Kyaw Min Htut
        /// Created date: 05-02-2015
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="culture"></param>
        /// <returns>DateTime</returns>
        public static DateTime ConvertStringToDateTime(string strDate, string culture)
        {
            try
            {
                return Convert.ToDateTime(strDate, new System.Globalization.CultureInfo(culture));
            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Convert DateTime to string according to Culture
        /// Created by: Kyaw Min Htut
        /// Created date: 05-02-2015
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="culture"></param>
        /// <returns>String</returns>
        public static String ConvertDateToString(DateTime dateTime, string culture)
        {
            return dateTime.ToString("d", new System.Globalization.CultureInfo(culture));
        }


        /// <summary>
        /// Convert DateTime to string according to Culture
        /// Created by: Aung Ye Kaung
        /// Created date: 14/10/2022
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="culture"></param>
        /// <returns>String</returns>
        public static String ConvertDateToFormattedString(DateTime dateTime, string culture, string format)
        {
            try
            {
                return dateTime.ToString(format, new System.Globalization.CultureInfo(culture));
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(String strDate, String Time)
        {
            String[] strs = strDate.Split('-');
            int year = Convert.ToInt32(strs[0]);
            int month = Convert.ToInt32(strs[1]);
            int day = Convert.ToInt32(strs[2]);
            return Convert.ToDateTime(new DateTime(year, month, day).ToShortDateString() + " " + Time);
        }

        public static decimal GetNumberOnly(string strNumber)
        {
            string strNumberOnly = "";
            foreach (char c in strNumber.ToCharArray())
            {
                if (Char.IsDigit(c))
                    strNumberOnly += c;
                if (c == '.')
                {
                    if (!strNumberOnly.Contains('.'))
                        strNumberOnly += c;
                }
            }

            return Convert.ToDecimal(strNumberOnly);
        }

        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr LoadLibrary(string lpFileName);

        public static bool CheckLibrary(string fileName)
        {
            return LoadLibrary(fileName) == IntPtr.Zero;
        }

        public static Int64 GenerateRandomNumber()
        {
            Random rnd = new Random();
            return rnd.Next(1, 1000000000);
        }

        public static string GetDomain(string email)
        {
            if (string.IsNullOrEmpty(email)) return string.Empty;

            if (!email.Contains("@")) return string.Empty;

            return email.Split('@')[1].Trim().ToLower();
        }

        public static string ConvertNoToMonth(int month)
        {
            switch (month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
            }
            return "";
        }

        //PCR2021
        public static List<DateTime> GetDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList(); // Load dates into a list
        }

        //PCR2021
        public static List<DateTime> GetDates(DateTime startDate, DateTime endDate)
        {
            List<DateTime> allDates = new List<DateTime>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                allDates.Add(date);
            }

            return allDates;
        }
    }
}
