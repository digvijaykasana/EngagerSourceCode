using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs
{
    public interface IDriverVariableSalarySummaryRepository
    {
        IEnumerable<DriverVariableSalaryReportViewModel> GetReport(DriverVariableSalaryReportCri cri);
    }
}
