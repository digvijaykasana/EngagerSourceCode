using EngagerMark4.ApplicationCore.Dummy.Entities.MeetingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EngagerMark4.Models
{
    [Serializable]
    public class MeetingServiceViewModel
    {
        public MeetingServiceViewModel()
        {
            this.MeetingService = new MeetingService();
        }

        public string SessionKey
        {
            get; set;
        }
        
        public MeetingService MeetingService
        {
            get; set;
        }

        public MeetingServiceDetails MeetingServiceDetails
        {
            get; set;
        }

        public MeetingService GetMeetingService(List<MeetingServiceDetails> msDetails)
        {
            this.MeetingService.MeetingServiceDetails = new List<MeetingServiceDetails>();

            foreach(MeetingServiceDetails detail in msDetails)
            {
                MeetingServiceDetails newDetail = new MeetingServiceDetails();
                newDetail.Serial = detail.Serial;
                newDetail.NoOfPax = detail.NoOfPax;
                newDetail.Charges = detail.Charges;
            }
            return this.MeetingService;
        }


    }
}