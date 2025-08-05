using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Common.Utilities
{
    public class KeyUtil
    {
        public static string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
