using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api
{
    public class MoveToBilledController : Controller
    {
        IWorkOrderService _service;
        IRolePermissionService _rolePermissionService;
        // GET: MoveToBilled
        public ActionResult Index()
        {
            return View();
        }

        public MoveToBilledController(IWorkOrderService service, IRolePermissionService rolePermisisonService)
        {
            this._service = service;
        }


        [HttpPost]
        public async Task<ActionResult> MoveToBill(Int64[] invoiceIds)
        {

                await _service.MovetoBill(invoiceIds);
                return Content("success");


        }
    }
}