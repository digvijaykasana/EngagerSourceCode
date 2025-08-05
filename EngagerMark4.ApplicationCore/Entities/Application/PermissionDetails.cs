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
    [Table("Tb_PermisisonDetails", Schema = "Core")]
    public class PermissionDetails : BasicEntity
    {
        public Int64 PermissionId
        {
            get;
            set;
        }

        [ForeignKey("PermissionId")]
        public Function Permission
        {
            get;
            set;
        }

        [Required]
        [StringLength(256)]
        public string Action
        {
            get;
            set;
        }

        [StringLength(50)]
        public string MethodType
        {
            get;
            set;
        }

        public override string ToString()
        {
            return nameof(PermissionDetails) + " : " + Id;
        }
    }
}
