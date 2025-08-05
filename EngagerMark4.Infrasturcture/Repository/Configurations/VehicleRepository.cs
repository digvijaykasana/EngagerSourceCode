using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EngagerMark4.ApplicationCore.Dummy.Entities.Users;

namespace EngagerMark4.Infrasturcture.Repository.Configurations
{
    public class VehicleRepository : GenericRepository<ApplicationDbContext, VehicleCri, Vehicle>, IVehicleRepository
    {
        public VehicleRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        protected override void ApplyCri(VehicleCri cri)
        {
            if (cri != null && !string.IsNullOrEmpty(cri.SearchValue))
                base.queryableData = queryableData.Where(x => x.VehicleNo.Contains(cri.SearchValue) || x.VehicleModel.Contains(cri.SearchValue));
        }

        public List<UserVehicle> GetWithDrivers()
        {
            return context.UserVehicles.Include(x => x.User).ToList();
        }
    }
}
