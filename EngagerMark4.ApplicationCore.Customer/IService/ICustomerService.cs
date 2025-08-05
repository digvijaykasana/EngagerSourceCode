using EngagerMark4.ApplicationCore.Customer.Cris;
using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Customer.IService
{
    public interface ICustomerService : IBaseService<CustomerCri, EngagerMark4.ApplicationCore.Customer.Entities.Customer>
    {
        Task<Customer.Entities.Customer> GetHeaderOnly(Int64 id);

        List<CustomerVessel> GetCustomerVesselList();

        //Detect Duplicate Customer based on Email, Phone No, Account No
        List<Customer.Entities.Customer> GetSimliarCustomers(string email, string phoneNo, string accountNo);
    }
}
