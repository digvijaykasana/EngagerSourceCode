using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Customer.Entities
{
    [Serializable]
    [Table("Tb_CustomerLocation", Schema = "Customer")]
    public class CustomerLocation : BaseEntity
    {
        public Int64 CustomerId
        {
            get;
            set;
        }

        [ForeignKey("CustomerId")]
        public Customer Customer
        {
            get;
            set;
        }

        public Int64 LocationId
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

        public bool Main
        {
            get;
            set;
        }

        [StringLength(255)]
        public string ContactPerson
        {
            get;
            set;
        }

        [StringLength(255)]
        public string ContactNo
        {
            get;
            set;
        }

        [StringLength(255)]
        public string Fax
        {
            get;
            set;
        }

        [StringLength(255)]
        [EmailAddress]
        public string Email
        {
            get;
            set;
        }

        [NotMapped]
        public bool Delete
        {
            get;
            set;
        }

        public override string ToString()
        {
            return nameof(CustomerLocation) + " : " + Id;
        }
    }
}
