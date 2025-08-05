using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EngagerMark4.ApplicationCore.Entities.Configurations.Letterhead;

namespace EngagerMark4.ApplicationCore.Cris.Configurations
{
    public class LetterheadCri : BaseCri
    {
        public string SearchValue
        {
            get;
            set;
        }

        public int Type
        {
            get;
            set;
        } = -1;
    }
}
