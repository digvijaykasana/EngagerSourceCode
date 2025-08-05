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
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.SOP.Repository.SalesInvoices
{
    public class MonthlyInvoiceReportRepository : GenericRepository<ApplicationDbContext, SalesInvoiceCri, SalesInvoice>, IMonthlyInvoiceReportRepository
    {
        public MonthlyInvoiceReportRepository(ApplicationDbContext aContext) : base(aContext)
        {

        }

        public List<MonthlyInvoiceReportByCompanyViewModel> getInvoices(MonthlyInvoiceReportCri aCri)
        {
            var dateTimeVal = aCri.GetDateTime();

            Int64 customerId = aCri.CustomerId;


            DateTime fromDate = dateTimeVal;

            DateTime toDate = dateTimeVal.AddMonths(1).AddDays(-1);

            var invoices = from invoiceSummary in this.context.SalesInvoiceSummaries.AsNoTracking()
                           join invoiceSummaryDetails in this.context.SalesInvoiceSummaryDetails.AsNoTracking() on invoiceSummary.Id equals invoiceSummaryDetails.SalesInvoiceSummaryId
                           join invoice in this.context.SalesInvoices.AsNoTracking() on invoiceSummaryDetails.SalesInvoiceId equals invoice.Id
                           where invoice.ParentCompanyId == GlobalVariable.COMPANY_ID &&
                           ( DbFunctions.TruncateTime(invoiceSummary.InvoiceDate.Value) >= DbFunctions.TruncateTime(fromDate) 
                           && DbFunctions.TruncateTime(invoiceSummary.InvoiceDate.Value) <= DbFunctions.TruncateTime(toDate))
                           select new MonthlyInvoiceReportByCompanyViewModel
                           {
                               ReferenceNo = invoiceSummary.ReferenceNo,
                               VesselName = invoice.VesselName,
                               InvoiceTotalNetAmount = invoice.TotalNetAmount,
                               CustomerId = invoice.CustomerId
                           };

            invoices = invoices.Where(x => x.CustomerId == customerId);

            return invoices.ToList();
        }
    }
}