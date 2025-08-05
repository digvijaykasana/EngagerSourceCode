using EngagerMark4.ApplicationCore.Cris.Application;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.Common;
using EngagerMark4.Common.Configs;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EngagerMark4.Infrasturcture.Repository.Application
{
    public class SystemSettingRepository : GenericRepository<ApplicationDbContext, SystemSettingCri, SystemSetting>, ISystemSettingRepository
    {
        public SystemSettingRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public async Task<int> GetWorkOrderNotifiedDay()
        {
            int notifiedDay = 1;

            var key = AppSettingKey.Key.WORKORDER_PUSH_NOTIFIED_DAY.ToString();
            var setting = await context.SystemSettings.AsNoTracking().FirstOrDefaultAsync(x => x.Code.Equals(key) && x.ParentCompanyId == GlobalVariable.COMPANY_ID);

            if (setting == null)
                return notifiedDay -1;

            Int32.TryParse(setting.Value, out notifiedDay);

            return notifiedDay - 1;
        }
    }
}
