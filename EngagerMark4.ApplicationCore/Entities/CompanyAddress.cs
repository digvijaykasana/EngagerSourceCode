using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities
{
    public class CompanyAddress : BaseEntity
    {
        [MaxLength(50)]
        [Display(Name = "Postal Code")]
        public String PostalCode
        {
            get;
            set;
        }

        [MaxLength(255)]
        [Display(Name = "Address")]
        public String Name
        {
            get;
            set;
        }

        [MaxLength(255)]
        public String City
        { get; set; }

        [MaxLength(255)]
        public String Country
        { get; set; }

        public bool IsDefault
        {
            get;
            set;
        }

        public Int64 CompanyId
        {
            get;
            set;
        }

        [ForeignKey("CompanyId")]
        public Company Company
        {
            get;
            set;
        }

        public AddressType Type
        {
            get;
            set;
        }

        public enum AddressType
        {
            MainAddress = 1,
            BillingAddress = 2
        }
    }
}
