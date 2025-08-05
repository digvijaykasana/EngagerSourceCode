using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice;

namespace EngagerMark4.ApplicationCore.SOP.Cris
{
    public class SalesInvoiceCri : BaseCri
    {
        public Int64 CustomerId
        {
            get;
            set;
        }

        public Int64 VesselId
        {
            get;
            set;
        }

        public DateTime? FromDate
        {
            get;
            set;
        }

        public DateTime? ToDate
        {
            get;
            set;
        }

        [Display(Name = "Invoice No.")]
        public string SalesInvoiceSummaryNo
        {
            get;
            set;
        }

        public string InvoiceDate
        {
            get;
            set;
        }

        public string DNNo
        {
            get;
            set;
        }

        public SalesInvoiceStatus Status
        {
            get; set;
        } = SalesInvoiceStatus.All;

        public Action ActionThing
        {
            get;
            set;
        }

        public enum Action
        {
            [Display(Name = "Generate and Download Taxable Invoice")]
            TaxableInvoice,
            [Display(Name = "Generate and Download Nontaxable Invoice")]
            NonTaxableInvoice
        }

        public static List<CommonConfiguration> GetInvoiceStatus()
        {
            List<CommonConfiguration> configurations = new List<CommonConfiguration>();
            CommonConfiguration all = new CommonConfiguration { Id = (int)SalesInvoice.SalesInvoiceStatus.All, Name = SalesInvoice.SalesInvoiceStatus.All.ToString() };
            configurations.Add(all);
            CommonConfiguration draft = new CommonConfiguration { Id = (int)SalesInvoice.SalesInvoiceStatus.Draft, Name = SalesInvoice.SalesInvoiceStatus.Draft.ToString() };
            configurations.Add(draft);
            CommonConfiguration billed = new CommonConfiguration { Id = (int)SalesInvoice.SalesInvoiceStatus.Billed, Name = SalesInvoice.SalesInvoiceStatus.Billed.ToString() };
            configurations.Add(billed);
            return configurations;
        }
    }
}
