using EngagerMark4.ApplicationCore.Customer.Cris;
using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.Customer.IRepository;
using EngagerMark4.ApplicationCore.Customer.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.Customer.ApplicationCore
{
    public class CustomerService : AbstractService<ICustomerRepository, CustomerCri, EngagerMark4.ApplicationCore.Customer.Entities.Customer>, ICustomerService
    {
        public CustomerService(ICustomerRepository repository) : base(repository)
        { }

        public async Task<EngagerMark4.ApplicationCore.Customer.Entities.Customer> GetHeaderOnly(long id)
        {
            return await this.repository.GetByHeaderOnly(id);
        }

        public List<CustomerVessel> GetCustomerVesselList()
        {
            return this.repository.GetCustomerVesselList();
        }

        //Detect Duplicate Customer based on Email, Phone No, Account No
        public List<EngagerMark4.ApplicationCore.Customer.Entities.Customer> GetSimliarCustomers(string email, string phoneNo, string accountNo)
        {
            return this.repository.GetSimliarCustomers(email, phoneNo, accountNo);
        }
    }
}
