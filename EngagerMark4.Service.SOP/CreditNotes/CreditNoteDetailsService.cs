using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IService.CreditNotes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.SOP.CreditNotes
{
    public class CreditNoteDetailsService : AbstractService<ICreditNoteDetailsRepository, CreditNoteDetailsCri, CreditNoteDetails>,ICreditNoteDetailsService
    {
        public CreditNoteDetailsService(ICreditNoteDetailsRepository repository) : base(repository)
        { }
    }
}
