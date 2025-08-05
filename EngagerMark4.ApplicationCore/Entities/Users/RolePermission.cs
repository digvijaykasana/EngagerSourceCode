using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities.Users
{
    [Serializable]
    [Table("Tb_RolePermission", Schema = "Core")]
    public class RolePermission : BasicEntity
    {
        public Int64 RoleId
        { get; set; }

        [ForeignKey("RoleId")]
        public Role Role
        {
            get;
            set;
        }

        public Int64 PermissionId
        {
            get;
            set;
        }

        public Int64 PermissionDetailsId
        {
            get;
            set;
        }

        public bool AllAccess
        {
            get;
            set;
        } = true;

        [NotMapped]
        public string AllAccessStr
        {
            get
            {
                return AllAccess ? "true" : "false";
            }
        }

        public override string ToString()
        {
            return nameof(RolePermission) + " : " + Id;
        }
    }
}
