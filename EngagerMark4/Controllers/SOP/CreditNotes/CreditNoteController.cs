using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IService.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;

namespace EngagerMark4.Controllers.SOP.CreditNotes
{
    public class CreditNoteController : BaseController<CreditNoteCri, CreditNote, ICreditNoteService>
    {
        IWorkOrderService workOrderService;
        ICommonConfigurationService commonConfigService;
        IGSTService gstService;
        ICustomerService _customerService;

        public CreditNoteController(ICreditNoteService service, 
                                    IWorkOrderService workOrderService, 
                                    ICommonConfigurationService commonConfigService, 
                                    IGSTService gstService,
                                    ICustomerService customerService) : base(service)
        {
            this.workOrderService = workOrderService;
            this.commonConfigService = commonConfigService;
            this.gstService = gstService;
            this._customerService = customerService;
            this._defaultColumn = "CreditNoteNo";
        }

        protected override CreditNoteCri GetCri()
        {
            var cri = base.GetCri();

            if (cri == null)
                cri = new CreditNoteCri();
            cri.Includes = new List<string>();
            cri.Includes.Add("Customer");

            var customers = Request["Customers"];
            var vessels = Request["Vessels"];

            ViewBag.CustomerId = customers;
            ViewBag.VesselId = vessels;;

            cri.NumberCris = new Dictionary<string, IntValue>();

            #region Customer
            Int64 customerId = 0;
            if (!string.IsNullOrEmpty(customers))
            {
                Int64.TryParse(customers, out customerId);
            }

            cri.CustomerId = customerId;
            #endregion

            #region Vessel
            Int64 vesselId = 0;
            if (!string.IsNullOrEmpty(vessels))
            {
                Int64.TryParse(vessels, out vesselId);
            }

            cri.VesselId = vesselId;

            #endregion

            return cri;
        }

        protected async override Task LoadReferencesForList(CreditNoteCri aCri)
        {

            var customer = Request["Customers"];
            var vessel = Request["Vessels"];

            var customers = (await this._customerService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.Customers = new SelectList(customers, "Id", "Name", customer);

            CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            configurationCri.Includes = new List<string>();
            configurationCri.Includes.Add("ConfigurationGroup");

            var vessels = (await commonConfigService.GetByCri(configurationCri)).Where(x => x.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.VesselId.ToString())).OrderBy(x => x.Name);
            ViewBag.Vessels = new SelectList(vessels, "Id", "Name", vessel);

            ////Fetching Work Orders for List View

            //WorkOrderCri woCri = new WorkOrderCri();

            //var wOReferences = await workOrderService.GetByCri(woCri);
            //ViewBag.OrderId = new SelectList(wOReferences, "Id", "RefereneceNo");

            ////Fetching Common Config "DiscountType" for List View

            //var configReferences = await commonConfigService.GetByCri(configurationCri);
            //var discTypeLst = configReferences.Where(s => s.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.DiscountType.ToString()));
            //ViewBag.DiscountType = new SelectList(discTypeLst, "Id", "Name");

            ////Fetching GST data for List View
            //var gstReferences = await gstService.GetByCri(null);
            //ViewBag.GSTs = gstReferences.ToList();
        }

        protected async override Task LoadReferences(CreditNote entity)
        {
            //Fetching Work Orders for Details View

            WorkOrderCri woCri = new WorkOrderCri();

            woCri.CustomerId = entity.CustomerId.Value;
            woCri.VesselId = entity.VesselId.Value;

            var wOReferences = await workOrderService.GetByCri(woCri);
            ViewBag.OrderId = new SelectList(wOReferences, "Id", "RefereneceNo", entity.OrderId);

            //Fetching Common Config "DiscountType" for Details View
            CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            configurationCri.Includes = new List<string>();
            configurationCri.Includes.Add("ConfigurationGroup");

            var configReferences = await commonConfigService.GetByCri(configurationCri);
            var discTypeLst = configReferences.Where(s => s.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.DiscountType.ToString()));
            ViewBag.DiscountType = new SelectList(discTypeLst, "Id", "Name", entity.DiscountType);

            //Fetching GST data for Details View
            var gstReferences = await gstService.GetByCri(null);
            ViewBag.GSTs = gstReferences.ToList();

            if(entity.GSTPercent == 0 && gstReferences != null)
            {
                entity.GSTPercent = gstReferences.ToList().FirstOrDefault().GSTPercent;
            }

        }
    }
}