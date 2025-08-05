using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IService.CreditNotes
{
    public interface ICreditNoteService : IBaseService<CreditNoteCri, CreditNote>
    {
        Task<CreditNoteReportPDF> GetDetailsReport(CreditNoteCri aCri);
    }
}
