using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.ViewModels
{
    public class MonthlyInvoiceReportByCompanyViewModel
    {
        [Display(Name = "Invoice No.")]
        public string ReferenceNo { get; set; }

        [Display(Name = "Vessel Name")]
        public string VesselName { get; set; }

        [Display(Name = "Total Amount")]
        public decimal InvoiceTotalNetAmount { get; set; } = 0;

        public Int64 CustomerId { get; set; }
    }
}
