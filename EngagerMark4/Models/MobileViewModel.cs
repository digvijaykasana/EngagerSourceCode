using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Job.Entities;
using EngagerMark4.ApplicationCore.Dummy.Entities.MeetingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EngagerMark4.Models
{
    public class MobileViewModel
    {
        public List<CheckList> Checklists
        {
            get;
            set;
        }

        public List<MeetingService> MeetingServices
        {
            get;
            set;
        }

        public List<CommonConfiguration> CustomDetentions
        {
            get;
            set;
        }

        public List<CommonConfiguration> Ranks
        {
            get;
            set;
        }
    }
}