using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IRepository
{
    public interface IBaseRepository<Cri, Entity>
        where Entity : BasicEntity
        where Cri : BaseCri
    {
        Task<IEnumerable<Entity>> GetByCri(Cri acri);

        Task<Entity> GetById(object id);

        void Save(Entity entity);

        void ExecuteWithGraph(Entity entity);

        void Delete(Entity entity);

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();

        void DisposeTransaction();

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
