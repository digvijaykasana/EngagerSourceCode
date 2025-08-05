using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities.Application
{
    [Table("Tb_Notification", Schema = "Core")]
    public class Notification : BasicEntity
    {
        [StringLength(256)]
        public string Description
        {
            get;
            set;
        }

        public bool Acknowledge
        {
            get;
            set;
        }

        public Int64 ReferenceId
        {
            get;
            set;
        }

        [StringLength(256)]
        public string DetailedDescription
        {
            get; set;
        }


        public Int64? NotifiedUserId
        {
            get;
            set;
        }

        [StringLength(256)]
        public string NotifiedUserName
        {
            get;
            set;
        }

        public NotificationType Type
        {
            get;
            set;
        }

        public enum NotificationType
        {
            WorkOrder,  
            WorkOrderByAgent,
            DriverSubmission,
            WorkOrderUpdated,
            TestNotification
        }

        public override string ToString()
        {
            return nameof(Notification) + " : " + Id;
        }
    }
}
