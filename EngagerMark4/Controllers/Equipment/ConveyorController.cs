using EngagerMark4.ApplicationCore.Equipment.Cris;
using EngagerMark4.ApplicationCore.Equipment.Entities.Conveyors;
using EngagerMark4.ApplicationCore.Equipment.IService.Conveyors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Equipment
{
    public class ConveyorController : BaseController<ConveyorCri, Conveyor, IConveyorService>
    {
        public ConveyorController(IConveyorService service) : base(service)
        {
        }
    }
}