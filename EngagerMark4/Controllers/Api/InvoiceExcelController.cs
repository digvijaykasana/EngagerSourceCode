using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Excels;
using EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.DocumentProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api
{
    public class InvoiceExcelController : Controller
    {
        ISalesInvoiceRepository _repository;
        IWorkOrderService _workOrderService;

        public InvoiceExcelController(ISalesInvoiceRepository repository, IWorkOrderService workOrderService)
        {
            this._repository = repository;
            this._workOrderService = workOrderService;
        }

        public async Task<ActionResult> Download(Int64[] invoices)
        {
            if(invoices == null || invoices.Count() == 0)
            {

                var customerIdStr = Request["Customers"];
                var vesselIdStr = Request["Vessels"];
                var status = Request["Status"];
                string fromDate = Request["FromDate"];
                string toDate = Request["ToDate"];
                var drivers = Request["DriverId"];
                var invoiceNo = Request["SalesInvoiceSummaryNo"];
                var isSearchByRange = Request["IsSearchByRange"];
                var startingInvoiceNo = Request["SalesInvoiceSummaryStartingNo"];
                var endingInvoiceNo = Request["SalesInvoiceSummaryEndingNo"];

                WorkOrderCri aCri = new WorkOrderCri();

                Int64 customerId = 0;
                if (!string.IsNullOrEmpty(customerIdStr))
                {
                    Int64.TryParse(customerIdStr, out customerId);
                }
                aCri.CustomerId = customerId;

                Int64 vesselId = 0;
                if (!string.IsNullOrEmpty(vesselIdStr))
                {
                    Int64.TryParse(vesselIdStr, out vesselId);
                }
                aCri.VesselId = vesselId;

                int statusInt = 0;
                if (!string.IsNullOrEmpty(status))
                {
                    Int32.TryParse(status, out statusInt);
                }
                aCri.Status = (EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders.WorkOrder.OrderStatus)statusInt;

                if (!string.IsNullOrEmpty(fromDate))
                    aCri.FromDate = Util.ConvertStringToDateTime(fromDate, DateConfig.CULTURE);

                if (!string.IsNullOrEmpty(toDate))
                    aCri.ToDate = Util.ConvertStringToDateTime(toDate, DateConfig.CULTURE);

                aCri.Drivers = drivers;
                long driverId = 0;
                Int64.TryParse(drivers, out driverId);
                aCri.DriverId = driverId;

                aCri.SalesInvoiceSummaryNo = invoiceNo;

                aCri.SearchByRange = Convert.ToBoolean(isSearchByRange);

                if (aCri.SearchByRange)
                {
                    aCri.SalesInvoiceSummaryStartingNo = startingInvoiceNo;

                    if (!String.IsNullOrEmpty(aCri.SalesInvoiceSummaryStartingNo))
                    {
                        var tempArr = aCri.SalesInvoiceSummaryStartingNo.Split('/');

                        aCri.StartingRefYearMonth = Convert.ToInt32(tempArr[0] + tempArr[1]);
                        aCri.StartingRefSerial = Convert.ToInt32(tempArr[2]);
                    }

                    aCri.SalesInvoiceSummaryEndingNo = endingInvoiceNo;

                    if (!String.IsNullOrEmpty(aCri.SalesInvoiceSummaryEndingNo))
                    {
                        var tempArr = aCri.SalesInvoiceSummaryEndingNo.Split('/');

                        aCri.EndingRefYearMonth = Convert.ToInt32(tempArr[0] + tempArr[1]);
                        aCri.EndingRefSerial = Convert.ToInt32(tempArr[2]);
                    }

                }

                var workOrders = await _workOrderService.GetByInvoiceDateCri(aCri);

                if (workOrders != null && workOrders.Count() > 0)
                {
                    var invoiceIdsTemp = workOrders.Where(x=>x.InvoiceId.HasValue).Select(x => x.InvoiceId.Value).ToList();

                    invoices = new long[invoiceIdsTemp.Count()];
                    for (int i = 0; i < invoiceIdsTemp.Count(); i++)
                    {
                        if(invoiceIdsTemp[i] != 0)
                        {
                            invoices[i] = invoiceIdsTemp[i];
                        }
                    }
                }
            }

            ExcelProcessor<InvoiceExcel> processor = new ExcelProcessor<InvoiceExcel>();

            string path = processor.ExportToExcel(_repository.GetData(invoices),Server.MapPath(FileConfig.EXPORT_EXCEL_INVOICES), KeyUtil.GenerateKey());

            return base.File(path, "content-disposition", "Ez_Invoice.xls");
        }
    }
}