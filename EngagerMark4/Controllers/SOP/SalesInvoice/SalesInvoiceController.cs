using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.Inventory.IService.Price;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using System.Threading.Tasks;
using EngagerMark4.ApplicationCore.Cris.Configurations;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.Job.IService;
using EngagerMark4.ApplicationCore.Inventory.Cris;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Common.Configs;
using EngagerMark4.ApplicationCore.Account.Cris;
using System.Text;

namespace EngagerMark4.Controllers.SOP.SalesInvoice
{
    public class SalesInvoiceController : BaseController<SalesInvoiceCri, ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice, ISalesInvoiceService>
    {
        ICustomerService _customerSerivce;
        IWorkOrderService _workOrderService;
        ICommonConfigurationService _commonConfigurationService;
        IGSTService _gstService;
        IPriceService _priceService;
        ICheckListService _checkListService;
        IGeneralLedgerService _glService;
        ISalesInvoiceSummaryService _salesInvoiceSummaryService;

        public SalesInvoiceController(ISalesInvoiceService service,
            ICustomerService customerService,
            IWorkOrderService workOrderService,
            ICommonConfigurationService commonConfigurationService,
            IGSTService gstService,
            IPriceService priceService,
            ICheckListService checklistService,
            ISalesInvoiceSummaryService salesInvoiceSummaryService,
            IGeneralLedgerService glService) : base(service)
        {
            this._customerSerivce = customerService;
            this._workOrderService = workOrderService;
            this._commonConfigurationService = commonConfigurationService;
            this._gstService = gstService;
            this._priceService = priceService;
            this._checkListService = checklistService;
            this._salesInvoiceSummaryService = salesInvoiceSummaryService;
            this._glService = glService;
            _defaultColumn = "ReferenceNoNumber";
            _defaultOrderBy = BaseCri.EntityOrderBy.Asc.ToString();
            _defaultDataType = BaseCri.DataType.Int64.ToString();
        }

        protected override SalesInvoiceCri GetCri()
        {
            var cri = base.GetCri();

            if (cri == null)
                cri = new SalesInvoiceCri();
            cri.Includes = new List<string>();
            cri.Includes.Add("Customer");

            var customers = Request["Customers"];
            var vessels = Request["Vessels"];
            var fromDate = Request["FromDate"];
            var toDate = Request["ToDate"];
            var status = Request["Status"];
            var invoiceNo = Request["SalesInvoiceSummaryNo"];
            var invoiceDate = Request["InvoiceDate"];

            ViewBag.CustomerId = customers;
            ViewBag.VesselId = vessels;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.Status = status;
            ViewBag.SalesInvoiceSummaryNo = invoiceNo;
            ViewBag.InvoiceDate = invoiceDate;

            cri.NumberCris = new Dictionary<string, IntValue>();

            #region Customer
            Int64 customerId = 0;
            if(!string.IsNullOrEmpty(customers))
            {
                Int64.TryParse(customers, out customerId);
            }

            cri.CustomerId = customerId;
            #endregion

            #region Vessel
            Int64 vesselId = 0;
            if(!string.IsNullOrEmpty(vessels))
            {
                Int64.TryParse(vessels, out vesselId);
            }

            cri.VesselId = vesselId;

            #endregion

            int statusInt = 0;
            if (!string.IsNullOrEmpty(status))
            {
                Int32.TryParse(status, out statusInt);
            }

            cri.Status = (EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice.SalesInvoiceStatus)statusInt;

            if(!string.IsNullOrEmpty(fromDate))
            {
                cri.FromDate = Util.ConvertStringToDateTime(fromDate, DateConfig.CULTURE);
            }

            if(!string.IsNullOrEmpty(toDate))
            {
                cri.ToDate = Util.ConvertStringToDateTime(toDate, DateConfig.CULTURE);
            }

            cri.SalesInvoiceSummaryNo = invoiceNo;
            cri.InvoiceDate = invoiceDate;

            return cri;
        }

        protected async override Task LoadReferencesForList(SalesInvoiceCri aCri)
        {
            var customer = Request["Customers"];
            var vessel = Request["Vessels"];

            var customers = (await this._customerSerivce.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.Customers = new SelectList(customers, "Id", "Name", customer);

            CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            configurationCri.Includes = new List<string>();
            configurationCri.Includes.Add("ConfigurationGroup");

            var vessels = (await _commonConfigurationService.GetByCri(configurationCri)).Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.VesselId.ToString())).OrderBy(x => x.Name);
            ViewBag.Vessels = new SelectList(vessels, "Id", "Name", vessel);

