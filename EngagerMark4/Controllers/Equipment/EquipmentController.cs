using EngagerMark4.ApplicationCore.Equipment.Cris;
using EngagerMark4.ApplicationCore.Equipment.IService.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Equipment
{
    public class EquipmentController : BaseController<EquipmentCri, EngagerMark4.ApplicationCore.Equipment.Entities.Equipments.Equipment, IEquipmentService>
    {
        public EquipmentController(IEquipmentService service) : base(service)
        { }
    }
}