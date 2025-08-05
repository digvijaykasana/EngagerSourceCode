using EngagerMark4.ApplicationCore.Equipment.Cris;
using EngagerMark4.ApplicationCore.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Equipment.IService.Equipments
{
    public interface IEquipmentService : IBaseService<EquipmentCri, Equipment.Entities.Equipments.Equipment>
    {
    }
}
