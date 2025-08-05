using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Equipment.Entities.Conveyors
{
    [Serializable]
    [Table("Tb_Conveyor", Schema = "Equipment")]
    public class Conveyor : BaseEntity
    {
        [Required]
        [StringLength(256)]
        public string Code { get; set; }

        [StringLength(256)]
        public string Name { get; set; }

        public Int64? ConveyorType { get; set; }

        public int SerialNo { get; set; }
    }
}
