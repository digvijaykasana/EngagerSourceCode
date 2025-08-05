using EngagerMark4.ApplicationCore.Cris.Jobs;
using EngagerMark4.ApplicationCore.ReportViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs
{
    public interface IDailySummaryJobByDriverRepository
    {
        IEnumerable<DailySummaryJobByDriverViewModel> GetReports(DailySummaryJobByDriverCri aCri);
    }
}
