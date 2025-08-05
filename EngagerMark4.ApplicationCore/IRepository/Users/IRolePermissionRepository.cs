using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IRepository.Users
{
    public interface IRolePermissionRepository : IBaseRepository<RolePermissionCri, RolePermission>
    {
        void Save(List<RolePermission> rolePermissions);

        IEnumerable<Function> GetByRole(Role role);

        bool HasPermission(string aControllerName,string aUserId);

        List<RolePermissionDetails> GetPermissionDetails(Int64 roleId, Int64 permissionId);

        void SavePermissionDetails(Int64 roleId, Int64 permissionId, Dictionary<RolePermissionDetails.PermissionType, bool> permissionDetails);

        RolePermission GetRolePermission(Int64 roleId, Int64 permissionId);
    }
}
