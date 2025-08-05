using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.ApplicationCore.Configurations
{
    public class LetterheadService : AbstractService<ILetterheadRepository, LetterheadCri, Letterhead>, ILetterheadService
    {
        public LetterheadService(ILetterheadRepository repository) : base(repository)
        { 
        }

        public Letterhead GetDefaultLetterhead()
        {
            return this.repository.GetDefaultLetterhead();
        }

        public async override Task<Int64> Save(Letterhead entity)
        {
            this.repository.Save(entity);
            await this.repository.SaveChangesAsync();

            return entity.Id;
        }
    }
}
