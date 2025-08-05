using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Equipment.Entities.Conveyors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Equipment.Entities.Equipments
{
    [Serializable]
    [Table("Tb_Equipment", Schema = "Equipment")]
    public class Equipment : BaseEntity
    {
        [Required]
        [StringLength(256)]
        public string Code { get; set; }

        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        public Int64 ConveyorId { get; set; }

        [ForeignKey("ConveyorId")]
        public Conveyor Conveyor { get; set; }

        public int SerialNo { get; set; }
    }
}
