using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.Common.Configs;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers
{
    public class FileUploadController : Controller
    {
        protected const string CONTENT_DISPOSITION = "content-disposition";
        protected const string CONTENT_JPEG = "image/jpeg";
        protected const string APPLICATION_ZIP = "application/zip";

        IBlobFileService service;
        IBlobFileRepository _repository;

        public FileUploadController(IBlobFileService service,
            IBlobFileRepository repository)
        {
            this.service = service;
            this._repository = repository;
        }

        public ActionResult Index(string tempFolder = "", bool findByReference = false, EntityConfig.EntityType entityType = EntityConfig.EntityType.Address, Int64 referenceId = 0, BlobFile.BlobFileStatus status = BlobFile.BlobFileStatus.Temp)
        {
            ViewBag.tempFolder = tempFolder;
            ViewBag.findByReference = findByReference == false ? 0 : 1;
            ViewBag.entityType = entityType;
            ViewBag.referenceId = referenceId;
            ViewBag.status = status;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateDocName(Int32 fileId, string docName)
        {
            if (fileId != 0 && !string.IsNullOrEmpty(docName))
            {
                var blobFile = await service.GetById(fileId);
                blobFile.Name = docName;
                await service.Save(blobFile);
            }
            return Content("success");
        }


        /// <summary>
        /// *** The default dropzone upload file name is file.
        /// *** Don't change the file name or else file uploaded will be null.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="tempFolder"></param>
        [HttpPost]
        public async Task Upload(string tempFolder = "", Int64 referenceId = 0, EntityConfig.EntityType entityType = EntityConfig.EntityType.All)
        {
            tempFolder = string.IsNullOrEmpty(tempFolder) ? "null" : tempFolder;
            foreach (string uploadFile in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[uploadFile] as HttpPostedFileBase;
                if (file != null && file.ContentLength > 0)
                {
                    var newFile = new BlobFile
                    {
                        FileName = file.FileName,
                        FileType = file.ContentType,
                        ContentType = file.ContentType,
                        Status = BlobFile.BlobFileStatus.Temp,
                        TempFolder = tempFolder,
                        ReferenceId = referenceId,
                        Type = entityType,
                        FileExtension = System.IO.Path.GetExtension(file.FileName),
                        CreatedByName = User == null ? "" : User.Identity.Name,
                        Version = 1,
                    };

                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        newFile.FileContent = new BlobFileContent();
                        newFile.FileContent.Content = reader.ReadBytes(file.ContentLength);
                    }
                    await service.Save(newFile);
                }
            }
            //service.SaveChanges();
        }

        /// <summary>
        /// Get the files list from the database and return partial view
        /// Created by: Kyaw Min Htut
        /// Created date: 22/08/2016
        /// </summary>
        /// <param name="tempFolder"></param>
        /// <param name="findByReference"></param>
        /// <param name="referenceId"></param>
        /// <param name="entityType"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetFiles(string tempFolder = "", bool findByReference = false, Int64 referenceId = 0, EntityConfig.EntityType entityType = EntityConfig.EntityType.All, BlobFile.BlobFileStatus status = BlobFile.BlobFileStatus.All)
        {
            try
            {
                IEnumerable<BlobFile> blobFiles = null;
                if (findByReference == false)
                    blobFiles = await service.GetByCri(new BlobFileCri { Type = entityType, TempFolder = tempFolder, Status = status });
                else
                {
                    blobFiles = _repository.GetAll(new BlobFileCri { Type = entityType, ReferenceId = referenceId, Status = BlobFile.BlobFileStatus.All });
                }
                List<BlobFile> files = blobFiles.ToList();
                return PartialView(blobFiles);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return PartialView(new List<BlobFile>());
            }
        }

        /// <summary>
        /// Download a file by id 
        /// Created by: Kyaw Min Htut
        /// Created date: 22/08/2016
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public FileContentResult Download(Int64 id = 0)
        {
            var blobFile = Task.Run(() => service.GetById(id)).Result;

            if (blobFile == null) return null;

            return base.File(blobFile.FileContent.Content, CONTENT_DISPOSITION, blobFile.FileName);
        }


        /// <summary>
        /// Download by Reference Id and Type
        /// Created by: Aung Ye Kaung
        /// Created date: 18/05/2022
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public FileContentResult DownloadByReferenceIdAndType(Int64 referenceId, EntityConfig.EntityType entityType, Int64 subReferenceId = 0)
        {
            var blobFile = Task.Run(() => service.GetByReferenceId(referenceId, entityType, subReferenceId)).Result;

            if (blobFile == null) return null;

            return base.File(blobFile.FileContent.Content, CONTENT_DISPOSITION, blobFile.FileName);
        }

        public ActionResult DownloadByFileType(int referenceId, EntityConfig.EntityType entityType, string fileType)
        {
            IEnumerable<BlobFile> files = service.GetByCri(new BlobFileCri { ReferenceId = referenceId, FileType = fileType, Type = entityType, IncludeFilecontent = true, Status = BlobFile.BlobFileStatus.Saved }).Result;

            if (files != null)
            {
                foreach (var file in files)
                {
                    return base.File(file.FileContent.Content, CONTENT_DISPOSITION, file.FileName);
                }
            }
            return null;
        }

        public ActionResult CheckContent(int id = 0)
        {
            var blobFile = service.GetById(id);
            if (blobFile == null)
                return Content("No");
            else
                return Content("Yes");
        }

        public ActionResult CheckContentByType(int referenceId, EntityConfig.EntityType entityType, string fileType)
        {
            IEnumerable<BlobFile> files = service.GetByCri(new BlobFileCri { ReferenceId = referenceId, FileType = fileType, Type = entityType, IncludeFilecontent = true, Status = BlobFile.BlobFileStatus.Saved }).Result;

            if (files != null)
            {
                return files.Any() == true ? Content("Yes") : Content("No");
            }

            return Content("No");
        }

        /// <summary>
        /// Download multiple files
        /// Created by: Kyaw Min Htut
        /// Created date: 22/08/2016
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Downloads(long[] ids)
        {
            using (var outputStream = new MemoryStream())
            {
                using (var zip = new ZipFile())
                {
                    foreach (long id in ids)
                    {
                        var blobFile = await service.GetById(id);

                        zip.AddEntry(blobFile.FileName.Split('.')[0] + blobFile.Version + blobFile.FileExtension, blobFile.FileContent.Content.ToArray());
                    }
                    zip.Save(outputStream);
                }
                outputStream.Seek(0, SeekOrigin.Begin);
                outputStream.Flush();
                return File(outputStream.ToArray(), APPLICATION_ZIP, "Documents.zip");
            }
        }

        /// <summary>
        /// Delete a file from database
        /// Created by: Kyaw Min Htut
        /// Created date: 22/08/2016
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(int id = 0)
        {
            var blobFileEntity = await service.GetById(id);
            await service.Delete(blobFileEntity);

            //service.Delete(new BlobFile { Id = id });
            //service.SaveChanges();
            return Content("success");
        }

    }
}