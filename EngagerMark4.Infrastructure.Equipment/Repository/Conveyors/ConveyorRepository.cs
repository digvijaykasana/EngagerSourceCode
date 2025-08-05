using EngagerMark4.ApplicationCore.Equipment.Cris;
using EngagerMark4.ApplicationCore.Equipment.IRepository.Conveyors;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.Equipment.Repository.Conveyors
{
    public class ConveyorRepository : GenericRepository<ApplicationDbContext, ConveyorCri, EngagerMark4.ApplicationCore.Equipment.Entities.Conveyors.Conveyor>, IConveyorRepository
    {
        public ConveyorRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }
    }
}
