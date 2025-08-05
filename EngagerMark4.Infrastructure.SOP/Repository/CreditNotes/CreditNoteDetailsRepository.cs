using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.SOP.Repository.CreditNotes
{
    public class CreditNoteDetailsRepository : GenericRepository<ApplicationDbContext, CreditNoteDetailsCri, CreditNoteDetails>, ICreditNoteDetailsRepository
    {
        public CreditNoteDetailsRepository(ApplicationDbContext aContext) : base(aContext)
        { }
    }
}
