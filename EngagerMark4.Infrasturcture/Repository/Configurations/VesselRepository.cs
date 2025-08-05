using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.Common.Exceptions;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.Repository.Configurations
{
    public class VesselRepository : GenericRepository<ApplicationDbContext, CommonConfigurationCri, CommonConfiguration>, IVesselRepository
    {
        public VesselRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public async override Task<CommonConfiguration> GetById(object id)
        {
            var vessel = await base.GetById(id);

            vessel.CustomerList = (from c in context.CustomerVessels.AsNoTracking().Where(x => x.VesselId == (Int64)id)
                                  select new CustomerVesselViewModel
                                  {
                                      CustomerId = c.CustomerId,
                                      Customer = c.Customer.Name,
                                      VesselId = c.VesselId,
                                      Vessel = c.Vessel
                                  }).ToList();

            return vessel;
        }

        public override void Save(CommonConfiguration model)
        {
            base.Save(model);

            if(model.Id != 0)
            {
                foreach (var detail in context.CustomerVessels.Where(x => x.VesselId == model.Id))
                {
                    context.CustomerVessels.Remove(detail);
                }
            }

            this.SaveChanges();

            foreach(var detail in model.GetCustomers())
            {
                CustomerVessel customer = new CustomerVessel
                {
                    CustomerId = detail.CustomerId,
                    VesselId = detail.VesselId,
                    Vessel = detail.Vessel,
                };
                context.CustomerVessels.Add(customer);
            }
        }

        protected override void ApplyCri(CommonConfigurationCri cri)
        {
            base.ApplyCri(cri);

            if (cri!= null && !(string.IsNullOrEmpty(cri.SearchValue)))
                base.queryableData = queryableData.Where(x => x.Code.Contains(cri.SearchValue) || x.Name.Contains(cri.SearchValue));
        }
    }
}
