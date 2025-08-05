using EngagerMark4.ApplicationCore.Entities;
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
    /// LongText1 = CheckList Values
    /// LongText2 = CheckList Values String
    /// LongText3 = Trip Fees Values
    /// LongText4 = Trip Fees Values String
    /// LongText5 = MS Fees Values
    /// LongText6 = MS Fees Values String
    /// </summary>
    [Serializable]
    [Table("Tb_WorkOrderHistory", Schema = "SOP")]
    public class WorkOrderHistory : BaseEntity
    {
        public Int64 WorkOrderId
        {
            get;
            set;
        }

        [StringLength(50)]
        public string WorkOrderNo
        {
            get;
            set;
        }

        public WorkOrder.OrderStatus WorkOrderStatus
        {
            get;
            set;
        }

        public string CurrentStateDescription
        {
            get;
            set;
        }

        public string ChangeDescription
        {
            get;
            set;
        }

        public string Vessel
        {
            get;
            set;
        }

        public string PickupPoint
        {
            get;
            set;
        }

        public string DropPoint
        {
            get;
            set;
        }

        public DateTime? PickupDate
        {
            get;
            set;
        }

        public DateTime? StandByDate
        {
            get;
            set;
        }

        public string ServiceJobOverallStatus
        {
            get;
            set;
        }
        
        [NotMapped]
        public string ChecklistValues
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

        [NotMapped]
        public string ChecklistValueStr
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
        public string TripFeesVal
        {
            get { return this.LongText3; }
            set { this.LongText3 = value; }
        }

        [NotMapped]
        public string TripFeesValStr
        {
            get { return this.LongText4; }
            set { this.LongText4 = value; }
        }

        [NotMapped]
        public string MSFeesVal
        {
            get { return this.LongText5; }
            set { this.LongText5 = value; }
        }

        [NotMapped]
        public string MSFeesValStr
        {
            get { return this.LongText6; }
            set { this.LongText6 = value; }
        }
    }
}
