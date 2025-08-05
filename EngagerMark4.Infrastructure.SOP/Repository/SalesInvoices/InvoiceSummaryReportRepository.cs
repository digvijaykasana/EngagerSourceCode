using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using EngagerMark4.Common;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.SOP.Repository.SalesInvoices
{
    public class InvoiceSummaryReportRepository : GenericRepository<ApplicationDbContext, SalesInvoiceCri, SalesInvoice>, IInvoiceSummaryReportRepository
    {
        public InvoiceSummaryReportRepository(ApplicationDbContext aContext) : base(aContext)
        {

        }

        public List<InvoiceSummaryReportViewModel> getInvoices(InvoiceSummaryReportCri aCri)
        {
            DateTime fromDate = Util.ConvertStringToDateTime(aCri.FromDate, DateConfig.CULTURE);
            TimeSpan ts = new TimeSpan(0, 0, 0);
            fromDate = fromDate.Date + ts;

            DateTime toDate = Util.ConvertStringToDateTime(aCri.ToDate, DateConfig.CULTURE);
            TimeSpan newts = new TimeSpan(23, 59, 59);
            toDate = toDate.Date + newts;

            var invoices = from invoiceSummary in this.context.SalesInvoiceSummaries.AsNoTracking()
                           join invoiceSummaryDetails in this.context.SalesInvoiceSummaryDetails.AsNoTracking() on invoiceSummary.Id equals invoiceSummaryDetails.SalesInvoiceSummaryId
                           join invoice in this.context.SalesInvoices.AsNoTracking() on invoiceSummaryDetails.SalesInvoiceId equals invoice.Id
                           join creditNote in this.context.CreditNotes.AsNoTracking() on invoice.WorkOrderId equals creditNote.OrderId
                           join customer in this.context.Customers.AsNoTracking() on invoice.CustomerId equals customer.Id
                           where invoice.ParentCompanyId == GlobalVariable.COMPANY_ID && 
                           (invoiceSummary.InvoiceDate >= fromDate && invoiceSummary.InvoiceDate <= toDate)
                           select new InvoiceSummaryReportViewModel
                           {
                               InvoiceDate = invoice.Created,
                               ReferenceNo = invoice.ShortText1,
                               CustomerName = customer.Name,
                               TotalAmt = invoice.TotalAmt,
                               GSTAmount = invoice.GSTAmount,
                               InvoiceTotalNetAmount = invoice.TotalNetAmount,
                               cnTotalAmt = creditNote.SubTotal,
                               cnGSTAmount = creditNote.GSTAmount,
                               cnGrandTotalAmount = creditNote.GrandTotal
                           };

            return invoices.ToList();
        }
    }
}

//

