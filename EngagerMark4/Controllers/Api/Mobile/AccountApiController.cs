using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using EngagerMark4.Common.Configs;
using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.Infrasturcture.MobilePushNotifications.FCM;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.Common.Utilities;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.MvcFilters;

namespace EngagerMark4.Controllers.Api.Account
{
    public class AccountApiController : BaseApiController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        IUserRepository _userRepository;


        public AccountApiController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public AccountApiController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [MobileApi]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string userName, string password, string fcmId)
        {
            List<General> results = new List<General>();

            if (!ValidateToken())
            {
                General invalidToken = new General();
                invalidToken.Id = ApiConfig.FAIL;
                invalidToken.Value = "Invalid Token";
                results.Add(invalidToken);
                return Json(results, JsonRequestBehavior.AllowGet);
            }


            //Modified - Kaung [ 19-06-2018 ] ["For Single Device Sign On"]

            var user = _userRepository.GetByUserName(userName);

            if (!(user.FCMIdStr == String.Empty))
            {
                General signedInToken = new General();
                signedInToken.Id = ApiConfig.FAIL;
                signedInToken.Value = "Already Signed In";
                results.Add(signedInToken);
                return Json(results, JsonRequestBehavior.AllowGet);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(userName, password, true, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    if (user == null) user = new ApplicationCore.Entities.Users.User();
                    this._userRepository.UpdateFCMID(user.Id, fcmId);
                    General success = new General
                    {
                        Id = 200,
                        Value = "success",
                        CurrentUserName = user.FirstName,
                        UserId = user.Id,
                        ParentCompanyId = user.ParentCompanyId
                    };
                    results.Add(success);
                    return Json(results, JsonRequestBehavior.AllowGet);
                case SignInStatus.LockedOut:
                    General lockout = new General
                    {
                        Id = 0,
                        Value = "lockout",
                    };
                    results.Add(lockout);
                    return Json(results, JsonRequestBehavior.AllowGet);
                case SignInStatus.RequiresVerification:
                    General sendCode = new General
                    {
                        Id = 0,
                        Value = "sendCode",
                    };
                    results.Add(sendCode);
                    return Json(results, JsonRequestBehavior.AllowGet);
                case SignInStatus.Failure:
                default:
                    General value = new General
                    {
                        Id = 0,
                        Value = "Invalid Login Attempt",
                    };
                    results.Add(value);
                    return Json(results, JsonRequestBehavior.AllowGet);
            }
        }

        [MobileApi]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Logout(string userName)
        {
            string fcmId = "";

            List<General> results = new List<General>();

            if (!ValidateToken())
            {
                General invalidToken = new General();
                invalidToken.Id = ApiConfig.FAIL;
                invalidToken.Value = "Invalid Token";
                results.Add(invalidToken);
                return Json(results, JsonRequestBehavior.AllowGet);
            }

            var user = _userRepository.GetByUserName(userName);

            //this._userRepository.UpdateFCMID(user.Id, fcmId);

            General success = new General
            {
                Id = 200,
                Value = "success",
                UserId = user.Id,
                ParentCompanyId = user.ParentCompanyId
            };

            results.Add(success);

            return Json(results, JsonRequestBehavior.AllowGet);

        }
    }
}