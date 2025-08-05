using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities.Application
{
    [Serializable]
    [Table("Tb_SystemSetting", Schema ="Core")]
    public class SystemSetting : BasicEntity
    {
        [StringLength(50)]
        [Required]
        public string Code
        {
            get;
            set;
        }

        [StringLength(256)]
        [Required]
        public string Name
        {
            get;
            set;
        }

        [Required]
        public string Value
        {
            get;
            set;
        }

        public override string ToString()
        {
            return nameof(SystemSetting) + " : " + Name;
        }
    }
}
