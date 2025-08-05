using EngagerMark4.ApplicationCore.Equipment.Cris;
using EngagerMark4.ApplicationCore.Equipment.IRepository.Equipments;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.Equipment.Repository.Equipments
{
    public class EquipmentRepository : GenericRepository<ApplicationDbContext, EquipmentCri, EngagerMark4.ApplicationCore.Equipment.Entities.Equipments.Equipment>, IEquipmentRepository
    {
        public EquipmentRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }
    }
}
