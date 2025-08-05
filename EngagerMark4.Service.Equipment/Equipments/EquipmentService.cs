using EngagerMark4.ApplicationCore.Equipment.Cris;
using EngagerMark4.ApplicationCore.Equipment.IRepository.Equipments;
using EngagerMark4.ApplicationCore.Equipment.IService.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.Equipment.Equipments
{
    public class EquipmentService : AbstractService<IEquipmentRepository, EquipmentCri, EngagerMark4.ApplicationCore.Equipment.Entities.Equipments.Equipment>, IEquipmentService
    {
        public EquipmentService(IEquipmentRepository repository) : base(repository)
        { }
    }
}
