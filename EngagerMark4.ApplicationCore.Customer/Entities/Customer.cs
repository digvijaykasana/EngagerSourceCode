using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using static EngagerMark4.Common.Configs.TransferVoucherConfig;

namespace EngagerMark4.ApplicationCore.Customer.Entities
{
    /// <summary>
    /// YesNo1 = RequiresSpecialTransVoucher
    /// 
    /// PCR2021
    /// YesNo2 = ExcludeInCustomerDailySummaryReport
    /// </summary>
    [Serializable]
    [Table("Tb_Customer", Schema = "Customer")]
    public class Customer : BaseEntity
    {
        [Required]
        [StringLength(256)]
        public string Name
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Email
        {
            get;
            set;
        }

        [StringLength(256)]
        public string OfficeNo
        {
            get;
            set;
        }

        [StringLength(50)]
        public string AccNo
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Fax
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Address
        {
            get;
            set;
        }

        [StringLength(50)]
        public string Acronym
        {
            get;
            set;
        } = "NA";

        public Int64? VesselId
        {
            get;
            set;
        }

        [Range(0, 999999999999999999.9999999999)]
        public decimal DiscountPercent
        {
            get;
            set;
        }

        [Range(0, 999999999999999999.9999999999)]
        public decimal DiscountAmt
        {
            get;
            set;
        }

        public Int64 DiscountType
        {
            get;
            set;
        }

        public DiscountTypes DiscountTy
        {
            get;
            set;
        }

        [NotMapped]
        public List<CustomerLocation> Locations
        {
            get;
            set;
        } = new List<CustomerLocation>();

        public List<CustomerLocation> GetLocations()
        {
            foreach(var detail in Locations)
            {
                detail.Customer = this;
            }

            return Locations.Where(x => x.Delete == false).ToList();
        }

        [NotMapped]
        public List<CustomerVessel> VesselList
        {
            get;
            set;
        } = new List<CustomerVessel>();

        public List<CustomerVessel> GetVessels()
        {
            foreach(var detail in VesselList)
            {
                detail.Customer = this;
            }

            return VesselList.Where(x => x.Delete == false).ToList();
        }

        public enum DiscountTypes
        {
            [Display(Name ="By Percent")]
            ByPercent,
            [Display(Name = "By Amount")]
            ByAmount
        }

        [NotMapped]
        public bool RequiresSpecialTransVoucher
        {
            get
            {
                return YesNo1;
            }
            set
            {
                YesNo1 = value;
            }
        }

        //PCR2021
        public Int64? LetterheadId
        {
            get;
            set;
        }

        //PCR2021
        public TransferVoucherFormat TransferVoucherFormat
        {
            get;
            set;
        }

        //PCR2021
        public bool TFRequireAllPassSignatures
        {
            get;
            set;
        }

        //PCR2021
        [NotMapped]
        public bool ExcludeInCustomerDailySummaryReport
        {
            get
            {
                return YesNo2;
            }
            set
            {
                YesNo2 = value;
            }
        }

        public override string ToString()
        {
            return nameof(Customer) + " : " + Name;
        }
    }
}
