using EngagerMark4.ApplicationCore.Account.Entities;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices
{
    /// <summary>
    /// ShortText1 = Summary Invoice No.
    /// ShortText2 = Summary Invoice Id
    /// LongText1 = Original Reference No.
    /// </summary>
    [Serializable]
    [Table("Tb_SalesInvoice", Schema = "SOP")]
    public class SalesInvoice : BaseEntity
    {
        [NotMapped]
        public Int64 SummaryInvoiceId
        {
            get
            {
                return this.SalesInvoiceSummaryId;
            }
            set
            {
                this.SalesInvoiceSummaryId = value;
            }
        }


        public long SalesInvoiceSummaryId
        {
            get;
            set;
        }


        [Required]
        [Index(IsUnique = false)]
        [StringLength(50)]
        public string ReferenceNo
        {
            get;
            set;
        } = "TBD";

        [Index(IsUnique = false)]
        public Int64 ReferenceNoNumber
        {
            get;
            set;
        }

        public Int64? VersionNumber
        {
            get;
            set;
        }

        [NotMapped]
        public String OriginalRefStr
        {
            get
            {
                return LongText1 == null ? String.Empty : LongText1;
            }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    LongText1 = value;
                }
            }
        }

        [NotMapped]
        public bool requiresVersioning
        {
            get;
            set;
        } = false;

        public DateTime? InvoiceDate
        {
            get;
            set;
        }

        [NotMapped]
        public string InvoiceDateStr
        {
            get
            {
                return InvoiceDate == null ? "" : Util.ConvertDateToString(InvoiceDate.Value, DateConfig.CULTURE);
            }
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    InvoiceDate = Util.ConvertStringToDateTime(value, DateConfig.CULTURE);
                }
            }
        }

        //public Int64? DiscountTypeId
        //{
        //    get;
        //    set;
        //}

        //[StringLength(256)]
        //public string DiscountType
        //{
        //    get;
        //    set;
        //}

        public Int64? MeetingServiceId
        {
            get;
            set;
        }

        [StringLength(256)]
        public string MeetingService
        {
            get;
            set;
        }

        public Int64? WorkOrderId
        {
            get;
            set;
        }

        [StringLength(256)]
        public string WorkOrderNo
        {
            get;
            set;
        }

        public Int64? VesselId
        {
            get;
            set;
        }

        [StringLength(256)]
        public string VesselName
        {
            get;
            set;
        }

        [StringLength(256)]
        public string CompanyAddress
        {
            get;
            set;
        }

        public string Remark
        {
            get;
            set;
        }

        public Int64 CustomerId
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

        [NotMapped]
        public List<SalesInvoiceDetails> Details
        {
            get;
            set;
        } = new List<SalesInvoiceDetails>();

        public List<SalesInvoiceDetails> GetDetails()
        {
            foreach(var detail in Details)
            {
                detail.SalesInvoice = this;
                detail.PriceObj = null;
                detail.SalesInvoiceId = Id;
            }
            return Details;
        }

        public string GetReferenceNo(Int64 serialNo)
        {
            var temp = Created.Date.ToString("yy") + Created.Month.ToString("00") + serialNo.ToString();
            this.ReferenceNoNumber = Convert.ToInt64(temp);
            return $"IND-{Created.Date.ToString("yy")}-{Created.Month.ToString("00")}-{serialNo.ToString()}";

        }

        public long? GetVersionNo(long? versionNumber)
        {
            var versionNo = versionNumber == null ? 1 : this.VersionNumber + 1;

            return versionNo;
        }

        public string AddVersionNo(string referenceNo, long? versionNo)
        {
            return referenceNo + "-V" + versionNo.Value.ToString();

            //if(versionNo == 1)
            //{
            //    return referenceNo + "-V" + versionNo.Value.ToString();
            //}
            //else
            //{
            //    var temp = latestStr.Substring(0, latestStr.LastIndexOf("-V"));

            //    return temp + (versionNo).ToString();
            //}
        }

        public Int64? GSTId
        {
            get;
            set;
        }

        [ForeignKey("GSTId")]
        public GST GST
        {
            get;
            set;
        }

        public decimal TotalAmt
        {
            get;
            set;
        }

        public decimal TotalNonTaxable
        {
            get;
            set;
        }

        public decimal GSTPercent
        {
            get;
            set;
        }

        public decimal GSTAmount
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

        public decimal TotalNetAmount
        {
            get;
            set;
        }

        [NotMapped]
        public string InvoiceNo
        {
            get
            {
                return this.ShortText1;
            }
            set
            {
                this.ShortText1 = value;
            }
        }

        public SalesInvoiceStatus Status
        {
            get;
            set;
        }

        [NotMapped]
        public string StatusColor
        {
            get
            {
                switch (Status)
                {
                    case SalesInvoiceStatus.Draft:
                        return "#5D4037";
                    case SalesInvoiceStatus.Billed:
                        return "#000000";
                    case SalesInvoiceStatus.Revised:
                        return "#4A148C";
                    default:
                        return "#FAFAFA";
                }
            }
        }

        public DiscountType DisType
        {
            get;
            set;
        }


        public enum SalesInvoiceStatus
        {
            All,
            Draft = 10,
            Billed = 20,
            Revised = 30
        }

        public enum DiscountType
        {
            [Display(Name ="By Percent")]
            ByPercent,
            [Display(Name = "By Amount")]
            ByAmount
        }


        public void GenerateInvoiceDetail(WorkOrder workOrder,List<Price> priceList, GST gst)
        {
            this.GSTId = gst.Id;
            this.CustomerId = workOrder.CustomerId == null ? 0 : workOrder.CustomerId.Value;
            this.CompanyAddress = workOrder.Customer == null ? string.Empty : workOrder.Customer.Address;
            this.VesselId = workOrder.VesselId;
            this.VesselName = workOrder.VesselName;
            switch (workOrder.Customer.DiscountTy)
            {
                case ApplicationCore.Customer.Entities.Customer.DiscountTypes.ByPercent:
                    this.DisType = DiscountType.ByPercent;
                    this.DiscountPercent = workOrder.Customer.DiscountPercent;
                    break;
                case ApplicationCore.Customer.Entities.Customer.DiscountTypes.ByAmount:
                    this.DisType = DiscountType.ByAmount;
                    this.DiscountAmount = workOrder.Customer.DiscountAmt;
                    break;
                default:
                    this.DisType = DiscountType.ByPercent;
                    break;
            }

            if (priceList == null) priceList = new List<Price>();

            int currentIndex = 0;

            foreach(var price in priceList.OrderBy(x => x.OrderBy))
            {
                SalesInvoiceDetails detail = Details.FirstOrDefault(x => x.PriceId == price.Id);

                if(detail!=null)
                {
                    if(detail.Qty == 0 && !price.IncludeCustomerDiscountAmount) detail.Qty++;
                    detail.CalculateTotal();
                    continue;
                }

                detail = new SalesInvoiceDetails
                {
                    PriceId = price.Id,
                    Type = SalesInvoiceDetails.DetailsType.NonInventory,
                    Code = price.Display,
                    Qty = 1,
                    Price = price.AssignedPrice,
                    DiscountPercent = price.DiscountPercent,
                    DiscountAmount = price.DiscountAmt,
                    Id1 = price.Id1 //Order By
                };


                //If price is 'Trip Charges' or 'Waiting Time', if the picked up pax exceeds the max pax of Price, the quantity will be calculated and a description 
                if (price.IncludeCustomerDiscountAmount)
                {
                    var passengerInCharge = workOrder.WorkOrderPassengerList.FirstOrDefault(x => x.InCharge == true);

                    if (passengerInCharge != null)
                    {
                        if (passengerInCharge.NoOfPax > price.MaxPax && price.MaxPax > 0)
                        {
                            detail.Qty = Math.Ceiling(Convert.ToDecimal(passengerInCharge.NoOfPax) / Convert.ToDecimal(price.MaxPax));
                            detail.Description = "Used " + detail.Qty + " Combi & Luggages";                                
                        }
                    }
                }


                if (currentIndex == 0 && !String.IsNullOrEmpty(workOrder.PickUpTimeBinding))
                {
                    detail.Code += " @ " + (workOrder.PickUpTimeBinding.Replace(":", "")) + " Hr";
                }

                //If price is 'Trip Charges' or 'Waiting Time', if discount percent or discount amount is zero, the price will take the discount percent and discount amount of the customer.
                if(price.IncludeCustomerDiscountAmount)
                {
                    if (detail.DiscountPercent != this.DiscountPercent) this.DiscountPercent = detail.DiscountPercent;

                    if (detail.DiscountPercent <= 0) detail.DiscountPercent = workOrder.Customer.DiscountPercent;

                    if (detail.DiscountAmount <= 0) detail.DiscountAmount = workOrder.Customer.DiscountAmt;
                }                

                detail.SalesInvoice = this;
                //details.Taxable = price.GeneralLedger == null ? true : price.GeneralLedger.Taxable;
                detail.Taxable = price.IsTaxable;
                detail.CalculateTotal();
                this.Details.Add(detail);

                currentIndex++;
            }
            CalculateTotal(gst);
        }

        public void CalculateTotal(GST gst)
        {
            if (Details == null) Details = new List<SalesInvoiceDetails>();

            TotalAmt = 0;
            TotalNonTaxable = 0;
            DiscountAmount = 0;
            TotalNetAmount = 0;
            GSTAmount = 0;

            foreach(var detail in Details)
            {
                if (detail.Taxable)
                    TotalAmt += detail.TotalAmt;
                if (detail.Taxable == false)
                    TotalNonTaxable += detail.TotalAmt;
                DiscountAmount += detail.DiscountAmount;
            }

            TotalNetAmount = TotalAmt;

            //Getting Discount Percent by dividing the total discount amount of sales invoice details by total amount
            //if (TotalAmt != 0)
                //DiscountPercent = (DiscountAmount / TotalAmt) * 100;

            if(gst!=null)
            {
                GSTAmount = TotalNetAmount * (gst.GSTPercent / 100);
                TotalNetAmount += GSTAmount;
            }

            TotalNetAmount += TotalNonTaxable;
        }

        [NotMapped]
        public WorkOrder WorkOrder
        {
            get;
            set;
        }

        public override string ToString()
        {
            return nameof(SalesInvoice) + " : " + ReferenceNo;
        }
    }
}
