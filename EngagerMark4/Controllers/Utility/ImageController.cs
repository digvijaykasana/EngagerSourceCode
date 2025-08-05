using EngagerMark4.Common.Configs;
using EngagerMark4.Common.DTO;
using EngagerMark4.Common.Utilities;
using EngagerMark4.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Utility
{
    public class ImageController : Controller
    {
        /// <summary>
        /// Get item image from the local file system.
        /// Created by: Kyaw Min Htut
        /// Created date: 06-04-2015
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Item(string img = "")
        {
            try
            {
                if (img == null || img.Equals(string.Empty))
                    return null;
                var path = Path.Combine(Server.MapPath(ImageConfig.TYPE_FILE_LOCATION), img);
                var returnType = "image/jpeg";
                if (System.IO.File.Exists(path))
                    return base.File(path, returnType);
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public string UploadImage(string CKEditor = "", int CKEditorFuncNum = 0, string langCode = "")
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase posted_file = Request.Files[file] as HttpPostedFileBase;
                    if (posted_file.ContentLength == 0)
                        continue;
                    if (posted_file.ContentLength > 0)
                    {
                        ImageUpload imageUpload = new ImageUpload { Width = ImageConfig.WIDTH };
                        ImageResult imageResult = imageUpload.RenameUploadFile(posted_file);
                        if (imageResult.Success)
                        {
                            string url = "/Image/Item?img=" + imageResult.ImageName;
                            return "<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", '" + url + "', 'The file has been uploaded');</script>";
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DragUploadedImage(string CKEditor = "", int CKEditorFuncNum = 0, string langCode = "")
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase posted_file = Request.Files[file] as HttpPostedFileBase;
                    if (posted_file.ContentLength == 0)
                        continue;
                    if (posted_file.ContentLength > 0)
                    {
                        ImageUpload imageUpload = new ImageUpload { Width = ImageConfig.WIDTH };
                        ImageResult imageResult = imageUpload.RenameUploadFile(posted_file);
                        if (imageResult.Success)
                        {
                            string url = "/Image/Item?img=" + imageResult.ImageName;
                            DragUploadedImage image = new DragUploadedImage();
                            image.uploaded = 1;
                            image.fileName = imageResult.ImageName;
                            image.url = url;
                            return Json(image, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        [AllowAnonymous]
        public ActionResult Company(string img = "")
        {
            try
            {
                if (img == null || img.Equals(string.Empty))
                    return null;
                var path = Path.Combine(Server.MapPath(ImageConfig.TYPE_COMPANY_LOGO_LOCATION), img);
                var returnType = "image/jpeg";
                if (System.IO.File.Exists(path))
                    return base.File(path, returnType);
                else
                    return null;
            }
            catch (Exception ex)
            {
                //Logger.Error(ex);
                return null;
            }
        }
    }
}