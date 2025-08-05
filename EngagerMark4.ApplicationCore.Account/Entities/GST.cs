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
    [Table("Tb_GST", Schema ="Account")]
    public class GST : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Code
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Name
        {
            get;
            set;
        }

        [Required]
        [Range(0, 999999999999999999.9999999999)]
        public decimal GSTPercent
        {
            get;
            set;
        }

        [Required]
        public bool isDefault
        {
            get;
            set;
        }

        public override string ToString()
        {
            return nameof(GST) + " : " + Name;
        }
    }
}
