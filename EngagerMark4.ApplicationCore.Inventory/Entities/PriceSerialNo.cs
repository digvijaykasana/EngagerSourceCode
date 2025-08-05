using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Inventory.Entities
{
    [Serializable]
    [Table("Tb_PriceSerialNo", Schema = "Inventory")]
    public class PriceSerialNo : SerialNo
    {
        public override string ToString()
        {
            return nameof(PriceSerialNo) + " : " + RunningNo;
        }
    }
}
