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
    [Table("Tb_SalesInvoiceReportSerialNo", Schema = "SOP")]
    public class SalesInvoiceReportSerialNo : SerialNo
    {
        public override string ToString()
        {
            return nameof(SalesInvoiceReportSerialNo) + " : " + RunningNo;
        }
    }
}
