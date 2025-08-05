using EngagerMark4.ApplicationCore.Cris.Application;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.IService.Application;
using EngagerMark4.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace EngagerMark4.Controllers.Application
{
    [SMTPNavigationAttriibute]
    public class SMTPController : BaseController<SMTPCri,SMTP, ISMTPService>
    {
        public SMTPController(ISMTPService service) : base(service)
        {
            _defaultColumn = "SMTPServer";
        }

        public async override Task<ActionResult> Details(long id = 0)
        {
            var smtp = (await _service.GetByCri(null)).FirstOrDefault();
            if (smtp == null) smtp = new SMTP();

            return View(smtp);
        }
    }
}