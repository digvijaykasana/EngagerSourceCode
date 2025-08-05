using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities
{
    [Serializable]
    public abstract class BasicEntity
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public virtual Int64 Id
        {
            get;
            set;
        }

        [Index(IsUnique = false)]
        [Required]
        public DateTime Created
        {
            get;
            set;
        }

        public string CreatedStr
        {
            get
            {   
                return Util.ConvertDateToString(Created, DateConfig.CULTURE);
            }
        }

        public string CreatedBy
        {
            get;
            set;
        }

        [StringLength(255)]
        public string CreatedByName
        {
            get;
            set;
        }

        [StringLength(255)]
        public string ModifiedByName
        {
            get;
            set;
        }

        [NotMapped]
        public string ModifiedStr
        {
            get
            {
                if(!(Modified == null))
                {
                    return Util.ConvertDateToString(Modified.Value, DateConfig.CULTURE);
                }
                else
                {
                    return "";
                }
            }
        }

        public DateTime? Modified
        {
            get;
            set;
        }

        public string ModifiedBy
        {
            get;
            set;
        }

        [Index(IsUnique = false)]
        public virtual Int64 ParentCompanyId
        {
            get;
            set;
        }

        public BaseEntity.ModelState State
        {
            get;
            set;
        }

        [NotMapped]
        public virtual string SearchDescription
        {
            get
            { return ""; }
        }
    }
}
