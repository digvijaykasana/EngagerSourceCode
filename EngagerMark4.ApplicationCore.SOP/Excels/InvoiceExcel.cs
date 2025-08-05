using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Excels
{
    public class InvoiceExcel
    {
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

        [Display(Name = "PRODUCT_NO")]
        public string PRODUCT_NO
        {
            get;
            set;
        }

        public string PRODUCT_NAME
        {
            get;
            set;
        }

        [Display(Name = "PRODUCT_NAME")]
        public string PRODUCT_DISPLAY
        {
            get
            {
                return PRODUCT_NAME.Contains('-') ? PRODUCT_NAME.Split('-')[1].Trim() : PRODUCT_NAME;
            }
        }

        [Display(Name ="QTY")]
        public decimal QTY
        {
            get;
            set;
        }

        [Display(Name = "UOM")]
        public string UOM
        {
            get;
            set;
        } = "uom";

        [Display(Name ="PRICE")]
        public decimal PRICE
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

        public decimal TaxPercent
        {
            get;
            set;
        }

        //[Display(Name = "AMOUNT")]
        //public decimal TotalAmount
        //{
        //    //get
        //    //{
        //    //    return Taxable == false ? AMOUNT : (AMOUNT + (AMOUNT * (TaxPercent / 100)));
        //    //    //return AMOUNT;
        //    //}
        //    get
        //    {
        //        //return Taxable == false ? AMOUNT : (AMOUNT + (AMOUNT * (TaxPercent / 100)));
        //        return AMOUNT;
        //    }
        //}

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

        [Display(Name = "PROJ_NO")]
        public string PROJ_NO
        {
            get;
            set;
        }

        [Display(Name = "INCL_GST")]
        public string INCL_GST
        {
            get
            {
                return Taxable == true ? "N" : "Z";
            }
        }

        [Display(Name = "VESSEL")]
        public string Vessel
        {
            get;
            set;
        }
    }
}
