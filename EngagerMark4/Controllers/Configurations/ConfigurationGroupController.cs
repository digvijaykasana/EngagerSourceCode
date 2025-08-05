using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Configurations
{
    public class ConfigurationGroupController : BaseController<ConfigurationGroupCri, ConfigurationGroup,IConfigurationGroupService>
    {
        public ConfigurationGroupController(IConfigurationGroupService service) : base(service)
        {
        }
    }
}