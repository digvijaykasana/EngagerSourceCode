using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.Job.Cris;
using EngagerMark4.ApplicationCore.Job.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Job.IRepository
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICheckListRepository : IBaseRepository<CheckListCri, CheckList>
    {
    }
}
