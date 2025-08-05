using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Threading.Tasks;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.Service.ApplicationCore.Configurations;
using System.Data.Entity.Infrastructure;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Models;

namespace EngagerMark4.Controllers.Configurations
{
    public class VesselController : BaseController<CommonConfigurationCri, CommonConfiguration, IVesselService>
    {
        IConfigurationGroupService _configurationGroupService;
        ICustomerService _customerService;
        ICommonConfigurationService _commonConfigurationService;

        public VesselController(IVesselService service,
            IConfigurationGroupService configurationGroupService,
            ICustomerService customerService,
            ICommonConfigurationService commonConfigurationService) : base(service)
        {
            this._defaultColumn = "Code";
            this._defaultOrderBy = "Asc";
            this._defaultDataType = "String";
            this._configurationGroupService = configurationGroupService;
            this._customerService = customerService;
            this._commonConfigurationService = commonConfigurationService;
        }

        protected async override Task<IPagedList<CommonConfiguration>> GetEntities(CommonConfigurationCri aCri)
        {
            var cri = GetCri();
            var cri2 = GetOrderBy(cri);

            cri2.Includes = new List<string>();

            cri2.Includes.Add("ConfigurationGroup");

            var entities = (await _service.GetByCri(cri2)).Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.VesselId.ToString()));

            return entities.ToPagedList(aCri.CurrentPage, aCri.NoOfPage);
        }

        protected async override Task LoadReferences(CommonConfiguration entity)
        {
            var vessel = (await _configurationGroupService.GetByCri(null)).FirstOrDefault(x => x.Code.Equals(ConfigurationGrpCodes.VesselId.ToString()));
            ViewBag.Customers = new SelectList((await _customerService.GetByCri(null)).OrderBy(x => x.Name), "Id", "Name");
            entity.ConfigurationGroupId = vessel == null ? 0 : vessel.Id;
        }

        protected override CommonConfigurationCri GetCri()
        {
            CommonConfigurationCri cri = new CommonConfigurationCri();

            cri.SearchValue = Request["SearchValue"];
            ViewBag.SearchValue = cri.SearchValue;

            return cri;
        }

        /// <summary>
        /// Check if similar vessels already exist
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpPost]
        public async  Task<JsonResult> CheckForSimilarVessels(string Code, string Name)
        {
            var vesselGrp = (await _configurationGroupService.GetByCri(null)).FirstOrDefault(x => x.Code.Equals(ConfigurationGrpCodes.VesselId.ToString()));

            var existingConfig = _commonConfigurationService.GetSimilarConfig(Code, Name, vesselGrp.Id);

            if (existingConfig == null)
            {
                return Json("NoSimilarVessel");
            }

            return Json("Exists");
        }

        /// <summary>
        /// Check if the vessel if in use
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async override Task<bool> DeleteEntity(Object id)
        {
            var entity = new CommonConfiguration();
            entity = await _service.GetById(id);
            entity.Id = (Int64)id;

            bool isVesselUnderUse = false;

            isVesselUnderUse = _commonConfigurationService.IsVesselUnderUse(entity.Id);

            if (isVesselUnderUse)
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