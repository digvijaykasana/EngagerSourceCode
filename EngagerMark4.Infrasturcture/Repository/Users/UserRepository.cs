using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EngagerMark4.Common;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;

namespace EngagerMark4.Infrasturcture.Repository.Users
{
    public class UserRepository : GenericRepository<ApplicationDbContext, UserCri, User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        protected override void ApplyCri(UserCri cri)
        {
            if (!string.IsNullOrEmpty(cri.SearchValue))
                base.queryableData = queryableData.Where(x => x.LastName.Contains(cri.SearchValue) || x.FirstName.Contains(cri.SearchValue) || x.UserName.Contains(cri.SearchValue) || x.Email.Contains(cri.SearchValue));
        }

        public override void Save(User model)
        {
            base.Save(model);
            context.Entry(model).Property(x => x.FCMId).IsModified = false;
        }

        public IEnumerable<User> GetAgent(long customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAgentMgr(long customerId)
        {
            throw new NotImplementedException();
        }

        public User GetByApplicatioNId(string applicationUserId)
        {
            return context.EngagerUsers.AsNoTracking().FirstOrDefault(x => x.ApplicationUserId.Equals(applicationUserId));
        }

        public User GetByUserId(Int64 userId)
        {
            return context.EngagerUsers.AsNoTracking().FirstOrDefault(x => x.Id.Equals(userId));
        }

        public async Task<IEnumerable<User>> GetByCustomerId(long customerId)
        {
            var users = from userCustomer in context.UserCustomers.AsNoTracking()
                        join user in context.EngagerUsers.AsNoTracking() on userCustomer.UserId equals user.Id
                        where userCustomer.CustomerId == customerId
                        where user.StatusId == 4265
                        select user;

            return users;
        }

        public User GetByUserName(string aUserName)
        {
            return context.EngagerUsers.AsNoTracking().FirstOrDefault(x => x.UserName.ToLower() == aUserName.ToLower());
        }

        public IEnumerable<User> GetDrivers()
        {
            Int64 userActiveStatus = context.CommonConfigurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.UserStatus.ToString())).OrderBy(x => x.SerialNo).Where(x => x.Code.Equals(UserStatusCodes.Active.ToString())).FirstOrDefault().Id;

            var users = from rolePermission in context.RolePermissions.AsNoTracking()
                        join function in context.Functions.AsNoTracking() on rolePermission.PermissionId equals function.Id
                        join userRole in context.EngagerUserRole.AsNoTracking() on rolePermission.RoleId equals userRole.RoleId
                        join user in context.EngagerUsers.AsNoTracking() on userRole.UserId equals user.Id
                        where user.ParentCompanyId == GlobalVariable.COMPANY_ID && function.Controller.Equals("IsDriverController") && user.StatusId.Equals(userActiveStatus)
                        select user;

            return users.OrderBy(x => x.LastName).ThenBy(x => x.FirstName);
        }

        public void UpdateFCMID(long id, string fcmId)
        {
            var user = context.EngagerUsers.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return;

            user.FCMId = fcmId;
            this.context.SaveChanges();
        }

        public bool ResetFCMId(string userName)
        {
            var user = context.EngagerUsers.FirstOrDefault(x => x.UserName == userName);

            if (user == null)
                return false;

            user.FCMId = null;
            this.context.SaveChanges();
            return true;
        }

        public async Task<IEnumerable<User>> GetByRole(UserCri aCri)
        {
            if (aCri == null) aCri = new UserCri();

            var users = from user in context.EngagerUsers.AsNoTracking() 
                        join userRole in context.EngagerUserRole.AsNoTracking() on user.Id equals userRole.UserId
                        join role in context.EngagerRoles.AsNoTracking() on userRole.RoleId equals role.Id
                        where user.ParentCompanyId == GlobalVariable.COMPANY_ID && role.Code.Equals(aCri.Role)
                        select user;

            if(aCri!=null && !string.IsNullOrEmpty(aCri.SearchValue))
            {
                users = users.Where(x => x.LastName.Contains(aCri.SearchValue) || x.FirstName.Contains(aCri.SearchValue) || x.UserName.Contains(aCri.SearchValue) || x.Email.Contains(aCri.SearchValue));
            }

            return users.OrderBy(x => x.LastName).ThenBy(x => x.FirstName);
        }
    }
}
