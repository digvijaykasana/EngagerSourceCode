using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.IService.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngagerMark4.ApplicationCore.Dummy.Entities.Users;

namespace EngagerMark4.Service.ApplicationCore.Configurations
{
    public class VehicleService : AbstractService<IVehicleRepository, VehicleCri, Vehicle>, IVehicleService
    {
        public VehicleService(IVehicleRepository repository) : base(repository)
        {
        }

        public IEnumerable<UserVehicle> GetWithDrivers()
        {
            return this.repository.GetWithDrivers();
        }
    }
}
