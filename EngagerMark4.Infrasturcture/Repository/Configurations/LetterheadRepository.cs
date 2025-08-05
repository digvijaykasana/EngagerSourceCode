using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.Repository.Configurations
{
    public class LetterheadRepository : GenericRepository<ApplicationDbContext, LetterheadCri, Letterhead>, ILetterheadRepository
    {
        public LetterheadRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public Letterhead GetDefaultLetterhead()
        {
            return dbSet.AsNoTracking().Where(x => x.IsDefault).FirstOrDefault();
        }

        protected override void ApplyCri(LetterheadCri cri)
        {
            if (cri != null && !string.IsNullOrEmpty(cri.SearchValue))
                base.queryableData = queryableData.Where(x => x.Name.Contains(cri.SearchValue));


            if(cri.Type != -1)
            {
                base.queryableData = queryableData.Where(x => x.Type == (Letterhead.LetterheadType)cri.Type);
            }
        }
    }
}
