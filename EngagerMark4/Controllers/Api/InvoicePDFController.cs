using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.ReportViewModels;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.Common;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.DocumentProcessor;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api
{
    public class InvoicePDFController : BaseController<WorkOrderCri, EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders.WorkOrder, IWorkOrderService>
    {
        ICompanyService _companyService;
        ISalesInvoiceService _invoiceService;
        ICustomerService _customerService;
        IGSTService _gstService;
        ISalesInvoiceSummaryService _salesInvoiceSummaryService;
        ApplicationDbContext db;

        public InvoicePDFController(IWorkOrderService service,
            ICompanyService companyService,
            ISalesInvoiceService salesInvoiceService, 
            ICustomerService customerService,
            IGSTService gstService,
            ISalesInvoiceSummaryService salesInvoiceSummaryService,
            ApplicationDbContext db) : base(service)
        {
            this._companyService = companyService;
            this._invoiceService = salesInvoiceService;
            this._customerService = customerService;
            this._gstService = gstService;
            this._salesInvoiceSummaryService = salesInvoiceSummaryService;
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult> Download(Int64[] invoiceIds, bool TaxInclude = true, CommonEnumConfig.DownloadFileFormat DownloadFileFormat = CommonEnumConfig.DownloadFileFormat.PDF, string SalesInvoiceSummaryNo = "", string DNNo = "",string InvoiceDate="")
        {
            InvoiceReportViewModel report = new InvoiceReportViewModel();

            report.IsTaxInvoice = TaxInclude;

            Int64 randomNumber = Util.GenerateRandomNumber();

            SalesInvoiceSummary salesInvoiceSummary = new SalesInvoiceSummary();

            salesInvoiceSummary.InvoiceDate = string.IsNullOrEmpty(InvoiceDate) ? TimeUtil.GetLocalTime() : Util.ConvertStringToDateTime(InvoiceDate, DateConfig.CULTURE);

            report.Date = salesInvoiceSummary.InvoiceDate.Value;

            var company = await _companyService.GetById(GlobalVariable.COMPANY_ID);

            if (company == null) company = new ApplicationCore.Entities.Company();

            foreach (var invoiceId in invoiceIds)
            {
                SalesInvoiceSummaryDetails salesInvoiceSummaryDetails = new SalesInvoiceSummaryDetails();
                salesInvoiceSummaryDetails.SalesInvoiceId = invoiceId;
                var invoice = await this._invoiceService.GetById(invoiceId);

                if (invoice == null) continue;

                var workorder = await this._service.GetById(invoice.WorkOrderId);

                if (workorder == null) workorder = new ApplicationCore.SOP.Entities.WorkOrders.WorkOrder();

                salesInvoiceSummaryDetails.WorkOrderId = workorder.Id;
                salesInvoiceSummary.Details.Add(salesInvoiceSummaryDetails);

                if (workorder == null) return HttpNotFound();
                var customer = await _customerService.GetHeaderOnly(workorder.CustomerId.Value);

                var workOrderPassenger = workorder.WorkOrderPassengerList.FirstOrDefault(x => x.InCharge == true);

                if (workOrderPassenger == null) workOrderPassenger = new ApplicationCore.SOP.Entities.WorkOrders.WorkOrderPassenger();

                report.HeaderLogo = company.ReportHeaderLogo;
                report.Customer = customer.Name;

                report.Address = invoice.CompanyAddress;
                
                report.Vessel = invoice.VesselName;
                invoice.WorkOrder = workorder;
                var gst = await _gstService.GetById(invoice.GSTId);
                report.TaxDescription = gst.GSTPercent.ToString();
                List<SalesInvoice> salesInvoices = new List<SalesInvoice>();
                salesInvoices.Add(invoice);
                report.TotalAmount += invoice.TotalAmt;
                report.TaxAmount += invoice.GSTAmount;
                report.TotalNonTaxableAmount += invoice.TotalNonTaxable;
                report.GrandTotal += invoice.TotalNetAmount;
                foreach (var salesInvoice in salesInvoices)
                {
                    int currentIndex = 0;
                    InvoiceDetailsReportViewModel details = new InvoiceDetailsReportViewModel
                    {
                        WorkOrderNo = salesInvoice.WorkOrderNo + " " + workOrderPassenger.Name + " (" + workOrderPassenger.Rank + ") x" + workOrderPassenger.NoOfPax.ToString(),
                        IsHeader = true,
                    };
                    if (invoice.WorkOrder.PickUpdateDate != null)
                        details.InvoiceDate = invoice.WorkOrder.PickUpdateDate.Value;
                    report.Details.Add(details);
                    foreach (var invoiceDetails in salesInvoice.Details)
                    {
                        InvoiceDetailsReportViewModel invDetail = new InvoiceDetailsReportViewModel
                        {
                            Code = invoiceDetails.Code,
                            Description = invoiceDetails.Description,
                            Qty = invoiceDetails.Qty,
                            Price = invoiceDetails.Price,
                            TotalAmount = invoiceDetails.TotalAmt
                        };
                        if(currentIndex==0)
                        {
                            if (details.InvoiceDate != null)
                                invDetail.Code += " @ " + (details.InvoiceDate.ToString("HH:mm")).Replace(":", "") + " Hr";
                        }
                        if(report.IsTaxInvoice == false && invoiceDetails.GSTEssentials)
                        {
                            invDetail.TotalAmount += invDetail.TotalAmount * (report.TaxPercent / 100);
                        }
                        report.Details.Add(invDetail);
                        currentIndex++;
                    }
                }
            }

            if (!string.IsNullOrEmpty(DNNo))
            {
                InvoiceDetailsReportViewModel dnNo = new InvoiceDetailsReportViewModel
                {
                    WorkOrderNo = DNNo,
                    IsHeader = true,
                    IsDNNo = true
                };
                report.Details.Add(dnNo);
            }

            var dbsalesInvoice = await _salesInvoiceSummaryService.GetByInvoiceNo(SalesInvoiceSummaryNo);
            if (dbsalesInvoice != null) 
            {
                report.InvoiceNo = dbsalesInvoice.ReferenceNo;
            }
            else
            {
                salesInvoiceSummary.DNNo = DNNo;
                await _salesInvoiceSummaryService.Save(salesInvoiceSummary);

                report.SetInvoiceNo(new SerialNoRepository<SalesInvoiceReportSerialNo>(db).GetSerialNoByMonth(salesInvoiceSummary.Id, report.Date));

                salesInvoiceSummary.ReferenceNo = report.InvoiceNo;

                await db.SaveChangesAsync();
            }
            report.CalculateNoOfPage();
            var pdfFilePath = base.GeneratePDF<InvoiceReportViewModel>(report, FileConfig.INVOICES, "Inv_" + report.InvoiceNo.Replace('/', '_'));
            if (DownloadFileFormat == CommonEnumConfig.DownloadFileFormat.PDF)
            {
                return base.File(pdfFilePath, CONTENT_DISPOSITION);
                //return base.File(pdfFilePath, CONTENT_DISPOSITION, "INV_" + report.InvoiceNo.Replace('/', '_') + ".pdf");                
            }
            else
            {
                PDFProcessor pDFProcessor = new PDFProcessor();
                var excelPath = pDFProcessor.ConvertToExcel(pdfFilePath, Server.MapPath(FileConfig.EXPORT_EXCEL_INVOICES));
                return base.File(excelPath, CONTENT_DISPOSITION, "INV_" + report.InvoiceNo.Replace('/', '_') + ".xls");
            }
        }
    }
}