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
    [Table("Tb_CommonConfiguration", Schema = "Core")]
    public class CommonConfiguration : BaseEntity
    {
        public Int64 ConfigurationGroupId { get; set; }

        [ForeignKey("ConfigurationGroupId")]
        public ConfigurationGroup ConfigurationGroup { get; set; }

        [Required]
        [StringLength(255)]
        public string Code { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public int SerialNo
        {
            get;
            set;
        }

        [NotMapped]
        public List<CustomerVesselViewModel> CustomerList
        {
            get;
            set;
        } = new List<CustomerVesselViewModel>();

        public List<CustomerVesselViewModel> GetCustomers()
        {
            foreach(var customer in CustomerList.Where(x => x.Delete == false))
            {
                customer.VesselId = this.Id;
                customer.Vessel = this.Name;
            }

            return CustomerList.Where(x => x.Delete == false).ToList();
        }

        public override string ToString()
        {
            return nameof(CommonConfiguration) + " : " + Name;
        }
    }
}
