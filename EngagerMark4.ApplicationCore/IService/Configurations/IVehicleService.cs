using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Dummy.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IService.Configurations
{
    public interface IVehicleService : IBaseService<VehicleCri, Vehicle>
    {
        IEnumerable<UserVehicle> GetWithDrivers();
    }
}
