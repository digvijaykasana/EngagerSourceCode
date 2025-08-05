using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api
{
    public class VesselApiController : Controller
    {
        ICommonConfigurationService _commonConfigurationService;
        IConfigurationGroupService _configurationGroupService;
        ICustomerService _customerService;

        public VesselApiController(ICommonConfigurationService commonConfigurationService,
            IConfigurationGroupService configurationGroupService,
            ICustomerService customerService)
        {
            this._commonConfigurationService = commonConfigurationService;
            this._configurationGroupService = configurationGroupService;
            this._customerService = customerService;
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetByCustomer(Int64 id = 0)
        {
            try
            {
                if (id != 0)
                {
                    Customer customer = await _customerService.GetById((Int64)id);

                    if (customer == null)
                        return Json("No Record");

                    return Json(customer.VesselList.OrderBy(x => x.Vessel), JsonRequestBehavior.AllowGet);
                }
                //else
                //{
                //    return Json(new List<CustomerVessel>(), JsonRequestBehavior.AllowGet);
                //}
                else
                {
                    var vessels = _commonConfigurationService.GetVessels().ToList();

                    List<CustomerVesselViewModel> vesselList = new List<CustomerVesselViewModel>();

                    if (vessels.Count() > 0)
                    {
                        foreach (var vessel in vessels)
                        {
                            vesselList.Add(new CustomerVesselViewModel()
                            {
                                Vessel = vessel.Name,
                                VesselId = vessel.Id
                            });
                        }
                    }

                    return Json(vesselList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}