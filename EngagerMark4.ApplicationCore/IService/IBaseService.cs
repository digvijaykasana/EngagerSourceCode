using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IService
{
    public interface IBaseService<Cri,Entity>
        where Entity : BasicEntity
        where Cri: BaseCri
    {
        Task<IEnumerable<Entity>> GetByCri(Cri aCri);

        Task<Entity> GetById(object id);

        Task<Int64> Save(Entity entity);

        Task<Int64> SaveWithGraph(Entity entity);

        Task<bool> Delete(Entity entity);
    }
}
