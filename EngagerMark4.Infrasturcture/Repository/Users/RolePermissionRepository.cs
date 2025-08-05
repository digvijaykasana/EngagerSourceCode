using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngagerMark4.ApplicationCore.Entities.Application;
using System.Data.Entity;

namespace EngagerMark4.Infrasturcture.Repository.Users
{
    public class RolePermissionRepository : GenericRepository<ApplicationDbContext, RolePermissionCri, RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public IEnumerable<Function> GetByRole(Role role)
        {
            if (role == null) return new List<Function>();

            var query = from rolePermission in context.RolePermissions.AsNoTracking()
                        join function in context.Functions on rolePermission.PermissionId equals function.Id
                        where rolePermission.RoleId == role.Id
                        select function;
            return query;
        }

        public List<RolePermissionDetails> GetPermissionDetails(long roleId, long permissionId)
        {
            var query = from rolePermission in context.RolePermissions.AsNoTracking()
                        join rolePermissionDetails in context.RolePermissionDetails.AsNoTracking() on rolePermission.Id equals rolePermissionDetails.RolePermissionId
                        where rolePermission.RoleId == roleId && rolePermission.PermissionId == permissionId
                        select rolePermissionDetails;

            return query.ToList();
        }

        public RolePermission GetRolePermission(long roleId, long permissionId)
        {
            return this.context.RolePermissions.AsNoTracking().FirstOrDefault(x => x.RoleId == roleId && x.PermissionId == permissionId);
        }

        public bool HasPermission(string aControllerName,string aUserId)
        {
            var query = from rolePermission in context.RolePermissions.AsNoTracking()
                        join permission in context.Functions.AsNoTracking() on rolePermission.PermissionId equals permission.Id
                        join userRole in context.EngagerUserRole.AsNoTracking() on rolePermission.RoleId equals userRole.RoleId
                        join user in context.EngagerUsers.AsNoTracking() on userRole.UserId equals user.Id
                        where user.ApplicationUserId.Equals(aUserId) && permission.Controller.Equals(aControllerName)
                        select new
                        {
                            Id = rolePermission.Id
                        };

            var count = query.ToList().Count;

            return count > 0;
        }

        public void Save(List<RolePermission> rolePermissions)
        {
            if (rolePermissions == null)
                return;
            if (rolePermissions.Count == 0)
                return;

            var rolePermission = rolePermissions.FirstOrDefault();

            var existings = context.RolePermissions.Where(x => x.RoleId == rolePermission.RoleId);

            foreach(var exiting in existings)
            {
                this.Delete(exiting);
            }

            foreach(var toSave in rolePermissions)
            {
                Save(toSave);
            }
        }

        public void SavePermissionDetails(long roleId, long permissionId, Dictionary<RolePermissionDetails.PermissionType, bool> permissionDetails)
        {
            var rolePermission = context.RolePermissions.FirstOrDefault(x => x.RoleId == roleId && x.PermissionId == permissionId);

            if(rolePermission == null)
            {
                RolePermission toSave = new RolePermission
                {
                  RoleId = roleId,
                  PermissionId = permissionId,
                  AllAccess = false
                };
                context.RolePermissions.Add(toSave);
            }
            {
                foreach(var rolePermissionDetails in context.RolePermissionDetails.Where(x => x.RolePermissionId == rolePermission.Id ))
                {
                    context.RolePermissionDetails.Remove(rolePermissionDetails);
                }
            }
            int accessCount = 0;
            foreach(var detailKey in permissionDetails.Keys)
            {
                if(permissionDetails[detailKey] == true)
                {
                    accessCount++;
                    RolePermissionDetails create = new RolePermissionDetails
                    {
                        RolePermission = rolePermission,
                        Type = detailKey
                    };
                    context.RolePermissionDetails.Add(create);
                }
            }

            rolePermission.AllAccess = accessCount == 3;
        }
    }
}
