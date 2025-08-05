using EngagerMark4.ApplicationCore.IService.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api
{
    public class AgentApiController : Controller
    {
        IUserService _userService;

        public AgentApiController(IUserService userService)
        {
            this._userService = userService;
        }

        [AllowAnonymous]
        public async Task<ActionResult> GetByCustomerId(Int64 customerId)
        {
            var agents = await _userService.GetByCustomerId(customerId);

            return Json(agents, JsonRequestBehavior.AllowGet);
        }
    }
}