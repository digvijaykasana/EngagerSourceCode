using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Configurations
{
    /// <summary>
    /// Controller Class for Vehicle
    /// Created     : Ye Kaung Aung
    /// Modified    : 
    /// </summary>
    /// 
    public class VehicleController : BaseController<VehicleCri, Vehicle, IVehicleService>
    {
        public VehicleController(IVehicleService service) : base(service)
        {
            this._defaultColumn = "VehicleNo";
        }

        protected override VehicleCri GetCri()
        {
            var cri = base.GetCri();
            cri.SearchValue = Request["SearchValue"];
            return cri;
        }

    }
}