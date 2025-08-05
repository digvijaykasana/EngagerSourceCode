using EngagerMark4.ApplicationCore.Entities.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Caches
{
    public class Cache
    {
        public static Dictionary<string, List<CommonConfiguration>> CommonConfigurations
        {
            get;
            set;
        } = new Dictionary<string, List<CommonConfiguration>>();
    }
}
