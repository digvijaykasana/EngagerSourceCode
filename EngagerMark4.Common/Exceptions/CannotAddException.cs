using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Common.Exceptions
{
    public class CannotAddException : Exception
    {
        public CannotAddException(string aMessage) : base(aMessage)
        {

        }
    }
}
