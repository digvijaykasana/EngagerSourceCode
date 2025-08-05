using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders
{
    [Serializable]
    [Table("Tb_WorkOrderLocation", Schema ="SOP")]
    public class WorkOrderLocation : BasicEntity
    {
        public Int64? WorkOrderId
        {
            get;
            set;
        }

        [ForeignKey("WorkOrderId")]
        public WorkOrder WorkOrder
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

        public Int64? HotelId
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Hotel
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

        public LocationType Type
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

        public enum LocationType
        {
            PickUp = 10,
            AdditionalStop = 20,
            DropOff = 30
        }

        public static List<CommonConfiguration> GetLocationTypes()
        {
            List<CommonConfiguration> configurations = new List<CommonConfiguration>();
            CommonConfiguration pickUp = new CommonConfiguration { Id = (int)LocationType.PickUp, Name = "Pick up" };
            configurations.Add(pickUp);
            CommonConfiguration additionalStop = new CommonConfiguration { Id = (int)LocationType.AdditionalStop, Name = "Additional Stop" };
            configurations.Add(additionalStop);
            CommonConfiguration dropOff = new CommonConfiguration { Id = (int)LocationType.DropOff, Name = "Drop Off" };
            configurations.Add(dropOff);
            return configurations;
        }

        public override string ToString()
        {
            return nameof(WorkOrderLocation) + " : " + Id;
        }
    }
}
