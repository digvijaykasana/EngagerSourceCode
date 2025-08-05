using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders
{
    [Serializable]
    [Table("Tb_WorkOrderSerialNo", Schema = "SOP")]
    public class WorkOrderSerialNo : SerialNo
    {
        public override string ToString()
        {
            return nameof(WorkOrderSerialNo) + " : " + RunningNo;
        }
    }
}
