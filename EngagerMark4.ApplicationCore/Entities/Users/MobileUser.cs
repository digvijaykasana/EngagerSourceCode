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
    [Table("Tb_MobileUser", Schema = "Core")]
    public class MobileUser : BasicEntity
    {
        [StringLength(256)]
        [Required]
        public string UserName
        {
            get; set;
        }

        public bool IsSignedIn
        {
            get; set;
        }
    }
}
