using EngagerMark4.ApplicationCore.Cris.Application;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.ApplicationCore.IService.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.ApplicationCore.Application
{
    public class SMTPService : AbstractService<ISMTPRepository, SMTPCri, SMTP>, ISMTPService
    {
        public SMTPService(ISMTPRepository repository) : base(repository)
        {
        }
    }
}
