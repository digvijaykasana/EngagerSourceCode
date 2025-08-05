using EngagerMark4.ApplicationCore.Cris.Application;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.Repository.Application
{
    public class SMTPRepository : GenericRepository<ApplicationDbContext, SMTPCri, SMTP>, ISMTPRepository
    {
        public SMTPRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }
    }
}
