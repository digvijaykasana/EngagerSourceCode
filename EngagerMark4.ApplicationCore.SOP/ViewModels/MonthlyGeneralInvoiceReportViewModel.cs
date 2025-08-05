using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.ViewModels
{
    public class MonthlyGeneralInvoiceReportViewModel
    {
        public Int64 SalesInvoiceSummaryId
        {
            get;
            set;
        }

        [Display(Name = "Date")]
        public string InvoiceDateStr
        {
            get
            {
                return this.InvoiceDate.HasValue ? InvoiceDate.Value.ToString("dd.MM.yyyy") : "";
            }
        }

        [NotMapped]
        public DateTime? InvoiceDate
        {
            get; set;
        }

        [Display(Name = "Invoice No")]
        public string ReferenceNo { get; set; }

        [Display(Name = "Company Name")]
        public string CustomerName { get; set; }

        public decimal Price { get; set; } = 0;

        public decimal NonTaxableAmount { get; set; } = 0;

        [Display(Name = "Price")]
        public string PriceStr
        {
            get
            {
                var total = this.Price + this.NonTaxableAmount;

                return "$  " + total.ToString("0.00");
            }
        }
        
        public decimal GST { get; set; }

        [Display(Name = "GST")]
        public string GSTStr
        {
            get
            {
                return "$  " + this.GST.ToString("0.00");
            }
        }


        public decimal InvoiceTotalNetAmount { get; set; } = 0;

        [Display(Name = "TOTAL AMOUNT")]
        public string InvoiceTotalNetAmountStr
        {
            get
            {
                return "$  " + this.InvoiceTotalNetAmount.ToString("0.00");
            }
        }


        public decimal CNAmount { get; set; } = 0;

        [Display(Name = "CN")]
        public string CNAmountStr
        {
            get
            {
                return "$  " + this.CNAmount.ToString("0.00");
            }
        }


        public decimal CNGST { get; set; }

        [Display(Name = "GST")]
        public string CNGSTStr
        {
            get
            {
                return "$  " + this.CNGST.ToString("0.00");
            }
        }


        public decimal CreditNoteTotalAmount { get; set; }

        [Display(Name = "TOTAL AMOUNT")]
        public string CreditNoteTotalAmountStr
        {
            get
            {
                return "$  " + this.CreditNoteTotalAmount.ToString("0.00");
            }
        }


        public decimal CreditNoteTotalNetAmount
        {
            get
            {
                return (this.Price + this.NonTaxableAmount) - this.CNAmount;
            }
        }

        [Display(Name = "NET")]
        public string CreditNoteTotalNetAmountStr
        {
            get
            {
                return "$  " + this.CreditNoteTotalNetAmount.ToString("0.00");
            }
        }
    }
}
