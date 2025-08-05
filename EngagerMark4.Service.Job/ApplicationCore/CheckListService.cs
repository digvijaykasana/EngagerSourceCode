using EngagerMark4.ApplicationCore.Job.Cris;
using EngagerMark4.ApplicationCore.Job.Entities;
using EngagerMark4.ApplicationCore.Job.IRepository;
using EngagerMark4.ApplicationCore.Job.IService;
using EngagerMark4.Service.ApplicationCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.Job.ApplicationCore
{
    public class CheckListService : AbstractService<ICheckListRepository, CheckListCri, CheckList>, ICheckListService
    {
        public CheckListService (ICheckListRepository repository) : base(repository)
        { }
    }
}
