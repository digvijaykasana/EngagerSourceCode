using EngagerMark4.ApplicationCore.Caches;
using EngagerMark4.ApplicationCore.Common;
using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api
{
    public class WaitingTimeApiController : Controller
    {
        ICommonConfigurationService _commonConfigurationService;
        
        public WaitingTimeApiController(ICommonConfigurationService commonConfigurationService)
        {
            this._commonConfigurationService = commonConfigurationService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return null;
            var waitingTime = Cache.CommonConfigurations[Util.GetDomain(User.Identity.Name)].Where(x => x.ConfigurationGroup.Code.Equals(GeneralConfig.ConfigurationGrpCodes.WaitingTime.ToString())).OrderBy(x => x.Name);
            return Json(waitingTime.Select(x => x.Name), JsonRequestBehavior.AllowGet);
        }
    }
}