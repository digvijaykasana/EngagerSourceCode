using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders.WorkOrderLocation;

namespace EngagerMark4.Models
{
    public class WorkOrdersViewModel
    {
    }

    public class SimliarOrderLocationViewModel
    {
        public Int64 LocationId { get; set; }
        public Int64 HotelId { get; set; }
        public LocationType LocationType { get; set; }
    }
}