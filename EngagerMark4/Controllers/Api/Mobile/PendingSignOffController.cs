using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.Job.Entities;
using EngagerMark4.ApplicationCore.Job.IService;
using EngagerMark4.ApplicationCore.Dummy.IService.MeetingServices;
using EngagerMark4.Controllers.Api.Account;
using EngagerMark4.Models;
using EngagerMark4.MvcFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;

namespace EngagerMark4.Controllers.Api.Mobile
{
    public class PendingSignOffController : BaseApiController
    {
        ICheckListService _service;
        ICommonConfigurationService _configurationService;
        IMeetingServiceService _meetingService;
        

        public PendingSignOffController(ICheckListService service,
            ICommonConfigurationService configurationService,
            IMeetingServiceService meetingService)
        {
            this._service = service;
            this._meetingService = meetingService;
            this._configurationService = configurationService;
        }

        [MobileApi]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> GetReferences()
        {
            if(!ValidateToken())
            {
                return Content("Invalid Token");
            }

            SetParentCompanyId();

            var checklist = await _service.GetByCri(null);
            var meetingService = await _meetingService.GetByCri(null);
            CommonConfigurationCri cri = new CommonConfigurationCri();
            cri.Includes = new List<string>();
            cri.Includes.Add("ConfigurationGroup");
            var configurations = await _configurationService.GetByCri(cri);

            MobileViewModel viewModel = new MobileViewModel
            {
                Checklists = checklist.OrderBy(x => x.Name).ToList(),
                MeetingServices = meetingService.OrderBy(x => x.Name).ToList(),
                CustomDetentions = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.CustomDetention.ToString())).OrderBy(x => x.Name).ToList(),
                Ranks = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.Rank.ToString())).OrderBy(x => x.Name).ToList()
            };

            List<CheckList> referenceList = new List<CheckList>();
            if (viewModel.Checklists != null)
            {
                foreach (var checkList in viewModel.Checklists)
                {
                    CheckList obj = new CheckList
                    {
                        Id = checkList.Id,
                        Code = checkList.Code,
                        Name = checkList.Name,
                        ShortText10 = "Checklist",
                    };
                    referenceList.Add(obj);
                }
            }

            if(viewModel.MeetingServices!=null)
            {
                foreach(var ms in viewModel.MeetingServices)
                {
                    CheckList obj1 = new CheckList
                    {
                        Id = ms.Id,
                        Code = ms.Name,
                        Name = ms.Name,
                        ShortText10 = "MeetingService",
                    };
                    referenceList.Add(obj1);
                }
            }

            if(viewModel.CustomDetentions!=null)
            {
                foreach(var cd in viewModel.CustomDetentions)
                {
                    CheckList obj2 = new CheckList
                    {
                        Id = cd.Id,
                        Code = cd.Code,
                        Name = cd.Name,
                        ShortText10 = "CustomDetention"
                    };
                    referenceList.Add(obj2);
                }
            }

            if (viewModel.Ranks != null)
            {
                foreach (var rank in viewModel.Ranks)
                {
                    CheckList obj3 = new CheckList
                    {
                        Id = rank.Id,
                        Code = rank.Code,
                        Name = rank.Name,
                        ShortText10 = "Rank"
                    };
                    referenceList.Add(obj3);
                }
            }

            return Json(referenceList, JsonRequestBehavior.AllowGet);
        }
    }
}