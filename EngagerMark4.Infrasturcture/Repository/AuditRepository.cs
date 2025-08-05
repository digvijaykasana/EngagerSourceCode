using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.Repository
{
    public class AuditRepository : GenericRepository<ApplicationDbContext, AuditCri, Audit>, IAuditRepository
    {
        public AuditRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public void Saves(List<Audit> audits)
        {
            if (audits != null)
            {
                context.Audits.AddRange(audits);
            }

            context.SaveChanges();
        }
    }
}
