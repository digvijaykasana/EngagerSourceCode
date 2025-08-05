using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.Dummy.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IRepository.Users
{
    public interface IUserVehicleRepository : IBaseRepository<UserVehicleCri, UserVehicle>
    {
        List<UserVehicle> GetByUserId(Int64 aUserId);

        Int64 GetUserIdByVehicleId(Int64? vehicleId);

        List<Int64> GetUserIdListByVehicleId(Int64? vehicleId);
    }
}
