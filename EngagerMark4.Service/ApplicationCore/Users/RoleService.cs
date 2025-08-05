using EngagerMark4.ApplicationCore.Cris.Users;
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
    public class RoleService : AbstractService<IRoleRepository, RoleCri, Role>, IRoleService
    {
        public RoleService(IRoleRepository repository) : base(repository)
        {
        }

        public IEnumerable<Role> GetByApplicationUserId(string applicationUserId)
        {
            return this.repository.GetByApplicationUserId(applicationUserId);
        }
    }
}
