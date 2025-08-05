using EngagerMark4.ApplicationCore.Job.Cris;
using EngagerMark4.ApplicationCore.Job.Entities;
using EngagerMark4.ApplicationCore.Job.IRepository;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.Job.Repository
{
    public class CheckListRepository : GenericRepository<ApplicationDbContext, CheckListCri, CheckList>, ICheckListRepository
    {
        public CheckListRepository (ApplicationDbContext aContext) : base(aContext)
        { }
    }
}
