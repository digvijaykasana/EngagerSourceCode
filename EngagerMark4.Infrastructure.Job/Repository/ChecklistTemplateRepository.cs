using EngagerMark4.ApplicationCore.Job.Cris;
using EngagerMark4.ApplicationCore.Job.Entities;
using EngagerMark4.ApplicationCore.Job.IRepository;
using EngagerMark4.Common;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EngagerMark4.Infrastructure.Job.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class ChecklistTemplateRepository : GenericRepository<ApplicationDbContext, ChecklistTemplateCri, ChecklistTemplate>, IChecklistTemplateRepository
    {
        public ChecklistTemplateRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public async override Task<ChecklistTemplate> GetById(object id)
        {
            var template = await dbSet.SingleOrDefaultAsync(x => x.Id == (Int64)id && x.ParentCompanyId == GlobalVariable.COMPANY_ID);
            template.Details = context.ChecklistTemplateDetails.Include(x => x.Checklist).AsNoTracking().Where(x => x.ChecklistTemplateId == template.Id).ToList();
            return template;
        }

        public override void Save(ChecklistTemplate model)
        {
            if (model.Id != 0)
            {
                foreach(var detail in context.ChecklistTemplateDetails.Where(x => x.ChecklistTemplateId == model.Id))
                {
                    context.ChecklistTemplateDetails.Remove(detail);
                }
            }

            foreach (var detail in model.GetDetails())
            {
                context.Entry(detail).State = EntityState.Added;
            }

            base.Save(model);
        }
    }
}
