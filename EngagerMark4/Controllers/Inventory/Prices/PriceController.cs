using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.ApplicationCore.Inventory.Cris;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.Inventory.IService.Price;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static EngagerMark4.ApplicationCore.Inventory.Entities.PriceLocation;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;
using EngagerMark4.ApplicationCore.Cris.Configurations;

namespace EngagerMark4.Controllers.Inventory.Prices
{
    /// <summary>
    /// Controller Class for Price List
    /// Created     : Ye Kaung Aung
    /// Modified    : 
    /// </summary>
    public class PriceController : BaseController<PriceCri, Price, IPriceService>
    {
        IRolePermissionService _rolePermissionService;
        ICustomerService customerService;
        IGeneralLedgerService generalLedgerService;
        ILocationService locationService;
        IPriceLocationService priceLocationService;
        ICommonConfigurationService _configurationService;

        public PriceController(IPriceService service, IRolePermissionService _rolePermissionService, ICustomerService customerService, IGeneralLedgerService generalLedgerService, ILocationService locationService, IPriceLocationService priceLocationService, ICommonConfigurationService configurationService) : base(service)
        {
            this._rolePermissionService = _rolePermissionService;
            this.customerService = customerService;
            this.generalLedgerService = generalLedgerService;
            this.locationService = locationService;
            this.priceLocationService = priceLocationService;
            this._configurationService = configurationService;
        }

        protected async override Task LoadReferencesForList(PriceCri aCri)
        {
            var customerId = Request["CustomerId"];
            var GLCodeId = Request["GLCodeId"];
            var searchValue = Request["SearchValue"];
            var orderPage = Request["CurrentPage"];
            string orderColumn = Request["Column"];
            string orderOrderBy = Request["OrderBy"];
            string orderDataType = Request["DataType"];
            string currentId = Request["CurrentId"];

            var customers = await customerService.GetByCri(null);            
            var glCodes = await generalLedgerService.GetByCri(null);           
            
            ViewBag.CustomerId = new SelectList(customers.OrderBy(x => x.Name), "Id", "Name", customerId);
            ViewBag.GLCodeId = new SelectList(glCodes.ToList().OrderBy(x=> x.Name), "Id", "DescriptionStr");
            ViewBag.SearchValue = searchValue;

            ViewBag.orderPage = orderPage;
            ViewBag.orderColumn = orderColumn;
            ViewBag.orderOrderBy = orderOrderBy;
            ViewBag.orderDataType = orderDataType;
            ViewBag.CurrentId = currentId;           
        }

        protected override PriceCri GetCri()
        {
            var cri = new PriceCri
            {
                Includes = new List<string>()
            };
            cri.Includes.Add("Customer");
            cri.Includes.Add("GeneralLedger");

            var customerIdStr = Request["CustomerId"];
            var glCodeIdStr = Request["GLCodeId"];
            var itemCode = Request["ItemCode"];
            var pickupPoint = Request["PickupPoint"];
            var dropPoint = Request["DropPoint"];
            var orderPage = Request["CurrentPage"];
            var orderColumn = Request["Column"];
            var orderOrderBy = Request["OrderBy"];
            var orderDataType = Request["DataType"];

            Int64 customerId = 0;
            if(!string.IsNullOrEmpty(customerIdStr))
            {
                Int64.TryParse(customerIdStr, out customerId);
            }
            Int64 glcodeId = 0;
            if (!string.IsNullOrEmpty(glCodeIdStr))
            {
                Int64.TryParse(glCodeIdStr, out glcodeId);
            }

            cri.CustomerId = customerId;
            cri.GLCodeId = glcodeId;
            cri.ItemCode = itemCode;
            cri.PickupPoint = pickupPoint;
            cri.DropPoint = dropPoint;


            ViewBag.CustomerId = customerId;
            ViewBag.GLCodeId = glcodeId;
            ViewBag.ItemCode = itemCode;
            ViewBag.PickupPoint = pickupPoint;
            ViewBag.DropPoint = dropPoint;
            ViewBag.orderPage = orderPage;
            ViewBag.orderColumn = orderColumn;
            ViewBag.orderOrderBy = orderOrderBy;
            ViewBag.orderDataType = orderDataType;
            ViewBag.CurrentId = Request["CurrentId"];
            return cri;
        }

        protected async override Task LoadReferences(Price entity)
        {
            ViewBag.ReturnUrl = Request["returnUrl"];
            ViewBag.CurrentId = Request["CurrentId"];

            var cusReferences = await customerService.GetByCri(null);
            ViewBag.CustomerId = new SelectList(cusReferences, "Id", "Name", entity.CustomerId);

            var glReferences = await generalLedgerService.GetByCri(null);
            ViewBag.GLCodeId = new SelectList(glReferences, "Id", "DescriptionStr", entity.GLCodeId);

            PriceLocation pl = new PriceLocation();

            ViewBag.LocationType = new SelectList(PriceLocation.GetPriceLocationTypes(), "Id", "Name");

            var locations = await this.locationService.GetByCri(null);
            ViewBag.Locations = new SelectList(locations.OrderBy(x => x.Display), "Id", "Display");

            var locationReferences = await locationService.GetByCri(null);

            ViewBag.HasPermissionForPriceLocation = _rolePermissionService.HasPermission(nameof(PriceLocationController), User.Identity.GetUserId());

            //PriceLocation pickupLocation = entity.PriceLocationList.Where(x => x.Type == PriceLocationType.PickUp).FirstOrDefault();
            //PriceLocation dropLocation = entity.PriceLocationList.Where(x => x.Type == PriceLocationType.DropOff).FirstOrDefault();

            //ViewBag.PickupPointId = new SelectList(locationReferences, "Id", "Display", entity.PriceLocationList.Where(x => x.Type == PriceLocationType.PickUp).FirstOrDefault().LocationId);
            //ViewBag.DropPointId = new SelectList(locationReferences, "Id", "Display", entity.PriceLocationList.Where(x => x.Type == PriceLocationType.DropOff).FirstOrDefault().LocationId);

            //ViewBag.HasPermissionForLocation = _rolePermissionService.HasPermission(nameof(PriceController), User.Identity.GetUserId());
            //ViewBag.HasPermisisonForLocation = true;

        }
    }
}