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
    /// <summary>
    /// FUJI XEROX INTERNAL USE ONLY<<RESTRICTED>>
    /// Disclose to : PSTG T&T Team
    /// Protected until:
    /// Author: Kyaw Min Htut
    /// Prepared on: 2-12-2015
    /// </summary>
    [Serializable]
    public abstract class BaseEntity : BasicEntity
    {
        public virtual string CreatedDateStr
        {
            get
            {
                return Util.ConvertDateToString(Created, DateConfig.CULTURE) + " " + this.Created.ToShortTimeString();
            }
        }


        [StringLength(255)]
        public string ShortText1
        {
            get;
            set;
        }

        [StringLength(255)]
        public string ShortText2
        {
            get;
            set;
        }

        [StringLength(255)]
        public string ShortText3
        {
            get;
            set;
        }

        [StringLength(255)]
        public string ShortText4
        {
            get;
            set;
        }

        [StringLength(255)]
        public string ShortText5
        {
            get;
            set;
        }

        [StringLength(255)]
        public string ShortText6
        {
            get;
            set;
        }

        [StringLength(255)]
        public string ShortText7
        {
            get;
            set;
        }

        [StringLength(255)]
        public string ShortText8
        {
            get;
            set;
        }

        [StringLength(255)]
        public string ShortText9
        {
            get;
            set;
        }

        [StringLength(255)]
        public string ShortText10
        {
            get;
            set;
        }

        public string LongText1
        {
            get;
            set;
        }

        public string LongText2
        {
            get;
            set;
        }

        public string LongText3
        {
            get;
            set;
        }

        public string LongText4
        {
            get;
            set;
        }

        public string LongText5
        {
            get;
            set;
        }

        public string LongText6
        {
            get;
            set;
        }

        public string LongText7
        {
            get;
            set;
        }

        public string LongText8
        {
            get;
            set;
        }

        public string LongText9
        {
            get;
            set;
        }

        public string LongText10
        {
            get;
            set;
        }

        public decimal Num1
        {
            get;
            set;
        }

        public decimal Num2
        {
            get;
            set;
        }

        public decimal Num3
        {
            get;
            set;
        }

        public decimal Num4
        {
            get;
            set;
        }

        public decimal Num5
        {
            get;
            set;
        }

        public decimal Num6
        {
            get;
            set;
        }

        public decimal Num7
        {
            get;
            set;
        }

        public decimal Num8
        {
            get;
            set;
        }

        public decimal Num9
        {
            get;
            set;
        }

        public decimal Num10
        {
            get;
            set;
        }

        public bool YesNo1
        {
            get;
            set;
        }

        public bool YesNo2
        {
            get;
            set;
        }

        public bool YesNo3
        {
            get;
            set;
        }

        public bool YesNo4
        {
            get;
            set;
        }

        public bool YesNo5
        {
            get;
            set;
        }

        public bool YesNo6
        {
            get;
            set;
        }

        public bool YesNo7
        {
            get;
            set;
        }

        public bool YesNo8
        {
            get;
            set;
        }

        public bool YesNo9
        {
            get;
            set;
        }

        public bool YesNo10
        {
            get;
            set;
        }

        public int Id1
        {
            get;
            set;
        }

        public int Id2
        {
            get;
            set;
        }

        public int Id3
        {
            get;
            set;
        }

        public int Id4
        {
            get;
            set;
        }

        public int Id5
        {
            get;
            set;
        }

        public int Id6
        {
            get;
            set;
        }

        public int Id7
        {
            get;
            set;
        }

        public int Id8
        {
            get;
            set;
        }

        public int Id9
        {
            get;
            set;
        }

        public int Id10
        {
            get;
            set;
        }

        public enum ModelState
        {
            Added,
            Modified,
            Deleted,
            UnChanged
        }
    }
}
