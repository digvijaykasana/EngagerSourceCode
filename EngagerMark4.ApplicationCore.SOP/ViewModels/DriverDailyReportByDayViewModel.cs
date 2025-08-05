using EngagerMark4.ApplicationCore.DTOs;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.ReportViewModels
{
    public class DriverDailyReportByDayViewModel
    {
        [Display(Name = "DATE")]
        public string DateStr { get; set; } = string.Empty;


        [Display(Name = "Trip")]
        public string TripFeesStr
        {
            get
            {
                if(TripFees == 0) return string.Empty;

                return this.TripFees.ToString("0.00");
            }
        }
        public decimal TripFees { get; set; }

        [Display(Name = "MS")]
        public string MSFeesStr
        {
            get
            {
                if (MSFees == 0) return string.Empty;

                return this.MSFees.ToString("0.00");
            }
        }
        public decimal MSFees { get; set; }

        [Display(Name = "Parking Fees")]
        public string ParkFeesStr
        {
            get
            {
                if (ParkFees == 0) return string.Empty;

                return this.ParkFees.ToString("0.00");
            }
        }
        public decimal ParkFees { get; set; }


        [Display(Name = "ERP")]
        public string ERPFeesStr
        {
            get
            {
                if (ERPFees == 0) return string.Empty;

                return this.ERPFees.ToString("0.00");
            }
        }
        public decimal ERPFees { get; set; }


        [Display(Name = "Meal")]
        public string MealFeesStr
        {
            get
            {
                if (MealFees == 0) return string.Empty;

                return this.MealFees.ToString("0.00");
            }
        }
        public decimal MealFees { get; set; }

        [Display(Name = "NEA Cert.")]
        public string NEACertFeesStr
        {
            get
            {
                if (NEACertFees == 0) return string.Empty;

                return this.NEACertFees.ToString("0.00");
            }
        }
        public decimal NEACertFees { get; set; }

        [Display(Name = "Ferry Ticket")]
        public string FerryTicketFeesStr
        {
            get
            {
                if (FerryTicketFees == 0) return string.Empty;

                return this.FerryTicketFees.ToString("0.00");
            }
        }
        public decimal FerryTicketFees { get; set; }


        [Display(Name = "JP Pass Fee")]
        public string JPPassFeesStr
        {
            get
            {
                if (JPPassFees == 0) return string.Empty;

                return this.JPPassFees.ToString("0.00");
            }
        }
        public decimal JPPassFees { get; set; }


        [Display(Name = "PSA Pass Fee")]
        public string PSAPassFeesStr
        {
            get
            {
                if (PSAPassFees == 0) return string.Empty;

                return this.PSAPassFees.ToString("0.00");
            }
        }
        public decimal PSAPassFees { get; set; }


        [Display(Name = "Others")]
        public string OtherFeesStr
        {
            get
            {
                if (OtherFees == 0) return string.Empty;

                return this.OtherFees.ToString("0.00");
            }
        }
        public decimal OtherFees { get; set; }
    }
}
