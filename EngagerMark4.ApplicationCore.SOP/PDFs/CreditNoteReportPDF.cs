using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.ViewModels
{
    public class CreditNoteReportPDF
    {
        public string HeaderLogo
        {
            get;
            set;
        }

        public void CalculateNoOfPage()
        {
            var temp = Details.Count / MaxRecord;

            if (temp == 0)
            {
                NoOfPage = 1;
            }
            else
            {
                var divi = Details.Count % MaxRecord;
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

        public int GetMaxRecord()
        {
            if (CurrentPage == NoOfPage)
                return 34;
            else
                return 34;
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
        }

        public string DateStr
        {
            get
            {
                return Util.ConvertDateToString(Date, DateConfig.CULTURE);
            }
        }

        public decimal SubTotal
        {
            get
            {
                if (Details == null) return 0;

                return Details.Sum(x => x.TotalAmount);
            }
        }

        public decimal GSTPercent
        {
            get
            {
                var details = this.Details.FirstOrDefault();
                if (details == null) details = new CreditNoteDetailsReportViewModel();

                return details.GSTPercent;
            }
        }

        public decimal GSTAmount
        {
            get
            {
                return SubTotal * (GSTPercent / 100);
            }
        }

        public decimal GrandTotal
        {
            get
            {
                return SubTotal + GSTAmount;
            }
        }

        public List<CreditNoteDetailsReportViewModel> Details
        {
            get;
            set;
        } = new List<CreditNoteDetailsReportViewModel>();
    }
}
