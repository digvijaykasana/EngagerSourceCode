using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IService.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api
{
    public class UserApiController : Controller
    {
        IUserService _userService;

        public UserApiController(IUserService userService)
        {
            this._userService = userService;
        }

        [AllowAnonymous]
        public async Task<ActionResult> ResetFCMId(string userName = "")
        {
            try
            {
                bool result =  _userService.ResetFCMId(userName);

                return Content(result ? "success" : "fail");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}