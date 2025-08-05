using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes
{
    [Serializable]
    [Table("Tb_CreditNoteSummaryDetails", Schema = "SOP")]
    public class CreditNoteSummaryDetails : BaseEntity
    {
        public Int64 CreditNoteSummaryId
        {
            get;
            set;
        }

        [ForeignKey("CreditNoteSummaryId")]
        public CreditNoteSummary CreditNoteSummary
        {
            get;
            set;
        }

        public Int64 SalesInvoiceSummaryId
        {
            get;
            set;
        }
    }
}
