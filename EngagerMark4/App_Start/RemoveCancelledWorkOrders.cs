using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.ApplicationCore.SOP.IRepository.WorkOrders;
using EngagerMark4.Common.Configs;
using EngagerMark4.Infrastructure.SOP.Repository;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository.Application;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EngagerMark4.App_Start
{
    /// <summary>
    /// PCR2021 - Phase 3
    /// Remove Cancelled Orders after a specified period of time
    /// </summary>
    public class RemoveCancelledWorkOrders
    {
        public static void Start(ApplicationDbContext db)
        {
            try
            {
                ISystemSettingRepository systemSettingRepository = new SystemSettingRepository(db);

                var removalIntervalKey = AppSettingKey.Key.WORKORDER_CANCELLEDJOB_REMOVAL_INTERVAL_MIN.ToString();
                var setting = db.SystemSettings.AsNoTracking().FirstOrDefault(x => x.Code.Equals(removalIntervalKey));

                int removalPeriodMins = 0;
                Int32.TryParse(setting.Value, out removalPeriodMins);

                if (removalPeriodMins > 0)
                {
                    SqlParameter removalPeriodMinsPararm = new SqlParameter("@RemovalPeriodMins", removalPeriodMins);

                    var affectedRows = db.Database.ExecuteSqlCommand("SP_RemoveCancelledOrders @RemovalPeriodMins", removalPeriodMinsPararm);
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}