using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities.Users
{
    [Serializable]
    [Table("Tb_Role", Schema = "Core")]
    public class Role : BasicEntity
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
    
        [StringLength(255)]
        public string DefaultUrl
        {
            get;
            set;
        }="/";

        public override string ToString()
        {
            return nameof(Role) + " : " + Name;
        }
    }
}
