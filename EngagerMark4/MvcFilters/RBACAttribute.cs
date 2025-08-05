using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.Common.Configs;
using EngagerMark4.MvcFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EngagerMark4.Filters
{
    public class RBACAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(MobileApiAttribute), false).Any())
            {
                return;
            }
            try
            {
                if(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Account"))
                {
                    return;
                }

                var requestUrl = filterContext.HttpContext.Request.Url.PathAndQuery;
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    if (!ContainAllowAnonymousAttribute(filterContext))
                    {
                        filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                        {"controller", "Account"},
                        {"action","Login"},
                        {"returnUrl",requestUrl }
                        }
                        );
                        return;
                    }
                }

                if ((!filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Company")) && filterContext.HttpContext.User.Identity.Name.Equals(SecurityConfig.SUPER_ADMIN))
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                        {"controller", "Company"},
                        {"action","Index"},
                        }
                        );
                    return;
                }

                if (filterContext.HttpContext.User.Identity.IsAuthenticated && !filterContext.HttpContext.User.Identity.Name.Equals(SecurityConfig.SUPER_ADMIN))
                {
                    string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "Controller";

                    List<Role> roles = filterContext.RequestContext.HttpContext.Items[AppKeyConfig.ROLE] as List<Role>;
                    List<Function> permissionList = filterContext.RequestContext.HttpContext.Items[AppKeyConfig.FUNCTION] as List<Function>;

                    if (permissionList == null || roles == null)
                    {
                        if (!ContainAllowAnonymousAttribute(filterContext))
                        {
                            filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary
                                {
                                    {"controller", "Account"},
                                    {"action","Login"},
                                    { "returnUrl", requestUrl}
                                }
                                );
                            return;
                        }
                    }

                    var permission = permissionList.FirstOrDefault(x => x.Controller.Equals(controllerName));

                    if (permission == null)
                    {
                        if (!ContainAllowAnonymousAttribute(filterContext))
                        {
                            filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary
                                {
                                    {"controller", "Account"},
                                    {"action","Login"},
                                    { "returnUrl",requestUrl }
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private bool ContainAllowAnonymousAttribute(AuthorizationContext filterContext)
        {
            object[] objects = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);
            if (objects == null || objects.Length == 0)
                return false;
            else
                return true;
        }
    }
}