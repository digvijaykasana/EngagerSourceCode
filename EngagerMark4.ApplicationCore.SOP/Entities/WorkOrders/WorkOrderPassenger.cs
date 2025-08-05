using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Configurations;
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
    [Table("Tb_WorkOrderPassenger", Schema = "SOP")]
    public class WorkOrderPassenger : BasicEntity
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

        public bool InCharge
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Name
        {
            get;
            set;
        }

        public Int64? RankId
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Rank
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

        public int NoOfPax
        {
            get;
            set;
        }

        //PCR2021
        public SignStatus IsSigned
        {
            get;
            set;
        } = SignStatus.NotSigned;

        public enum SignStatus
        {
            NotSigned,
            Signed
        }

        //PCR2021 - For Multiple Signature required Transfer Voucher generation
        public Int64? ServiceJobId
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
            return nameof(WorkOrderPassenger) + " : " + Id;
        }
    }
}
