using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.ViewModels
{
    public class InvoiceSummaryReportViewModel
    {
        
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Date")]
        public string InvoiceDateStr
        {
            get
            {
                return Util.ConvertDateToString(InvoiceDate, DateConfig.CULTURE);
            }
        }

        [Display(Name = "Invoice No.")]
        public string ReferenceNo { get; set; }

        [Display(Name = "Company Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Price")]
        public decimal TotalAmt { get; set; } = 0;

        [Display(Name = "GST")]
        public decimal GSTAmount { get; set; } = 0;

        [Display(Name = "Invoice Total Amount")]
        public decimal InvoiceTotalNetAmount { get; set; } = 0;

        [Display(Name = "CN")]
        public decimal cnTotalAmt { get; set; } = 0;

        [Display(Name = "CN GST")]
        public decimal cnGSTAmount { get; set; } = 0;

        [Display(Name = "CN Total Amount")]
        public decimal cnGrandTotalAmount { get; set; } = 0;

        [Display(Name = "Net")]
        public decimal Net
        {
            get
            {
                return TotalAmt - cnTotalAmt;
            }
        }
    }
}
