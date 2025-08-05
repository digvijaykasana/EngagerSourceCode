using EngagerMark4.ApplicationCore.Equipment.Cris;
using EngagerMark4.ApplicationCore.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Equipment.IRepository.Conveyors
{
    public interface IConveyorRepository : IBaseRepository<ConveyorCri, Entities.Conveyors.Conveyor>
    {
    }
}
