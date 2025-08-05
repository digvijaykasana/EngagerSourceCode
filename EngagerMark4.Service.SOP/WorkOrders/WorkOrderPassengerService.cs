using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IRepository.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.SOP.WorkOrders
{
    public class WorkOrderPassengerService : AbstractService<IWorkOrderPassengerRepository, WorkOrderPassengerCri, WorkOrderPassenger>, IWorkOrderPassengerService
    {
        public WorkOrderPassengerService(IWorkOrderPassengerRepository repository) : base(repository)
        {
            
        }
    }
}
