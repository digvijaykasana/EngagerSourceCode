using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
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

        public class GetSnapShotApiController : BaseApiController
        {
            IBlobFileService _blobFileService;
            IServiceJobService _serviceJobService;
            IUserService _userService;
            IWorkOrderService _workOrderService;
            ICommonConfigurationService _commonConfigService;

            public GetSnapShotApiController(IBlobFileService blobFileService,
                IServiceJobService serviceJobService,
                IUserService userService, IWorkOrderService workOrderService, ICommonConfigurationService commonConfigService)
            {
                this._blobFileService = blobFileService;
                this._serviceJobService = serviceJobService;
                this._userService = userService;
                this._workOrderService = workOrderService;
                this._commonConfigService = commonConfigService;
            }

            [MobileApi]
            [HttpPost]
            [AllowAnonymous]
            public async Task<ActionResult> Index(Int64 serviceJobId = 0, Int64 userId = 0, string fileName = "", string fileDescription="")
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
                
                string guid = Guid.NewGuid().ToString();

                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase posted_file = Request.Files[file] as HttpPostedFileBase;

                    if (posted_file.ContentLength > 0)
                    {
                        var newFile = new BlobFile
                        {
                            FileName = fileName +  System.IO.Path.GetExtension(posted_file.FileName),
                            Name = fileDescription,
                            FileType = System.IO.Path.GetExtension(posted_file.FileName),
                            ContentType = System.IO.Path.GetExtension(posted_file.FileName),
                            Status = BlobFile.BlobFileStatus.Saved,
                            TempFolder = KeyUtil.GenerateKey(),
                            ReferenceId = serviceJob.WorkOrderId == null ? 0 : serviceJob.WorkOrderId.Value,
                            Type = Common.Configs.EntityConfig.EntityType.WorkOrder,
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

                        
                        //_blobFileService.DeleteByEntityTypeAndReferenceId(Common.Configs.EntityConfig.EntityType.ServiceJobSignature, serviceJobId);

                        await _blobFileService.SaveForMobile(newFile);
                    }
                }

                List<General> results = new List<General>();
                General result = new General
                {
                    Id = _successId,
                    Value = "Snapshot Uploaded successfully!"
                };
                results.Add(result);

                //return Content("success");
                return Json(results, JsonRequestBehavior.AllowGet);
            }
        }
    }
