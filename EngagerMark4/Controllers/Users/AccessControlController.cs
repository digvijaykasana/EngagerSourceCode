using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IService.Application;
using EngagerMark4.ApplicationCore.IService.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using EngagerMark4.ApplicationCore.Cris.Application;
using PagedList;

namespace EngagerMark4.Controllers.Application
{
    public class AccessControlController : BaseController<RolePermissionCri, RolePermission, IRolePermissionService>
    {
        IModuleService _moduleService;
        IModulePermissionService _modulePermissionService;
        IUserRoleService _userRoleService;
        IRoleService _roleService;

        public AccessControlController(IRolePermissionService service,
            IModuleService moduleService,
            IModulePermissionService modulePermissionService,
            IUserRoleService userRoleService,
            IRoleService roleService) : base(service)
        {
            this._modulePermissionService = modulePermissionService;
            this._moduleService = moduleService;
            this._userRoleService = userRoleService;
            this._roleService = roleService;
        }

        protected override Task<IPagedList<RolePermission>> GetEntities(RolePermissionCri aCri)
        {
            return null;
        }

        protected async override Task LoadReferencesForList(RolePermissionCri aCri)
        {
            ViewBag.Modules = (await _moduleService.GetByCri(null)).OrderBy(x => x.SerialNo);
            var modulePermissionCri = new ModulePermissionCri();
            modulePermissionCri.Includes = new List<string>();
            modulePermissionCri.Includes.Add("Permission");
            ViewBag.Functions = (await _modulePermissionService.GetByCri(modulePermissionCri)).OrderBy(x => x.Permission.SerialNo);
            var roles = await _roleService.GetByCri(null);
            var userRoleId = Request["UserRoleId"];
            Int64 userRoleIdInt = 0;
            Int64.TryParse(userRoleId, out userRoleIdInt);
            RolePermissionCri cri = new RolePermissionCri();
            if(userRoleIdInt == 0)
            {
                var role = roles.OrderBy(x => x.Name).FirstOrDefault();
                if (role != null)
                    userRoleIdInt = role.Id;
            }
            ViewBag.UserRoleId = new SelectList(roles.OrderBy(x => x.Name), "Id", "Name", userRoleIdInt);
            cri.NumberCris["RoleId"] = new ApplicationCore.Cris.IntValue { ComparisonOperator = ApplicationCore.Cris.BaseCri.NumberComparisonOperator.Equal, Value = userRoleIdInt };
            ViewBag.SelectedFunctions = userRoleIdInt == 0 ? new List<RolePermission>() : await _service.GetByCri(cri);
        }

        [HttpPost]
        public async Task<ActionResult> Index(int UserRoleId,Int64[] selectedFunctions)
        {
            var rolePermissions = PrepareData(UserRoleId, selectedFunctions);

            await this._service.Save(rolePermissions);

            TempData["message"] = "Saved successfully!";
            TempData["color"] = "green";

            return RedirectToAction(nameof(Index), new { UserRoleId = UserRoleId });
        }

        private List<RolePermission> PrepareData(int UserRoleId,Int64[] selectedFunctions)
        {
            List<RolePermission> rolePermissions = new List<RolePermission>();

            if (selectedFunctions == null)
                return rolePermissions;

            foreach(var functionId in selectedFunctions)
            {
                RolePermission rolePermission = new RolePermission
                {
                    RoleId = UserRoleId,
                    PermissionId = functionId,
                };
                rolePermissions.Add(rolePermission);
            }
            return rolePermissions;
        }
    }
}