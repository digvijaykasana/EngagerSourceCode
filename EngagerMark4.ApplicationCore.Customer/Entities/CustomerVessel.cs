using EngagerMark4.ApplicationCore.Entities;
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
    [Table("Tb_CustomerVessel", Schema ="Customer")]
    public class CustomerVessel : BasicEntity
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

        public Int64 VesselId
        {
            get;set;
        }

        [StringLength(255)]
        public string Vessel
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
            return nameof(CustomerVessel) + " : " + Id;
        }

    }
}
