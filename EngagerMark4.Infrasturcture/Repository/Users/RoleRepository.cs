using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EngagerMark4.Infrasturcture.Repository.Users
{
    public class RoleRepository : GenericRepository<ApplicationDbContext, RoleCri, Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public IEnumerable<Role> GetByApplicationUserId(string applicationUserId)
        {
            try
            {
                var user = context.EngagerUsers.AsNoTracking().SingleOrDefault(x => x.ApplicationUserId == applicationUserId);
                if (user == null)
                    return new List<Role>();

                return context.EngagerUserRole.AsNoTracking().Where(x => x.UserId == user.Id).Select(x => x.Role);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
