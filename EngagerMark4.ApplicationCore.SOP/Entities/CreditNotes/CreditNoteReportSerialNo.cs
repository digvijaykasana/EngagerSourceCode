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
    [Table("Tb_CreditNoteReportSerialNo", Schema ="SOP")]
    public class CreditNoteReportSerialNo : SerialNo
    {
        public override string ToString()
        {
            return nameof(CreditNoteReportSerialNo) + " : " + RunningNo;
        }
    }
}
