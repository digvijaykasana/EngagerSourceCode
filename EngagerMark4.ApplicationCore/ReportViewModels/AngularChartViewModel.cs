using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.ReportViewModels
{
    [Serializable]
    public class AngularChartViewModel
    {
        public const string LINECHART = "chart chart-line";
        public const string BARCHART = "chart chart-bar";
        public const string DONUTCHART = "chart chart-doughnut";
        public const string RADARCHART = "chart chart-radar";
        public const string PIECHART = "chart chart-polar-area";
        public const string HORIZONTALBARCHART = "chart-horizontal-bar";
        public const string BUBBLECHART = "chart-bubble";
        public const string DYNAMICCHART = "chart-base";

        public string AngularChartType
        {
            get;
            set;
        } = LINECHART;

        public string Title
        {
            get;
            set;
        }

        public string[] Series
        {
            get;
            set;
        }

        public string SeriesStr
        {
            get
            {
                string str = "";
                try
                {
                    foreach (string m in Series)
                    {
                        str += m + ",";
                    }
                    str = str.Substring(0, str.Length - 1);
                }
                catch
                {

                }
                return str;
            }
        }

        public string[] Labels
        {
            get;
            set;
        }

        public string LabelsStr
        {
            get
            {
                string str = "";
                try
                {
                    foreach (string m in Labels)
                    {
                        str += m + ",";
                    }
                    str = str.Substring(0, str.Length - 1);
                }
                catch { }
                return str;
            }
        }

        public Dictionary<string, decimal[]> Points
        {
            get;
            set;
        }

        public string PointsStr
        {
            get
            {
                string str = "";
                try
                {
                    foreach (decimal[] value in Points.Values)
                    {
                        string substr = "";
                        foreach (decimal dec in value)
                        {
                            substr += dec.ToString() + ",";
                        }
                        substr = substr.Substring(0, substr.Length - 1);
                        substr += "]";
                        str += substr;
                    }
                    str = str.Substring(0, str.Length - 1);
                }
                catch { }
                return str;
            }
        }
    }
    public enum ChartType
    {
        Area,
        Bar,
        Column,
        Line,
        Pie,
        Stock
    }
}
