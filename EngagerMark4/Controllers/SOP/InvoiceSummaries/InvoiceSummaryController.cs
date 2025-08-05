using EngagerMark4.ApplicationCore.Common;
using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.SOP.InvoiceSummaries
{
    public class InvoiceSummaryController : BaseController<SalesInvoiceSummaryCri,SalesInvoiceSummary,ISalesInvoiceSummaryService>
    {
        ICustomerService _customerService;
        ICommonConfigurationService _commonConfigService;

        // GET: InvoiceSummary
        public InvoiceSummaryController(ISalesInvoiceSummaryService service, 
                                        ICustomerService customerService,
                                        ICommonConfigurationService commonConfigService) : base(service)
        {
            this._defaultColumn = "ReferenceNo";
            this._defaultOrderBy = "Dsc";

            this._customerService = customerService;
            this._commonConfigService = commonConfigService;
        }

        protected override SalesInvoiceSummaryCri GetCri()
        {
            var cri = base.GetCri();

            cri.SearchValue = Request["SearchValue"];
            var customerIdStr = Request["Customers"];
            var vesselIdStr = Request["Vessels"];
            string fromDate = Request["FromDate"];
            string toDate = Request["ToDate"];

            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.CustomerId = customerIdStr;
            ViewBag.VesselId = vesselIdStr;

            Int64 customerId = 0;
            if (!string.IsNullOrEmpty(customerIdStr))
            {
                Int64.TryParse(customerIdStr, out customerId);
            }
            cri.CustomerId = customerId;

            Int64 vesselId = 0;
            if (!string.IsNullOrEmpty(vesselIdStr))
            {
                Int64.TryParse(vesselIdStr, out vesselId);
            }
            cri.VesselId = vesselId;

            if (!string.IsNullOrEmpty(fromDate))
                cri.FromDate = Util.ConvertStringToDateTime(fromDate, DateConfig.CULTURE);

            if (!string.IsNullOrEmpty(toDate))
                cri.ToDate = Util.ConvertStringToDateTime(toDate, DateConfig.CULTURE);

            return cri;
        }

        protected async override Task LoadReferencesForList(SalesInvoiceSummaryCri aCri)
        {
            var customerId = Request["Customers"];
            var vesselId = Request["Vessels"];

            var orderPage = Request["CurrentPage"];
            string orderColumn = Request["Column"];
            string orderOrderBy = Request["OrderBy"];
            string orderDataType = Request["DataType"];
            string currentId = Request["CurrentId"];

            ViewBag.orderPage = orderPage;
            ViewBag.orderColumn = orderColumn;
            ViewBag.orderOrderBy = orderOrderBy;
            ViewBag.orderDataType = orderDataType;
            ViewBag.CurrentId = currentId;         

            var customers = (await this._customerService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.Customers = new SelectList(customers, "Id", "Name", customerId);

            CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            configurationCri.Includes = new List<string>();
            configurationCri.Includes.Add("ConfigurationGroup");
            var vessels = (await _commonConfigService.GetByCri(configurationCri)).Where(x => x.ConfigurationGroup.Code.Equals(GeneralConfig.ConfigurationGrpCodes.VesselId.ToString())).OrderBy(x => x.Name);
            ViewBag.Vessels = new SelectList(vessels, "Id", "Name", vesselId);


            string fromDate = Request["FromDate"];
            string toDate = Request["ToDate"];

            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

        }

    }
}