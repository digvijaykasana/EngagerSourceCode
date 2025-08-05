using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices
{
    //Id1 = Sort
    [Serializable]
    [Table("Tb_SalesInvoiceDetails", Schema = "SOP")]
    public class SalesInvoiceDetails : BaseEntity
    {
        [NotMapped]
        public bool Delete
        {
            get;
            set;
        }

        public bool GSTEssentials
        {
            get;
            set;
        }

        public bool Taxable
        {
            get;
            set;
        }

        public Int64 SalesInvoiceId
        {
            get;
            set;
        }

        [ForeignKey("SalesInvoiceId")]
        public SalesInvoice SalesInvoice
        {
            get;
            set;
        }

        public int SerialNo
        {
            get;
            set;
        }

        public Int64? PriceId
        {
            get;
            set;
        }

        [ForeignKey("PriceId")]
        public Price PriceObj
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Code
        {
            get;
            set;
        }

        [StringLength(256)]
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
        }

        public decimal TotalAmt
        {
            get;
            set;
        }

        public decimal DiscountPercent
        {
            get;
            set;
        }

        public decimal DiscountAmount
        {
            get;
            set;
        }

        public decimal TotalNetAmount
        {
            get;
            set;
        }

        public DetailsType Type
        {
            get;
            set;
        } = DetailsType.NonInventory;

        public enum DetailsType
        {
            Inventory,
            NonInventory,
            Memo
        }

        public void CalculateTotal()
        {
            TotalAmt = Qty * Price;

            if (SalesInvoice.DisType == SalesInvoice.DiscountType.ByPercent)
            {
                DiscountAmount = TotalAmt * (DiscountPercent / 100);
                TotalNetAmount = TotalAmt - DiscountAmount;
            }
            else if(SalesInvoice.DisType == SalesInvoice.DiscountType.ByAmount)
            {
                DiscountAmount = DiscountAmount * Qty;
                //if (TotalAmt != 0)
                    //DiscountPercent = (DiscountAmount / TotalAmt) * 100;
                TotalNetAmount = TotalAmt - DiscountAmount;
            }
        }

        [NotMapped]
        public int SortOrder
        {
            get
            {
                return this.Id1;
            }
            set
            {
                this.Id1 = value;
            }
        }

        public override string ToString()
        {
            return nameof(SalesInvoiceDetails) + " : " + Id;
        }
    }
}
