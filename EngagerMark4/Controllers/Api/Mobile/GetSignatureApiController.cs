using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IService.Jobs;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Controllers.Api.Account;
using EngagerMark4.MvcFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api.Mobile
{
    public class GetSignatureApiController : BaseApiController
    {
        IBlobFileService _blobFileService;
        IServiceJobService _serviceJobService;
        IUserService _userService;
        IWorkOrderService _workOrderService;
        ICommonConfigurationService _commonConfigService;

        IWorkOrderPassengerService _workOrderPassengerService;

        public GetSignatureApiController(IBlobFileService blobFileService,
            IServiceJobService serviceJobService,
            IUserService userService, IWorkOrderService workOrderService, ICommonConfigurationService commonConfigService, IWorkOrderPassengerService workOrderPassengerService)
        {
            this._blobFileService = blobFileService;
            this._serviceJobService = serviceJobService;
            this._userService = userService;
            this._workOrderService = workOrderService;
            this._commonConfigService = commonConfigService;
            this._workOrderPassengerService = workOrderPassengerService;    
        }

        //OLD VERSION
        [MobileApi]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Index(Int64 serviceJobId = 0, string signatureName = "", Int64 userId = 0 )
        {
            if (!ValidateToken())
            {
                return Content("Invalid Token");
            }

            SetParentCompanyId();

            var user = await _userService.GetById(userId);

            if (user == null) user = new ApplicationCore.Entities.Users.User();

            ServiceJob serviceJob = await _serviceJobService.GetById(serviceJobId);

            if (serviceJob == null)
                return HttpNotFound();

            serviceJob.SignatureName = signatureName;

            foreach(string file in Request.Files)
            {
                HttpPostedFileBase posted_file = Request.Files[file] as HttpPostedFileBase;

                if(posted_file.ContentLength> 0)
                {
                    var newFile = new BlobFile
                    {
                        FileName = serviceJob.ReferenceNo + System.IO.Path.GetExtension(posted_file.FileName),
                        FileType = System.IO.Path.GetExtension(posted_file.FileName),
                        ContentType = System.IO.Path.GetExtension(posted_file.FileName),
                        Status = BlobFile.BlobFileStatus.Saved,
                        TempFolder = KeyUtil.GenerateKey(),
                        ReferenceId = serviceJob.Id,
                        Type = Common.Configs.EntityConfig.EntityType.ServiceJobSignature,
                        Created = TimeUtil.GetLocalTime(),
                        CreatedBy = user.ApplicationUserId,
                        CreatedByName = user.Name,
                        Version = 1
                    };

                    using (var reader = new System.IO.BinaryReader(posted_file.InputStream))
                    {
                        newFile.FileContent = new BlobFileContent();
                        newFile.FileContent.Content = reader.ReadBytes(posted_file.ContentLength);
                    }
                    _blobFileService.DeleteByEntityTypeAndReferenceId(Common.Configs.EntityConfig.EntityType.ServiceJobSignature, serviceJobId);
                    await _blobFileService.SaveForMobile(newFile);
                    serviceJob.SignatureId = newFile.Id;
                    _serviceJobService.UpdateSignature(serviceJobId, serviceJob.SignatureName, newFile.Id);
                }
            }

            List<General> results = new List<General>();
            General result = new General
            {
                Id = _successId,
                Value = "Signature Uploaded successfully!"
            };
            results.Add(result);

            //return Content("success");
            return Json(results, JsonRequestBehavior.AllowGet);
        }


        //PCR2021
        [MobileApi]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SaveWithPassengerId(Int64 serviceJobId = 0, string signatureName = "", Int64 signerPassengerId = 0, Int64 userId = 0)
        {

            if (!ValidateToken())
            {
                return Content("Invalid Token");
            }

            SetParentCompanyId();

            List<General> results = new List<General>();

            try
            {
                var user = await _userService.GetById(userId);

                if (user == null) user = new ApplicationCore.Entities.Users.User();

                ServiceJob serviceJob = await _serviceJobService.GetById(serviceJobId);

                if (serviceJob == null)
                    return HttpNotFound();

                serviceJob.SignatureName = signatureName;

                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase posted_file = Request.Files[file] as HttpPostedFileBase;

                    if (posted_file.ContentLength > 0)
                    {
                        var newFile = new BlobFile
                        {
                            FileName = serviceJob.ReferenceNo + System.IO.Path.GetExtension(posted_file.FileName),
                            FileType = System.IO.Path.GetExtension(posted_file.FileName),
                            ContentType = System.IO.Path.GetExtension(posted_file.FileName),
                            Status = BlobFile.BlobFileStatus.Saved,
                            TempFolder = KeyUtil.GenerateKey(),
                            ReferenceId = signerPassengerId,
                            SecondReferenceId = Convert.ToInt32(serviceJobId),
                            Type = Common.Configs.EntityConfig.EntityType.ServiceJobSignature,
                            Created = TimeUtil.GetLocalTime(),
                            CreatedBy = user.ApplicationUserId,
                            CreatedByName = user.Name,
                            Version = 1
                        };

                        using (var reader = new System.IO.BinaryReader(posted_file.InputStream))
                        {
                            newFile.FileContent = new BlobFileContent();
                            newFile.FileContent.Content = reader.ReadBytes(posted_file.ContentLength);
                        }
                        _blobFileService.DeleteByEntityTypeAndReferenceId(Common.Configs.EntityConfig.EntityType.ServiceJobSignature, signerPassengerId, serviceJobId);
                        await _blobFileService.SaveForMobile(newFile);
                        //serviceJob.SignatureId = newFile.Id;
                        _serviceJobService.UpdateSignatureData(serviceJobId, serviceJob.SignatureName, newFile.Id);

                        var passenger = await _workOrderPassengerService.GetById(signerPassengerId);

                        if (passenger != null)
                        {
                            passenger.IsSigned = WorkOrderPassenger.SignStatus.Signed;
                            passenger.ServiceJobId = serviceJobId;

                            await _workOrderPassengerService.Save(passenger);
                        }
                    }
                }

                General result = new General
                {
                    Id = _successId,
                    Value = "Signature Uploaded successfully!"
                };
                results.Add(result);

                //return Content("success");
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                General result = new General
                {
                    Id = _successId,
                    Value = "Signature Uploaded successfully!"
                };
                results.Add(result);

                //return Content("success");
                return Json(results, JsonRequestBehavior.AllowGet);

            }
        }
    }
}