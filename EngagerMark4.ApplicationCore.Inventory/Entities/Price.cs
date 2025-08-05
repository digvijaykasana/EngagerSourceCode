using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.Account.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngagerMark4.Common.Utilities;

namespace EngagerMark4.ApplicationCore.Inventory.Entities
{
    [Serializable]
    [Table("Tb_Price", Schema = "Inventory")]
    public class Price : BaseEntity
    {
        [NotMapped]
        public int OrderBy
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

        [Required]
        [StringLength(50)]
        public string Code
        {
            get;
            set;
        } = "TBD";

        [StringLength(256)]
        public string Name
        {
            get;
            set;
        }

        [NotMapped]
        public string DisplayForList
        {
            get
            {
                return this.Code + " - " + this.Name;
            }
        }

        [NotMapped]
        public string Display
        {
            get
            {
                return this.Name;
            }
        }

        public string DisplayForSalesInvoiceEdit
        {
            get
            {
                if (!String.IsNullOrEmpty(this.Name))
                {
                    var isTripCharges = this.Name.ToLower().Trim().Contains("trip charges");

                    if (!isTripCharges) return this.Name + " " + this.Code;

                    if (this.PickUpPointId != 0 && this.DropPointId != 0)
                    {
                        return this.PickUpPoint + " to " + this.DropPoint;
                    }
                    else
                    {
                        return this.Name + " " + this.Code;
                    }
                }
                else
                {
                    return this.Name + " " + this.Code;
                }
            }
        }

        public Int64? CustomerId
        {
            get;
            set;
        }

        [ForeignKey("CustomerId")]
        public EngagerMark4.ApplicationCore.Customer.Entities.Customer Customer
        {
            get;
            set;
        }

        public Int64? GLCodeId
        {
            get;
            set;
        }

        [ForeignKey("GLCodeId")]
        public GeneralLedger GeneralLedger
        {
            get;
            set;
        }

        public bool IsTaxable
        {
            get;
            set;
        }

        [Required]
        [Range(0, 999999999999999999.9999999999)]
        public decimal AssignedPrice
        {
            get;
            set;
        }

        [StringLength(256)]
        public string PickUpPoint
        {
            get;
            set;
        }

        public Int64 PickUpPointId
        {
            get;
            set;
        }

        [StringLength(256)]
        public string DropPoint
        {
            get;
            set;
        }

        public Int64 DropPointId
        {
            get;
            set;
        }

        public decimal DiscountPercent
        {
            get;
            set;
        }

        public decimal DiscountAmt
        {
            get;
            set;
        }

        public Int64 MaxPax
        {
            get;
            set;
        }

        public decimal ExceedAmt
        {
            get;
            set;
        }

        #region Start Time
        public DateTime? StartTime
        {
            get;
            set;
        }

        [StringLength(50)]
        public string StartTimeBinding
        {
            get;
            set;
        }

        #endregion

        #region End Time

        public DateTime? EndTime
        {
            get;
            set;
        }

        [StringLength(50)]
        public string EndTimeBinding
        {
            get;
            set;
        }
        #endregion

        public bool ViceVersa
        {
            get;
            set;
        }

        [NotMapped]
        public List<PriceLocation> PriceLocationList
        {
            get;
            set;
        } = new List<PriceLocation>();

        public List<PriceLocation> GetLocations()
        {
            foreach (var detail in PriceLocationList)
            {
                detail.Price = this;
                if (detail.Type == PriceLocation.PriceLocationType.PickUp)
                    this.PickUpPoint = detail.Location.Name;
                if (detail.Type == PriceLocation.PriceLocationType.DropOff)
                    this.DropPoint = detail.Location.Name;
                detail.Location = null;
            }

            return PriceLocationList.Where(x => x.Delete == false).ToList();
        }

        public string GetPriceCode(Int64 serialNo, Int64 valCount)
        {
            string strFormat = "";

            for (int i = 0; i < valCount; i++)
            {
                strFormat = strFormat + "0";
            }
            return $"{serialNo.ToString(strFormat)}";
        }

        public override string ToString()
        {
            return nameof(Price) + " : " + Code;
        }

        //Aung Ye Kaung - 20191108
        //For items except 'Trip Charges' and 'Waiting Time/Disposal', discount amount and discount percent will be kept at the Price Item level and will not call the Customer level amounts.
        [NotMapped]
        public bool IncludeCustomerDiscountAmount
        {
            get;
            set;
        } = false;
       
    }
}
