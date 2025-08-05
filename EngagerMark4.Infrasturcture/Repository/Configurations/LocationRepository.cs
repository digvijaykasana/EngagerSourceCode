using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.Repository.Configurations
{
    public class LocationRepository : GenericRepository<ApplicationDbContext, LocationCri, Location>, ILocationRepository
    {
        public LocationRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        protected override void ApplyCri(LocationCri cri)
        {
            if (cri != null && !string.IsNullOrEmpty(cri.SearchValue))
                base.queryableData = queryableData.Where(x => x.Code.Contains(cri.SearchValue) || x.Name.Contains(cri.SearchValue) || x.PostalCode.Contains(cri.SearchValue) || x.Address.Contains(cri.SearchValue));
        }

        public async Task Saves(List<Location> locations)
        {
            foreach(var location in locations.Distinct())
            {
                var dbLocation = context.Locations.FirstOrDefault(x => x.Code.Equals(location.Code));

                if (dbLocation == null)
                    context.Locations.Add(location);
            }

            await this.context.SaveChangesAsync();
        }
    }
}
