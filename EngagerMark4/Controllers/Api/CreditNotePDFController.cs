using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.ReportViewModels;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IService.CreditNotes;
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
using static EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice;

namespace EngagerMark4.Controllers.Api
{
    public class CreditNotePDFController : BaseController<WorkOrderCri, ApplicationCore.SOP.Entities.WorkOrders.WorkOrder, IWorkOrderService>
    {
        ICompanyService _companyService;
        ICreditNoteService _creditNoteService;
        ICustomerService _customerService;
        IGSTService _gstService;
        ApplicationDbContext context;
        IWorkOrderService workOrderService;
        ISalesInvoiceSummaryService _salesInvoiceSummaryService;

        public CreditNotePDFController(IWorkOrderService service,
            ICompanyService companyService,
            ICreditNoteService creidtNoteService,
            ICustomerService customerService,
            IGSTService gstService,
            ISalesInvoiceSummaryService salesInvoiceSummaryService,
            ApplicationDbContext context,
            IWorkOrderService workOrderService) : base(service)
        {
            this._companyService = companyService;
            this._creditNoteService = creidtNoteService;
            this._customerService = customerService;
            this._gstService = gstService;
            this.context = context;
            this.workOrderService = workOrderService;
            this._salesInvoiceSummaryService = salesInvoiceSummaryService;
        }

        public async Task<ActionResult> Download(Int64[] creditNoteDetailIds, WorkOrderCri aCri,  bool TaxInclude = true, bool ExcelFormat = false, string creditNoteNo = "", bool IncludeVesselInFrontOfCompanyName = false)
        {
            if (creditNoteDetailIds == null) return null;

            var l_creditNoteDetailId = creditNoteDetailIds.FirstOrDefault();

            var workOrder = await workOrderService.GetByCreditNoteId(l_creditNoteDetailId);

            if (workOrder == null) workOrder = new ApplicationCore.SOP.Entities.WorkOrders.WorkOrder();

            List<CreditNote> creditNoteDetailList = new List<CreditNote>();

            foreach (var creditNoteDetailId in creditNoteDetailIds)
            {
                var creditNoteDetailResult = await this._creditNoteService.GetById(creditNoteDetailId);

                if (creditNoteDetailResult != null)
                {
                    creditNoteDetailList.Add(creditNoteDetailResult);
                }
            }

            CreditNoteReportViewModel report = new CreditNoteReportViewModel();

            if (creditNoteDetailList != null && creditNoteDetailList.Count() > 0)
            {
                var firstCNDetail = creditNoteDetailList.FirstOrDefault();
                report.IsTaxCN = TaxInclude;
                report.CNNo = workOrder.SummaryInvoiceNo;

                var company = await _companyService.GetById(GlobalVariable.COMPANY_ID);
                if (company == null) company = new ApplicationCore.Entities.Company();
                report.HeaderLogo = company.ReportHeaderLogo;

                var customer = await _customerService.GetById(firstCNDetail.CustomerId);
                report.Customer = customer.Name;

                report.Vessel = firstCNDetail.VesselName;
                report.Address = firstCNDetail.CompanyAddress;
                report.TaxDescription = firstCNDetail.GSTPercent.ToString();

                var GSTPercentage = firstCNDetail.GSTPercent;

                foreach (var creditNoteDetailEntity in creditNoteDetailList)
                {
                    report.TotalAmount += creditNoteDetailEntity.SubTotal;
                    //report.TaxAmount += creditNote.GSTAmount;
                    //report.GrandTotal += creditNote.GrandTotal;
                    report.InvoiceTotal += creditNoteDetailEntity.InvoiceTotalAmount;
                }

                //Calculate Tax Amount Ad Hoc 
                report.TaxAmount = Math.Round((report.TotalAmount * GSTPercentage / 100), 2, MidpointRounding.AwayFromZero);

                report.GrandTotal = Math.Round(report.TotalAmount + report.TaxAmount, 2, MidpointRounding.AwayFromZero);

                var salesInvoiceSummary = await _salesInvoiceSummaryService.GetByInvoiceNo(workOrder.SummaryInvoiceNo);

                report.Date = salesInvoiceSummary == null ? TimeUtil.GetLocalTime() : Util.ConvertStringToDateTime(salesInvoiceSummary.InvoiceDateStr, DateConfig.CULTURE);
     
                
                decimal discountPercent = 0;

                //if(firstCNDetail.DiscountType == 0)
                //{
                //    if (report.InvoiceTotal != 0) discountPercent = (report.TotalAmount / report.InvoiceTotal) * 100;
                //}

                CreditNoteDetailsReportViewModel creditNoteDetail = new CreditNoteDetailsReportViewModel
                {
                    Description = firstCNDetail.Details.FirstOrDefault().Description,
                    TotalAmount = report.TotalAmount
                };

                report.Details.Add(creditNoteDetail);

                report.needsVesselNameInFrontCompanyName = IncludeVesselInFrontOfCompanyName;

            }

            if (ExcelFormat)
            {
                var excelTemplatePath = Server.MapPath("~/App_Data/Templates/Excels/CreditNote.xlsx");
                var excelFilePath = new ExcelProcessor<CreditNoteDetailsReportViewModel>().GenerateCreditNote(report, excelTemplatePath, Server.MapPath(FileConfig.EXPORT_EXCEL_CRNS));
                return base.File(excelFilePath, CONTENT_DISPOSITION, "CN_" + report.CNNo.Replace('/', '_') + ".xlsx");
            }
            else
            {
                return base.File(base.GeneratePDF<CreditNoteReportViewModel>(report, FileConfig.CREDITNOTES, "CN_" + report.CNNo.Replace('/', '_')), CONTENT_DISPOSITION, "CN_" + report.CNNo.Replace('/', '_') + ".pdf");
            }

        }
    }
}