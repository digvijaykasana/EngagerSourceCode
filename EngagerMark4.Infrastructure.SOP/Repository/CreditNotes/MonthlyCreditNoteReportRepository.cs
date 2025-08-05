using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes;
using EngagerMark4.Common;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.SOP.Repository.CreditNotes
{
    public class MonthlyCreditNoteReportRepository : GenericRepository<ApplicationDbContext, CreditNoteCri, CreditNote>, IMonthlyCreditNoteReportRepository
    {
        public MonthlyCreditNoteReportRepository(ApplicationDbContext aContext) : base(aContext)
        {

        }

        public List<CreditNote> getCreditNotes(MonthlyCreditNoteCri aCri)
        {
            var dateTimeVal = aCri.GetDateTime();

            Int64 customerId = aCri.CustomerId;

            DateTime fromDate = dateTimeVal;
            TimeSpan ts = new TimeSpan(0, 0, 0);
            fromDate = fromDate.Date + ts;

            DateTime toDate = dateTimeVal.AddMonths(1).AddDays(-1);
            TimeSpan newts = new TimeSpan(23, 59, 59);
            toDate = toDate.Date + newts;

            var creditNotes = context.CreditNotes.Where(x => x.ParentCompanyId == GlobalVariable.COMPANY_ID);

            if(customerId != 0)
            {
                creditNotes = creditNotes.Where(x => x.CustomerId == aCri.CustomerId);
            }

            if(fromDate != null  && toDate != null)
            {
                creditNotes = creditNotes.Where(x => x.CreditNoteDate >= fromDate && x.CreditNoteDate <= toDate);
            }

            return creditNotes.ToList();
        }
    }
}
