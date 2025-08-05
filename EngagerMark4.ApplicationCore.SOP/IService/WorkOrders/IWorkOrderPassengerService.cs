using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IService.WorkOrders
{
    public interface IWorkOrderPassengerService : IBaseService<WorkOrderPassengerCri, WorkOrderPassenger>
    {
    }
}
