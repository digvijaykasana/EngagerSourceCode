using EngagerMark4.ApplicationCore.Account.Entities;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes
{
    [Serializable]
    [Table("Tb_CreditNote", Schema = "SOP")]
    public class CreditNote : BaseEntity
    {   

        public Int64? VesselId
        {
            get;
            set;
        }

        public string VesselName
        {
            get;
            set;
        }

        public Int64? CustomerId
        {
            get;
            set;
        }

        public Customer.Entities.Customer Customer
        {
            get;
            set;
        }

        public Int64? OrderId
        {
            get; set;
        }

        [ForeignKey("OrderId")]
        public WorkOrders.WorkOrder WorkOrder
        {
            get; set;
        }

        [StringLength(256)]
        public string OrderNo
        {
            get; set;
        }

        [Required]
        [StringLength(256)]
        public string CreditNoteNo
        {
            get; set;
        } = "TBD";

        [Required]
        public DateTime? CreditNoteDate
        {
            get;
            set;
        } = TimeUtil.GetLocalTime();

        [NotMapped]
        public string CreditNoteDateStr
        {
            get
            {
                return CreditNoteDate == null ? "" : Util.ConvertDateToString(CreditNoteDate.Value, DateConfig.CULTURE);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    CreditNoteDate = Util.ConvertStringToDateTime(value, DateConfig.CULTURE);
                }
            }
        }

        [Required]
        public Int64 DiscountType
        {
            get;
            set;
        }

        [StringLength(256)]
        public string DiscountTypeName
        {
            get;
            set;
        }

        [StringLength(256)]
        public string CompanyAddress
        {
            get;
            set;
        }

        [Range(0, 999999999999999999.9999999999)]
        public decimal SubTotal
        {
            get;
            set;
        }

        public Int64 GSTId
        {
            get;
            set;
        }

        [ForeignKey("GSTId")]
        public GST gst
        {
            get;set;
        }

        public decimal GSTPercent
        {
            get;
            set;
        }

        public decimal GSTAmount
        {
            get;
            set;
        }

        public decimal InvoiceTotalAmount
        {
            get;
            set;
        }

        [Required]
        [Range(0, 999999999999999999.9999999999)]
        public decimal GrandTotal
        {
            get;
            set;
        }

        [NotMapped]
        public List<CreditNoteDetails> Details
        {
            get;
            set;
        } = new List<CreditNoteDetails>();

        public List<CreditNoteDetails> GetDetails()
        {
            foreach (var detail in Details)
            {
                detail.CreditNote = this;
            }

            return Details.Where(x => x.Delete == false).ToList();
        }

        public string GetCreditNoteNo(Int64 serialNo, Customer.Entities.Customer cusEntity)
        {
            return $"CRN-{(cusEntity.Acronym == null ? string.Empty : cusEntity.Acronym)}-MTS-{serialNo.ToString()}";
        }

        public void GenerateCreditNote(SalesInvoice salesInvoice,GST gst)
        {
            this.VesselId = salesInvoice.VesselId;
            this.VesselName = salesInvoice.VesselName;
            this.CustomerId = salesInvoice.CustomerId;
            this.OrderId = salesInvoice.WorkOrderId;
            this.OrderNo = salesInvoice.WorkOrderNo;
            this.CreditNoteDate = salesInvoice.InvoiceDate;
            this.DiscountType = (int)salesInvoice.DisType;
            this.DiscountTypeName = salesInvoice.DisType.ToString();
            this.CompanyAddress = salesInvoice.CompanyAddress;
            this.SubTotal = salesInvoice.DiscountAmount;
            this.GSTId = salesInvoice.GSTId == null ? 0 : salesInvoice.GSTId.Value;
            this.GSTPercent = gst.GSTPercent;
            this.InvoiceTotalAmount = salesInvoice.TotalAmt;
            this.CalculateGST();

            decimal discountPercent = 0;
            
            discountPercent = salesInvoice.DiscountPercent;
            
            CreditNoteDetails details = this.Details.FirstOrDefault();

            if (details == null) details = new CreditNotes.CreditNoteDetails();

            details.CreditNote = this;
            details.SerialNo = 1;
            details.Qty = 1;
            details.Price = this.SubTotal;
            details.TotalAmount = this.SubTotal;
            
            if(discountPercent > 0)
            {
                details.Description = $"Being {Math.Round(discountPercent, 2, MidpointRounding.AwayFromZero)}% Discount on Transport Charges";
            }
            else
            {
                details.Description = $"Being Discount on Transport Charges";
            }


            if (details.Id == 0) this.Details.Add(details);
        }

        public void CalculateGST()
        {
            this.GSTAmount =Math.Round(SubTotal * (this.GSTPercent / 100),2,MidpointRounding.AwayFromZero);
            this.GrandTotal = this.SubTotal + GSTAmount;
        }

        public override string ToString()
        {
            return nameof(CreditNote) + " : " + CreditNoteNo;
        }
    }
}
