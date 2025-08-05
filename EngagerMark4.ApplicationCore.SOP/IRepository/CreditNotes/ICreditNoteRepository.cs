using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.Excels;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes
{
    public interface ICreditNoteRepository : IBaseRepository<CreditNoteCri, CreditNote>
    {
        Task<CreditNote> GetByWorkOrderId(Int64 workOrderId);

        List<CreditNoteExcel> GetData(long[] creditNoteIds);

        Task<CreditNoteReportPDF> GetCreditNoteReportViewModel(CreditNoteCri aCri);
        
        long? GetCreditNoteDetailVesselId(Int64 Id);

        Task<bool> UpdateVesselIdAndVesselName(long VesselId, string VesselName, long? Id);
    }
}
