using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service
{
    public abstract class AbstractService<Repository, Cri, Entity>
        where Cri : BaseCri
        where Entity : BasicEntity
        where Repository : IBaseRepository<Cri, Entity>
    {
        protected Repository repository;

        public AbstractService(Repository repository)
        {
            this.repository = repository;
        }

        public async virtual Task<IEnumerable<Entity>> GetByCri(Cri aCri)
        {
            return await this.repository.GetByCri(aCri);
        }

        public async virtual Task<Entity> GetById(object id)
        {
            return await this.repository.GetById(id);
        }

        public async virtual Task<Int64> Save(Entity entity)
        {
            this.repository.Save(entity);
            await this.repository.SaveChangesAsync();
            return entity.Id;
        }

        public async virtual Task<Int64> SaveWithGraph(Entity entity)
        {
            this.repository.ExecuteWithGraph(entity);
            await this.repository.SaveChangesAsync();
            return entity.Id;
        }

        public async virtual Task<bool> Delete(Entity entity)
        {
            this.repository.Delete(entity);
            await this.repository.SaveChangesAsync();
            return true;
        }
    }
}
