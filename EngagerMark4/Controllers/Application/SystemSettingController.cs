using EngagerMark4.ApplicationCore.Cris.Application;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.IService.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using EngagerMark4.Common.Configs;

namespace EngagerMark4.Controllers.Application
{
    public class SystemSettingController : BaseController<SystemSettingCri, SystemSetting, ISystemSettingService>
    {
        public SystemSettingController(ISystemSettingService service) : base(service)
        {
        }
    }
}