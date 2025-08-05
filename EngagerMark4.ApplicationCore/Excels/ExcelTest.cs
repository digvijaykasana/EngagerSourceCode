using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Excels
{
    public class ExcelTest
    {
        [Display(Name ="Name")]
        public string Column1
        {
            get;
            set;
        }

        [Display(Name = "Value")]
        public decimal Column2
        {
            get;
            set;
        }
    }
}
