using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.ViewModels
{
    public class DriverVariableSalaryReportViewModel
    {
        [Display(Name = "EmpID")]
        public string EmployeeId
        {
            get
            {
                if(!string.IsNullOrEmpty(DriverNRIC))
                {
                    return DriverNRIC;
                }
                else
                {
                    return DriverName;
                }
            }
        }

        public Int64 DriverId { get; set; }
        public string DriverName { get; set; }
        public string DriverNRIC { get; set; }

        [Display(Name = "Pay Item")]
        public string PayItemCatStr { get; set; }
        public string PayItemCat { get; set; }

        [Display(Name = "Amount")]
        public string AmountStr 
        {
            get
            {
                return this.Amount.ToString("0.00");
            }
        
        }

        public decimal Amount { get; set; }
        public DateTime? PickUpDate
        {
            get;
            set;
        }

        public long ServiceJobId { get; set; }
        public decimal Salary { get; set; }

        public int SerialNo { get; set; }

    }
}
