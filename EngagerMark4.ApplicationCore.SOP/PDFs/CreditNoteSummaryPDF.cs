using EngagerMark4.ApplicationCore.SOP.ViewModels;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.PDFs
{
    public class CreditNoteSummaryPDF
    {
        public void CalculateNoOfPage()
        {
            var temp = CreditNotes.Count / MaxRecord;

            if (temp == 0)
            {
                NoOfPage = 1;
            }
            else
            {
                var divi = CreditNotes.Count % MaxRecord;
                if (divi > 0)
                    NoOfPage = temp + 1;
            }
        }

        public int MaxRecord
        {
            get;
            set;
        } = 34;

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
                return 34;
            else
                return 34;
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

        public string HeaderLogo { get; set; }

        public string Customer { get
            {
                var details = this.CreditNotes.FirstOrDefault();
                if (details == null) return string.Empty;
                return details.Customer;
            }
        }

        public string Address { get
            {
                var details = this.CreditNotes.FirstOrDefault();
                if (details == null) return string.Empty;
                return details.Address;
            }
        }

        public string CreditNoteNo { get; set; }

        public DateTime Date { get; set; } = TimeUtil.GetLocalTime();

        public string DateStr
        {
            get
            {
                return Date.ToString("dd-MM-yyyy");
            }
        }

        public List<CreditNoteSummaryViewModel> CreditNotes { get; set; }

        public decimal SubTotal
        {
            get
            {
                return CreditNotes.Where(x => x.SubTotal != null).Sum(x => x.SubTotal).Value;
            }
        }

        public decimal GrandTotal
        {
            get
            {
                //return CreditNotes.Where(x => x.TotalAmt != null).Sum(x => x.TotalAmt).Value;
                return SubTotal + GST;
            }
        }

        public decimal GST
        {
            get
            {
                return Math.Round(SubTotal * (GSTPercent/100),2,MidpointRounding.AwayFromZero);
            }
        }

        public decimal GSTPercent
        {
            get;
            set;
        }
    }
}
