using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.Dummy.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.ApplicationCore.Users
{
    public class UserVehicleService : AbstractService<IUserVehicleRepository, UserVehicleCri, UserVehicle>, IUserVehicleService
    {
        public UserVehicleService(IUserVehicleRepository repository) : base(repository)
        {
        }

        public Int64 GetUserIdByVehicleId(Int64? vehicleId)
        {
            Int64 userId = repository.GetUserIdByVehicleId(vehicleId);

            return userId;
        }
    }
}
