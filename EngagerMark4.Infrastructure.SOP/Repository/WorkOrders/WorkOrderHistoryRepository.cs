using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IRepository.WorkOrder;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.SOP.Repository.WorkOrders
{
    public class WorkOrderHistoryRepository : GenericRepository<ApplicationDbContext, WorkOrderHistoryCri, WorkOrderHistory>, IWorkOrderHistoryRepository
    {
        public WorkOrderHistoryRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }
    }
}
