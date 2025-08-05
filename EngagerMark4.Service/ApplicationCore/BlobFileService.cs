using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngagerMark4.Common.Configs;

namespace EngagerMark4.Service.ApplicationCore
{
    public class BlobFileService : AbstractService<IBlobFileRepository, BlobFileCri, BlobFile>, IBlobFileService
    {
        public async override Task<BlobFile> GetById(object id)
        {
            try
            {
                Int64 fileId = Convert.ToInt64(id);

                return await repository.GetByIdIncluded(fileId);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public BlobFileService(IBlobFileRepository repository) : base(repository)
        {
        }

        

        public void DeleteByEntityTypeAndReferenceId(EntityConfig.EntityType entityType, Int64 referenceId, Int64 subReferenceId = 0)
        {
            this.repository.DeleteByEntityTypeAndReferenceId(entityType, referenceId, subReferenceId);
            this.repository.SaveChanges();
        }

        public void DeleteByEntityTypeReferenceIdAndFileType(EntityConfig.EntityType entitType, Int64 referenceId, string fileType)
        {
            this.repository.DeleteByEntityTypeReferenceIdAndFileType(entitType, referenceId, fileType);
            this.repository.SaveChanges();
        }

        public BlobFile GetByReferenceId(Int64 referenceId, EntityConfig.EntityType entityType, Int64 subReferenceId = 0)
        {
            return this.repository.GetByReferenceId(referenceId, entityType, subReferenceId);
        }

        public IEnumerable<BlobFile> GetListByReferenceId(long referenceId, EntityConfig.EntityType entityType)
        {
            return this.repository.GetListByReferencedId(referenceId, entityType);
        }

        public async Task<Int64> SaveForMobile(BlobFile entity)
        {
            this.repository.SaveForMobile(entity);
            await this.repository.SaveChangesAsync();
            return entity.Id;
        }

    }
}
