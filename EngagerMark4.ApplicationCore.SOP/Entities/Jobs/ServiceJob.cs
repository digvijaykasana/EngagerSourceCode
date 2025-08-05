using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Entities.Jobs
{
    /// <summary>
    /// Id1 = PickUpPax
    /// LongText1 = ServiceJobReason
    /// ShortText1 = CompanyName
    /// ShortText2 = MeetingServiceIds
    /// ShortText9 = Signature Name
    /// YesNo10 = Copy Sign
    /// YesNo9 = Is Create Notification Sent
    /// </summary>
    [Serializable]
    [Table("Tb_ServiceJob", Schema ="SOP")]
    public class ServiceJob : BaseEntity
    {
        [NotMapped]
        public bool CopySign
        {
            get { return this.YesNo10; }
            set
            {
                this.YesNo10 = value;
            }
        }

        [NotMapped]
        public bool IsCreatedNotiSent
        {
            get { return this.YesNo9; }
            set
            {
                this.YesNo9 = value;
            }
        }

        //PCR2021 - OLD VERSION - OBSOLETE
        public Int64 SignatureId
        {
            get;
            set;
        }

        [NotMapped]
        public string CompanyName
        {
            get { return this.ShortText1; }
            set
            {
                this.ShortText1 = value;
            }
        }

        [NotMapped]
        public string SignatureName
        {
            get
            {
                return this.ShortText9;
            }
            set
            {
                this.ShortText9 = value;
            }
        }

        [Index(IsUnique = false)]
        public Int64 ReferenceNoNumber
        {
            get;
            set;
        }

        public Int64? ChecklistTemplateId
        {
            get;
            set;
        }

        public Int64? CustomerId
        {
            get;
            set;
        }

        public string Customer
        {
            get;
            set;
        }

        public Int64? WorkOrderId
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

        public Int64? UserId
        {
            get;
            set;
        }
        [ForeignKey("UserId")]
        public User User
        {
            get;
            set;
        }

        public Int64? VehicleId
        {
            get;
            set;
        }

        [ForeignKey("VehicleId")]
        public Vehicle Vehicle
        {
            get;
            set;
        }

        [Required]
        [Index(IsUnique = false)]
        [StringLength(50)]
        public string ReferenceNo
        {
            get;
            set;
        } = "TBD";

        public string GetSJno(Int64 serialNo)
        {
            var temp = Created.Date.ToString("yy") + Created.Month.ToString("00") + serialNo.ToString();
            ReferenceNoNumber = Convert.ToInt64(temp);
            return $"SJ-{Created.Date.ToString("yy")}-{Created.Month.ToString("00")}-{serialNo.ToString()}";
        }

        public DateTime? StartDateTime
        {
            get;
            set;
        }

        public DateTime? EndDateTime
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

        [NotMapped]
        public string EndExecutionTimeStr
        {
            get
            {
                return EndExecutionTime == null ? string.Empty : EndExecutionTime.Value.ToShortTimeString();
            }
        }

        [StringLength(256)]
        public string StartExecutionPlace
        {
            get;
            set;
        }

        [StringLength(256)]
        public string EndExecutionPlace
        {
            get;
            set;
        }

        public Int64? CustomDetentionId
        { get; set; }

        [StringLength(256)]
        public string CustomDetention
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Disposals
        {
            get;
            set;
        }

        [StringLength(256)]
        public string AdditionalStops
        {
            get;
            set;
        }

        [StringLength(256)]
        public string WaitingTime
        {
            get;
            set;
        }

        [StringLength(256)]
        public string CheckListIds
        {
            get;
            set;
        }

        //Currently not in Use
        public string ChecklistStr
        {
            get;
            set;
        }

        [NotMapped]
        public string MeetingServiceIds
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
        public string ServiceJobSubmission
        {
            get
            {
                return this.LongText1;
            }
            set
            {
                this.LongText1 = value;
            }
        }

        public string DriverRemark
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

        public ServiceJobStatus Status
        {
            get;
            set;
        }

        //Modified - Kaung [ 22-06-2018 ] 
        //[ For display on the Work Order Details Form ]
        [NotMapped]
        public string ServiceJobStatusStr
        {
            get
            {
                return Status.ToString();
            }
        }


        //Modified - Kaung [ 21-06-2018 ] 
        //[ Removed Status ' Completed = 2 ' ] [ Added Status 'Scheduled = 1' and 'Pending_Sign = 2' ]
        public enum ServiceJobStatus
        {
            Pending = 0,
            Scheduled = 1,
            In_Progress = 2, 
            Pending_Sign = 3,
            Submitted = 4           
        }

        [NotMapped]
        public int PickUpPax
        {
            get
            {
                return this.Id1;
            }
            set
            {
                this.Id1 = value;
            }
        }

        //PCR2021
        [NotMapped]
        public Int64 LetterheadId
        {
            get;
            set;
        }

        //PCR2021
        public decimal TripFees
        { 
            get; 
            set;
        } = 0;

        //PCR2021
        public decimal MSFees
        {
            get;
            set;
        } = 0;

        public override string ToString()
        {
            return nameof(ServiceJob) + " : " + Id;
        }

        //PCR2021
        public List<ServiceJobChecklistItem> GetChecklistItemList()
        {
            List<ServiceJobChecklistItem> list = new List<ServiceJobChecklistItem>();

            if (String.IsNullOrEmpty(this.CheckListIds)) return list;

            var checklistValArr = this.CheckListIds.Split(','); //Item level split

            foreach(var checklistVal in checklistValArr)
            {
                if(!string.IsNullOrEmpty(checklistVal))
                {
                    ServiceJobChecklistItem item = new ServiceJobChecklistItem();
                    item.ServiceJobId = this.Id;

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
