using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.Common.Configs;
using EngagerMark4.DocumentProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;

namespace EngagerMark4.Controllers.Configurations
{
    public class ImportRankController : Controller
    {
        ICommonConfigurationRepository _commonConfigurationRepository;
        IConfigurationGroupRepository _configurationGroupRepository;

        public ImportRankController(ICommonConfigurationRepository commonConfigurationRepository,
            IConfigurationGroupRepository configurationGroupRepository)
        {
            this._commonConfigurationRepository = commonConfigurationRepository;
            this._configurationGroupRepository = configurationGroupRepository;
        }

        // GET: ImportRank
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string post = "")
        {
            var groups = await _configurationGroupRepository.GetByCri(null);
            var rank = groups.Where(s => s.Code.Equals(ConfigurationGrpCodes.Rank.ToString())).FirstOrDefault();

            List<CommonConfiguration> ranks = new List<CommonConfiguration>();
            
            foreach(string uploadFile in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[uploadFile] as HttpPostedFileBase;

                if(file!=null && file.ContentLength > 0)
                {
                    ExcelProcessor<CommonConfiguration> excelProcessor = new ExcelProcessor<CommonConfiguration>();
                    ranks = excelProcessor.ImportFromExcel(file.InputStream);
                }

                
            }

            foreach (var obj in ranks)
            {
                obj.ConfigurationGroupId = rank.Id;
            }

            await this._commonConfigurationRepository.Saves(ranks);
            TempData["color"] = ApplicationConfig.MESSAGE_SAVE_COLOR;
            TempData["message"] = ApplicationConfig.MESSAGE_IMPORT_MESSAGE;
            return RedirectToAction(nameof(Index));
        }
    }
}