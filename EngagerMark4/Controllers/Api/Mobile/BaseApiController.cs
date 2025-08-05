using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.Common;
using EngagerMark4.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api.Account
{
    public class BaseApiController : Controller
    {
        public int _successId = 200;

        protected bool ValidateToken()
        {
            var token = Request[ApiConfig.TOKEN_NAME];

            if (string.IsNullOrEmpty(token))
                return false;

            if (!token.Equals(ApiConfig.KEY))
                return false;

            return true;
        }

        protected void SetParentCompanyId()
        {
            var parentCompanyIdStr = Request["ParentCompanyId"];

            Int64 parentCompanyId = 0;

            Int64.TryParse(parentCompanyIdStr, out parentCompanyId);

            GlobalVariable.COMPANY_ID = parentCompanyId;
        }

        protected async Task SetCurrentUserId(long userId, IUserService userService)
        {
            var user = await userService.GetById(userId);
            if (user != null)
            {
                GlobalVariable.mobile_userId = user.ApplicationUserId;
                GlobalVariable.mobile_userName = user.LastName + " " + user.FirstName;
            }
        }

    }
}