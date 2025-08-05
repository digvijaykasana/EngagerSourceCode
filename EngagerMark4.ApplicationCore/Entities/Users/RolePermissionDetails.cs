using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities.Users
{
    [Serializable]
    [Table("Tb_RolePermissionDetails", Schema = "Core")]
    public class RolePermissionDetails : BasicEntity
    {
        public Int64 RolePermissionId
        {
            get;
            set;
        }

        [ForeignKey("RolePermissionId")]
        public RolePermission RolePermission
        {
            get;
            set;
        }

        public PermissionType Type
        {
            get;
            set;
        }

        public enum PermissionType
        {
            Create,
            Edit,
            Delete
        }

        public override string ToString()
        {
            return nameof(RolePermissionDetails) + " : " + Id;
        }
    }
}
