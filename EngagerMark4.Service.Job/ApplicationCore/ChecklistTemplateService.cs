using EngagerMark4.ApplicationCore.Job.Cris;
using EngagerMark4.ApplicationCore.Job.Entities;
using EngagerMark4.ApplicationCore.Job.IRepository;
using EngagerMark4.ApplicationCore.Job.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.Job.ApplicationCore
{
    public class ChecklistTemplateService : AbstractService<IChecklistTemplateRepository, ChecklistTemplateCri, ChecklistTemplate>, IChecklistTemplateService
    {
        public ChecklistTemplateService(IChecklistTemplateRepository repository) : base(repository)
        {
        }

        public async override Task<long> Save(ChecklistTemplate entity)
        {
            this.repository.Save(entity);

            await repository.SaveChangesAsync();

            return entity.Id;
        }
    }
}
