using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.IService.Users;
using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using EngagerMark4.ApplicationCore.Cris.Configurations;
using System.Data.Entity.Infrastructure;
using EngagerMark4.Common.Configs;
using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Customer.IService;
using PagedList;

namespace EngagerMark4.Controllers.Users
{
    public class AgentController : BaseController<UserCri, User, IUserService>
    {
        IConfigurationGroupService configurationGroupService;
        ICommonConfigurationService commonConfigurationService;
        IRoleService roleService;
        IVehicleService _vehicleService;
        ICustomerService _customerService;
        IRolePermissionService _rolePermissionService;

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private string userStatus = "UserStatus";

        public AgentController(IUserService service,
            IConfigurationGroupService configurationGroupService,
            ICommonConfigurationService commonConfigurationService,
            IRoleService roleService,
            IVehicleService vehicleService,
            ICustomerService customerService,
            IRolePermissionService rolePermissionService) : base(service)
        {
            this.configurationGroupService = configurationGroupService;
            this.commonConfigurationService = commonConfigurationService;
            this.roleService = roleService;
            this._defaultColumn = "LastName";
            this._vehicleService = vehicleService;
            this._customerService = customerService;
            this._rolePermissionService = rolePermissionService;
        }

        protected async override Task LoadReferences(User entity)
        {
            var configurationGroupCri = new ConfigurationGroupCri();
            configurationGroupCri.StringCris = new Dictionary<string, ApplicationCore.Cris.StringValue>();
            configurationGroupCri.StringCris["Code"] = new ApplicationCore.Cris.StringValue { ComparisonOperator = ApplicationCore.Cris.BaseCri.StringComparisonOperator.Equal, Value = userStatus };
            var configurationGroup = (await configurationGroupService.GetByCri(configurationGroupCri)).FirstOrDefault();
            var configurationCri = new CommonConfigurationCri();
            configurationCri.NumberCris = new Dictionary<string, ApplicationCore.Cris.IntValue>();
            configurationCri.NumberCris["ConfigurationGroupId"] = new ApplicationCore.Cris.IntValue { ComparisonOperator = ApplicationCore.Cris.BaseCri.NumberComparisonOperator.Equal, Value = configurationGroup.Id };
            var userStatuses = await commonConfigurationService.GetByCri(configurationCri);
            ViewBag.StatusId = new SelectList(userStatuses.OrderBy(x => x.SerialNo), "Id", "Name", entity.StatusId);
            var roles = (await roleService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.Roles = roles.Where(x => x.Code.Equals(DummyRoleConfig.Agent));
            var vehicles = (await _vehicleService.GetByCri(null)).OrderBy(x => x.VehicleNo);
            ViewBag.Vehicles = vehicles;
            var customers = (await _customerService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.Customers = customers;

            ViewBag.HasPermissionForUserVehicle = _rolePermissionService.HasPermission(nameof(UserVehicleController), User.Identity.GetUserId());
            ViewBag.HasPermissionForUserCustomer = _rolePermissionService.HasPermission(nameof(UserCustomerController), User.Identity.GetUserId());
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected async override Task SaveEntity(User aEntity)
        {
            if (aEntity.Id == 0)
            {
                var user = new ApplicationUser { UserName = aEntity.UserName, Email = aEntity.UserName };

                var result = String.IsNullOrEmpty(aEntity.PasswordStr) ? await UserManager.CreateAsync(user, SecurityConfig.DEFAULT_PASSWORD) : await UserManager.CreateAsync(user, aEntity.PasswordStr);

                if (result.Succeeded)
                {
                    aEntity.ApplicationUserId = user.Id;
                    await base.SaveEntity(aEntity);
                }
                else
                {
                    AddErrors(result);
                    throw new DbUpdateException();
                }
            }
            else
            {

                if (!(String.IsNullOrEmpty(aEntity.PasswordStr)))
                {
                    ApplicationUser user = await UserManager.FindByIdAsync(aEntity.ApplicationUserId.ToString());

                    if (user != null)
                    {
                        var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                        var result = await UserManager.ResetPasswordAsync(user.Id, token, aEntity.PasswordStr);

                        if (!result.Succeeded)
                        {
                            AddErrors(result);
                            throw new DbUpdateException();
                        }
                    }

                }


                await base.SaveEntity(aEntity);
            }
        }

        protected override UserCri GetCri()
        {
            UserCri cri = new UserCri();

            cri.SearchValue = Request["SearchValue"];
            ViewBag.SearchValue = cri.SearchValue;
            cri.Role = DummyRoleConfig.Agent;
            return cri;
        }

        protected override Task<IEnumerable<User>> GetData(UserCri cri)
        {
            return this._service.GetByRole(cri);
        }
    }
}