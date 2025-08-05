using EngagerMark4.ApplicationCore.Account.Cris;
using EngagerMark4.ApplicationCore.Account.Entities;
using EngagerMark4.ApplicationCore.Account.IRepository;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.Account.Repository
{
    public class GeneralLedgerRepository : GenericRepository<ApplicationDbContext, GeneralLedgerCri, GeneralLedger>, IGeneralLedgerRepository
    {
        public GeneralLedgerRepository(ApplicationDbContext aContext) : base(aContext)
        {

        }

        protected override void ApplyCri(GeneralLedgerCri cri)
        {
            base.ApplyCri(cri);

            if (cri != null && !string.IsNullOrEmpty(cri.SearchValue))
                base.queryableData = queryableData.Where(x => x.Name.ToLower().Trim().Contains(cri.SearchValue.ToLower().Trim()));
        }
    }
}
