using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.ReportViewModels
{
    public class InvoiceDetailsReportViewModel
    {
        public DateTime InvoiceDate
        {
            get;
            set;
        }

        public string InvoiceDateStr
        {
            get
            {
                return Util.ConvertDateToString(InvoiceDate, DateConfig.CULTURE).Replace('/', '.');
            }
        }

        public string InvoiceNo
        {
            get;
            set;
        }

        public string WorkOrderNo
        {
            get;
            set;
        }

        public string DisplayDescription
        {
            get
            {
                try
                {
                    //return Code.Contains('-') ? Code.Split('-')[1].Trim() : Code;
                    return Code;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public string Code
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public decimal Qty
        {
            get;
            set;
        }

        public decimal Price
        {
            get;
            set;
        } = 0;

        public decimal TotalAmount
        {
            get;
            set;
        } = 0;
             
        public bool IsHeader
        {
            get;set;
        }

        public bool IsDNNo
        {
            get;
            set;
        }

        public bool IsTaxable
        {
            get;
            set;
        } = false;
    }
}
