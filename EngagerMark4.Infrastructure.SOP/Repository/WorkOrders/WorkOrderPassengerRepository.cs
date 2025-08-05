using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IRepository.WorkOrders;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.SOP.Repository.WorkOrders
{
    public class WorkOrderPassengerRepository : GenericRepository<ApplicationDbContext, WorkOrderPassengerCri, WorkOrderPassenger>, IWorkOrderPassengerRepository
    {
        public WorkOrderPassengerRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }
    }
}
