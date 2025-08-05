using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IService.Users
{
    public interface IUserService : IBaseService<UserCri, User>
    {
        Task<IEnumerable<User>> GetByCustomerId(Int64 customerId);

        User GetByApplicatioNId(string applicationUserId);

        User GetByUserId(Int64 userId);

        IEnumerable<User> GetDrivers();

        IEnumerable<User> GetAgent(Int64 customerId=0);

        IEnumerable<User> GetAgentMgr(Int64 customerId = 0);

        User GetByUserName(string aUserName);

        bool ResetFCMId(string userName);

        Task<IEnumerable<User>> GetByRole(UserCri aCri);
    }
}
