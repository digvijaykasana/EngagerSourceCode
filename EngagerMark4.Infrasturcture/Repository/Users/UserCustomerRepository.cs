using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.ApplicationCore.Dummy.Entities.Users;
using EngagerMark4.Common;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.Repository.Users
{
    public class UserCustomerRepository : GenericRepository<ApplicationDbContext, UserCustomerCri, UserCustomer>, IUserCustomerRepository
    {
        public UserCustomerRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public List<UserCustomer> GetByUserId(long aUserId)
        {
            var userCustomers = context.UserCustomers.AsNoTracking().Where(x => x.UserId == aUserId && x.ParentCompanyId == GlobalVariable.COMPANY_ID).ToList();

            List<UserCustomer> customerList = new List<UserCustomer>();
            foreach (var userCustomer in userCustomers)
            {
                var customer = context.Customers.AsNoTracking().FirstOrDefault(x => x.Id == userCustomer.CustomerId);
                if (customer != null)
                    userCustomer.CustomerName = customer.Name;
                customerList.Add(userCustomer);
            }
            return customerList;
        }
    }
}
