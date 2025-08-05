using EngagerMark4.Common.Configs;
using EngagerMark4.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EngagerMark4.Common.Utilities
{
    /// <summary>
    /// FUJI XEROX INTERNAL USE ONLY<<RESTRICTED>>
    /// Disclose to : PSTG T&T Team
    /// Protected until:
    /// Author: Kyaw Min Htut
    /// Prepared on: 2-12-2015
    /// </summary>
    public class ImageUpload
    {
        // set default size here
        public int Width { get; set; }

        public int Height { get; set; }

        // folder for the upload, you can put this in the web.config
        public string UploadPath = ImageConfig.TYPE_FILE_LOCATION;

        public ImageResult RenameUploadFile(HttpPostedFileBase file, Int32 counter = 0)
        {
            var fileName = Path.GetFileName(file.FileName);

            string prepend = "item_";
            string finalFileName = prepend + ((counter).ToString()) + "_" + fileName;
            if (System.IO.File.Exists
                (HttpContext.Current.Request.MapPath(UploadPath + finalFileName)))
            {
                //file exists => add country try again
                return RenameUploadFile(file, ++counter);
            }
            //file doesn't exist, upload item but validate first
            return UploadFile(file, finalFileName);
        }

        private ImageResult UploadFile(HttpPostedFileBase file, string fileName)
        {
            ImageResult imageResult = new ImageResult { Success = true, ErrorMessage = null };

            var path =
          Path.Combine(HttpContext.Current.Request.MapPath(UploadPath), fileName);
            string extension = Path.GetExtension(file.FileName);

            //make sure the file is valid
            if (!ValidateExtension(extension))
            {
                imageResult.Success = false;
                imageResult.ErrorMessage = "Invalid Extension";
                return imageResult;
            }

            try
            {
                var directoryPath = HttpContext.Current.Request.MapPath(UploadPath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                file.SaveAs(path);

                Image imgOriginal = Image.FromFile(path);

                //pass in whatever value you want
                Image imgActual = Scale(imgOriginal);
                imgOriginal.Dispose();
                imgActual.Save(path);
                imgActual.Dispose();

                imageResult.ImageName = fileName;

                return imageResult;
            }
            catch (Exception ex)
            {
                // you might NOT want to show the exception error for the user
                // this is generaly logging or testing

                imageResult.Success = false;
                imageResult.ErrorMessage = ex.Message;
                return imageResult;
            }
        }

        public Image Load(string fileName)
        {
            var path = Path.Combine(HttpContext.Current.Request.MapPath(UploadPath), fileName);
            if (File.Exists(path))
                return Image.FromFile(path);
            return null;
        }

        public void Save(string fileName)
        {
            var path = Path.Combine(HttpContext.Current.Request.MapPath(UploadPath), fileName);

            Image loaded = Image.FromFile(path);

            Image newImage = Scale(loaded);
            Delete(fileName);
            newImage.Save(path);
            loaded.Dispose();
            newImage.Dispose();
        }

        public ImageResult Delete(string fileName)
        {
            var path = Path.Combine(HttpContext.Current.Request.MapPath(UploadPath), fileName);
            if (File.Exists(path))
                File.Delete(path);
            return new ImageResult { Success = true };
        }

        private bool ValidateExtension(string extension)
        {
            extension = extension.ToLower();
            switch (extension)
            {
                case ".jpg":
                    return true;
                case ".png":
                    return true;
                case ".gif":
                    return true;
                case ".jpeg":
                    return true;
                default:
                    return false;
            }
        }

        public Image Scale(Image imgPhoto)
        {
            float sourceWidth = imgPhoto.Width;
            float sourceHeight = imgPhoto.Height;
            float destHeight = 0;
            float destWidth = 0;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            // force resize, might distort image
            if (Width != 0 && Height != 0)
            {
                destWidth = Width;
                destHeight = Height;
            }
            // change size proportially depending on width or height
            else if (Height != 0)
            {
                destWidth = (float)(Height * sourceWidth) / sourceHeight;
                destHeight = Height;
            }
            else
            {
                destWidth = Width;
                destHeight = (float)(sourceHeight * Width / sourceWidth);
            }

            Bitmap bmPhoto = new Bitmap((int)destWidth, (int)destHeight,
                                        PixelFormat.Format32bppPArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, (int)destWidth, (int)destHeight),
                new Rectangle(sourceX, sourceY, (int)sourceWidth, (int)sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();

            return bmPhoto;
        }

        public bool SaveBase64Image(string imageBase64, string fullFilePath, string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
            }

            using (FileStream fs = new FileStream(fullFilePath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(imageBase64);
                    bw.Write(data);
                    bw.Close();
                    return true;
                }
            }
        }

        public bool SaveImage(string imageBase64, string fullFilePath, string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
            }

            using (FileStream fs = new FileStream(fullFilePath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(imageBase64);
                    bw.Write(data);
                    bw.Close();
                    return true;
                }
            }
        }

        public byte[] ConvertFromBase64StringToImage(string imageBase64)
        {
            if (string.IsNullOrEmpty(imageBase64))
                return null;
            return Convert.FromBase64String(imageBase64);
        }

        public byte[] LoadImage(string fullFilePath)
        {
            if (!File.Exists(fullFilePath))
                return null;
            using (FileStream fs = new FileStream(fullFilePath, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] data = br.ReadBytes(int.MaxValue);
                    br.Close();
                    return data;
                }
            }

        }
    }
}
