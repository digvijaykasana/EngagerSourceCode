using EngagerMark4.ApplicationCore.Cris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Inventory.Cris
{
    public class PriceCri : BaseCri
    {
        public Int64 CustomerId
        {
            get;
            set;
        }

        public Int64 GLCodeId
        {
            get;
            set;
        }

        public string ItemCode
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
    }
}
