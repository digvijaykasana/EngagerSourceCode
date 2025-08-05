using EngagerMark4.ApplicationCore.Common;
using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.Customer.Cris;
using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.Customer.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;
using EngagerMark4.ApplicationCore.IService.Users;
using Microsoft.AspNet.Identity;
using EngagerMark4.ApplicationCore.Cris.Configurations;
using static EngagerMark4.ApplicationCore.Entities.Configurations.Letterhead;

namespace EngagerMark4.Controllers.Customers
{
    /// <summary>
    /// Controller Class for Customers
    /// Created     : Aung Ye Kaung
    /// Modified    : 
    /// </summary>

    public class CustomerController : BaseController<CustomerCri, Customer, ICustomerService>
    {
        ICommonConfigurationService commonConfigurationService;
        IRolePermissionService _rolePermissionService;

        ILetterheadService letterheadService;

        public CustomerController(ICustomerService service, 
            ICommonConfigurationService commonConfigurationService,
            IRolePermissionService rolePermissionService,
            ILetterheadService _letterheadService) : base(service)
        {
            this.commonConfigurationService = commonConfigurationService;
            this._rolePermissionService = rolePermissionService;

            this.letterheadService = _letterheadService;

            this._defaultColumn = "Name";
        }

        protected override CustomerCri GetCri()
        {

            var cri = base.GetCri();
            cri.SearchValue = Request["Name"];
            return cri;

            //var cri = base.GetCri();

            //cri.StringCris = new Dictionary<string, StringValue>();

            //var name = Request["name"];

            //if (!string.IsNullOrEmpty(name))
            //    cri.StringCris.Add("Name", new StringValue { ComparisonOperator = BaseCri.StringComparisonOperator.Contains, Value = name });

            //ViewBag.Name = name;

            //return cri;
        }

        protected async override Task LoadReferencesForList(CustomerCri aCri)
        {
            var name = Request["name"];
            ViewBag.Name = name;
            //CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            //configurationCri.Includes = new List<string>();
            //configurationCri.Includes.Add("ConfigurationGroup");

            //var references = await commonConfigurationService.GetByCri(configurationCri);

            //var vesselLst = references.Where(s => s.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.VesselId.ToString()));
            //ViewBag.VesselId = new SelectList(vesselLst, "Id", "Name");

            //var discTypeLst = references.Where(s => s.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.DiscountType.ToString()));
            //ViewBag.DiscountTypeId = new SelectList(discTypeLst, "Id", "Name");
        }


        protected async override Task SaveEntity(Customer aEntity)
        {
            //PCR2021
            if(aEntity.TransferVoucherFormat == Common.Configs.TransferVoucherConfig.TransferVoucherFormat.WallemFormat)
            {
                aEntity.TFRequireAllPassSignatures = true;
            }
            else
            {
                aEntity.TFRequireAllPassSignatures = false;
            }

            await base.SaveEntity(aEntity);
        }

        protected async override Task LoadReferences(Customer entity)
        {
            if (entity == null) entity = new Customer();

            CommonConfigurationCri configurationCri = new CommonConfigurationCri();
            configurationCri.Includes = new List<string>();
            configurationCri.Includes.Add("ConfigurationGroup");

            var references = await commonConfigurationService.GetByCri(configurationCri);

            var vesselLst = references.Where(s => s.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.VesselId.ToString()));
            ViewBag.VesselId = new SelectList(vesselLst, "Id", "Name", entity.VesselId);
            ViewBag.Vessels = new SelectList(vesselLst, "Id", "Name");

            var letterheadLst = await letterheadService.GetByCri(new LetterheadCri() { Type = (int)LetterheadType.TransferVoucher});
            ViewBag.LetterheadId = new SelectList(letterheadLst.OrderByDescending(x => x.IsDefault), "Id", "Name", entity.LetterheadId);

            var discTypeLst = references.Where(s => s.ConfigurationGroup.Code.Equals(ConfigurationGrpCodes.DiscountType.ToString()));
            ViewBag.DiscountType = new SelectList(discTypeLst, "Id", "Name", entity.DiscountType);

            ViewBag.HasPermissionForFileUpload = _rolePermissionService.HasPermission(nameof(CustomerFileUploadController), User.Identity.GetUserId());
            ViewBag.HasPermissionForLocation = _rolePermissionService.HasPermission(nameof(CustomerLocationController), User.Identity.GetUserId());
            ViewBag.HasPermissionForVessel = _rolePermissionService.HasPermission(nameof(CustomerVesselController), User.Identity.GetUserId());
            ViewBag.HasPermissionForAdvancedTab = _rolePermissionService.HasPermission(nameof(CustomerAdvancedTabController), User.Identity.GetUserId());
        }
        
        protected override ActionResult DetailRedirect(string returnUrl)
        {
            return RedirectToAction("Details", new { Id = _currentEntityId });
        }

        //Detect Duplicate Customer based on Email, Phone No, Account No
        [HttpPost]
        public JsonResult CheckForSimilarCustomers(string Email= "", string OfficeNo = "", string AccountNo = "")        
        {
            var resultLst = _service.GetSimliarCustomers(Email, OfficeNo, AccountNo);

            if (resultLst == null || resultLst.Count == 0)
                return Json("NoSimilarCustomer");

            string message = "";

            foreach (var customer in resultLst)
            {
                message += customer.Name + " [";
                
                if(!string.IsNullOrEmpty(customer.Email))
                {
                    message += customer.Email + ", ";
                }

                if (!string.IsNullOrEmpty(customer.OfficeNo))
                {
                    message += customer.OfficeNo + ", ";
                }

                if (!string.IsNullOrEmpty(customer.AccNo))
                {
                    message += customer.AccNo + "";
                }

                message += "], ";
            }

            if (!string.IsNullOrEmpty(message))
            {
                message = message.Remove(message.Length - 2);
            }

            return Json(message);
        }

    }
}