using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities
{
    [Serializable]
    [Table("Tb_Audit", Schema = "Core")]
    public class Audit : BasicEntity
    {
        public string Description
        {
            get;
            set;
        }

        public DateTime? StartProcessingTime
        {
            get;
            set;
        }

        public DateTime? EndProcessingTime
        {
            get;
            set;
        }

        public AuditType Type
        {
            get;
            set;
        }

        public enum AuditType
        {
            Normal,
            Error,
            RemovedCancelledJobs
        }
    }
}
