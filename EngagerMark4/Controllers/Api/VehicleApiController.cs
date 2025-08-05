using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.ApplicationCore.IService.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api
{
    public class VehicleApiController : Controller
    {
        IUserVehicleRepository _userVehicleRepository;

        public VehicleApiController(IUserVehicleRepository userVehicleRepository)
        {
            this._userVehicleRepository = userVehicleRepository;
        }
        
        [AllowAnonymous]
        public JsonResult GetByUserId(Int64 userId=0)
        {
            var vehicles =  _userVehicleRepository.GetByUserId((Int64)userId);

            return Json(vehicles, JsonRequestBehavior.AllowGet);
        }
    }
}