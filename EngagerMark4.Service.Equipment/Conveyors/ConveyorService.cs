using EngagerMark4.ApplicationCore.Equipment.Cris;
using EngagerMark4.ApplicationCore.Equipment.Entities.Conveyors;
using EngagerMark4.ApplicationCore.Equipment.IRepository.Conveyors;
using EngagerMark4.ApplicationCore.Equipment.IService.Conveyors;
using EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.Equipment.Conveyors
{
    public class ConveyorService : AbstractService<IConveyorRepository, ConveyorCri, Conveyor>, IConveyorService
    {
        public ConveyorService(IConveyorRepository repository) : base(repository)
        {
        }
    }
}
