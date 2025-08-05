using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities
{
    [Serializable]
    public abstract class SerialNo : BasicEntity
    {
        [Index("CombineKey", 1, IsUnique = true)]
        public Int64 CompanyId
        {
            get;
            set;
        }

        [Index("CombineKey", 2, IsUnique = true)]
        public int Year
        {
            get;
            set;
        }

        [Index("CombineKey", 3, IsUnique = true)]
        public int Month
        {
            get;
            set;
        }

        [Index("CombineKey", 4, IsUnique = true)]
        public int Day
        {
            get;
            set;
        }

        [Index("CombineKey", 5, IsUnique = true)]
        public Int64 RunningNo
        {
            get;
            set;
        }

        [Index(IsUnique = true)]
        public Int64 ReferenceId
        {
            get;
            set;
        }
    }
}
