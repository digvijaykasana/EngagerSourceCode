using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.ViewModels
{
    public class ServiceJobWallemViewModel
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

        public string StartDateTimeStr
        {
            get;
            set;
        }

        public string EndExecutionDateTimeStr
        {
            get;
            set;
        }

        public Int64 LetterheadId
        {
            get;
            set;
        }

        public Int64? ServiceJobId
        {
            get;
            set;
        }

        public List<WorkOrderPassenger> GetPassengers(Int64? vehicleId = 0, Int64? serviceJobId = 0 )
        {
            var passengers = WorkOrder.GetPassengers();

            List<WorkOrderPassenger> wallemPassengers = new List<WorkOrderPassenger>();

            foreach (var passenger in passengers)
            {
                if(serviceJobId.HasValue && serviceJobId.Value > 0)
                {
                    if(passenger.ServiceJobId.HasValue && passenger.ServiceJobId.Value == serviceJobId.Value)
                    {
                        wallemPassengers.Add(passenger);
                    }
                }
                else
                {
                    if (passenger.VehicleId == vehicleId)
                    {
                        wallemPassengers.Add(passenger);
                    }
                }
            }

            return wallemPassengers;
        }

        //For Old APK integration
        public List<WorkOrderPassenger> GetPassengers(Int64? vehicleId = 0)
        {
            var passengers = WorkOrder.GetPassengers();

            List<WorkOrderPassenger> wallemPassengers = new List<WorkOrderPassenger>();

            foreach (var passenger in passengers)
            {
                if (vehicleId.HasValue && vehicleId.Value > 0)
                {
                    if (passenger.VehicleId == vehicleId)
                    {
                        wallemPassengers.Add(passenger);
                    }
                }
            }

            return wallemPassengers;
        }
    }
}
