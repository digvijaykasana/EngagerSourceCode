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
    public class MobileUserRepository : GenericRepository<ApplicationDbContext, MobileUserCri, MobileUser>, IMobileUserRepository
    {
        public MobileUserRepository(ApplicationDbContext aContext) : base(aContext)
        { }
    }
}
