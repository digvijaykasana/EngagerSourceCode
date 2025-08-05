using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Inventory.Entities
{
    [Serializable]
    [Table("Tb_PriceLocation", Schema = "Inventory")]
    public class PriceLocation : BaseEntity
    {
        public Int64? PriceId
        {
            get;
            set;
        }

        [ForeignKey("PriceId")]
        public Price Price
        {
            get;
            set;
        }

        public Int64? LocationId
        {
            get;
            set;
        }

        [ForeignKey("LocationId")]
        public Location Location
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Description
        {
            get;
            set;
        }

        public PriceLocationType Type
        {
            get;
            set;
        }

        [NotMapped]
        public int TypeInt
        {
            get
            {
                return (int)Type;
            }
        }

        [NotMapped]
        public string TypeStr
        {
            get
            {
                return Type.ToString();
            }
        }

        [NotMapped]
        public bool Delete
        {
            get;
            set;
        }

        
        public enum PriceLocationType
        {
            PickUp = 10,
            AdditionalStop = 20,
            DropOff = 30
        }

        public static List<CommonConfiguration> GetPriceLocationTypes()
        {
            List<CommonConfiguration> configurations = new List<CommonConfiguration>();
            CommonConfiguration pickUp = new CommonConfiguration { Id = (int)PriceLocationType.PickUp, Name = "Pick up" };
            configurations.Add(pickUp);
            CommonConfiguration additionalStop = new CommonConfiguration { Id = (int)PriceLocationType.AdditionalStop, Name = "Additional Stop" };
            configurations.Add(additionalStop);
            CommonConfiguration dropOff = new CommonConfiguration { Id = (int)PriceLocationType.DropOff, Name = "Drop Off" };
            configurations.Add(dropOff);
            return configurations;
        }

        public override string ToString()
        {
            return nameof(PriceLocation) + " : " + Id;
        }
    }
}
