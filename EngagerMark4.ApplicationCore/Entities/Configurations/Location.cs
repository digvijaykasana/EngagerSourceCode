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
    [Table("Tb_Location", Schema = "Core")]
    public class Location : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Code
        {
            get;
            set;
        }

        [Required]
        [StringLength(256)]
        public string Name
        {
            get;
            set;
        }

        [StringLength(100)]
        public string PostalCode
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Address
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Latitude
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Longitude
        {
            get;
            set;
        }

        [StringLength(256)]
        public string City
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Country
        {
            get;
            set;
        }

        //for use in Price List _ Pickup Point and Drop Off Point
        [NotMapped]
        public string Display
        {
            get
            {
                return Name + " - " + PostalCode;
            }
        }

        public override string ToString()
        {
            return nameof(Location) + " : " + Name;
        }
    }
}
