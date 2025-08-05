using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices
{
    [Serializable]
    [Table("Tb_SalesInvoiceSummaryDetails", Schema = "SOP")]
    public class SalesInvoiceSummaryDetails : BaseEntity
    {
        public Int64 SalesInvoiceSummaryId
        {
            get;
            set;
        }

        [ForeignKey("SalesInvoiceSummaryId")]
        public SalesInvoiceSummary SalesInvoiceSummary
        {
            get;
            set;
        }

        public Int64 WorkOrderId
        {
            get;
            set;
        }

        public Int64 SalesInvoiceId
        {
            get;
            set;
        }

        public Int64 CreditNoteId
        {
            get;
            set;
        }

        public override string ToString()
        {
            return nameof(SalesInvoiceSummaryDetails) + " : " + Id;
        }
    }
}
