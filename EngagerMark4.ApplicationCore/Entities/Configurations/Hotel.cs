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
    [Table("Tb_Hotel", Schema = "Core")]
    public class Hotel : BaseEntity
    {
        [NotMapped]
        public string Display
        {
            get
            {
                return this.Name + " - " + this.PostalCode;
            }
        }

        [Required]
        [StringLength(255)]
        public string Name
        {
            get;
            set;
        }

        [StringLength(50)]
        public string PostalCode
        {
            get;
            set;
        }

        [StringLength(255)]
        public string Address
        {
            get;
            set;
        }

        [StringLength(255)]
        public string City
        {
            get;
            set;
        }

        [StringLength(255)]
        public string Country
        {
            get;
            set;
        }

        [NotMapped]
        public string Latitude
        {
            get
            {
                return this.ShortText1;
            }
            set
            {
                this.ShortText1 = value;
            }
        }

        [NotMapped]
        public string Longitude
        {
            get
            {
                return this.ShortText2;
            }
            set
            {
                this.ShortText2 = value;
            }
        }

        public override string ToString()
        {
            return nameof(Hotel) + " : " + Name;
        }
    }
}
