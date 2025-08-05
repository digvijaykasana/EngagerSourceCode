using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.Job.Cris;
using EngagerMark4.ApplicationCore.Job.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Job.IService
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChecklistTemplateService : IBaseService<ChecklistTemplateCri, ChecklistTemplate>
    {
    }
}
