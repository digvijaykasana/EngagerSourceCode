using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Dummy.Entities.MeetingServices;
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
    public class WorkOrderAirportMeetingService : BasicEntity
    {
        public Int64 WorkOrderId
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

        public Int64 AirportMeetingServiceId
        {
            get;
            set;
        }

        [ForeignKey("AirportMeetingServiceId")]
        public MeetingService MeetingService
        {
            get;
            set;
        }

        public Int64? VesselId
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Vessel
        {
            get;set;
        }

        [StringLength(256)]
        public string FlightNo
        {
            get;
            set;
        }

        [StringLength(256)]
        public string MeetingServicePassengerInCharge
        {
            get;
            set;
        }

        public bool IsLastMinuteCharge
        {
            get;
            set;
        } = false;

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

        public bool IsMajorAmendment
        {
            get;
            set;
        } = false;

        public decimal MajorAmendmentCharge
        {
            get;
            set;
        } 

        public int NoOfPax
        {
            get;
            set;
        }

        public string PerPaxChargeLabel
        {
            get;
            set;
        } = "";

        public decimal PerPaxCharge
        {
            get;
            set;
        }

        public int MaxPaxRange
        {
            get;
            set;
        }

        public Int64 MeetingServiceDetailId
        {
            get;
            set;
        }

       public decimal AdditionalPersonCharge
        {
            get;
            set;
        }

        public decimal TotalPaxCharge
        {
            get;
            set;
        }

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
            return nameof(WorkOrderAirportMeetingService) + " : " + Id;
        }
    }
}
