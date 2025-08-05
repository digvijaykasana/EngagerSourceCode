using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Common.Configs
{
    public class General
    {
        public Int64 Id
        {
            get;
            set;
        }

        public Int64 UserId
        {
            get;
            set;
        }

        public Int64 ParentCompanyId
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public string CurrentUserName
        {
            get;
            set;
        } = "";
    }
}
