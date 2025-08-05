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
    public class DriverDailyReportViewModel
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
                return EndExecutionTime == null ? string.Empty : EndExecutionTime.Value.ToString("dd/MM/yyyy HH:mm");
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



        [Display(Name = "Trip")]
        public string TripStr
        {
            get
            {
                if (Trip == 0) return string.Empty;

                return this.Trip.ToString("0.00");
            }
        }
        public decimal Trip { get; set; }

        [Display(Name = "MS")]
        public string MSStr
        {
            get
            {
                if (MS == 0) return string.Empty;

                return this.MS.ToString("0.00");
            }
        }
        public decimal MS { get; set; }

        [Display(Name = "Parking Fees")]
        public string ParkFeeStr
        {
            get
            {
                if (ParkFee == 0) return string.Empty;

                return this.ParkFee.ToString("0.00");
            }
        }
        public decimal ParkFee { get; set; }

        [Display(Name = "ERP")]
        public string ERPStr
        {
            get
            {
                if (ERP == 0) return string.Empty;

                return this.ERP.ToString("0.00");
            }
        }
        public decimal ERP { get; set; }


        [Display(Name = "Meal")]
        public string MealStr
        {
            get
            {
                if (Meal == 0) return string.Empty;

                return this.Meal.ToString("0.00");
            }
        }
        public decimal Meal { get; set; }


        [Display(Name = "NEA Cert.")]
        public string NEACertStr
        {
            get
            {
                if (NEACert == 0) return string.Empty;

                return this.NEACert.ToString("0.00");
            }
        }
        public decimal NEACert { get; set; }

        [Display(Name = "Ferry Ticket")]
        public string FerryTicketStr
        {
            get
            {
                if (FerryTicket == 0) return string.Empty;

                return this.FerryTicket.ToString("0.00");
            }
        }
        public decimal FerryTicket { get; set; }

        [Display(Name = "JP Pass Fee")]
        public string JPPassStr
        {
            get
            {
                if (JPPass == 0) return string.Empty;

                return this.JPPass.ToString("0.00");
            }
        }
        public decimal JPPass { get; set; }

        [Display(Name = "PSA Pass Fee")]
        public string PSAPassStr
        {
            get
            {
                if (PSAPass == 0) return string.Empty;

                return this.PSAPass.ToString("0.00");
            }
        }
        public decimal PSAPass { get; set; }

        [Display(Name = "Others")]
        public string OthersStr
        {
            get
            {
                if (Others == 0) return string.Empty;

                return this.Others.ToString("0.00");
            }
        }
        public decimal Others
        {
            get;
            set;
        }

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
        public string CheckListIds
        {
            get;
            set;
        }

        //PCR2021
        public List<ServiceJobChecklistItem> GetChecklistItemList()
        {
            List<ServiceJobChecklistItem> list = new List<ServiceJobChecklistItem>();

            if (String.IsNullOrEmpty(this.CheckListIds)) return list;

            var checklistValArr = this.CheckListIds.Split(','); //Item level split

            foreach (var checklistVal in checklistValArr)
            {
                if (!string.IsNullOrEmpty(checklistVal))
                {
                    ServiceJobChecklistItem item = new ServiceJobChecklistItem();
                    item.ServiceJobId = this.ServiceJobId;

                    var currentChecklistValArr = checklistVal.Split('|'); //Value level split

                    item.ChecklistId = Convert.ToInt64(currentChecklistValArr[0]);

                    if (currentChecklistValArr.Length > 1)
                    {
                        item.ChecklistPrice = Convert.ToDecimal(currentChecklistValArr[1]);
                    }

                    list.Add(item);
                }
            }

            return list;
        }
    }
}
