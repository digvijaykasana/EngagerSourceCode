using EngagerMark4.ApplicationCore.Dummy.Entities.MeetingServices;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.ViewModels
{
    public class MeetingServiceFormViewModel
    {
        public string CurrentDate
        {
            get
            {
                return Util.ConvertDateToString(DateTime.Now, DateConfig.CULTURE);
            }
        }

        public WorkOrder WorkOrder { get; set; }

        public WorkOrderAirportMeetingService WorkOrderMeetingServiceEntity { get; set; }

        public MeetingService MeetingServiceEntity { get; set; }

        public decimal GSTAmount { get; set; }

        public decimal GrandTotal
        {
            get
            {
                return this.GSTAmount + this.WorkOrderMeetingServiceEntity.Charges;
            }
        }
    }
}
