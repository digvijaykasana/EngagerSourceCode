using EngagerMark4.ApplicationCore.Job.Cris;
using EngagerMark4.ApplicationCore.Job.Entities;
using EngagerMark4.ApplicationCore.Job.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using EngagerMark4.ApplicationCore.Customer.IService;

namespace EngagerMark4.Controllers.Job
{
    public class ChecklistTemplateController : BaseController<ChecklistTemplateCri, ChecklistTemplate, IChecklistTemplateService>
    {
        ICheckListService _checkListService;
        ICustomerService _customerService;

        public ChecklistTemplateController(IChecklistTemplateService service,
            ICheckListService checklistService,
            ICustomerService customerService) : base(service)
        {
            this._checkListService = checklistService;
            this._customerService = customerService;
        }

        protected async override Task LoadReferences(ChecklistTemplate entity)
        {
            var checklist = (await this._checkListService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.CheckList = checklist;
            ViewBag.ReferenceId = new SelectList((await _customerService.GetByCri(null)).OrderBy(x => x.Name), "Id", "Name", entity.ReferenceId);
        }
    }
}