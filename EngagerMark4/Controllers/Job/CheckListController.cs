using EngagerMark4.ApplicationCore.Common;
using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Account.Entities;
using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.ApplicationCore.Job.Cris;
using EngagerMark4.ApplicationCore.Job.Entities;
using EngagerMark4.ApplicationCore.Job.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.Controllers.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;
using EngagerMark4.ApplicationCore.Cris.Configurations;

namespace EngagerMark4.Controllers.Job
{
    /// <summary>
    /// Controller Class for CheckList
    /// Created     : Ye Kaung Aung
    /// Modified    : 
    /// </summary>
    public class CheckListController : BaseController<CheckListCri, CheckList, ICheckListService>
    {
        IGeneralLedgerService generalLedgerService;
        IRolePermissionService rolePermissionService;
        ICommonConfigurationService configurationService;

        public CheckListController(ICheckListService service, IGeneralLedgerService generalLedgerService,
            IRolePermissionService rolePermissionService, ICommonConfigurationService configurationService) : base(service)
        {
            this.generalLedgerService = generalLedgerService;
            this.rolePermissionService = rolePermissionService;
            this.configurationService = configurationService;
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        protected async override Task LoadReferencesForList(CheckListCri aCri)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            
        }

        protected async override Task LoadReferences(CheckList entity)
        {
            if (entity == null) entity = new CheckList();

            var references = await generalLedgerService.GetByCri(null);

            ViewBag.GLCodeId = new SelectList(references, "Id", "Name", entity.GLCodeId);

            string userId = User.Identity.GetUserId();

            ViewBag.HasPermissionForGLCode = rolePermissionService.HasPermission(nameof(GLCodeController), userId);

            //Load Salary Pay Items
            CommonConfigurationCri cri = new CommonConfigurationCri();
            cri.Includes = new List<string>();
            cri.Includes.Add("ConfigurationGroup");
            var configs = await configurationService.GetByCri(cri);

            ViewBag.SalaryPayItemCode = new SelectList(configs.Where(x => x.ConfigurationGroup.Code == ConfigurationGrpCodes.SalaryPayItem.ToString()), "Code", "Name", entity.SalaryPayItemCode);
        }
    }
}