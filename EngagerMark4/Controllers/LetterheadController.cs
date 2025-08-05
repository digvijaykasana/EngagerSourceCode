using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Service.ApplicationCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers
{
    public class LetterheadController : BaseController<LetterheadCri, Letterhead, ILetterheadService>
    {
        BlobFileService fileService;

        public LetterheadController(ILetterheadService service, BlobFileService _fileService) : base(service)
        {
            _defaultColumn = "Name";

            this.fileService = _fileService;
        }

        protected override LetterheadCri GetCri()
        {
            var cri = base.GetCri();
            cri.SearchValue = Request["SearchValue"];
            return cri;
        }

        protected override async Task LoadReferences(Letterhead entity)
        {
            await base.LoadReferences(entity);

            if(entity.Id > 0)
            {
                var uploadedFile = fileService.GetByReferenceId(entity.Id, Common.Configs.EntityConfig.EntityType.Letterhead);

                if(uploadedFile != null)
                {
                    entity.LetterheadImageId = uploadedFile.Id;
                }
            }
        }

        protected override void ValidateEntity(Letterhead aEntity)
        {
            base.ValidateEntity(aEntity);

            if (string.IsNullOrEmpty(aEntity.Name))
            {
                ModelState.AddModelError("", "Letterhead Name is required.");
            }

            if (aEntity.LetterheadImageId == 0)
            {
                if (Request.Files.Count == 0)
                {
                    ModelState.AddModelError("", "Letterhead Image is required.");
                }
            }
        }

        protected async override Task SaveEntity(Letterhead aEntity)
        {
            long id = aEntity.Id;

            if(ModelState.IsValid)
            {
               id = await this._service.Save(aEntity);
            }

            if (id > 0 && aEntity.IsDefault)
            {
                var existingLetterheads = await this._service.GetByCri(new LetterheadCri());

                foreach (var letterhead in existingLetterheads)
                {
                    if (letterhead.Id != id && letterhead.Type == aEntity.Type)
                    {
                        var currentLetterhead = await this._service.GetById(letterhead.Id);

                        if(currentLetterhead != null)
                        {
                            currentLetterhead.IsDefault = false;
                            await this._service.Save(currentLetterhead);
                        }
                    }
                }
            }

            if (id > 0)
            {
                #region Save the uploaded image into DB

                if(Request.Files.Count > 0)
                {


                    foreach (string uploadFile in Request.Files)
                    {
                        if (uploadFile.Equals("letterheadImage"))
                        {
                            HttpPostedFileBase file = Request.Files[uploadFile] as HttpPostedFileBase;
                            if (file != null && file.ContentLength > 0)
                            {
                                var existingFile = fileService.GetByReferenceId(id, Common.Configs.EntityConfig.EntityType.Letterhead);

                                if (existingFile != null)
                                {
                                    await fileService.Delete(existingFile);
                                }

                                var newFile = new BlobFile
                                {
                                    FileName = file.FileName,
                                    FileType = file.ContentType,
                                    ContentType = file.ContentType,
                                    Status = BlobFile.BlobFileStatus.Saved,
                                    TempFolder = KeyUtil.GenerateKey(),
                                    ReferenceId = id,
                                    Type = Common.Configs.EntityConfig.EntityType.Letterhead,
                                    FileExtension = System.IO.Path.GetExtension(file.FileName),
                                    CreatedByName = User == null ? "" : User.Identity.Name,
                                    Version = 1,
                                };

                                using (var reader = new System.IO.BinaryReader(file.InputStream))
                                {
                                    newFile.FileContent = new BlobFileContent();
                                    newFile.FileContent.Content = reader.ReadBytes(file.ContentLength);
                                }

                                await fileService.Save(newFile);
                            }
                        }
                    }
                }

                #endregion
            }

        }
    }
}