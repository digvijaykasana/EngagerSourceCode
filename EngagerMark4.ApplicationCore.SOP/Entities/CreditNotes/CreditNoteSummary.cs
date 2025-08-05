using EngagerMark4.ApplicationCore.Entities;
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
    /// <summary>
    /// Num1 = SubTotal
    /// Num2 = GrandTotal
    /// </summary>
    [Serializable]
    [Table("Tb_CreditNoteSummary", Schema ="SOP")]
    public class CreditNoteSummary : BaseEntity
    {
        [Required]
        [Index(IsUnique = false)]
        [StringLength(50)]
        public string ReferenceNo
        {
            get;
            set;
        } = "TBD";

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
                return this.CreditNoteDate == null ? string.Empty : Util.ConvertDateToString(CreditNoteDate.Value, DateConfig.CULTURE);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    CreditNoteDate = Util.ConvertStringToDateTime(value, DateConfig.CULTURE);
                }
            }
        }

        public DateTime? FromDate
        {
            get;
            set;
        } = TimeUtil.GetLocalTime().AddMonths(-1);

        [NotMapped]
        public string FromDateStr
        {
            get
            {
                return FromDate == null ? string.Empty : Util.ConvertDateToString(FromDate.Value, DateConfig.CULTURE);
            }
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    FromDate = Util.ConvertStringToDateTime(value, DateConfig.CULTURE);
                }
            }
        }

        public DateTime? ToDate
        {
            get;
            set;
        } = TimeUtil.GetLocalTime();

        [NotMapped]
        public string ToDateStr
        {
            get
            {
                return ToDate == null ? string.Empty : Util.ConvertDateToString(ToDate.Value, DateConfig.CULTURE);
            }
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    ToDate = Util.ConvertStringToDateTime(value, DateConfig.CULTURE);
                }
            }
        }

        public void SetReferenceNo(Int64 serialNo)
        {
            this.ReferenceNo = CreditNoteDate.Value.Year.ToString("0000") + "/" + CreditNoteDate.Value.Month.ToString("00") + "/" + serialNo.ToString("0000") + "A";
        }

        public Int64 CustomerId
        {
            get;
            set;
        }

        [ForeignKey("CustomerId")]
        public Customer.Entities.Customer Customer
        {
            get;
            set;
        }

        [NotMapped]
        public decimal SubTotal
        {
            get { return this.Num1; }
            set { this.Num1 = value; }
        }

        [NotMapped]
        public decimal GrandTotal
        {
            get { return this.Num2; }
            set { this.Num2 = value; }
        }

        public ICollection<CreditNoteSummaryDetails> Details { get; set; } = new List<CreditNoteSummaryDetails>();

        public void GenerateDetails(Int64[] salesInvoiceIds)
        {
            if (salesInvoiceIds == null) return;

            foreach(var salesInvoiceId in salesInvoiceIds)
            {
                Details.Add(new CreditNoteSummaryDetails { SalesInvoiceSummaryId = salesInvoiceId });
            }
        }

        public bool IsInDetails(long salesInvoiceSummaryId)
        {
            var salesInvoiceSummary = this.Details.FirstOrDefault(x => x.SalesInvoiceSummaryId == salesInvoiceSummaryId);

            return salesInvoiceSummary != null ? true : false;
        }

        public CreditNoteStatus? Status
        {
            get;
            set;
        } = CreditNoteStatus.Valid;

        public enum CreditNoteStatus
        {
            All = -1,
            Valid = 1,
            Invalid =0 
        }
    }
}
