using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.Repository.Users
{
    public class UserRoleRepository : GenericRepository<ApplicationDbContext, UserRoleCri, UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }
    }
}
