using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.ApplicationCore.IService.Application;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.DocumentProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Configurations
{
    public class ImportLocationController : Controller
    {
        ILocationRepository _locationRepository;
        ISystemSettingService _systemSettingService;

        public ImportLocationController(ILocationRepository locationRepository,
            ISystemSettingService systemSettingService)
        {
            this._locationRepository = locationRepository;
            this._systemSettingService = systemSettingService;
        }

        // GET: ImportLocation
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string post="")
        {
            List<Location> locations = new List<Location>();
            foreach(string uploadFile in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[uploadFile] as HttpPostedFileBase;
                if(file!=null && file.ContentLength>0)
                {
                    ExcelProcessor<Location> excelProcessor = new ExcelProcessor<Location>();
                    locations = excelProcessor.ImportFromExcel(file.InputStream);
                }
            }
            List<SystemSetting> settings = (await _systemSettingService.GetByCri(null)).ToList();
            var key = settings.FirstOrDefault(x => x.Code.Equals(AppSettingKey.Key.GOOGLE_MAP_KEY.ToString()))?.Value;
            var url = settings.FirstOrDefault(x => x.Code.Equals(AppSettingKey.Key.GOOGLE_MAP_URL.ToString()))?.Value;

            foreach (var location in locations)
            {
                try
                {
                    var result = GoogleMapUtil.GetCoordinateofLocation(location.PostalCode, key, url);

                    var lat = result.Split(',')[0];
                    var longi = result.Split(',')[1];
                    location.Latitude = lat;
                    location.Longitude = longi;
                }
                catch
                {

                }
            }
            try
            {

                await _locationRepository.Saves(locations);
            }
            catch(Exception ex)
            {
                string message = ex.Message;
            }
            TempData["color"] = ApplicationConfig.MESSAGE_SAVE_COLOR;
            TempData["message"] = ApplicationConfig.MESSAGE_IMPORT_MESSAGE;
            return RedirectToAction(nameof(Index));
        }
    }
}