using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Cris
{
    public class BlobFileCri : BaseCri
    {
        public int? Id
        { get; set; }

        public EntityConfig.EntityType Type
        { get; set; }

        public Int64? ReferenceId
        { get; set; }

        public string FileType
        {
            get; set;
        }

        public string TempFolder
        { get; set; }

        public BlobFile.BlobFileStatus Status
        { get; set; }

        public bool IncludeFilecontent
        { get; set; }
        
        public int SecondReferenceId
        {
            get; set;
        }
    }
}
