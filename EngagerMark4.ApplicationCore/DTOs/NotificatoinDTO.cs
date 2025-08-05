using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.DTOs
{
    public class NotificationDTO
    {
        public Int64 Id
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string DetailedDescription
        {
            get; set;
        }

        public Int64 ReferenceId
        {
            get;
            set;
        }
    }
}
