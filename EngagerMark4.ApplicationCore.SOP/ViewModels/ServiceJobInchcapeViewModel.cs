using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.ViewModels
{
    public class ServiceJobInchcapeViewModel
    {
        public string ReferenceNo
        {
            get;
            set;
        }

        public Int64 SignatureId
        {
            get;
            set;
        }

        public Int64 ReferenceNoNumber
        {
            get;
            set;
        }

        public Int64? WorkOrderId
        {
            get;
            set;
        }

        public Int64 LetterheadId
        {
            get;
            set;
        }

        public WorkOrder WorkOrder
        {
            get;
            set;
        }


        public Int64? VehicleId
        {
            get;
            set;
        }

        public Vehicle Vehicle
        {
            get;
            set;
        }

        public string StartTimeStr
        {
            get;
            set;
        }

        public string EndExecutionTimeStr
        {
            get;
            set;
        }

        public List<WorkOrderPassenger> GetPassengers(Int64? vehicleId = 0 )
        {
            var passengers = WorkOrder.GetPassengers();

            List<WorkOrderPassenger> inchcapePassengers = new List<WorkOrderPassenger>();

            foreach (var passenger in passengers)
            {
                if(passenger.VehicleId == vehicleId)
                {
                    inchcapePassengers.Add(passenger);
                }
            }

            return inchcapePassengers;
        }
    }
}
