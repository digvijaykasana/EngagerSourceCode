using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.SOP.TempQueryModels
{
    public class WorkOrderTempQuery
    {
        public WorkOrder WorkOrder
        {
            get; set;
        }

        public DateTime? InvoiceDate
        {
            get;
            set;
        }

        public int YearMonthNo
        {
            get;
            set;
        }

        public int SerialNo
        {
            get;
            set;
        }
    }
}
