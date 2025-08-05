using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EngagerMark4.Common.Configs.EntityConfig;

namespace EngagerMark4.ApplicationCore.Entities
{
    public class BlobFile : BaseEntity
    {
        [NotMapped]
        public string CreatedTimeStr
        {
            get
            {
                return Util.ConvertDateToString(Created, EngagerMark4.Common.Configs.DateConfig.CULTURE) + " " + Created.ToShortTimeString();
            }
        }


        [Index("IX_ImageTypeAndReference", 1, IsUnique = false)]
        public EntityType Type
        {
            get;
            set;
        }

        [Index("IX_ImageTypeAndReference", 2, IsUnique = false)]
        public Int64 ReferenceId
        {
            get;
            set;
        }

        [NotMapped]
        public Int32 SecondReferenceId
        {
            get
            {
                return this.Id1;
            }
            set
            {
                this.Id1 = value;
            }
        }

        [StringLength(255)]
        [Required]
        [Index(IsUnique = false)]
        public string FileName
        {
            get;
            set;
        }

        [StringLength(255)]
        public string Name
        {
            get;
            set;
        }

        public Int64 Version
        {
            get;
            set;
        }

        [StringLength(255)]
        public string FileType
        {
            get;
            set;
        }

        [StringLength(255)]
        public string ContentType
        { get; set; }

        [StringLength(255)]
        public string FileExtension
        {
            get;
            set;
        }

        public Int64 FileContentId
        { get; set; }

        [ForeignKey("FileContentId")]
        public BlobFileContent FileContent
        {
            get;
            set;
        }

        [StringLength(255)]
        [Required]
        [Index(IsUnique = false)]
        public string TempFolder
        { get; set; }

        public BlobFileStatus Status
        { get; set; }

        public enum BlobFileStatus
        {
            All = -1,
            Temp,
            Saved
        }

        public void SetReference(Int64 referenceId, EntityType type)
        {
            this.ReferenceId = referenceId;
            this.Type = type;
            this.Status = BlobFileStatus.Saved;
        }
    }
}
