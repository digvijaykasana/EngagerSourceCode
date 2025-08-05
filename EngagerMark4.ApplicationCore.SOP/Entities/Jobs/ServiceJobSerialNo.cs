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
    [Table("Tb_ServiceJobSerialNo", Schema = "SOP")]
    public class ServiceJobSerialNo : SerialNo
    {
        public override string ToString()
        {
            return nameof(ServiceJobSerialNo) + " : " + RunningNo;
        }
    }
}
