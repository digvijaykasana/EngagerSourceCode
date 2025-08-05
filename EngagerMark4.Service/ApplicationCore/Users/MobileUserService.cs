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
    public class MobileUserService : AbstractService<IMobileUserRepository,MobileUserCri,MobileUser>, IMobileUserService
    {
        public MobileUserService(IMobileUserRepository repository) : base(repository)
        { }
    }
}
