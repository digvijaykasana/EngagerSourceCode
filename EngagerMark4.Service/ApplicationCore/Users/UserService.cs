using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.ApplicationCore.IService.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using EngagerMark4.ApplicationCore.IRepository.Configurations;

namespace EngagerMark4.Service.ApplicationCore.Users
{
    /// <summary>
    /// 
    /// </summary>
    public class UserService : AbstractService<IUserRepository, UserCri, User>, IUserService
    {
        IUserRoleRepository userRoleRepository;
        IRolePermissionService _rolePermissionService;
        IUserCustomerRepository _userCustomerRepository;
        IUserVehicleRepository _userVehicleRepository;
        IVehicleRepository _vehicleRepository;

        public UserService(IUserRepository repository,
            IUserRoleRepository userRoleRepository,
            IRolePermissionService rolePermissionService,
            IUserCustomerRepository userCustomerRepository,
            IUserVehicleRepository userVehicleRepository,
            IVehicleRepository vehicleRepository) : base(repository)
        {
            this.userRoleRepository = userRoleRepository;
            this._rolePermissionService = rolePermissionService;
            this._userCustomerRepository = userCustomerRepository;
            this._userVehicleRepository = userVehicleRepository;
            this._vehicleRepository = vehicleRepository;
        }

        public IEnumerable<User> GetAgent(Int64 customerId = 0)
        {
            return this.repository.GetAgent(customerId);
        }

        public IEnumerable<User> GetAgentMgr(Int64 customerId = 0)
        {
            return this.repository.GetAgentMgr(customerId);
        }

        public User GetByApplicatioNId(string applicationUserId)
        {
            return this.repository.GetByApplicatioNId(applicationUserId);
        }

        public User GetByUserId(Int64 userId)
        {
            return this.repository.GetByUserId(userId);
        }

        public async Task<IEnumerable<User>> GetByCustomerId(long customerId)
        {
            return await repository.GetByCustomerId(customerId);
        }

        public async override Task<User> GetById(object id)
        {
            var user = await base.GetById(id);
            var cri = new UserRoleCri();
            cri.Includes = new List<string>();
            cri.Includes.Add("Role");
            cri.NumberCris = new Dictionary<string, EngagerMark4.ApplicationCore.Cris.IntValue>();
            cri.NumberCris["UserId"] = new EngagerMark4.ApplicationCore.Cris.IntValue { ComparisonOperator = EngagerMark4.ApplicationCore.Cris.BaseCri.NumberComparisonOperator.Equal, Value = user.Id };
            var userRoleList = await this.userRoleRepository.GetByCri(cri);
            user.UserRoleList = userRoleList.ToList();

            //var userId = HttpContext.Current.User.Identity.GetUserId();

            var userCustomerList = this._userCustomerRepository.GetByUserId(user.Id);
            user.CustomerList = userCustomerList.ToList();


            var userVehicleList = this._userVehicleRepository.GetByUserId(user.Id);
            user.VehicleList = userVehicleList.ToList();

            return user;
        }

        public User GetByUserName(string aUserName)
        {
            return this.repository.GetByUserName(aUserName);
        }

        public IEnumerable<User> GetDrivers()
        {
            return this.repository.GetDrivers();
        }

