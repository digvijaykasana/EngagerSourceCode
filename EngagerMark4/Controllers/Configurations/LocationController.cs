using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IService.Application;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Configurations
{
    /// <summary>
    /// Controller Class for Location
    /// Created     : Ye Kaung Aung
    /// Modified    : 
    /// </summary>
    public class LocationController : BaseController<LocationCri, Location, ILocationService>
    {
        ISystemSettingService _systemSettingService;

        public LocationController(ILocationService service,
            ISystemSettingService aSystemSettingService): base(service)
        {
            this._defaultColumn = "Name";
            this._systemSettingService = aSystemSettingService;
        }

        protected override LocationCri GetCri()
        {
            var cri = base.GetCri();
            cri.SearchValue = Request["SearchValue"];
            return cri;
        }

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