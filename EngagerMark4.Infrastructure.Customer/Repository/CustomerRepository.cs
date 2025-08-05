using EngagerMark4.ApplicationCore.Customer.Cris;
using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.Customer.IRepository;
using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using EngagerMark4.ApplicationCore.Customer.DataImportViewModel;

namespace EngagerMark4.Infrastructure.Customer.Repository
{
    public class CustomerRepository : GenericRepository<ApplicationDbContext, CustomerCri, EngagerMark4.ApplicationCore.Customer.Entities.Customer>, ICustomerRepository
    {
        IRolePermissionRepository _rolePermissionRepository;

        public CustomerRepository (ApplicationDbContext aContext,
            IRolePermissionRepository rolePermissionRepository) : base(aContext)
        {
            this._rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<ApplicationCore.Customer.Entities.Customer> GetByHeaderOnly(long id)
        {
            var customer = await base.GetById(id);

            return customer;
        }

        public async override Task<ApplicationCore.Customer.Entities.Customer> GetById(object id)
        {
            var customer = await base.GetById(id);

            customer.Locations = context.CustomerLocations.Include(c => c.Location).AsNoTracking().Where(x => x.CustomerId == customer.Id).ToList();

            customer.VesselList = context.CustomerVessels.AsNoTracking().Where(x => x.CustomerId == customer.Id).ToList();

            return customer;
        }

        public override void Save(ApplicationCore.Customer.Entities.Customer model)
        {
            base.Save(model);

            var hasPermissionForLocation = _rolePermissionRepository.HasPermission("CustomerLocationController", HttpContext.Current.User.Identity.GetUserId());

            if (hasPermissionForLocation)
            {
                if(model.Id!=0)
                {
                    foreach(var detail in context.CustomerLocations.Where(x => x.CustomerId == model.Id))
                    {
                        var location = context.Locations.FirstOrDefault(x => x.Id == detail.LocationId);
                        if (location != null)
                            context.Locations.Remove(location);
                        context.CustomerLocations.Remove(detail);
                    }
                }

                foreach (var detail in model.GetLocations())
                {
                    context.CustomerLocations.Add(detail);
                }
            }

            var hasPermissionForVessel = _rolePermissionRepository.HasPermission("CustomerVesselController", HttpContext.Current.User.Identity.GetUserId());

            if(hasPermissionForVessel)
            {
                if(model.Id != 0)
                {
                    foreach(var detail in context.CustomerVessels.Where(x => x.CustomerId == model.Id))
                    {
                        context.CustomerVessels.Remove(detail);
                    }
                }

                foreach(var detail in model.GetVessels())
                {
                    context.CustomerVessels.Add(detail);
                }
            }
        }

        public async Task Saves(List<ApplicationCore.Customer.Entities.Customer> customers)
        {
            foreach(var customer in customers)
            {
                var dbCustomer = context.Customers.FirstOrDefault(x => x.Name.Equals(customer.Name));
                if(dbCustomer == null)
                {
                    context.Customers.Add(customer);

                    foreach(var vessel in customer.GetVessels())
                    {
                        context.CustomerVessels.Add(vessel);
                    }
                }
                else
                {
                    foreach(var vessle in customer.GetVessels())
                    {
                        var inVessle = context.CustomerVessels.AsNoTracking().FirstOrDefault(x => x.CustomerId == customer.Id && x.VesselId == vessle.VesselId);

                        if (inVessle == null)
                            context.CustomerVessels.Add(vessle);
                    }
                }
            }
            await this.SaveChangesAsync();
        }

        public List<CustomerVessel> GetCustomerVesselList()
        {
            var customerVesselList = context.CustomerVessels.ToList();

            customerVesselList = customerVesselList == null ? new List<CustomerVessel>() : customerVesselList;

            return customerVesselList;
        }

        protected override void ApplyCri(CustomerCri cri)
        {
            if (cri != null && !string.IsNullOrEmpty(cri.SearchValue))
                base.queryableData = queryableData.Where(x => x.Name.ToLower().Contains(cri.SearchValue.ToLower()) || x.Email.ToLower().Contains(cri.SearchValue.ToLower()) || x.Address.ToLower().Contains(cri.SearchValue.ToLower()));
        }

        //Detect Duplicate Customer based on Email, Phone No, Account No
        public List<EngagerMark4.ApplicationCore.Customer.Entities.Customer> GetSimliarCustomers(string email, string phoneNo, string accountNo)
        {
            try
            {
                var resultLst = context.Customers.AsNoTracking().AsQueryable(); 
                    
                resultLst = resultLst.Where(x => ((x.Email ?? string.Empty) != string.Empty && x.Email.Trim().ToLower().Contains(email.Trim().ToLower())) 
                || ((x.OfficeNo ?? string.Empty) != string.Empty && x.OfficeNo.Trim().ToLower().Contains(phoneNo.Trim().ToLower()))
                || ((x.AccNo ?? string.Empty) != string.Empty && x.AccNo.Trim().ToLower().Contains(accountNo.Trim().ToLower())));

                return resultLst.ToList();
            }
            catch(Exception ex)
            {
                return new List<ApplicationCore.Customer.Entities.Customer>();
            }
        }
    }
}
