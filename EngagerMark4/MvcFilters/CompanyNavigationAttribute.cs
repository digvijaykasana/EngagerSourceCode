using EngagerMark4.Common;
using EngagerMark4.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EngagerMark4.Filters
{
    public class CompanyNavigationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = filterContext.ActionDescriptor.ActionName;

            if(controllerName.ToLower().Equals("company") && actionName.ToLower().Equals("index") && !filterContext.RequestContext.HttpContext.User.Identity.Name.Equals(SecurityConfig.SUPER_ADMIN))
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                        {"controller", "Company"},
                        {"action","Details"},
                            {"Id",GlobalVariable.COMPANY_ID }
                        }
                        );
            }
        }
    }
}