using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EngagerMark4.Models
{
    public class TestCombineViewModel
    {
        public Customer Customer
        {
            get;
            set;
        }

        public ServiceJob ServiceJob
        {
            get;
            set;
        }
    }

}