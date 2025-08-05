using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EngagerMark4.ApplicationCore.Entities
{
    [Serializable]
    public class Company : BaseEntity
    {
        [Required]
        [Index(IsUnique =true)]
        [StringLength(255)]
        public string Domain
        {
            get;
            set;
        }

        [StringLength(50)]
        public string Prefix
        {
            get;
            set;
        }

        [StringLength(255)]
        public String ApplicationName
        {
            get;
            set;
        }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(255)]
        public String Name
        {
            get;
            set;
        }

        [StringLength(255)]
        public string Address
        {
            get;
            set;
        }

        [StringLength(255)]
        public string City
        {
            get;
            set;
        }

        [StringLength(255)]
        public String ZipCode
        {
            get;
            set;
        }

        [StringLength(255)]
        public String Country
        {
            get;
            set;
        }

        [Display(Name = "Main Phone")]
        [StringLength(255)]
        public string MainPhone
        {
            get;
            set;
        }

        [Display(Name = "After Hours Phone")]
        [StringLength(255)]
        public string AfterHoursPhone
        {
            get;
            set;
        }

        [Display(Name = "Support Email")]
        [StringLength(255)]
        [EmailAddress]
        public string SupportEmail
        {
            get;
            set;
        }

        [Display(Name = "Marketing Email")]
        [StringLength(255)]
        [EmailAddress]
        public string MarketingEmail
        {
            get;
            set;
        }

        [Display(Name = "General Email")]
        [StringLength(255)]
        [EmailAddress]
        public string GeneralEmail
        {
            get;
            set;
        }

        [Display(Name = "Content")]
        [Column(TypeName = "text")]
        [AllowHtml]
        public string Description1
        {
            get;
            set;
        }

        [Display(Name = "Content")]
        [Column(TypeName = "text")]
        [AllowHtml]
        public string Description2
        {
            get;
            set;
        }

        [Display(Name = "Description 3")]
        [Column(TypeName = "text")]
        public string Description3
        {
            get;
            set;
        }

        [StringLength(255)]
        public String Logo
        {
            get;
            set;
        }

        [StringLength(255)]
        [Display(Name ="Report Header Logo")]
        public string ReportHeaderLogo
        {
            get;
            set;
        }


        [StringLength(255)]
        [Display(Name ="Transfer Voucher Logo")]
        public string TransferVoucherLogo
        {
            get;
            set;
        }

        [StringLength(255)]
        [Display(Name="Report Footer Logo")]
        public string ReportFooterLogo
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public bool IsDefault
        {
            get;
            set;
        }

        public virtual ICollection<CompanyAddress> Addresses { get; set; } = new HashSet<CompanyAddress>() { new CompanyAddress() };

        [NotMapped]
        public override string SearchDescription
        {
            get
            {
                return this.ApplicationName + " " + Name + " " + Address + " " + City + " " + State + " " + ZipCode + " " + Country + " " + MainPhone + " " + AfterHoursPhone + " " + SupportEmail + " " + MarketingEmail + " " + GeneralEmail + " " + Description1 + " " + Description2 + " " + Description3;
            }
        }

        public Company()
        {
            this.Created = DateTime.Now;
            this.Width = 200;
            this.Addresses = new HashSet<CompanyAddress>();
        }

        
        [NotMapped]
        public string ShippingAddress
        {
            get
            {
                return this.Address + " " + City + " , " + this.Country + " " + this.ZipCode;
            }
        }

    }
}
