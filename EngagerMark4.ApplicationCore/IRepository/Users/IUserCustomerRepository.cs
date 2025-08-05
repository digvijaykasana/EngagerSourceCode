using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Dummy.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IRepository.Users
{
    public interface IUserCustomerRepository : IBaseRepository<UserCustomerCri, UserCustomer>
    {
        List<UserCustomer> GetByUserId(long aUserId);
    }
}
