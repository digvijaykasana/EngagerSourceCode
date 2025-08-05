using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.ApplicationCore.Dummy.Entities.Users;
using EngagerMark4.Common;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.Repository.Users
{
    public class UserVehicleRepository : GenericRepository<ApplicationDbContext, UserVehicleCri, UserVehicle>, IUserVehicleRepository
    {
        public UserVehicleRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public List<UserVehicle> GetByUserId(long aUserId)
        {
            var userVehicles = context.UserVehicles.AsNoTracking().Where(x => x.UserId == aUserId && x.ParentCompanyId == GlobalVariable.COMPANY_ID);

            List<UserVehicle> vehicleList = new List<UserVehicle>();
            foreach(var userVehicle in userVehicles)
            {
                var vehicle = context.Vehicles.AsNoTracking().FirstOrDefault(x => x.Id == userVehicle.VehicleId);
                if (vehicle != null)
                    userVehicle.VehicleNo = vehicle.VehicleNo;
                vehicleList.Add(userVehicle);
            }
            return vehicleList;
        }

        public Int64 GetUserIdByVehicleId(Int64? vehicleId)
        {
            var entities = from userVehicles in this.context.UserVehicles.AsNoTracking()
                         join users in this.context.EngagerUsers.AsNoTracking() on userVehicles.UserId equals users.Id
                         join commmonConfigurations in this.context.CommonConfigurations.AsEnumerable() on users.StatusId equals commmonConfigurations.Id
                         where userVehicles.VehicleId == vehicleId && commmonConfigurations.Code == "Active" && userVehicles.ParentCompanyId == GlobalVariable.COMPANY_ID
                         select userVehicles;

            if(entities != null && entities.Count() > 0)
            {
                return entities.FirstOrDefault().UserId;
            }
            else
            {
                return 0;
            }
        }

        public List<Int64> GetUserIdListByVehicleId(Int64? vehicleId)
        {
            var userVehicleList = context.UserVehicles.Where(x => x.VehicleId == vehicleId && x.ParentCompanyId == GlobalVariable.COMPANY_ID);

            if (userVehicleList == null || userVehicleList.Count() == 0) return null;

            List<Int64> userIdList = new List<long>();

            foreach(var userVehicle in userVehicleList)
            {
                userIdList.Add(userVehicle.UserId);
            }

            return userIdList;
        }
    }
}
