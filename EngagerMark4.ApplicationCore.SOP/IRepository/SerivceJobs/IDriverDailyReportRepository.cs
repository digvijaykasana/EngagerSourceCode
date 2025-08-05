using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.ReportViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs
{
    public interface IDriverDailyReportRepository
    {
        IEnumerable<DriverDailyReportViewModel> GetDriverDailyReport(DriverDailyReportCri cri);

        IEnumerable<DriverDailyReportViewModelMobile> GetDriverDailyReportForMobile(DriverDailyReportCri cri);
    }
}
