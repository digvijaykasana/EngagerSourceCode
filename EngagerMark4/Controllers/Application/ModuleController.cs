using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Cris.Application;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.IService.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace EngagerMark4.Controllers.Application
{
    public class ModuleController : BaseController<ModuleCri, Module,IModuleService>
    {
        IFunctionService functionService;
        public ModuleController(IModuleService service,
            IFunctionService functionService) : base(service)
        {
            this.functionService = functionService;
        }

        protected async override Task LoadReferences(Module entity)
        {
            var functionList = await this.functionService.GetByCri(null);
            ViewBag.Functions = functionList;
        }
    }
}