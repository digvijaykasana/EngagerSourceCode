using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Job.Entities
{
    [Serializable]
    [Table("Tb_ChecklistTemplate", Schema ="Job")]
    public class ChecklistTemplate : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Code
        {
            get;
            set;
        }

        [Required]
        [StringLength(255)]
        public string Name
        {
            get;
            set;
        }

        public Int64? ReferenceId
        {
            get;
            set;
        }

        [NotMapped]
        public virtual ICollection<CheckListTemplateDetail> Details
        {
            get;
            set;
        } = new HashSet<CheckListTemplateDetail>();

        public ICollection<CheckListTemplateDetail> GetDetails()
        {
            foreach(var detail in Details)
            {
                detail.Checklist = null;
                detail.ChecklistTemplate = this;
            }

            return Details.Where(x => x.Delete == false).ToList();
        }

        public override string ToString()
        {
            return nameof(ChecklistTemplate) + " : " + Name;
        }
    }
}
