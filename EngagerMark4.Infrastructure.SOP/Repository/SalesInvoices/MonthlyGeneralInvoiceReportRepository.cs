using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using EngagerMark4.Common;
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
    public class MonthlyGeneralInvoiceReportRepository : GenericRepository<ApplicationDbContext, SalesInvoiceCri, SalesInvoice>, IMonthlyGeneralInvoiceReportRepository
    {
        public MonthlyGeneralInvoiceReportRepository(ApplicationDbContext aContext) : base(aContext)
        {

        }

        public IEnumerable<MonthlyGeneralInvoiceReportViewModel> GetReport(MonthlyGeneralInvoiceReportCri aCri)
        {
            var dateTimeVal = aCri.GetFromDateTime();

            DateTime fromDate = dateTimeVal;

            DateTime toDate = dateTimeVal.AddMonths(1);

            string query = "SELECT Inv.Id SalesInvoiceSummaryId, " +
                            "Max(Inv.InvoiceDate) InvoiceDate, " +
                            "Max(Inv.ReferenceNo) ReferenceNo, " +
                            "Max(Cus.Name) CustomerName, " +
                            "SUM(InvDetails.TotalAmt) Price, " +
                            "SUM(InvDetails.TotalNonTaxable) NonTaxableAmount," +
                            "SUM(InvDetails.GSTAmount) GST, " +
                            "SUM(InvDetails.TotalNetAmount) InvoiceTotalNetAmount, " +
                            "SUM(InvDetails.DiscountAmount) CNAmount," +
                            "SUM(InvDetails.DiscountAmount * (InvDetails.GSTPercent / 100)) CNGST, " +
                            "SUM(InvDetails.DiscountAmount + (InvDetails.DiscountAmount * (InvDetails.GSTPercent / 100))) CreditNoteTotalAmount " +
                            "FROM SOP.Tb_SalesInvoice InvDetails " +
                            "INNER JOIN SOP.Tb_SalesInvoiceSummaryDetails InvSummaryDetails ON InvDetails.Id = InvSummaryDetails.SalesInvoiceId " +
                            "INNER JOIN SOP.Tb_SalesInvoiceSummary Inv ON InvSummaryDetails.SalesInvoiceSummaryId = Inv.Id " +
                            "INNER JOIN Customer.Tb_Customer Cus ON Inv.CustomerId = Cus.Id " +
                            "WHERE Inv.InvoiceDate >= '" + fromDate.Date.Year + fromDate.Date.Month.ToString("00") + fromDate.Date.Day.ToString("00") + "' " +
                            "AND Inv.InvoiceDate <'" + toDate.Date.Year + toDate.Date.Month.ToString("00") + toDate.Date.Day.ToString("00") + "' " +
                            "GROUP BY Inv.Id";

            var invoices = context.Database.SqlQuery<MonthlyGeneralInvoiceReportViewModel>(query);

            //var tempInvoices = from invoiceDetails in this.context.SalesInvoices.AsNoTracking()
            //                   join invoicesummaries in this.context.SalesInvoiceSummaries.AsNoTracking() on invoiceDetails.SalesInvoiceSummaryId equals invoicesummaries.Id
            //                   join customers in this.context.Customers.AsNoTracking() on invoiceDetails.CustomerId equals customers.Id
            //                   where DbFunctions.TruncateTime(invoicesummaries.InvoiceDate.Value) >= DbFunctions.TruncateTime(fromDate.Date) &&
            //                   DbFunctions.TruncateTime(invoicesummaries.InvoiceDate.Value) <= DbFunctions.TruncateTime(toDate.Date)
            //                   select new MonthlyGeneralInvoiceReportViewModel
            //                   {
            //                       SalesInvoiceSummaryId = invoicesummaries.Id,
            //                       InvoiceDate = invoicesummaries.InvoiceDate,
            //                       ReferenceNo = invoicesummaries.ReferenceNo,
            //                       CustomerName = customers.Name,
            //                       Price = invoiceDetails.TotalAmt,
            //                       NonTaxableAmount = invoiceDetails.TotalNonTaxable,
            //                       GST = invoiceDetails.GSTAmount,
            //                       InvoiceTotalNetAmount = invoiceDetails.TotalNetAmount,
            //                       CNAmount = invoiceDetails.DiscountAmount,
            //                       CNGST = invoiceDetails.DiscountAmount * (invoiceDetails.GSTPercent / 100),
            //                       CreditNoteTotalAmount = invoiceDetails.DiscountAmount + (invoiceDetails.DiscountAmount * (invoiceDetails.GSTPercent / 100))
            //                   };

            //var invoices = from ti in tempInvoices
            //               group ti by ti.SalesInvoiceSummaryId into i
            //               select new MonthlyGeneralInvoiceReportViewModel()
            //               {
            //                   SalesInvoiceSummaryId = i.FirstOrDefault().SalesInvoiceSummaryId,
            //                   InvoiceDate = i.FirstOrDefault().InvoiceDate,
            //                   ReferenceNo = i.FirstOrDefault().ReferenceNo,
            //                   CustomerName = i.FirstOrDefault().CustomerName,
            //                   Price = i.Sum(x => x.Price),
            //                   NonTaxableAmount = i.Sum(x => x.NonTaxableAmount),
            //                   GST = i.Sum(x => x.GST),
            //                   InvoiceTotalNetAmount = i.Sum(x=>x.InvoiceTotalNetAmount),
            //                   CNAmount = i.Sum(x=> x.CNAmount),
            //                   CNGST = i.Sum(x=>x.CNGST),
            //                   CreditNoteTotalAmount = i.Sum(x=>x.CreditNoteTotalAmount)
            //               };

            var invoiceList = invoices.ToList();

            var invoiceIdList = invoiceList.Select(x => x.SalesInvoiceSummaryId).ToList();

            var creditnoteSummaryDetailsList = (from creditNoteSummaryDetails in this.context.CreditNoteSummaryDetails.Include(x => x.CreditNoteSummary.Customer).AsNoTracking()
                                                where invoiceIdList.Contains(creditNoteSummaryDetails.SalesInvoiceSummaryId)
                                                select creditNoteSummaryDetails).ToList();

            var creditNoteSummaryIds = creditnoteSummaryDetailsList.Select(x => x.CreditNoteSummaryId).Distinct().ToList();

            foreach(var creditNoteSummaryId in creditNoteSummaryIds)
            {
                var invoiceSummaryIds = creditnoteSummaryDetailsList.Where(x => x.CreditNoteSummaryId == creditNoteSummaryId).Select(x => x.SalesInvoiceSummaryId).Distinct().ToList();

                var creditNoteSummary = creditnoteSummaryDetailsList.FirstOrDefault(x => x.CreditNoteSummaryId == creditNoteSummaryId).CreditNoteSummary;

                MonthlyGeneralInvoiceReportViewModel viewModel = new MonthlyGeneralInvoiceReportViewModel
                {
                    InvoiceDate = creditNoteSummary.CreditNoteDate,
                    ReferenceNo = creditNoteSummary.ReferenceNo,
                    CustomerName = creditNoteSummary.Customer.Name,
                    Price = 0,
                    NonTaxableAmount = 0,
                    GST = 0,
                };
                viewModel.CNAmount = invoiceList.Where(x => invoiceSummaryIds.Contains(x.SalesInvoiceSummaryId)).Sum(x => x.CNAmount);
                viewModel.CNGST = invoiceList.Where(x => invoiceSummaryIds.Contains(x.SalesInvoiceSummaryId)).Sum(x => x.CNGST);
                viewModel.CreditNoteTotalAmount = invoiceList.Where(x => invoiceSummaryIds.Contains(x.SalesInvoiceSummaryId)).Sum(x => x.CreditNoteTotalAmount);

                
                var lastIndex = 0;
                foreach(var invoice in invoiceList.Where(x => invoiceSummaryIds.Contains(x.SalesInvoiceSummaryId)))
                {
                    invoice.CNAmount = 0;
                    invoice.CNGST = 0;
                    invoice.CreditNoteTotalAmount = 0;
                    lastIndex = invoiceList.FindIndex(x => x.SalesInvoiceSummaryId == invoice.SalesInvoiceSummaryId);
                }
                lastIndex++;
                invoiceList.Insert(lastIndex, viewModel);
            }

            return invoiceList;
        }
    }
}
