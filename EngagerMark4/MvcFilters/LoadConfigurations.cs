using EngagerMark4.ApplicationCore.Caches;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using EngagerMark4.ApplicationCore.Entities.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using EngagerMark4.Common.Utilities;

namespace EngagerMark4.MvcFilters
{
    public class LoadConfigurationsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //using (ApplicationDbContext context = new ApplicationDbContext())
            //{
            //    var userName = HttpContext.Current.User.Identity.GetUserName();

            //    if(!string.IsNullOrEmpty(userName))
            //    {
            //        if(!Cache.CommonConfigurations.ContainsKey(Util.GetDomain(userName)))
            //        {
            //            Cache.CommonConfigurations[Util.GetDomain(userName)] = context.CommonConfigurations.AsNoTracking().Include(x => x.ConfigurationGroup).ToList();
            //        }
            //    }
            //}
        }
    }
}