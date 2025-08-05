using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities.Application
{
    [Serializable]
    [Table("Tb_ModulePermission", Schema = "Core")]
    public class ModulePermission : BasicEntity
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

        public Int64 ModuleId
        {
            get;
            set;
        }

        [ForeignKey("ModuleId")]
        public Module Module
        {
            get;
            set;
        }

        [NotMapped]
        public bool Delete
        {
            get;
            set;
        }

        public override string ToString()
        {
            return nameof(ModulePermission) + " : " + Id;
        }
    }
}