            string fromDate = Request["FromDate"];
            string toDate = Request["ToDate"];

            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            var status = Request["Status"];

            ViewBag.Status = new SelectList(SalesInvoiceCri.GetInvoiceStatus(), "Id", "Name", status);

            var salesInvoiceSummary = await _salesInvoiceSummaryService.GetByInvoiceNo(aCri.SalesInvoiceSummaryNo);
            if (salesInvoiceSummary != null)
            {
                aCri.DNNo = salesInvoiceSummary.DNNo;
            }
        }

        protected async override Task<IEnumerable<ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice>> GetData(SalesInvoiceCri cri)
        {
            //if (string.IsNullOrEmpty(cri.SalesInvoiceSummaryNo))
                return await base.GetData(cri);
            //else
            //{
            //    return await this._service.GetByInvoiceNo(cri.SalesInvoiceSummaryNo);
            //}
        }


        protected async override Task LoadReferences(ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice entity)
        {
            try
            {
                if (entity == null) entity = new ApplicationCore.SOP.Entities.SalesInvoices.SalesInvoice();

                ViewBag.ReturnUrl = Request["returnUrl"];

                var workOrderIdStr = Request["fromWorkOrderId"];
                Int64 workOrderId = 0;
                Int64.TryParse(workOrderIdStr, out workOrderId);
                //entity.WorkOrderId = workOrderId;

                var gsts = (await _gstService.GetByCri(null)).OrderBy(x => x.Name);

                if (workOrderId != 0)
                {
                    var workOrder = await this._workOrderService.GetById(workOrderId);
                    workOrder.Customer = await this._customerSerivce.GetHeaderOnly(workOrder.CustomerId.Value);
                    List<Price> priceList = new List<Price>();
                    await PreparePriceListFromTransferVoucher(workOrder, priceList);
                    await PreparePriceListForMeetingService(workOrder, priceList);
                    await PreparePriceList(workOrder, priceList);

                    if(entity.GSTId.HasValue) {
                        entity.GenerateInvoiceDetail(workOrder, priceList, gsts.Where(x => x.Id == entity.GSTId.Value).FirstOrDefault());
                    }
                    else
                    {
                        entity.GenerateInvoiceDetail(workOrder, priceList, gsts.Where(x => x.isDefault).FirstOrDefault());
                    }
                }

                CommonConfigurationCri configurationCri = new CommonConfigurationCri();
                configurationCri.Includes = new List<string>();
                configurationCri.Includes.Add("ConfigurationGroup");
                var configurations = await _commonConfigurationService.GetByCri(configurationCri);

                var customers = (await _customerSerivce.GetByCri(null)).OrderBy(x => x.Name);
                //var workOrders = (await _workOrderService.GetByCri(null)).OrderBy(x => x.RefereneceNo);
                var discountTypes = configurations.Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.DiscountType.ToString()));


                //ViewBag.DiscountTypeId = new SelectList(discountTypes, "Id", "Name", entity.DiscountTypeId);
                ViewBag.CustomerId = new SelectList(customers, "Id", "Name", entity.CustomerId);
                //ViewBag.WorkOrderId = new SelectList(workOrders, "Id", "RefereneceNo", workOrderId != 0 ? workOrderId : entity.WorkOrderId);


                //if (entity.GSTId.HasValue)
                //{
                //    ViewBag.GSTId = new SelectList(gsts, "Id", "Name", entity.GSTId);
                //}
                //else
                //{
                //    ViewBag.GSTId = new SelectList(gsts, "Id", "Name", gsts.Where(x=>x.isDefault).FirstOrDefault().Id);
                //}
                ViewBag.GSTs = gsts.ToList();

