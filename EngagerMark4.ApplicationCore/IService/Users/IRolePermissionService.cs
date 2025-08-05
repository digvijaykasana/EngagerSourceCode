using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IService.Users
{
    public interface IRolePermissionService : IBaseService<RolePermissionCri, RolePermission>
    {
        Task Save(List<RolePermission> rolePermissions);

        bool HasPermission(string aControllerName,string aUserId);

        List<RolePermissionDetails> GetPermissionDetails(Int64 roleId, Int64 permissionId);

        void SavePermissionDetails(Int64 roleId, Int64 permissionId, Dictionary<RolePermissionDetails.PermissionType, bool> permissionDetails);

        RolePermission GetRolePermission(Int64 roleId, Int64 permissionId);
    }
}
