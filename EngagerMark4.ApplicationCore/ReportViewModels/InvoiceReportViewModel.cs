using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.ReportViewModels
{
    public class InvoiceReportViewModel
    {
        public bool needsVesselNameInFrontCompanyName { get; set; }

        public int TotalNoOfDetailLines
        {
            get;
            set;
        }

        public void CalculateTotalDetailsLines()
        {
            int count = 0;

            foreach(var detail in Details)
            {
                count++;

                if(!string.IsNullOrEmpty(detail.DisplayDescription) && detail.DisplayDescription.Length > 89)
                {
                    count++;
                }

                if(!String.IsNullOrEmpty(detail.Description))
                {
                    count++;
                }
            }

            var lineBreakCount = Details.Where(x => x.IsHeader).Count();

            count = count + lineBreakCount;

            TotalNoOfDetailLines = count;
        }

        public void CalculateNoOfPage()
        {
            try
            {
                decimal temp = 0;

                if (IsTaxInvoice)
                {
                    //temp = Details.Count / MaxRecord;
                    temp = Math.Floor(Convert.ToDecimal(TotalNoOfDetailLines) / Convert.ToDecimal(MaxRecord));
                }
                else
                {
                    //temp = Details.Count / MaxRecordForNonTax;
                    temp = Math.Floor(Convert.ToDecimal(TotalNoOfDetailLines) / Convert.ToDecimal(MaxRecordForNonTax));
                }


                if (temp == 0)
                {
                    NoOfPage = 1;
                }
                else
                {
                    NoOfPage = Convert.ToInt32(temp);

                    var totalNumberOfItemsInCurrentPage = 0;
                    
                    if (IsTaxInvoice)
                    {
                        totalNumberOfItemsInCurrentPage = Convert.ToInt32(temp) * MaxRecord;
                        //divi = Math.Ceiling(Convert.ToDecimal(TotalNoOfDetailLines) / Convert.ToDecimal(MaxRecord));
                    }
                    else
                    {
                        totalNumberOfItemsInCurrentPage = Convert.ToInt32(temp) * MaxRecordForNonTax;
                        //divi = Math.Ceiling(Convert.ToDecimal(TotalNoOfDetailLines) / Convert.ToDecimal(MaxRecordForNonTax));
                    }

                    if(totalNumberOfItemsInCurrentPage < TotalNoOfDetailLines)
                        NoOfPage = Convert.ToInt32(temp) + 1;
                }
            }
            catch(Exception ex)
            {
               
            }            
        }

        public int MaxRecord
        {
            get;
            set;
        }= 45; //= 34;

        public int MaxRecordForNonTax
        {
            get;
            set;
        } = 52;

        public bool IsLastPage
        {
            get
            {
                return CurrentPage == NoOfPage;
            }
        }

        public int GetMaxRecord()
        {
            if (CurrentPage == NoOfPage)
                return IsTaxInvoice ? MaxRecord : MaxRecordForNonTax;
            else
                return MaxRecordForNonTax;
        }

        public int CurrentPage
        {
            get;
            set;
        } = 0;

        public int NoOfPage
        {
            get;
            set;
        } = 1;

        public string HeaderLogo
        {
            get;
            set;
        }
        
        public string Vessel
        {
            get;
            set;
        }

        public string Customer
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        } = TimeUtil.GetLocalTime();

        public void SetInvoiceNo(Int64 serialNo)
        {
            this.InvoiceNo = Date.Year.ToString("0000") + "/" + Date.Month.ToString("00") + "/" + serialNo.ToString("0000");
        }

        public string DateStr
        {
            get
            {
                return Util.ConvertDateToString(Date, DateConfig.CULTURE);
            }
        }

        public string InvoiceNo
        {
            get;
            set;
        }

        public bool IsTaxInvoice
        {
            get;
            set;
        }

        public string TaxDescription
        {
            get;
            set;
        }

        public Decimal TotalAmount
        {
            get;
            set;
        }

        public decimal TaxPercent
        {
            get;
            set;
        }

        public decimal TaxAmount
        {
            get;
            set;
        }

        public decimal TotalNonTaxableAmount
        {
            get;
            set;
        }

        public decimal GetGrandTotalWithoutTax()
        {
            if (IsTaxInvoice)
                return TotalAmount + TotalNonTaxableAmount;
            else
                return Details.Sum(x => x.TotalAmount);
        }

        public decimal GrandTotal
        {
            get;
            set;
        }

        public List<InvoiceDetailsReportViewModel> Details
        {
            get;
            set;
        } = new List<InvoiceDetailsReportViewModel>();

        //V1.0.2.4 - Added - Aung Ye Kaung - 20200305
        public string AttnStr
        {
            get;
            set;
        } = string.Empty;
    }
}
