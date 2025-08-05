using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IRepository.WorkOrders
{
    public interface IWorkOrderPassengerRepository : IBaseRepository<WorkOrderPassengerCri, WorkOrderPassenger>
    {
    }
}
