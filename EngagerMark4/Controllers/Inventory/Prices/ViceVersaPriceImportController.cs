using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.Inventory.Cris;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.Inventory.IService.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Inventory.Prices
{
    public class ViceVersaPriceImportController : BaseController<PriceCri, Price, IPriceService>
    {
        Int32 processedPriceItemsCount = 0;

        ICustomerService _customerService;

        public ViceVersaPriceImportController(IPriceService service, ICustomerService customerService) : base(service)
        {
            _customerService = customerService;
        }

        protected async override Task LoadReferencesForList(PriceCri aCri)
        {
            var customers = (await this._customerService.GetByCri(null)).OrderBy(x => x.Name);
            ViewBag.CustomerId = new SelectList(customers, "Id", "Name");
            
            await base.LoadReferencesForList(aCri);
        }


        [AllowAnonymous]
        public async Task<JsonResult> ImportReturnTripChargesForCustomer(Int64 customerId)
        {
            if (customerId != 0 && customerId > 0)
            {
                processedPriceItemsCount = await _service.ImportReturnTripChargesByCustomerId(customerId);
            }

            return Json(processedPriceItemsCount.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}