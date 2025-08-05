using EngagerMark4.ApplicationCore.Equipment.Cris;
using EngagerMark4.ApplicationCore.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Equipment.IRepository.Equipments
{
    public interface IEquipmentRepository : IBaseRepository<EquipmentCri, Equipment.Entities.Equipments.Equipment>
    {
    }
}
