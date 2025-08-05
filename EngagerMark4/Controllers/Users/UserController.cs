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
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Common;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.Infrasturcture.MobilePushNotifications.FCM;

namespace EngagerMark4.Controllers.Users
{
    /// <summary>
    /// 
    /// </summary>
    public class UserController : BaseController<UserCri, User, IUserService>
    {
        IConfigurationGroupService configurationGroupService;
        ICommonConfigurationService commonConfigurationService;
        IRoleService roleService;
        IVehicleService _vehicleService;
        ICustomerService _customerService;
        IRolePermissionService _rolePermissionService;
        FCMSender _fcmSender;

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

        public UserController(IUserService service,
            IConfigurationGroupService configurationGroupService,
            ICommonConfigurationService commonConfigurationService,
            IRoleService roleService,
            IVehicleService vehicleService,
            ICustomerService customerService,
            IRolePermissionService rolePermissionService,
            FCMSender fCMSender) : base(service)
        {
            this.configurationGroupService = configurationGroupService;
            this.commonConfigurationService = commonConfigurationService;
            this.roleService = roleService;
            this._defaultColumn = "LastName";
            this._vehicleService = vehicleService;
            this._customerService = customerService;
            this._rolePermissionService = rolePermissionService;
            this._fcmSender = fCMSender;
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
            ViewBag.Roles = roles;
            var vehicles = (await _vehicleService.GetByCri(null)).OrderBy(x => x.VehicleNo);
            ViewBag.Vehicles = vehicles;
            var customers = (await _customerService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.Customers = customers;

            ViewBag.HasPermissionForUserVehicle = _rolePermissionService.HasPermission(nameof(UserVehicleController), User.Identity.GetUserId());
            ViewBag.HasPermissionForUserCustomer = _rolePermissionService.HasPermission(nameof(UserCustomerController), User.Identity.GetUserId());
            ViewBag.HasPermissionForGeneralAdmin = _rolePermissionService.HasPermission(nameof(IsGeneralAdminController), User.Identity.GetUserId());
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

                    await this.SetUserStatus(aEntity.ApplicationUserId);
                }
                else
                {
                    AddErrors(result);
                    //throw new DbUpdateException();
                }
            }
            else
            {

                if(!(String.IsNullOrEmpty(aEntity.PasswordStr)))
                {
                    ApplicationUser user = await UserManager.FindByIdAsync(aEntity.ApplicationUserId.ToString());

                    if (user != null)
                    {
                        var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                        var result = await UserManager.ResetPasswordAsync(user.Id, token, aEntity.PasswordStr);

                        if (!result.Succeeded)
                        {
                            AddErrors(result);
                            //throw new DbUpdateException();
                        }
                    }

                }

                await base.SaveEntity(aEntity);

                await this.SetUserStatus(aEntity.ApplicationUserId);
            }
        }

        protected async Task SetUserStatus(string userAppId)
        {
            User user = _service.GetByApplicatioNId(userAppId);

            if (user == null) return;

            CommonConfiguration userStatus = await commonConfigurationService.GetById(user.StatusId);

            if (userStatus == null) return;


            if (userStatus.Code == GeneralConfig.UserStatusCodes.InActive.ToString())
            {
                bool isLockedOut = await UserManager.IsLockedOutAsync(userAppId);

                if (isLockedOut) return;

                var result = await UserManager.SetLockoutEnabledAsync(userAppId, true);

                if(result.Succeeded)
                {
                    result = await UserManager.SetLockoutEndDateAsync(userAppId, DateTimeOffset.MaxValue);
                }
            }

            if(userStatus.Code == GeneralConfig.UserStatusCodes.Active.ToString())
            {
                bool isLockedOut = await UserManager.IsLockedOutAsync(userAppId);

                if (!isLockedOut) return;

                var result = await UserManager.SetLockoutEnabledAsync(userAppId, false);

                if(result.Succeeded)
                {
                    await UserManager.ResetAccessFailedCountAsync(userAppId);
                }
            }


        }

        protected override UserCri GetCri()
        {
            UserCri cri = new UserCri();

            cri.SearchValue = Request["SearchValue"];
            ViewBag.SearchValue = cri.SearchValue;

            return cri;
        }


        [AllowAnonymous]
        public async Task<ActionResult> SendTestNotification(string fcmId = "")
        {
            try
            {
                await this._fcmSender.Send(fcmId, "Test Notification", "Test Notification Sent at " + DateTime.Today.ToLongDateString());

                return Content("success");
            }
            catch (Exception ex)
            {
                return Content("failure");
            }
        }
    }
}