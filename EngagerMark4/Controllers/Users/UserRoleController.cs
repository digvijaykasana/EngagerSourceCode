using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.IService.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Users
{
    public class UserRoleController : BaseController<UserRoleCri, UserRole, IUserRoleService>
    {
        public UserRoleController(IUserRoleService service) : base(service)
        {
        }
    }
}