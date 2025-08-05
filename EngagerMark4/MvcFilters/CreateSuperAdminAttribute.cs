using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.Common.Configs;
using System.Threading.Tasks;

namespace EngagerMark4.Filters
{
    public class CreateSuperAdminAttribute : ActionFilterAttribute
    {
        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {


                base.OnActionExecuting(filterContext);
                var superAdmin = new ApplicationUser { UserName = SecurityConfig.SUPER_ADMIN, Email = SecurityConfig.SUPER_ADMIN };
                var result = UserManager.CreateAsync(superAdmin, SecurityConfig.DEFAULT_PASSWORD).Result;
                if (result.Succeeded)
                {

                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
            }
        }
    }
}