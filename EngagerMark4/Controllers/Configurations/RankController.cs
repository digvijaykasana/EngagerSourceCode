using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;
using PagedList;

namespace EngagerMark4.Controllers.Configurations
{
    public class RankController : BaseController<CommonConfigurationCri, CommonConfiguration, ICommonConfigurationService>
    {
        IConfigurationGroupService configurationGroupService;

        public RankController(ICommonConfigurationService service,
            IConfigurationGroupService configurationGroupService) : base(service)
        {
            this.configurationGroupService = configurationGroupService;
        }

        protected override CommonConfigurationCri GetCri()
        {
            var cri = base.GetCri();
            cri.SearchValue = Request["SearchValue"];
            return cri;
        }

        //protected async override Task<IPagedList<CommonConfiguration>> GetEntities(CommonConfigurationCri aCri)
        //{
        //    aCri.Includes = new List<string>();
        //    aCri.Includes.Add("ConfigurationGroup");
        //    aCri.SearchValue = Request["SearchValue"];
        //    aCri.StringCris.Add("Name", new ApplicationCore.Cris.StringValue { ComparisonOperator = ApplicationCore.Cris.BaseCri.StringComparisonOperator.Contains, Value = aCri.SearchValue });
        //    var entities = (await _service.GetByCri(aCri)).Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.Rank.ToString()));
        //    return entities.ToPagedList(aCri.CurrentPage, aCri.NoOfPage);
        //}

        protected async override Task LoadReferences(CommonConfiguration entity)
        {
            var rank = (await configurationGroupService.GetByCri(null)).FirstOrDefault(x => x.Code.Equals(ConfigurationGrpCodes.Rank.ToString()));
            entity.ConfigurationGroupId = rank == null ? 0 : rank.Id;
        }

        //protected override CommonConfigurationCri GetCri()
        //{
        //    CommonConfigurationCri cri = new CommonConfigurationCri();
        //    cri.StringCris.Add("Name", new ApplicationCore.Cris.StringValue { ComparisonOperator = ApplicationCore.Cris.BaseCri.StringComparisonOperator.Contains, Value = cri.SearchValue });
        //    return cri;
        //}

        //protected override void ApplyCri(CommonConfigurationCri cri)
        //{
        //    if (cri != null && !(string.IsNullOrEmpty(cri.SearchValue)))
        //        base.queryableData = queryableData.Where(x => x.Code.Contains(cri.SearchValue) || x.Name.Contains(cri.SearchValue));
        //}
    }
}