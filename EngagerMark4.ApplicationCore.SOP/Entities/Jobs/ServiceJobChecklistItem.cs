using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Entities.Jobs
{
    [Serializable]
    [Table("Tb_ServiceJobChecklistItem", Schema = "SOP")]
    public class ServiceJobChecklistItem : BaseEntity
    {
        public long ServiceJobId { get; set; }
        public long ChecklistId { get; set; }
        public decimal ChecklistPrice { get; set; }
    }
}
