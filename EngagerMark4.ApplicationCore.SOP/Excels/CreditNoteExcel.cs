using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Excels
{
    public class CreditNoteExcel
    {
        [NotMapped]
        public long SalesInvoiceSummaryId
        {
            get;
            set;
        }

        [Display(Name = "INV_NO")]
        public string INV_NO
        {
            get;
            set;
        }

        [Display(Name = "AC_NO")]
        public string AC_NO
        {
            get;
            set;
        }

        [Display(Name = "AC_NAME")]
        public string AC_NAME
        {
            get;
            set;
        }

        [Display(Name = "DESCRIPTION")]
        public string DESCIRPTION
        {
            get;
            set;
        }


        [Display(Name = "AMOUNT")]
        public decimal AMOUNT
        {
            get;
            set;
        }

        [Display(Name = "DATETIME")]
        public string DATETIME
        {
            get
            {
                return InvoiceDate == null ? string.Empty : InvoiceDate.Value.ToString("dd-MM-yyyy");
            }
        }
        
        public DateTime? InvoiceDate
        {
            get;
            set;
        }

        [Display(Name = "GL_CODE")]
        public string GL_CODE
        {
            get;
            set;
        }

        public bool Taxable
        {
            get;
            set;
        }

        public string TaxableStr
        {
            get
            {
                return Taxable == true ? "N" : "Z";
            }
        }


        [Display(Name = "INCL_GST")]
        public string INCL_GST
        {
            get
            {
                return Taxable == true ? "N" : "Z";
            }
        }

    }
}
