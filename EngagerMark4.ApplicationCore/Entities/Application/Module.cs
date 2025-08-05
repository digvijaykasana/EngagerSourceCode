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
    [Table("Tb_Module", Schema = "Core")]
    public class Module : BasicEntity
    {
        [StringLength(50)]
        [Index(IsUnique = false)]
        [Required]
        public string Code
        {
            get;
            set;
        }

        [StringLength(256)]
        [Index(IsUnique = false)]
        [Required]
        public string Name
        {
            get;
            set;
        }

        public Int64 ParentModuleId
        {
            get;
            set;
        }

        public int SerialNo
        {
            get;
            set;
        }

        [StringLength(255)]
        public string IconName
        {
            get;
            set;
        }

        [NotMapped]
        public string IconNameStr
        {
            get
            {
                return string.IsNullOrEmpty(IconName) ? "" : IconName;
            }
        }

        [NotMapped]
        public List<ModulePermission> FunctionList
        {
            get; set;
        } = new List<ModulePermission>();

        public List<ModulePermission> GetFunctionList()
        {
            if (FunctionList == null)
                return new List<ModulePermission>();

            foreach (var function in FunctionList.Where(x => x.Delete == false))
            {
                function.Id = 0;
                function.Module = this;
                function.Permission = null;
            }

            return FunctionList.Where(x => x.Delete == false).ToList();
        }

        public override string ToString()
        {
            return nameof(Module) + " : " + Name;
        }
    }
}
