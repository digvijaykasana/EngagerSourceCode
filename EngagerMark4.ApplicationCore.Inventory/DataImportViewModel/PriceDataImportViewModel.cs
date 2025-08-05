using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Inventory.DataImportViewModel
{
    public class PriceDataImportViewModel
    {
        public string Customer
        {
            get;
            set;
        }

        public string GLCode
        {
            get;
            set;
        }

        public string ProductName
        {
            get;
            set;
        }

        public decimal AssignedPrice
        {
            get;
            set;
        }

        public decimal MaxPax
        {
            get;
            set;
        }

        public decimal ExceededPrice
        {
            get;
            set;
        }

        public decimal DiscountPercent
        {
            get;
            set;
        }

        public decimal DiscountAmount
        {
            get;
            set;
        }

        public string PickupPoint
        {
            get;
            set;
        }

        public string DropOffPoint
        {
            get;
            set;
        }

        public int Order
        {
            get;
            set;
        }

        public string Gst
        {
            get;
            set;
        }

        //Added - Aung Ye Kaung - 20190423
        public string ViceVersa
        {
            get;
            set;
        }
    }
}
