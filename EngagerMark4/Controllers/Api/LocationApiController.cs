using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IService.Application;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api
{
    public class LocationApiController : Controller
    {
        ISystemSettingService _systemSettingService;
        public LocationApiController(ISystemSettingService systemSettingService)
        {
            this._systemSettingService = systemSettingService;
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetCoordinate(string aPostalCode = "")
        {
            List<SystemSetting> settings = (await _systemSettingService.GetByCri(null)).ToList();

            var key = settings.FirstOrDefault(x => x.Code.Equals(AppSettingKey.Key.GOOGLE_MAP_KEY.ToString()))?.Value;
            var url = settings.FirstOrDefault(x => x.Code.Equals(AppSettingKey.Key.GOOGLE_MAP_URL.ToString()))?.Value;
            var result = GoogleMapUtil.GetCoordinateofLocation(aPostalCode, key, url);

            Location location = new Location();
            try
            {
                var lat = result.Split(',')[0];
                var longi = result.Split(',')[1];
                location.Latitude = lat;
                location.Longitude = longi;
            }
            catch
            {

            }
            return Json(location, JsonRequestBehavior.AllowGet);
        }
    }
}