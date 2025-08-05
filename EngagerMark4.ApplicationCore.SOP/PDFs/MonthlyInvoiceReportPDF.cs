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
    public class MonthlyInvoiceReportPDF
    {
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

        public DateTime LastDate
        {
            get;
            set;
        }

        public string DateStr
        {
            get
            {
                return Util.ConvertDateToString(LastDate, DateConfig.CULTURE).Replace('/', '-');
            }
        }

        public List<MonthlyInvoiceReportByCompanyViewModel> Invoices
        {
            get;
            set;
        }

        public string HeaderLogo
        {
            get;
            set;
        }

        public void CalculateNoOfPage()
        {
            var temp = Invoices.Count / MaxRecord;

            if (temp == 0)
            {
                NoOfPage = 1;
            }
            else
            {
                var divi = Invoices.Count % MaxRecord;
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
    }
}
