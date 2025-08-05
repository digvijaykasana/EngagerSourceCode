using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.ReportViewModels
{
    public class CreditNoteDetailsReportViewModel
    {
        public DateTime CreditNoteDate
        {
            get;
            set;
        }

        public string CreditNoteStr
        {
            get
            {
                return Util.ConvertDateToString(CreditNoteDate, DateConfig.CULTURE);
            }
        }

        public string Description
        {
            get;
            set;
        }

        public decimal TotalAmount
        {
            get;
            set;
        }

        public bool IsHeader
        {
            get;
            set;
        }
    }
}
