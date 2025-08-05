using EngagerMark4.ApplicationCore.Customer.Cris;
using EngagerMark4.ApplicationCore.Customer.DataImportViewModel;
using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Customer.IRepository
{
    public interface ICustomerRepository : IBaseRepository<CustomerCri, EngagerMark4.ApplicationCore.Customer.Entities.Customer>
    {
        Task<Customer.Entities.Customer> GetByHeaderOnly(Int64 id);

        Task Saves(List<Customer.Entities.Customer> customers);

        List<CustomerVessel> GetCustomerVesselList();

        //Detect Duplicate Customer based on Email, Phone No, Account No
        List<Customer.Entities.Customer> GetSimliarCustomers(string email, string phoneNo, string accountNo);
    }
}
