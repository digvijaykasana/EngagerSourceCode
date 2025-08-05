using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.IService.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using EngagerMark4.ApplicationCore.Cris.Configurations;

namespace EngagerMark4.Controllers.Users
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleController : BaseController<RoleCri, Role, IRoleService>
    {
        private string _roleStatus = "RoleStatus";
        IConfigurationGroupService _configurationGroupService;
        ICommonConfigurationService _commonConfigurationService;

        public RoleController(IRoleService service,
            IConfigurationGroupService configurationGroupService,
            ICommonConfigurationService commonConfigurationService) : base(service)
        {
            this._configurationGroupService = configurationGroupService;
            this._commonConfigurationService = commonConfigurationService;
        }
    }
}