                var priceCri = new PriceCri();
                priceCri.Includes = new List<string>();
                priceCri.Includes.Add("GeneralLedger");
                priceCri.CustomerId = entity.CustomerId;
                var prices = (await _priceService.GetByCri(priceCri)).OrderBy(x => x.Name).ThenBy(x => x.PickUpPoint).ThenBy(x=> x.DropPoint).ToList();
                ViewBag.Prices = prices.ToList();
            }
            catch(Exception ex)
            {

            }            
        }

        public async Task<ActionResult> GenerateDetails(Int64[] workOrderIds, bool TaxInclude = true)
        {
            StringBuilder sb = new StringBuilder(50);

            foreach(Int64 workOrderId in workOrderIds)
            {
                sb.Append(workOrderId);
                sb.Append(",");
            }

            Console.WriteLine(sb);

            Console.Read();

            return null;
        }

        public async Task<ActionResult> RedirectToInvoice(Int64 InvoiceId = 0)
        {
            if (InvoiceId == 0)
            {
                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                TempData["message"] = "Invoice cannot be found. Contact Administrator for assistance.";
                RedirectToAction(nameof(Index), "SalesInvoice");
            }

            var invoiceSummary = await _salesInvoiceSummaryService.GetById(InvoiceId);

            if (invoiceSummary == null)
            {
                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                TempData["message"] = "Invoice cannot be found. Contact Administrator for assistance.";
                RedirectToAction(nameof(Index), "SalesInvoice");
            }

            return RedirectToAction(nameof(Index), "GenerateInvoice", new { Customers = invoiceSummary.CustomerId, Vessels = invoiceSummary.VesselId, FromDate = "", ToDate = "", Status = 0, SalesInvoiceSummaryNo = invoiceSummary.ReferenceNo, DNNo = invoiceSummary.DNNo, InvoiceDate = invoiceSummary.InvoiceDate });

        }
        

        async Task PreparePriceListFromTransferVoucher(WorkOrder workOrder, List<Price> prices)
        {
            var price = await this._priceService.FindByPickUpAndDropOff(workOrder.GetPickupPointId(), workOrder.GetDropOffPointId(), workOrder.CustomerId == null ? 0 : workOrder.CustomerId.Value);
            if (price != null)
            {
                foreach (var serviceJob in workOrder.GetServiceJobs())
                {
                    price.Name = workOrder.PickUpPoint.Replace("-", "") + " to " + workOrder.DropPoint.Replace("-", "");
                    prices.Add(price);
                }
            }
        }

        async Task PreparePriceListForMeetingService(WorkOrder workOrder, List<Price> prices)
        {
            if (workOrder.MeetingServiceList.Count == 0)
                return;

            var cri = new GeneralLedgerCri
            {
                StringCris = new Dictionary<string, StringValue>()
            };

            cri.StringCris.Add("Name", new StringValue { ComparisonOperator = BaseCri.StringComparisonOperator.Contains, Value = "Meeting Services" });
            var glList = await _glService.GetByCri(cri);
            var glMeetingService = glList.FirstOrDefault();
            if (glMeetingService == null)
                return;

            Price meetingServicePrice = await _priceService.GetByGLCodeId(glMeetingService.Id, workOrder.CustomerId.Value);

            if (meetingServicePrice == null)
                return;

            meetingServicePrice.AssignedPrice = 0;

            foreach(var meetingService in workOrder.MeetingServiceList)
            {
                meetingServicePrice.AssignedPrice += meetingService.Charges;
                meetingServicePrice.Name = meetingServicePrice.Name + " " + meetingService.FlightNo + " ";
            }

            prices.Add(meetingServicePrice);
        }

        async Task PreparePriceList(WorkOrder workOrder, List<Price>  prices)
        {
            if (prices == null) prices = new List<Price>();
            if (workOrder.ServiceJobList == null) workOrder.ServiceJobList = new List<ApplicationCore.SOP.Entities.Jobs.ServiceJob>();
            foreach(var serviceJob in workOrder.GetServiceJobs())
            {
                if(!string.IsNullOrEmpty(serviceJob.CheckListIds))
                {
                    var sjChecklistItems = serviceJob.GetChecklistItemList();

                    if (sjChecklistItems.Any() && sjChecklistItems.Count > 0)
                    {
                        foreach (var checklistItem in sjChecklistItems)
                        {
                            var chkId = checklistItem.ChecklistId;
                            var checklist = await _checkListService.GetById(chkId);
                            if (checklist != null)
                            {
                                if (workOrder.CustomerId == null)
                                    continue;
                                var price = await _priceService.GetByGLCodeId(checklist.GLCodeId, workOrder.CustomerId == null ? 0 : workOrder.CustomerId.Value);
                                if (price == null) continue;

                                if (checklist.Name.ToLower().Trim().Contains("waiting time"))
                                {
                                    price.Name = price.Name + " " + serviceJob.WaitingTime;
                                }

                                if (checklist.Name.ToLower().Trim().Contains("additional stop"))
                                {
                                    price.Name = price.Name + " " + serviceJob.AdditionalStops;
                                }

                                if (price != null)
                                {
                                    prices.Add(price);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}