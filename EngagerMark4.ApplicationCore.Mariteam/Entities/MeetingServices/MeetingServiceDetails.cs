using EngagerMark4.ApplicationCore.Entities;
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
    [Table("Tb_MeetingServiceDetails", Schema = "Dummy")]
    public class MeetingServiceDetails : BaseEntity
    {
        [Required]
        public Int64 MeetingServiceId
        {
            get;
            set;
        }

        [ForeignKey("MeetingServiceId")]
        public MeetingService MeetingService
        {
            get;
            set;
        }

        [Required]
        public Int32 Serial
        {
            get;
            set;
        } = 0;

        [Required]
        [StringLength(500)]
        public string NoOfPax
        {
            get;
            set;
        }

        public int MinPax
        {
            get;
            set;
        }

        public int MaxPax
        {
            get;
            set;
        }

        [Range(0,999999999999999999.9999999999)]
        public decimal Charges
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
            return nameof(MeetingServiceDetails) + " : " + Id;
        }
    }
}
