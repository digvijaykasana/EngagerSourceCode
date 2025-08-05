using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders
{
    /// <summary>
    /// https://eonasdan.github.io/bootstrap-datetimepicker/
    /// ShortText1 = Vessel Name
    /// ShortText2 = Summary Invoice No
    /// ShortText3 = Summary Invoice Id
    /// ShortText4 = Customer Company Name
    /// YesNo1     = isSavingCopySign
    /// YesNo2     = HasPendingNotification
    /// LongText2 = Modification Comments By Admin
    /// </summary>
    [Serializable]
    [Table("Tb_SalesOrder", Schema ="SOP")]
    public class WorkOrder : BaseEntity
    {
        //Aung Ye Kaung - 25-03-2019
        //Kyaw Min Htut - 10-04-2019
        [NotMapped]
        public bool HasPendingNotification
        {
            get
            {
                return this.YesNo2;
            }
            set
            {
                this.YesNo2 = value;
            }
        }

        [NotMapped]
        public string ModificationComments
        {
            get
            {
                return this.LongText2;
            }
            set
            {
                this.LongText2 = value;
            }
        }

        [NotMapped]
        public Int64 SummaryInvoiceId
        {
            get
            {
                return this.SalesInvoiceSummaryId;
            }
            set
            {
                this.SalesInvoiceSummaryId = value;
            }
        }

        [NotMapped]
        public DateTime SalesInvoiceSummaryDate
        {
            get; set;
        }

        public long SalesInvoiceSummaryId
        {
            get;
            set;
        }

        public List<WorkOrderPassenger> getpass;

        [NotMapped]
        public string VesselName
        {
            get
            {
                return this.ShortText1;
            }
            set
            {
                this.ShortText1 = value;
            }
        }

        [NotMapped]
        public string SummaryInvoiceNo
        {
            get
            {
                return this.ShortText2;
            }
            set
            {
                this.ShortText2 = value;
            }
        }

        [NotMapped]
        public bool isSavingCopySign
        {
            get
            {
                return this.YesNo1;
            }
            set
            {
                this.YesNo1 = value;
            }
        }

        [NotMapped]
        public string CustomerCompanyName
        {
            get
            {
                return this.ShortText4;
            }
            set
            {
                this.ShortText4 = value;
            }
        }


        [Required]
        [Index(IsUnique = false)]
        [StringLength(50)]
        public string RefereneceNo
        {
            get;
            set;
        } = "TBD";

        [NotMapped]
        public bool isFromOps
        {
            get;
            set;
        } = false;

        public override string ToString()
        {
            return nameof(WorkOrder) + " : " + RefereneceNo;
        }

        [Index(IsUnique = false)]
        public Int64 ReferenceNoNumber
        {
            get;
            set;
        }

        public DateTime WorkOrderDate
        {
            get;
            set;
        } = TimeUtil.GetLocalTime();

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
       
        public Int64? AgentId
        {
            get;
            set;
        }

        [ForeignKey("AgentId")]
        public User User
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
        public string VoyageNo
        {
            get;
            set;
        }

        #region Pick up Date and Time
        public DateTime? PickUpdateDate
        {
            get;
            set;
        }

        [StringLength(50)]
        public string PickUpDateBinding
        {
            get;
            set;
        }

        [StringLength(50)]
        public string PickUpTimeBinding
        {
            get;
            set;
        }

        [NotMapped]
        public string PickUpDateStr
        {
            get
            {
                string pickupTimeStr = PickUpTimeBinding == null ? "" : PickUpTimeBinding.ToString();

                return PickUpdateDate == null ? "" : Util.ConvertDateToString(PickUpdateDate.Value, DateConfig.CULTURE) + " " + pickupTimeStr;
            }
        }

        [NotMapped]
        public string PickUpDateOnlyStr
        {
            get
            {
                return PickUpdateDate == null ? string.Empty : Util.ConvertDateToString(PickUpdateDate.Value, DateConfig.CULTURE);
            }
        }

        [NotMapped]
        public string PickUpdateTimeStr
        {
            get
            {
                return PickUpdateDate == null ? string.Empty : PickUpdateDate.Value.ToShortTimeString();
            }
        }

        #endregion

        #region Standby Date and Time
        public DateTime? StandByDate
        {
            get;
            set;
        }
     
        [StringLength(50)]
        public string StandByDateBinding
        {
            get;
            set;
        }

        [StringLength(50)]
        public string StandByTimeBinding
        {
            get;
            set;
        }

        public void SetStandByDateTime()
        {
            if (!string.IsNullOrEmpty(StandByDateBinding) && !string.IsNullOrEmpty(StandByTimeBinding))
                this.StandByDate = Util.ConvertStringToDateTime(StandByDateBinding + " " + StandByTimeBinding, DateConfig.CULTURE);
            else if (!string.IsNullOrEmpty(StandByDateBinding))
                this.StandByDate = Util.ConvertStringToDateTime(StandByDateBinding, DateConfig.CULTURE);
        }

        #endregion

        public Int64? BoardTypeId
        {
            get;
            set;
        }

        [StringLength(256)]
        public string BoardType
        {
            get;
            set;
        }

        public int? NoOfPax
        {
            get;
            set;
        }

        public int? PickUpPax
        {
            get;
            set;
        }

        public OrderStatus Status
        {
            get;
            set;
        } = OrderStatus.Draft;

        [NotMapped]
        public string StatusColor
        {
            get
            {
                switch (Status)
                {
                    case OrderStatus.Cancelled:
                        return "#616161";
                    case OrderStatus.Draft:
                        return "#5D4037";
                    case OrderStatus.Ordered:
                        return "#E64A19";
                    case OrderStatus.Pending:
                        return "#FFA000";
                    case OrderStatus.Assigned:
                        return "#AFB42B";
                    case OrderStatus.Scheduled:
                        return "#689F38";
                    case OrderStatus.In_Progress:
                        return "#00796B";
                    case OrderStatus.Submitted:
                        return "#303F9F";
                    case OrderStatus.Verified:
                        return "#1A237E";
                    case OrderStatus.With_Accounts:
                        return "#4A148C";
                    case OrderStatus.Billed:
                        return "#000000";
                    default:
                        return "#FAFAFA";
                }
            }
        }

        /// <summary>
        /// Modified - Kaung [ 22-06-2018 ] [Original Work Order Status Colors] 
        /// </summary>
        //[NotMapped]
        //public string StatusColor
        //{
        //    get
        //    {
        //        switch (Status)
        //        {
        //            case OrderStatus.Cancelled:
        //                return "#616161";
        //            case OrderStatus.Draft:
        //                return "#5D4037";
        //            case OrderStatus.Ordered:
        //                return "#E64A19";
        //            case OrderStatus.Pending:
        //                return "#FFA000";
        //            case OrderStatus.Assigned:
        //                return "#AFB42B";
        //            case OrderStatus.Scheduled:
        //                return "#689F38";
        //            case OrderStatus.In_Progress:
        //                return "#00796B";
        //            case OrderStatus.Pending_Sign:
        //                return "#0288D1";
        //            case OrderStatus.Submitted:
        //                return "#303F9F";
        //            case OrderStatus.Verified:
        //                return "#1A237E";
        //            case OrderStatus.With_Accounts:
        //                return "#4A148C";
        //            case OrderStatus.Billed:
        //                return "#000000";
        //            default:
        //                return "#FAFAFA";
        //        }
        //    }
        //}

        [NotMapped]
        public OrderStatus PreviousStatus
        {
            get;
            set;
        } = OrderStatus.Draft;

        public Int64? InvoiceId
        {
            get;
            set;
        }

        [StringLength(50)]
        public string InvoiceNo
        {
            get;
            set;
        }

        public Int64? CreditNoteId
        {
            get;
            set;
        }

        [StringLength(50)]
        public string CreditNoteNo
        {
            get;
            set;
        }

        [StringLength(256)]
        public string PickUpPoint
        {
            get;
            set;
        }

        [StringLength(256)]
        public string DropPoint
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Drivers
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Description
        {
            get;
            set;
        }

        public string Remark
        {
            get;
            set;
        }

        public string AdminRemarksToDriver
        {
            get;
            set;
        }

        public string AdminRemarkInVoucher
        {
            get;
            set;
        }

        //Modified - Kaung [21-06-2018] [ Removed Status ' Scheduled = 40 ' and ' Pending_Sign = 60 ' ]

        public enum OrderStatus
        {
            Cancelled = -1,
            All = 0,
            Draft = 1,
            Ordered = 10,
            Pending = 20,
            Assigned = 30,
            Scheduled = 40,
            In_Progress = 50,
            Submitted = 70,
            Verified = 80,
            With_Accounts = 90,
            Billed = 100
        }

        public string GetWorkOrderNo(Int64 serialNo)
        {
            var temp = WorkOrderDate.Date.ToString("yy") + WorkOrderDate.Month.ToString("00") + serialNo.ToString();
            this.ReferenceNoNumber = Convert.ToInt64(temp);    
            return $"WO-{WorkOrderDate.Date.ToString("yy")}-{WorkOrderDate.Month.ToString("00")}-{serialNo.ToString()}";
        }

        public void GenerateDateFromString()
        {
            if (!string.IsNullOrEmpty(PickUpDateBinding) && !string.IsNullOrEmpty(PickUpTimeBinding))
                this.PickUpdateDate = Util.ConvertStringToDateTime(PickUpDateBinding + " " + PickUpTimeBinding, DateConfig.CULTURE);
            else if (!string.IsNullOrEmpty(PickUpDateBinding))
                this.PickUpdateDate = Util.ConvertStringToDateTime(PickUpDateBinding, DateConfig.CULTURE);

            if (!string.IsNullOrEmpty(StandByDateBinding) && !string.IsNullOrEmpty(StandByTimeBinding))
                this.StandByDate = Util.ConvertStringToDateTime(StandByDateBinding + " " + StandByTimeBinding, DateConfig.CULTURE);
            else if (!string.IsNullOrEmpty(StandByDateBinding))
                this.StandByDate = Util.ConvertStringToDateTime(StandByDateBinding, DateConfig.CULTURE);
        }

        [NotMapped]
        public List<WorkOrderLocation> WorkOrderLocationList
        {
            get;
            set;
        } = new List<WorkOrderLocation>();

        public List<WorkOrderLocation> GetLocations()
        {
            foreach(var detail in WorkOrderLocationList)
            {
                detail.WorkOrder = this;
                detail.Location = null;
            }

            return WorkOrderLocationList.Where(x => x.Delete == false).ToList();
        }

        [NotMapped]
        public List<WorkOrderPassenger> WorkOrderPassengerList
        {
            get;
            set;
        } = new List<WorkOrderPassenger>();

        public List<WorkOrderPassenger> GetPassengers()
        {
            foreach(var detail in WorkOrderPassengerList)
            {
                detail.WorkOrder = this;
                detail.Vehicle = null;
            }

            return WorkOrderPassengerList.ToList();
        }

        [NotMapped]
        public List<WorkOrderAirportMeetingService> MeetingServiceList
        {
            get;
            set;
        } = new List<WorkOrderAirportMeetingService>();

        public List<WorkOrderAirportMeetingService> GetMeetingServices()
        {
            foreach(var detail in MeetingServiceList)
            {
                detail.MeetingService = null;
                detail.WorkOrder = this;
            }
            return MeetingServiceList.Where(x => x.Delete == false).ToList();
        }

        [NotMapped]
        public List<ServiceJob> ServiceJobList
        {
            get;
            set;
        } = new List<ServiceJob>();

        public List<ServiceJob> GetServiceJobs()
        {
            foreach(var serviceJob in ServiceJobList)
            {
                serviceJob.WorkOrder = this;
                serviceJob.User = null;
                serviceJob.Vehicle = null;
                serviceJob.CustomerId = CustomerId;
            }

            return ServiceJobList;
        }

        public Int64 GetPassengerInChargeId(Int64 vehicleId = 0)
        {
            var inCharge = WorkOrderPassengerList.FirstOrDefault(x => x.VehicleId == vehicleId && x.InCharge);

            return inCharge == null ? 0 : inCharge.Id;
        }

        public string GetPassengerInCharge(Int64 vehicleId = 0)
        {
            var inCharge = WorkOrderPassengerList.FirstOrDefault(x => x.VehicleId == vehicleId && x.InCharge);

            return inCharge == null ? string.Empty : inCharge.Name;
        }

        public string GetPassengerInChargeRank(Int64 vehcileId=0)
        {
            var inCharge = WorkOrderPassengerList.FirstOrDefault(x => x.VehicleId == vehcileId && x.InCharge);
            return inCharge == null ? string.Empty : inCharge.Rank;
        }

        public string GetNoOfPax(Int64 vehicleId = 0)
        {
            //Returns Total noOfPax for the vehicle [ Modified - Kaung - 2018 06 11 ]

            //Int32 totalPax = 0;

            //* Third Version - Returns total number of PickUp Pax for all Jobs

            //return NoOfPax == null ? "0" : NoOfPax.ToString();

            /**
             * Modified - Kaung [25-06-2018] [Changed to Third Version]
             * Second Version - Returns total Number of Pax per Vehicle [Accepts 'Int64 vehicleId = 0' as parameter]

            var passengers = WorkOrderPassengerList.Where(x => x.VehicleId == vehicleId).ToList();

            foreach(WorkOrderPassenger passenger in passengers)
            {
                totalPax = totalPax + Convert.ToInt32(passenger.NoOfPax);
            }

            return totalPax == 0 ? string.Empty : totalPax.ToString();**/

            
            //Original - Returns inCharge's noOfPax

            var inCharge = WorkOrderPassengerList.FirstOrDefault(x => x.VehicleId == vehicleId && x.InCharge);
            return inCharge == null ? string.Empty : inCharge.NoOfPax.ToString();


            //string noOfPax = " x " + WorkOrderPassengerList.Where(x => x.VehicleId == vehicleId).ToList().Count + " pax";
            //return noOfPax;
        }

        public string GetPickUpPoint()
        {
            string PickUpPointStr = "";

            var workOrderLocation = WorkOrderLocationList.FirstOrDefault(x => x.Type == WorkOrderLocation.LocationType.PickUp);
            if (workOrderLocation == null) return string.Empty;
            //return workOrderLocation.Location != null ? workOrderLocation.Location.Name : workOrderLocation.Description;
            if (workOrderLocation.Location != null)
            {
                PickUpPointStr = workOrderLocation.Location.Name == "Hotel" ? workOrderLocation.Location.Name + " - " + workOrderLocation.Hotel : workOrderLocation.Location.Name;
            }
            else
            {
                PickUpPointStr = workOrderLocation.Description;
            }
            
            return PickUpPointStr;
        }

        public string GetAdditionalStop()
        {
            try
            {
                string AdditionalStopStr = "";

                var additionalStops = WorkOrderLocationList.ToList().Where(X => X.Type == WorkOrderLocation.LocationType.AdditionalStop);

                if (additionalStops != null && additionalStops.Count() > 0)
                {
                    foreach (var stop in additionalStops.ToList())
                    {
                        if(stop.Location !=  null)
                        {
                            string currentAdditionalStop = "";

                            if(stop.Location.Name == "Hotel")
                            {
                                if(String.IsNullOrEmpty(stop.Hotel))
                                {
                                    var stopDescription = String.IsNullOrEmpty(stop.Description) ? string.Empty : stop.Description;

                                    currentAdditionalStop = stop.Location.Name + " - " + stopDescription + "; ";
                                }
                                else
                                {
                                    currentAdditionalStop = stop.Location.Name + " - " + stop.Hotel +  "; ";
                                }
                            }
                            else
                            {
                                currentAdditionalStop = stop.Location.Name + "; ";
                            }


                            AdditionalStopStr += currentAdditionalStop;

                        }
                        else
                        {
                            var stopDescription = String.IsNullOrEmpty(stop.Description) ? string.Empty : stop.Description;

                            AdditionalStopStr += stopDescription + "; ";
                        }
                    }
                }

                return AdditionalStopStr;
            }
            catch(Exception ex)
            {
                return string.Empty;
            }

        }

        //For use in Mobile Api Call
        public string GetPickUpPointDesc()
        {
            var workOrderLocation = WorkOrderLocationList.FirstOrDefault(x => x.Type == WorkOrderLocation.LocationType.PickUp);
            if (workOrderLocation == null) return string.Empty;
            return workOrderLocation.Location != null ? workOrderLocation.Description : string.Empty;
        }

        public string GetDropOffPoint()
        {
            var workOrderLocation = WorkOrderLocationList.FirstOrDefault(x => x.Type == WorkOrderLocation.LocationType.DropOff);
            if (workOrderLocation == null) return string.Empty;

            if(workOrderLocation.Location != null)
            {
                return workOrderLocation.Location.Name == "Hotel" ? workOrderLocation.Location.Name + " - " + workOrderLocation.Hotel : workOrderLocation.Location.Name;
            }
            else
            {
                return workOrderLocation.Description;
            }
        }

        //For use in Mobile Api Call
        public string GetDropOffPointDesc()
        {
            var workOrderLocation = WorkOrderLocationList.FirstOrDefault(x => x.Type == WorkOrderLocation.LocationType.DropOff);
            if (workOrderLocation == null) return string.Empty;
            return workOrderLocation.Location != null ? workOrderLocation.Description : string.Empty;
        }

        public string GetFlightNo()
        {
            var meetingServices = MeetingServiceList.ToList();

            if(meetingServices == null)
            {
                return string.Empty;
            }

            string flightNo = "";

            foreach(var service in meetingServices)
            {

                flightNo = String.IsNullOrEmpty(service.FlightNo) ? flightNo + ", " : flightNo + service.FlightNo + " - " + service.NoOfPax + " pax, ";

            }

            flightNo = flightNo.TrimEnd(new char[] { ',', ' ' });

            return flightNo;
        }

        public Int64 GetPickupPointId()
        {
            var workOrderLocation = WorkOrderLocationList.FirstOrDefault(x => x.Type == WorkOrderLocation.LocationType.PickUp);
            return workOrderLocation.LocationId.HasValue ? workOrderLocation.LocationId.Value : 0;
        }

        public Int64 GetDropOffPointId()
        {
            var workOrderLocation = WorkOrderLocationList.FirstOrDefault(x => x.Type == WorkOrderLocation.LocationType.DropOff);
            return workOrderLocation.LocationId.HasValue ? workOrderLocation.LocationId.Value : 0;
        }

        public int GetNoOfVouchers()
        {
            return ServiceJobList == null ? 0 : ServiceJobList.Where(x => x.Delete == false).ToList().Count;
        }

        public string GetLocationStr()
        {
            string locationStr = this.PickUpPoint + " to " + this.DropPoint;
            locationStr = locationStr.Replace("-", "");
            return locationStr;
        }

        [NotMapped]
        public List<Notification> WorkOrderNotificationList
        {
            get;
            set;
        } = new List<Notification>();

        [NotMapped]
        public List<WorkOrderHistory> WorkOrderHistoryList
        {
            get;
            set;
        } = new List<WorkOrderHistory>();

        [NotMapped]
        public bool IsOverNightJob
        {
            get
            {
                if(PickUpdateDate.HasValue)
                {
                    var workOrderDate = this.PickUpdateDate.Value.Date + new TimeSpan(00, 00, 00);

                    //yesterday 23:1 to today 07:00
                    var yesterdayFromTime = workOrderDate.AddDays(-1).Add(new TimeSpan(23, 31, 0));
                    var todayToTime = workOrderDate.Add(new TimeSpan(7, 0, 0));

                    if (yesterdayFromTime < this.PickUpdateDate && this.PickUpdateDate < todayToTime)
                    {
                        return true;
                    }

                    //today 23:31 to tomrrow 07:00
                    var todayFromTime = workOrderDate.Add(new TimeSpan(23, 31, 0));

                    var tmrToTime = workOrderDate.AddDays(1).Add(new TimeSpan(7, 0, 0));

                    if(todayFromTime < this.PickUpdateDate && this.PickUpdateDate < tmrToTime)
                    {
                        return true;
                    }
                        return false;
                    
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
