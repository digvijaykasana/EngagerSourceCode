using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.Common;
using EngagerMark4.Common.Configs;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EngagerMark4.Common.Configs.EntityConfig;
using System.Data.Entity;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.Entities.Users;

namespace EngagerMark4.Infrasturcture.Repository
{
    public class BlobFileRepository : GenericRepository<ApplicationDbContext, BlobFileCri, BlobFile>, ApplicationCore.IRepository.IBlobFileRepository
    {
        IUserService _userService;

        public BlobFileRepository(ApplicationDbContext aContext, IUserService userService) : base(aContext)
        {
            this._userService = userService;
        }

        public void Delete(int id)
        {
            base.Delete(new BlobFile { Id = id });
        }

        public void DeleteByEntityTypeAndReferenceId(EntityConfig.EntityType entityType, Int64 referenceId, Int64 subReferenceId = 0)
        {
            foreach (var entity in GetAll(new BlobFileCri { Type = entityType, ReferenceId = referenceId, SecondReferenceId = Convert.ToInt32(subReferenceId), Status = BlobFile.BlobFileStatus.All }).ToList())
            {
                this.Delete(new BlobFile { Id = entity.Id });
                var content = context.BlobFileContents.Find(entity.FileContentId);
                this.context.BlobFileContents.Remove(content);
            }
        }

        public void DeleteByEntityTypeAndReferenceIdAndSubReferenceId(EntityType entityType, long? referenceId, int? subReferenceId)
        {
            throw new NotImplementedException();
        }

        public void DeleteByEntityTypeReferenceIdAndFileType(EntityConfig.EntityType entitType, Int64 referenceId, string fileType)
        {
            foreach (var entity in GetAll(new BlobFileCri { Type = entitType, ReferenceId = referenceId, Status = BlobFile.BlobFileStatus.All, FileType = fileType }))
            {
                this.Delete(new BlobFile { Id = entity.Id });
                var content = context.BlobFileContents.Find(entity.FileContentId);
                this.context.BlobFileContents.Remove(content);
            }
        }

        public IEnumerable<BlobFile> GetAll(BlobFileCri aCri)
        {
            aCri = aCri == null ? new BlobFileCri() : aCri;

            IQueryable<BlobFile> entities = null;

            if (aCri.IncludeFilecontent)
                entities = context.BlobFiles.Include(x => x.FileContent);
            else
                entities = context.BlobFiles;

            if (aCri.Type != EntityConfig.EntityType.All)
                entities = entities.Where(x => x.Type == aCri.Type);
            if (aCri.ReferenceId != null)
                entities = entities.Where(x => x.ReferenceId == aCri.ReferenceId);
            if (!string.IsNullOrEmpty(aCri.FileType))
                entities = entities.Where(x => x.FileType == aCri.FileType);
            if (!string.IsNullOrEmpty(aCri.TempFolder))
                entities = entities.Where(x => x.TempFolder.Equals(aCri.TempFolder));
            if (aCri.Status != BlobFile.BlobFileStatus.All)
                entities = entities.Where(x => x.Status == aCri.Status);

            if(aCri.SecondReferenceId > 0)
            {
                entities = entities.Where(x => x.Id1 == aCri.SecondReferenceId);
            }

            return entities;
        }

        public async Task<BlobFile> GetByIdIncluded(Int64 id)
        {
            try
            {
                return await context.BlobFiles.Include(x => x.FileContent).SingleOrDefaultAsync(x => x.Id == id);
            }
            catch(Exception ex)
            {
                return null;
            }

        }

        public BlobFile GetByReferenceId(Int64 referenceId, EntityConfig.EntityType entityType, Int64 subReferenceId = 0)
        {
            var queryable = context.BlobFiles.Include(x => x.FileContent).Where(x => x.ReferenceId == referenceId && x.Type == entityType);

            if(subReferenceId > 0)
            {
                var secondReferenceId = Convert.ToInt32(subReferenceId);

                queryable = queryable.Where(x => x.Id1 == secondReferenceId);
            }

            return queryable.SingleOrDefault();
        }

        public IEnumerable<BlobFile> GetListByReferencedId(long referenceId, EntityType entityType)
        {
            throw new NotImplementedException();
        }

        public void Save(BlobFile entity)
        {

            if (entity.ReferenceId != 0)
            {
                List<BlobFile> duplicateFiles = context.BlobFiles.Where(x => x.Type == entity.Type && x.FileName.Equals(entity.FileName) && x.ReferenceId == entity.ReferenceId).ToList();
                if (duplicateFiles.Count > 0)
                {
                    try
                    {
                        entity.Version = duplicateFiles.Max(x => x.Version) + 1;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                IEnumerable<BlobFile> duplicateFiles = context.BlobFiles.Where(x => x.FileName.Equals(entity.FileName) && x.TempFolder.Equals(entity.TempFolder));
                if (duplicateFiles != null)
                {
                    try
                    {
                        entity.Version = duplicateFiles.Max(x => x.Version) + 1;
                    }
                    catch
                    {

                    }
                }
            }
            base.Save(entity);
        }

        public void SaveForMobile(BlobFile entity)
        {
            context.userId = entity.CreatedBy;
            userId = entity.CreatedBy;

            User user = _userService.GetByApplicatioNId(context.userId);

            userName = user.FirstName + " " + user.LastName;


            if (entity.ReferenceId != 0)
            {
                List<BlobFile> duplicateFiles = context.BlobFiles.Where(x => x.Type == entity.Type && x.FileName.Equals(entity.FileName) && x.ReferenceId == entity.ReferenceId).ToList();
                if (duplicateFiles.Count > 0)
                {
                    try
                    {
                        entity.Version = duplicateFiles.Max(x => x.Version) + 1;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                IEnumerable<BlobFile> duplicateFiles = context.BlobFiles.Where(x => x.FileName.Equals(entity.FileName) && x.TempFolder.Equals(entity.TempFolder));
                if (duplicateFiles != null)
                {
                    try
                    {
                        entity.Version = duplicateFiles.Max(x => x.Version) + 1;
                    }
                    catch
                    {

                    }
                }
            }
            base.Save(entity);
        }

    }
}
