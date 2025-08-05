using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities.Configurations
{
    [Serializable]
    [Table("Tb_Vehicle", Schema = "Core")]
    public class Vehicle : BaseEntity
    {
        [NotMapped]
        public string Display
        {
            get
            {
                return string.IsNullOrEmpty(ShortText1) ? VehicleNo : ShortText1;
            }
        }

        [Required]
        [StringLength(50)]
        public string VehicleNo
        {
            get;
            set;
        }

        [StringLength(256)]
        public string VehicleModel
        {
            get;
            set;
        }

        public override string ToString()
        {
            return nameof(Vehicle) + " : " + VehicleNo;
        }

    }
}
