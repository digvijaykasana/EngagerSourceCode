using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.Dummy.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.ApplicationCore.Users
{
    public class UserCustomerService : AbstractService<IUserCustomerRepository, UserCustomerCri, UserCustomer>, IUserCustomerService
    {
        IUserCustomerRepository _repository;

        public UserCustomerService(IUserCustomerRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public List<UserCustomer> GetByUserId(long UserId = 0)
        {
            return this._repository.GetByUserId(UserId);
        }
    }
}
