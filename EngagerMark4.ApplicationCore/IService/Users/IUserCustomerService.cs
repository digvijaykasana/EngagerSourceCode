using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.Dummy.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IService.Users
{
    public interface IUserCustomerService : IBaseService<UserCustomerCri, UserCustomer>
    {
        List<UserCustomer> GetByUserId(long UserId);
    }
}
