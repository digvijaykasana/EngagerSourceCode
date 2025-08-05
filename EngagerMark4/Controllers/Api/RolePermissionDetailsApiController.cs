using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IService.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api
{
    public class RolePermissionDetailsApiController : Controller
    {
        IRolePermissionService _service;

        public RolePermissionDetailsApiController(IRolePermissionService service)
        {
            this._service = service;
        }
        
        [AllowAnonymous]
        public ActionResult Index(Int64 roleId,Int64 permissionId)
        {
            List<SystemSetting> permissionDetails = new List<SystemSetting>();
            SystemSetting create = new SystemSetting
            {
                Code = RolePermissionDetails.PermissionType.Create.ToString(),
                Value = "false"
            };
            permissionDetails.Add(create);
            SystemSetting update = new SystemSetting
            {
                Code = RolePermissionDetails.PermissionType.Edit.ToString(),
                Value = "false"
            };
            permissionDetails.Add(update);
            SystemSetting delete = new SystemSetting
            {
                Code = RolePermissionDetails.PermissionType.Delete.ToString(),
                Value = "false"
            };
            permissionDetails.Add(delete);

            var rolePermissionDetails = _service.GetPermissionDetails(roleId, permissionId);

            if (rolePermissionDetails == null) rolePermissionDetails = new List<RolePermissionDetails>();

            var createPermission = rolePermissionDetails.FirstOrDefault(x => x.Type == RolePermissionDetails.PermissionType.Create);

            if (createPermission != null) create.Value = "true";

            var updatePermission = rolePermissionDetails.FirstOrDefault(x => x.Type == RolePermissionDetails.PermissionType.Edit);
            if (updatePermission != null) update.Value = "true";

            var deletePermission = rolePermissionDetails.FirstOrDefault(x => x.Type == RolePermissionDetails.PermissionType.Delete);
            if (deletePermission != null) delete.Value = "true";

            return Json(permissionDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Save(Int64 roleId, Int64 permissionId,bool create,bool edit,bool delete)
        {
            Dictionary<RolePermissionDetails.PermissionType, bool> values = new Dictionary<RolePermissionDetails.PermissionType, bool>();
            values[RolePermissionDetails.PermissionType.Create] = create;
            values[RolePermissionDetails.PermissionType.Edit] = edit;
            values[RolePermissionDetails.PermissionType.Delete] = delete;
            this._service.SavePermissionDetails(roleId, permissionId, values);
            return Content("success");
        }
    }
}