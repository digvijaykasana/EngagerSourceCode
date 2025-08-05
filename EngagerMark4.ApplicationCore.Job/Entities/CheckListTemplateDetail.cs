using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Job.Entities
{
    [Serializable]
    [Table("Tb_ChecklistTemplateDetail", Schema = "Job")]
    public class CheckListTemplateDetail : BaseEntity
    {
        public Int64 ChecklistTemplateId
        {
            get;
            set;
        }

        [ForeignKey("ChecklistTemplateId")]
        public ChecklistTemplate ChecklistTemplate
        {
            get;
            set;
        }

        public Int64 ChecklistId
        {
            get;
            set;
        }

        [ForeignKey("ChecklistId")]
        public CheckList Checklist
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
            return nameof(CheckListTemplateDetail) + " : " + Id;
        }
    }
}
