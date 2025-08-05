using EngagerMark4.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Cris
{
    public abstract class BaseCri
    {
        public int CurrentPage { get; set; } = 1;

        public int NoOfPage { get; set; } = ApplicationConfig.PAGE_SIZE;

        public bool IsPagination { get; set; } = false;

        public ReportType GeneratedReportType
        {
            get;
            set;
        } = ReportType.ByJob;

        public ReportFormat GeneratedReportFormat
        {
            get;
            set;
        }

        public EntityOrderBy OrderBy
        {
            get;
            set;
        }

        public DataType DType
        {
            get;
            set;
        }

        public enum EntityOrderBy
        {
            Asc,
            Dsc
        }

        public enum DataType
        {
            String,
            Int64,
            DateTime
        }

        public enum ReportType
        {
            ByDay,
            ByJob
        }

        public enum ReportFormat
        {
            Excel,
            PDF
        }

        public static string GetOrderString(string column, string customerSpecifiedColumn,string orderBy)
        {
            return column.Equals(customerSpecifiedColumn) ? orderBy.Equals(EntityOrderBy.Asc.ToString()) ? EntityOrderBy.Dsc.ToString() : EntityOrderBy.Asc.ToString() : EntityOrderBy.Asc.ToString();
        }

        public List<string> Includes
        {
            get;
            set;
        } = new List<string>();

        public Dictionary<string, StringValue> StringCris
        {
            get;
            set;
        } = new Dictionary<string, StringValue>();

        public Dictionary<string, IntValue> NumberCris
        {
            get;
            set;
        } = new Dictionary<string, IntValue>();

        public Dictionary<string, DecimalValue> DecimalCris
        {
            get;
            set;
        } = new Dictionary<string, DecimalValue>();

        public Dictionary<string, Dictionary<DataType, EntityOrderBy>> OrderBys
        {
            get;
            set;
        } = new Dictionary<string, Dictionary<DataType, EntityOrderBy>>();

        public enum StringComparisonOperator
        {
            Equal,
            Contains,
            StartsWith,
            EndsWith
        }

        public enum NumberComparisonOperator
        {
            Equal,
            GreaterThan,
            GreaterThanEqual,
            LessThan,
            LessThanEqual
        }
    }

    public class StringValue
    {
        public BaseCri.StringComparisonOperator ComparisonOperator
        {
            get;
            set;
        }
            
        public string Value
        {
            get;
            set;
        }
    }

    public class IntValue
    {
        public BaseCri.NumberComparisonOperator ComparisonOperator
        {
            get;
            set;
        }

        public Int64 Value
        {
            get;
            set;
        }
    }

    public class DecimalValue
    {
        public BaseCri.NumberComparisonOperator ComparisonOperator
        {
            get;
            set;
        }

        public decimal Value
        {
            get;
            set;
        }
    }
}
