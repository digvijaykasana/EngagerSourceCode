using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EngagerMark4.Common;
using EngagerMark4.Infrasturcture.EFContext;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace EngagerMark4.MvcFilters
{
    public class LoadUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //base.OnActionExecuting(filterContext);
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(MobileApiAttribute), false).Any())
            {
                return;
            }

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                string applicationUserId = filterContext.HttpContext.User.Identity.GetUserId();

                if (!GlobalVariable.USER_NAMES.ContainsKey(applicationUserId))
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        var user = db.EngagerUsers.AsNoTracking().FirstOrDefault(x => x.ApplicationUserId.Equals(applicationUserId));
                        if(user!=null)
                        {
                            GlobalVariable.USER_NAMES[applicationUserId] = user.Name;
                        }
                    }
                }
            }
        }
    }
}