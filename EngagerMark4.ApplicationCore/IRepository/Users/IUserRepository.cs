using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IRepository.Users
{
    public interface IUserRepository : IBaseRepository<UserCri, User>
    {
        Task<IEnumerable<User>> GetByCustomerId(Int64 customerId);

        IEnumerable<User> GetDrivers();

        IEnumerable<User> GetAgent(Int64 customerId);

        IEnumerable<User> GetAgentMgr(Int64 customerId);

        User GetByUserName(string aUserName);

        User GetByApplicatioNId(string applicationUserId);

        User GetByUserId(Int64 userId);

        void UpdateFCMID(Int64 id, string fcmId);

        bool ResetFCMId(string userName);

        Task<IEnumerable<User>> GetByRole(UserCri aCri);
    }
}
