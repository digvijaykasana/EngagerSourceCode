using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Account.Entities
{
    [Serializable]
    [Table("Tb_GeneralLedger", Schema = "Account")]
    public class GeneralLedger : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Code
        {
            get;
            set;
        }

        [Required]
        [StringLength(256)]
        public string Name
        {
            get;
            set;
        }

        [Required]
        public bool IrregularHour
        {
            get; set;
        }

        [Required]
        public bool LinkToLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Is this General Ledger Item Creditable or not
        /// </summary>
        [Required]
        public bool Creditable
        {
            get;
            set;
        }

        [Required]
        public bool Taxable
        {
            get;
            set;
        }

        /// <summary>
        /// Whether this General Ledger Item is a discount or not
        /// </summary>
        [Required]
        public bool DiscountType
        {
            get;
            set;
        }

        public override string ToString()
        {
            return nameof(GeneralLedger) + " : " + Name;
        }

        [NotMapped]
        public string DescriptionStr
        {
            get
            {
                return this.Name + " - " + this.Code;
            }
        }
    }
}