        public async override Task<long> Save(User entity)
        {
            this.repository.Save(entity);

            if(entity.Id !=0)
            {
                var cri = new UserRoleCri();
                cri.NumberCris = new Dictionary<string, EngagerMark4.ApplicationCore.Cris.IntValue>();
                cri.NumberCris["UserId"] = new EngagerMark4.ApplicationCore.Cris.IntValue
                {
                    ComparisonOperator = EngagerMark4.ApplicationCore.Cris.BaseCri.NumberComparisonOperator.Equal,
                    Value = entity.Id
                };
                var roleList = await this.userRoleRepository.GetByCri(cri);

                if(roleList != null)
                {
                    foreach(var userRole in roleList)
                    {
                        this.userRoleRepository.Delete(userRole);
                    }
                }
            }

            foreach(var role in entity.GetRoleList())
            {
                this.userRoleRepository.Save(role);
            }

            var userId = HttpContext.Current.User.Identity.GetUserId();

            #region Save Vehicle
            var hasPermissionForVehicle = _rolePermissionService.HasPermission("UserVehicleController", userId);
            if(hasPermissionForVehicle)
            {
                if (entity.Id != 0)
                {
                    var cri = new UserVehicleCri();
                    cri.NumberCris = new Dictionary<string, EngagerMark4.ApplicationCore.Cris.IntValue>();
                    cri.NumberCris["UserId"] = new EngagerMark4.ApplicationCore.Cris.IntValue
                    {
                        ComparisonOperator = EngagerMark4.ApplicationCore.Cris.BaseCri.NumberComparisonOperator.Equal,
                        Value = entity.Id
                    };
                    var vehicleList = await this._userVehicleRepository.GetByCri(cri);

                    if (vehicleList != null)
                    {
                        foreach (var userVehicle in vehicleList)
                        {
                            this._userVehicleRepository.Delete(userVehicle);
                        }
                    }
                }

                foreach (var vehicle in entity.GetVehicleList())
                {
                    this._userVehicleRepository.Save(vehicle);
                }
            }
            #endregion

            #region Save Customer 
            var hasPermissionForCustomer = _rolePermissionService.HasPermission("UserCustomerController", userId);
            if(hasPermissionForCustomer)
            {
                if (entity.Id != 0)
                {
                    var cri = new UserCustomerCri();
                    cri.NumberCris = new Dictionary<string, EngagerMark4.ApplicationCore.Cris.IntValue>();
                    cri.NumberCris["UserId"] = new EngagerMark4.ApplicationCore.Cris.IntValue
                    {
                        ComparisonOperator = EngagerMark4.ApplicationCore.Cris.BaseCri.NumberComparisonOperator.Equal,
                        Value = entity.Id
                    };
                    var customerList = await this._userCustomerRepository.GetByCri(cri);

                    if (customerList != null)
                    {
                        foreach (var customer in customerList)
                        {
                            this._userCustomerRepository.Delete(customer);
                        }
                    }
                }
                foreach (var customer in entity.GetCustomerList())
                {
                    this._userCustomerRepository.Save(customer);
                }
            }
            #endregion
            await this.repository.SaveChangesAsync();

            return entity.Id;
        }

        public bool ResetFCMId(string userName)
        {
            return this.repository.ResetFCMId(userName);
        }

        public async Task<IEnumerable<User>> GetByRole(UserCri aCri)
        {
            var userList = await this.repository.GetByRole(aCri);

            if (String.IsNullOrEmpty(aCri.Vehicle)) return userList;

            var vehicleList = await _vehicleRepository.GetByCri(new EngagerMark4.ApplicationCore.Cris.Configurations.VehicleCri
            { SearchValue = aCri.Vehicle });

            if (vehicleList == null || vehicleList.Count() == 0) return userList;

            var userIdList = _userVehicleRepository.GetUserIdListByVehicleId(vehicleList.First().Id);

            if (userIdList == null || userIdList.Count() == 0) return userList;

            List<User> newUserList = new List<User>();

            foreach (var userId in userIdList)
            {
                var user = userList.Where(x => x.Id == userId).FirstOrDefault();

                if (user != null) newUserList.Add(user);
            }

            return newUserList;
        }


        public async override Task<IEnumerable<User>> GetByCri(UserCri aCri)
        {
            var userList = await this.repository.GetByCri(aCri);

            if (String.IsNullOrEmpty(aCri.Vehicle)) return userList;

            var vehicleList = await _vehicleRepository.GetByCri(new EngagerMark4.ApplicationCore.Cris.Configurations.VehicleCri
            { SearchValue = aCri.Vehicle });

            if (vehicleList == null || vehicleList.Count() == 0) return userList;

            var userIdList = _userVehicleRepository.GetUserIdListByVehicleId(vehicleList.First().Id);

            if (userIdList == null || userIdList.Count() == 0) return userList;

            List<User> newUserList = new List<User>();

            foreach(var userId in userIdList)
            {
                var user = userList.Where(x => x.Id == userId).FirstOrDefault();

                if (user != null) newUserList.Add(user);
            }

            return newUserList;
        }
    }
}
