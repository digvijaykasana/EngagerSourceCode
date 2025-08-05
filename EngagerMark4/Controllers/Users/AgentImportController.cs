using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.IRepository.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.Common.Configs;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.DataImportViewModel;
using EngagerMark4.DocumentProcessor;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Cris.Configurations;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;
using EngagerMark4.ApplicationCore.Dummy.Entities.Users;

namespace EngagerMark4.Controllers.Users
{
    public class AgentImportController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IUserService _userService;
        private ICustomerService _customerService;
        private IRoleService _roleService;
        private ICommonConfigurationService _commonConfigurationService;


        public AgentImportController(IUserService userService,
            ICustomerService customerService,
            IRoleService roleService,
            ICommonConfigurationService commonConfigurationService)
        {
            this._userService = userService;
            this._customerService = customerService;
            this._roleService = roleService;
            this._commonConfigurationService = commonConfigurationService;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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

        // GET: AgentImport
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string post="")
        {
            List<Customer> customers = (await _customerService.GetByCri(null)).ToList();
            List<Role> roles = (await _roleService.GetByCri(null)).ToList();
            CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            configurationCri.Includes = new List<string>();
            configurationCri.Includes.Add("ConfigurationGroup");

            var references = await _commonConfigurationService.GetByCri(configurationCri);
            var userStatuses = references.Where(s => s.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.UserStatus.ToString()));
            var activeStatus = userStatuses.FirstOrDefault(x => x.Code.ToLower().Trim().Equals("active"));

            List<AgentDataImportViewModel> importedAgents = new List<AgentDataImportViewModel>();

            foreach(string uploadFile in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[uploadFile] as HttpPostedFileBase;

                if(file!=null && file.ContentLength > 0)
                {
                    ExcelProcessor<AgentDataImportViewModel> excelProcessor = new ExcelProcessor<AgentDataImportViewModel>();
                    importedAgents = excelProcessor.ImportFromExcel(file.InputStream);
                }
            }

            List<User> agents = new List<User>();

            foreach(var importedAgent in importedAgents)
            {
                User agent = new ApplicationCore.Entities.Users.User
                {
                    UserName = importedAgent.UserName.Replace(" ", string.Empty).ToLower()+ "@Dummy.com.sg",
                    LastName = importedAgent.LastName,
                    FirstName = importedAgent.FirstName,
                    Email = importedAgent.Email,
                    ContactNo = importedAgent.ContactNo,
                    StatusId = activeStatus.Id
                };
                if(!string.IsNullOrEmpty(importedAgent.Role))
                {
                    var role = roles.FirstOrDefault(x => x.Name.ToLower().Trim().Equals(importedAgent.Role.ToLower().Trim()));
                    if(role!=null)
                    {
                        UserRole userRole = new UserRole
                        {
                            RoleId = role.Id
                        };
                        agent.UserRoleList.Add(userRole);
                    }
                }
                if (!string.IsNullOrEmpty(importedAgent.Customer))
                {
                    var customer = customers.FirstOrDefault(x => x.Name.ToLower().Trim().Equals(importedAgent.Customer.ToLower().Trim()));
                    if(customer!=null)
                    {
                        UserCustomer userCustomer = new UserCustomer
                        {
                            CustomerId = customer.Id
                        };
                        agent.CustomerList.Add(userCustomer);
                    }
                }

                var applicationUser = new ApplicationUser
                {
                    UserName = agent.UserName,
                    Email = agent.UserName
                };
                var result = await UserManager.CreateAsync(applicationUser, SecurityConfig.DEFAULT_PASSWORD);
                if(result.Succeeded)
                {
                    agent.ApplicationUserId = applicationUser.Id;
                    await _userService.Save(agent);
                }

            }

            TempData["color"] = ApplicationConfig.MESSAGE_SAVE_COLOR;
            TempData["message"] = ApplicationConfig.MESSAGE_IMPORT_MESSAGE;
            return RedirectToAction(nameof(Index));
        }
    }
}