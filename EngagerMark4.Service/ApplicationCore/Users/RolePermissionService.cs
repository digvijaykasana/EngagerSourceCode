using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.ApplicationCore.IService.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.ApplicationCore.Users
{
    public class RolePermissionService : AbstractService<IRolePermissionRepository, RolePermissionCri, RolePermission>, IRolePermissionService
    {
        public RolePermissionService(IRolePermissionRepository repository) : base(repository)
        {
        }

        public async Task Save(List<RolePermission> rolePermissions)
        {
            this.repository.Save(rolePermissions);
            await this.repository.SaveChangesAsync();
        }

        public IEnumerable<Function> GetByRole(Role role)
        {
            return this.repository.GetByRole(role);
        }

        public bool HasPermission(string aControllerName,string aUserId)
        {
            return this.repository.HasPermission(aControllerName, aUserId);
        }

        public List<RolePermissionDetails> GetPermissionDetails(long roleId, long permissionId)
        {
            return this.repository.GetPermissionDetails(roleId, permissionId);
        }

        public void SavePermissionDetails(long roleId, long permissionId, Dictionary<RolePermissionDetails.PermissionType, bool> permissionDetails)
        {
            this.repository.SavePermissionDetails(roleId, permissionId, permissionDetails);
            this.repository.SaveChanges();
        }

        public RolePermission GetRolePermission(long roleId, long permissionId)
        {
            return this.repository.GetRolePermission(roleId, permissionId);
        }
    }
}
