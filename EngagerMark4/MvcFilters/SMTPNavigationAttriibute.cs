using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace EngagerMark4.Filters
{
    public class SMTPNavigationAttriibute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = filterContext.ActionDescriptor.ActionName;

            if(controllerName.ToLower().Equals("smtp") && actionName.ToLower().Equals("index"))
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                        {"controller", "SMTP"},
                        {"action","Details"},
                        }
                        );
            }
        }

    }
}