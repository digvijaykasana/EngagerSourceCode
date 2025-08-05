using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities.Configurations
{
    public class CustomerVesselViewModel
    {
        public Int64 CustomerId
        {
            get;
            set;
        }

        public string Customer
        {
            get;
            set;
        }

        public  Int64 VesselId
        {
            get;
            set;
        }

        public string Vessel
        {
            get;
            set;
        }

        public bool Delete
        {
            get;
            set;
        }
    }
}
