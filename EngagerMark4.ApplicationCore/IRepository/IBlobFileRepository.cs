using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EngagerMark4.Common.Configs.EntityConfig;

namespace EngagerMark4.ApplicationCore.IRepository
{
    public interface IBlobFileRepository : IBaseRepository<BlobFileCri, BlobFile>
    {
        IEnumerable<BlobFile> GetAll(BlobFileCri aCri);

        void DeleteByEntityTypeAndReferenceId(EntityType entityType, Int64 referenceId, Int64 subReferenceId = 0);

        void DeleteByEntityTypeAndReferenceIdAndSubReferenceId(EntityType entityType, Int64? referenceId, int? subReferenceId);

        void DeleteByEntityTypeReferenceIdAndFileType(EntityType entitType, Int64 referenceId, string fileType);

        BlobFile GetByReferenceId(Int64 referenceId, EntityType entityType, Int64 subReferenceId = 0);

        IEnumerable<BlobFile> GetListByReferencedId(Int64 referenceId, EntityType entityType);

        Task<BlobFile> GetByIdIncluded(Int64 id);

        void SaveForMobile(BlobFile entity);
    }
}
