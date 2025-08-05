using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IService
{
    public interface IBlobFileService : IBaseService<BlobFileCri, BlobFile>
    {
        /// <summary>
        /// Delete the blob file by entity type and referenceId
        /// Created by: Kyaw Min Htut
        /// Created date: 31/08/2016
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="referenceId"></param>
        void DeleteByEntityTypeAndReferenceId(EntityConfig.EntityType entityType, Int64 referenceId, Int64 subReferenceId = 0);

        /// <summary>
        /// Delete the blob file by entity type, referenceId and file type
        /// Created by: Kyaw Min Htut
        /// Created date: 31/08/2016
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="referenceId"></param>
        void DeleteByEntityTypeReferenceIdAndFileType(EntityConfig.EntityType entitType, Int64 referenceId, string fileType);


        /// <summary>
        /// Get BlobFile by referenceId.
        /// Created By : Myint Kyaw
        /// Created Date : 31st Jan 2017
        /// </summary>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        BlobFile GetByReferenceId(Int64 referenceId, EntityConfig.EntityType entityType, Int64 subReferenceId = 0);

        IEnumerable<BlobFile> GetListByReferenceId(Int64 referenceId, EntityConfig.EntityType entityType);

        Task<Int64> SaveForMobile(BlobFile entity);
    }
}
