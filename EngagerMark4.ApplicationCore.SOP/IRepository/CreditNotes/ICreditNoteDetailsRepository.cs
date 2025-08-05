using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes
{
    public interface ICreditNoteDetailsRepository : IBaseRepository<CreditNoteDetailsCri, CreditNoteDetails>
    {
    }
}
