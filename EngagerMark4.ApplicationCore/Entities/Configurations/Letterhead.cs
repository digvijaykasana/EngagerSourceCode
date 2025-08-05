using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities.Configurations
{
    [Serializable]
    [Table("Tb_Letterhead", Schema = "Core")]
    public class Letterhead : BaseEntity
    {
        public string Name { get; set; }

        [NotMapped]
        public long LetterheadImageId { get; set; }

        public LetterheadType Type { get; set; }

        [Display(Name = "Is Default")]
        public bool IsDefault { get; set; }

        public enum LetterheadType
        {
            TransferVoucher,
            //Invoice,
            //CreditNote
        }

        [NotMapped]
        public bool IsReadOnly
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
    }
}
