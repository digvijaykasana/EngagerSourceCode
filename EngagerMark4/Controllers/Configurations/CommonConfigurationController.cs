using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Exceptions;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Service.ApplicationCore.Configurations;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Aspose.Pdf.Operator;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;

namespace EngagerMark4.Controllers.Configurations
{
    public class CommonConfigurationController : BaseController<CommonConfigurationCri, CommonConfiguration, ICommonConfigurationService>
    {
        IConfigurationGroupService configurationGroupService;

        public CommonConfigurationController(ICommonConfigurationService service,
            IConfigurationGroupService configurationGroupService) : base(service)
        {
            this.configurationGroupService = configurationGroupService;
        }

        protected async override Task LoadReferencesForList(CommonConfigurationCri aCri)
        {
            var references = await configurationGroupService.GetByCri(null);
            /*
            List<string> groupList = new List<string>();

            foreach (ConfigurationGroup group in references)
            {
                groupList.Add(group.Name);
            }

            ViewBag.GroupList = groupList;*/
            ViewBag.ConfigurationGroupId = new SelectList(references, "Id", "Name");
        }

        protected async override Task LoadReferences(CommonConfiguration entity)
        {
            var references = await configurationGroupService.GetByCri(null);

            ViewBag.ConfigurationGroupId = new SelectList(references, "Id", "Name", entity?.ConfigurationGroupId);
        }

        /// <summary>
        /// Check for similar configurations
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Name"></param>
        /// <param name="ConfigurationGrpId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> CheckForSimilarConfigs(string Code, string Name, long ConfigurationGrpId)
        {
            var existingConfig = this._service.GetSimilarConfig(Code, Name, ConfigurationGrpId);

            if (existingConfig == null)
            {
                return Json("NoSimilarConfig");
            }

            return Json("Exists");
        }

        protected override CommonConfigurationCri GetCri()
        {
            string configurationGroupIdStr = Request["ConfigurationGroupId"];

            ViewBag.ConfigurationGroupId = configurationGroupIdStr;

            Int64 configurationGroupId = 0;

            Int64.TryParse(configurationGroupIdStr, out configurationGroupId);

            var cri = new CommonConfigurationCri
            {
                NumberCris = new Dictionary<string, ApplicationCore.Cris.IntValue>(),
                
            };

            #region Includes
            cri.Includes = new List<string> { "ConfigurationGroup" };
            #endregion

            #region Cri

            if (configurationGroupId > 0)
                cri.NumberCris["ConfigurationGroupId"] = new ApplicationCore.Cris.IntValue { ComparisonOperator = ApplicationCore.Cris.BaseCri.NumberComparisonOperator.Equal, Value = configurationGroupId };

            #endregion

            return cri;
        }

        /// <summary>
        //Check if config is a vessel and if the vessel is in use before deleting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async override Task<bool> DeleteEntity(Object id)
        {
            var entity = new CommonConfiguration();
            entity = await _service.GetById(id);
            entity.Id = (Int64)id;

            bool isVesselUnderUse = false;
            //Check if configuration is vessel type
            ConfigurationGroupCri configGrpCri = new ConfigurationGroupCri
            {
                StringCris = new Dictionary<string, ApplicationCore.Cris.StringValue>(),
            };
            configGrpCri.StringCris["Code"] = new ApplicationCore.Cris.StringValue { ComparisonOperator = ApplicationCore.Cris.BaseCri.StringComparisonOperator.Equal, Value = ConfigurationGrpCodes.VesselId.ToString() };
            var vesselGroup = (await configurationGroupService.GetByCri(configGrpCri)).FirstOrDefault();
            if (vesselGroup != null && entity.ConfigurationGroupId == vesselGroup.Id)
            {
                isVesselUnderUse = _service.IsVesselUnderUse(entity.Id);
            }

            if(isVesselUnderUse)
            {
                return false;
            }
            else
            {
               return await base.DeleteEntity(id);
            }
        }
    }
}