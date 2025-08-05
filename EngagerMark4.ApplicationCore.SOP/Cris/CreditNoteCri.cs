using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Cris
{
    public class CreditNoteCri : BaseCri
    {
        public string ReferenceNo
        {
            get; set;
        }


        public bool ValidateDateRange()
        {
            if (string.IsNullOrEmpty(FromDate))
                return true;
            if (string.IsNullOrEmpty(ToDate))
                return true;

            DateTime fromDateTime = Util.ConvertStringToDateTime(FromDate, DateConfig.CULTURE);
            DateTime toDateTime = Util.ConvertStringToDateTime(ToDate, DateConfig.CULTURE);
            if (toDateTime < fromDateTime)
                return false;
            else
                return true;
        }
        public Int64 CustomerId
        {
            get;
            set;
        }

        // Added - Aung Ye Kaung - 2019/04/24
        public Int64 VesselId
        {
            get;
            set;
        }

        public string FromDate
        {
            get;
            set;
        }

        public string ToDate
        {
            get;
            set;
        }

        public DateTime FromDateTime
        {
            get;
            set;
        }

        public DateTime ToDateTime
        {
            get;set;
        }

        public CreditNoteSummary.CreditNoteStatus? Status
        {
            get;
            set;
        } = CreditNoteSummary.CreditNoteStatus.All;

        public Action ActionThing
        {
            get;
            set;
        }

        public enum Action
        {
            [Display(Name = "Download Taxable Credit Note")]
            TaxableCN,
            [Display(Name = "Download Nontaxable Credit Note")]
            NonTaxableCN
        }

        public List<Int64> SalesInvoiceSummaryIds
        {
            get; set;
        } = new List<long>();

        public string SalesInvoiceSummaryIdsStr
        {
            get
            {
                if (SalesInvoiceSummaryIds.Count == 0) return null;

                string query = "(";

                foreach(var id in SalesInvoiceSummaryIds)
                {
                    query += id + ",";
                }

                query = query.Substring(0, query.Length - 1);

                query = query + ")";

                return query;
            }
        }
    }
}
