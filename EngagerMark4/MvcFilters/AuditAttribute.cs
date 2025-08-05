using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.MvcFilters
{
    public class AuditAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string modelInfo = filterContext.Controller.ViewData.Model == null ? string.Empty : filterContext.Controller.ViewData.Model.ToString();
        }
    }
}