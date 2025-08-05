using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.SOP.Repository.Jobs
{
    public class ServiceJobChecklistItemRepository : GenericRepository<ApplicationDbContext, ServiceJobChecklistItemCri, ServiceJobChecklistItem>, IServiceJobChecklistItemRepository
    {
        public ServiceJobChecklistItemRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }
    }
}
