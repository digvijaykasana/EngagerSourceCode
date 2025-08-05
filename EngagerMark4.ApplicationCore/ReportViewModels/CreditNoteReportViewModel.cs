using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.ReportViewModels
{
    public class CreditNoteReportViewModel
    {
        public bool needsVesselNameInFrontCompanyName { get; set; }

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

        public string CNNo
        {
            get;
            set;
        }

        public void SetCNNo(Int64 serialNo)
        {
            this.CNNo = Date.Year + "/" + Date.Month + "/" + serialNo;
        }

        public string DateStr
        {
            get
            {
                return Util.ConvertDateToString(Date, DateConfig.CULTURE);
            }
        }

        public bool IsTaxCN
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

        public decimal GrandTotal
        {
            get;
            set;
        }

        public decimal InvoiceTotal
        {
            get;
            set;
        }

        public List<CreditNoteDetailsReportViewModel> Details
        {
            get;
            set;
        } = new List<CreditNoteDetailsReportViewModel>();
    }
}
