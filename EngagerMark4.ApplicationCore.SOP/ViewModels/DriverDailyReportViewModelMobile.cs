using EngagerMark4.ApplicationCore.DTOs;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.ReportViewModels
{
    public class DriverDailyReportViewModelMobile
    {
        [Display(Name = "Time Start")]
        public string PickUpTime
        {
            get
            {
                if (PickUpDate == null) return string.Empty;

                return PickUpDate.Value.ToString("dd/MM/yyyy HH:mm");
            }
        }

        [Display(Name = "Time End")]
        public string EndExecutionTimeStr
        {
            get
            {
                return EndExecutionTime == null ? string.Empty : EndExecutionTime.Value.ToString("MM/dd/yyyy HH:mm");
            }
        }

        [Display(Name = "Agent")]
        public string AgentName
        {
            get;
            set;
        }

        [Display(Name = "Vessel")]
        public string VesselName
        {
            get;
            set;
        }

        [Display(Name = "From")]
        public string PickUpPoint
        {
            get;
            set;
        }

        [Display(Name="To")]
        public string DropOffPoint
        {
            get;
            set;
        }

        [Display(Name ="No of Pax")]
        public Int32? NoOfPax
        {
            get;
            set;
        } = 0;

        [Display(Name = "Pickup Pax")]
        public int PickupPax
        {
            get;
            set;
        }

        [Display(Name = "MS")]
        public string MS
        {
            get;
            set;
        } = string.Empty;

        [Display(Name = "Park Fees")]
        public string ParkFee
        {
            get;
            set;
        } = string.Empty;

        [Display(Name = "ERP")]
        public string ERP
        {
            get;
            set;
        } = string.Empty;

        [Display(Name = "Trip S$")]
        public string Trip
        {
            get;
            set;
        } = string.Empty;

        [Display(Name = "Remark")]
        public string Remark
        {
            get;
            set;
        } = string.Empty;

        public Int64 WorkOrderId
        {
            get;
            set;
        }

        public Int64 ServiceJobId
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string WorkOrderNo
        {
            get;
            set;
        }

        public DateTime? PickUpDate
        {
            get;
            set;
        }

        public string PickUpDateStr
        {
            get
            {
                return PickUpDate == null ? string.Empty : Util.ConvertDateToString(PickUpDate.Value, DateConfig.CULTURE);
            }
        }

        

        public DateTime? StandByDate
        {
            get;
            set;
        }

        public string StandByDateStr
        {
            get
            {
                return StandByDate == null ? string.Empty : Util.ConvertDateToString(StandByDate.Value, DateConfig.CULTURE);
            }
        }

        public string StandByTime
        {
            get
            {
                if (StandByDate == null) return string.Empty;

                return StandByDate.Value.ToString("HH:mm");
            }
        }

        public Int64? DriverId
        {
            get;
            set;
        }

        public string DriverName
        {
            get;
            set;
        }

        public string VehicleNo
        {
            get;
            set;
        }

        public DateTime? StartExecutionTime
        {
            get;
            set;
        }


        public DateTime? EndExecutionTime
        {
            get;
            set;
        }

        

        public Int64? AgendId
        {
            get;
            set;
        }

        

        public string CompanyName
        {
            get;
            set;
        }

        public Int64? VesselId
        {
            get;
            set;
        }

        

        

        public string PickUpPointDesc
        {
            get;
            set;
        }

        

        public string DropOffPointDesc
        {
            get;
            set;
        }


        public string AdditionalStops
        {
            get;
            set;
        } = "";

        public string FlightNo
        {
            get;
            set;
        }

        public string BoardTypeStr
        {
            get;
            set;
        }

        public Int64? BoardTypeId
        {
            get;
            set;
        }

        public string InChargePassenger
        {
            get;
            set;
        }

        public Int64? RankId
        {
            get;
            set;
        }

        public string RankStr
        {
            get;
            set;
        }

        public WorkOrder.OrderStatus Status
        {
            get;
            set;
        }

        public string StatusStr
        {
            get
            {
                return Status.ToString();
            }
        }

        public ServiceJob.ServiceJobStatus ServiceJobStatus
        {
            get;
            set;
        }

        public string ServiceJobStatusStr
        {
            get
            {
                return ServiceJobStatus.ToString();
            }
        }
        //PCR2021
        public List<WorkOrderPassengerDTO> WorkOrderPassengerList
        {
            get;
            set;
        } = new List<WorkOrderPassengerDTO>();

        //PCR2021
        public bool TFRequireAllPassSignatures
        {
            get;
            set;
        }

        //PCR2021
        public string TripFees { get; set; }

        //PCR2021
        public bool IncludeMeetingServiceFee { get; set; } = false;
    }
}
