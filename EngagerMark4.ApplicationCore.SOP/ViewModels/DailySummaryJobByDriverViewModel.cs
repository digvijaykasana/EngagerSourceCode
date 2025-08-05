using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.ReportViewModels
{
    public class DailySummaryJobByDriverViewModel
    {
        public Int64 DriverId
        {
            get;
            set;
        }

        [Display(Name = "Driver")]
        public string DriverName
        {
            get;
            set;
        }

        public int GetValue(int day)
        {
            switch (day)
            {
                case 1:
                    return _1;
                case 2:
                    return _2;
                case 3:
                    return _3;
                case 4:
                    return _4;
                case 5:
                    return _5;
                case 6:
                    return _6;
                case 7:
                    return _7;
                case 8:
                    return _8;
                case 9:
                    return _9;
                case 10:
                    return _10;
                case 11:
                    return _11;
                case 12:
                    return _12;
                case 13:
                    return _13;
                case 14:
                    return _14;
                case 15:
                    return _15;
                case 16:
                    return _16;
                case 17:
                    return _17;
                case 18:
                    return _18;
                case 19:
                    return _19;
                case 20:
                    return _20;
                case 21:
                    return _21;
                case 22:
                    return _22;
                case 23:
                    return _23;
                case 24:
                    return _24;
                case 25:
                    return _25;
                case 26:
                    return _26;
                case 27:
                    return _27;
                case 28:
                    return _28;
                case 29:
                    return _29;
                case 30:
                    return _30;
                case 31:
                    return _31;
                default:
                    return _1;
            }
        }

        public string CustomerName
        {
            get;
            set;
        }

        [Display(Name = "1")]
        public int _1
        {
            get;
            set;
        }

        [Display(Name = "2")]
        public int _2
        {
            get;
            set;
        }

        [Display(Name = "3")]
        public int _3
        {
            get;
            set;
        }

        [Display(Name = "4")]
        public int _4
        {
            get;
            set;
        }

        [Display(Name = "5")]
        public int _5
        {
            get;
            set;
        }

        [Display(Name = "6")]
        public int _6
        {
            get;
            set;
        }

        [Display(Name = "7")]
        public int _7
        {
            get;
            set;
        }

        [Display(Name = "8")]
        public int _8
        {
            get;
            set;
        }

        [Display(Name = "9")]
        public int _9
        {
            get;
            set;
        }

        [Display(Name = "10")]
        public int _10
        {
            get;
            set;
        }

        [Display(Name = "11")]
        public int _11
        {
            get;
            set;
        }

        [Display(Name = "12")]
        public int _12
        {
            get;
            set;
        }

        [Display(Name = "13")]
        public int _13
        {
            get;
            set;
        }

        [Display(Name = "14")]
        public int _14
        {
            get;
            set;
        }

        [Display(Name = "15")]
        public int _15
        {
            get;
            set;
        }

        [Display(Name = "16")]
        public int _16
        {
            get;
            set;
        }

        [Display(Name = "17")]
        public int _17
        {
            get;
            set;
        }

        [Display(Name = "18")]
        public int _18
        {
            get;
            set;
        }

        [Display(Name = "19")]
        public int _19
        {
            get;
            set;
        }

        [Display(Name = "20")]
        public int _20
        {
            get;
            set;
        }

        [Display(Name = "21")]
        public int _21
        {
            get;
            set;
        }

        [Display(Name = "22")]
        public int _22
        {
            get;
            set;
        }

        [Display(Name = "23")]
        public int _23
        {
            get;
            set;
        }

        [Display(Name = "24")]
        public int _24
        {
            get;
            set;
        }

        [Display(Name = "25")]
        public int _25
        {
            get;
            set;
        }

        [Display(Name = "26")]
        public int _26
        {
            get;
            set;
        }

        [Display(Name = "27")]
        public int _27
        {
            get;
            set;
        }

        [Display(Name = "28")]
        public int _28
        {
            get;
            set;
        }

        [Display(Name = "29")]
        public int _29
        {
            get;
            set;
        }

        [Display(Name = "30")]
        public int _30
        {
            get;
            set;
        }

        [Display(Name = "31")]
        public int _31
        {
            get;
            set;
        }

        [Display(Name = "Total")]
        public int Total
        {
            get;
            set;
        } = 0;
    }
}
