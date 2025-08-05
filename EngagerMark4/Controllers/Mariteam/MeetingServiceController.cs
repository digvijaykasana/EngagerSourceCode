using EngagerMark4.ApplicationCore.Dummy.Cris.MeetingServices;
using EngagerMark4.ApplicationCore.Dummy.Entities.MeetingServices;
using EngagerMark4.ApplicationCore.Dummy.IService.MeetingServices;
using EngagerMark4.ApplicationCore.Dummy.IRepository.MeetingServices;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EngagerMark4.ApplicationCore.IService.Users;

namespace EngagerMark4.Controllers.Dummy
{
    /// <summary>
    /// Controller class for MeetingService
    /// Created     : Aung Ye Kaung
    /// Modified    :
    /// </summary>

    public class MeetingServiceController : BaseController<MeetingServiceCri, MeetingService, IMeetingServiceService> 
    {
        IMeetingServiceService service;
        ICustomerService customerService;
        IRolePermissionService rolePermissionService;

        public MeetingServiceController(IMeetingServiceService service, ICustomerService customerService, IRolePermissionService rolePermissionService) : base(service)
        {
            this.customerService = customerService;
            this.rolePermissionService = rolePermissionService;
            this._defaultColumn = "Name";
        }

        protected async override Task LoadReferencesForList(MeetingServiceCri aCri)
        {
            var references = await customerService.GetByCri(null);

            ViewBag.CustomerId = new SelectList(references, "Id", "Name");
        }

        protected async override Task LoadReferences(MeetingService entity)
        {
            var references = await customerService.GetByCri(null);
                
            ViewBag.CustomerId = new SelectList(references, "Id", "Name", entity.CustomerId);
        }

        protected async override Task SaveEntity(MeetingService aEntity)
        {

            if (aEntity.CustomerId != null && aEntity.CustomerId != 0)
            {
                var customer = await customerService.GetById(aEntity.CustomerId);

                if(customer != null)
                {
                    aEntity.LongText1 = customer.Name;
                }
            }

            await _service.Save(aEntity);
        }
    }
}