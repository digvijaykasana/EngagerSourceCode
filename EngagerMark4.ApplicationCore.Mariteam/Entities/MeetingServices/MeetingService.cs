using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Dummy.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Dummy.Entities.MeetingServices
{
    [Serializable]
    [Table("Tb_MeetingService", Schema = "Dummy")]
    public class MeetingService : BaseEntity
    {
        public Int64? CustomerId
        {
            get;
            set;
        }

        [ForeignKey("CustomerId")]
        public Customer.Entities.Customer Customer
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

        public string Description
        {
            get;
            set;            
        }
        
        public Boolean IsDefault
        {
            get;
            set;
        }

        public decimal AdditionalPersonCharge
        {
            get;
            set;
        }

        public decimal LastMinuteCharge
        {
            get;
            set;
        }

        public decimal OvernightCharge
        {
            get;
            set;
        }

        public decimal MajorAmendmentCharge
        {
            get;
            set;
        }

        public bool IsHighCharge
        {
            get;
            set;
        }
        
        [NotMapped]
        public string CustomerName
        {
            get
            {
                return this.LongText1;
            }
        }

        
        [NotMapped]
        public ICollection<MeetingServiceDetails> MeetingServiceDetails
        {
            get;
            set;
        } = new List<MeetingServiceDetails>();

        public IEnumerable<MeetingServiceDetails> GetMeetingServiceDetails()
        {
            foreach (var detail in MeetingServiceDetails)
            {
                detail.MeetingService = this;
            }
            return this.MeetingServiceDetails.Where(x => x.Delete == false).ToList().OrderBy(x=>x.Serial);
        }

        public override string ToString()
        {
            return nameof(MeetingService) + " : " + Name + " : " + Id ;
        }

    }
}
