using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.Common;
using EngagerMark4.Common.Configs;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using EngagerMark4.Infrasturcture.Repository.Application;
using EngagerMark4.Infrasturcture.Repository.Users;
using EngagerMark4.MvcFilters;
using EngagerMark4.Service.ApplicationCore.Application;
using EngagerMark4.Service.ApplicationCore.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EngagerMark4.Filters
{
    public class LoadRolePermissionAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(MobileApiAttribute), false).Any())
            {
                return;
            }

            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var domain = GlobalVariable.GetDomain(filterContext.HttpContext.User.Identity.Name);
                    var company = new CompanyRepository(db).GetCompanyByDomain(domain);
                    if (company != null)
                    {
                        GlobalVariable.AddCompanyId(company.Id, filterContext.HttpContext.User.Identity.Name);
                    }

                    IRoleService roleService = new RoleService(new RoleRepository(db));
                    RolePermissionService permissionService = new RolePermissionService(new RolePermissionRepository(db));
                    string userId = filterContext.HttpContext.User.Identity.GetUserId();
                    IEnumerable<Role> roles = roleService.GetByApplicationUserId(userId);
                    List<Function> functionList = new List<Function>();
                    Int64 roleId = 0;
                    string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "Controller";
                    foreach (var role in roles)
                    {
                        roleId = role.Id;
                        foreach (var permission in permissionService.GetByRole(role))
                        {
                            var inPermission = functionList.FirstOrDefault(x => x.Id == permission.Id);
                            if (inPermission == null)
                                functionList.Add(permission);
                        }
                    }

                    ModulePermissionService modulePermissionService = new ModulePermissionService(new ModulePermissionRepository(db));
                    List<Module> modules = modulePermissionService.GetModules();

                    var function = functionList.FirstOrDefault(x => x.Controller.Equals(controllerName));
                    if (function == null) function = new Function();

                    var rolePermission = permissionService.GetRolePermission(roleId, function.Id);

                    if (rolePermission == null) rolePermission = new RolePermission();

                    filterContext.RequestContext.HttpContext.Items[AppKeyConfig.ROLE] = roles.ToList();
                    filterContext.RequestContext.HttpContext.Items[AppKeyConfig.FUNCTION] = functionList;
                    filterContext.RequestContext.HttpContext.Items[AppKeyConfig.MODULE] = modules;
                    filterContext.RequestContext.HttpContext.Items[AppKeyConfig.DETAILS_PERMISSION] = permissionService.GetPermissionDetails(roleId, function.Id);
                    filterContext.RequestContext.HttpContext.Items[AppKeyConfig.ALL_ACCESS] = rolePermission.AllAccess;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}