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
using EngagerMark4.Common.Configs;
using System.Reflection;

namespace EngagerMark4.Controllers.Application
{
    public class FunctionController : BaseController<PermissionCri, Function, IFunctionService>
    {
        public FunctionController(IFunctionService service) : base(service)
        {
            _defaultColumn = "Controller";
        }

        protected override PermissionCri GetCri()
        {
            var cri = base.GetCri();
            cri.SearchValue = Request["SearchValue"];
            return cri;
        }

        private void LoadControllers(Function permission)
        {
            List<General> controllers = new List<General>();

            Assembly asm = Assembly.GetExecutingAssembly();

            var types = asm.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type))
                //.SelectMany(type => type.GetMethods())
                .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)));

            foreach (var type in types)
            {
                General controller = new General
                {
                    Value = type.Name
                };
                controllers.Add(controller);
            }
            ViewBag.Controller = new SelectList(controllers.OrderBy(x => x.Value), "Value", "Value", permission.Controller ?? "");
        }

        protected override Task LoadReferences(Function entity)
        {
            LoadControllers(entity);
            return base.LoadReferences(entity);
        }
    }
}