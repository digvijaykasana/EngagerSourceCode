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

namespace EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices
{
    /// <summary>
    /// LongText1 - Customer Name
    /// LongText2 - Vessel Name
    /// LongText3 - ATTN
    /// YesNo1 - isTaxable
    /// Id1 - Year and Month Number of Invoice
    /// Id2 - Serial No
    /// Id3 - Invoice Summary Status
    /// Serial - Serial of Invoice
    /// </summary>
    [Serializable]
    [Table("Tb_SalesInvoiceSummary", Schema ="SOP")]
    public class SalesInvoiceSummary : BaseEntity
    {
        [Required]
        [Index(IsUnique = false)]
        [StringLength(50)]
        public string ReferenceNo
        {
            get;
            set;
        } = "TBD";

        [StringLength(50)]
        public string DNNo
        {
            get;
            set;
        }

        //Added - Aung Ye Kaung - 20190426
        public Int64 CustomerId
        {
            get;
            set;
        }

        //Added - Aung Ye Kaung - 20190426
        [NotMapped]
        public string CustomerName
        {
            get
            {
                return this.LongText1;
            }
            set
            {
                this.LongText1 = value;
            }
        }

        //Added - Aung Ye Kaung - 20190430
        public Int64 VesselId
        {
            get;
            set;
        }

        //Added - Aung Ye Kaung - 20190430
        [NotMapped]
        public string VesselName
        {
            get
            {
                return this.LongText2;
            }
            set
            {
                this.LongText2 = value;
            }
        }

        public DateTime? InvoiceDate
        {
            get;
            set;
        }

        public string InvoiceDateStr
        {
            get { return this.InvoiceDate == null ? string.Empty : Util.ConvertDateToString(InvoiceDate.Value, DateConfig.CULTURE); }
        }

        [NotMapped]
        public string CreditNoteSummaryNo
        {
            get;
            set;
        }


        [NotMapped]
        public List<SalesInvoiceSummaryDetails> Details
        {
            get;
            set;
        } = new List<SalesInvoiceSummaryDetails>();

        public List<SalesInvoiceSummaryDetails> GetDetails()
        {
            foreach(var detail in Details)
            {
                detail.SalesInvoiceSummary = this;
            }

            return Details;
        }

        public override string ToString()
        {
            return nameof(SalesInvoiceSummary) + " : " + ReferenceNo;
        }

        //V1.0.2.4 - Added - Aung Ye Kaung - 20200304
        [NotMapped]
        public bool IsTaxable
        {
            get
            {
                return this.YesNo1;
            }
            set
            {
                this.YesNo1 = value;
            }
        }

        //V1.0.2.4 - Added - Aung Ye Kaung - 20200304
        [NotMapped]
        public string AttnStr
        {
            get
            {
                return this.LongText3;
            }
            set
            {
                this.LongText3 = value;
            }
        }

        [NotMapped]
        public int YearMonthNo
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

        [NotMapped]
        public int SerialNo
        {
            get
            {
                return this.Id2;
            }
            set
            {
                this.Id2 = value;
            }
        }

        [NotMapped]
        public SalesInvoiceSummaryStatus Status
        {
            get
            {
                try
                {
                    return (SalesInvoiceSummaryStatus)this.Id3;
                }
                catch(Exception ex)
                {
                    return SalesInvoiceSummaryStatus.With_Accounts;
                }
            }
            set
            {
                this.Id3 = (int)value;
            }
        }


        public enum SalesInvoiceSummaryStatus
        {
            With_Accounts = 0,
            Billed = 1
        }

        [NotMapped]
        public string StatusColor
        {
            get
            {
                switch (Status)
                {
                    case SalesInvoiceSummaryStatus.With_Accounts:
                        return "#4A148C";
                    case SalesInvoiceSummaryStatus.Billed:
                        return "#000000";
                    default:
                        return "#000000";
                }
            }
        }
    }
}
