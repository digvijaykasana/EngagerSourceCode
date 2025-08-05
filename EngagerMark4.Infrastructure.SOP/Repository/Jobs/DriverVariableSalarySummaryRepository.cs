using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using EngagerMark4.Common;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.SOP.Repository.Jobs
{
    public class DriverVariableSalarySummaryRepository : IDriverVariableSalarySummaryRepository
    {
        ApplicationDbContext _context;
        public DriverVariableSalarySummaryRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<DriverVariableSalaryReportViewModel> GetReport(DriverVariableSalaryReportCri aCri)
        {
            var query = from checklistItem in _context.ServiceJobChecklistItems.AsNoTracking()
                        join serviceJob in _context.ServiceJobs.AsNoTracking() on checklistItem.ServiceJobId equals serviceJob.Id
                        join workOrder in _context.WorkOrders.AsNoTracking() on serviceJob.WorkOrderId equals workOrder.Id
                        join user in _context.EngagerUsers.AsNoTracking() on serviceJob.UserId equals user.Id
                        join checklist in _context.CheckLists.AsNoTracking() on checklistItem.ChecklistId equals checklist.Id
                        where serviceJob.ParentCompanyId == GlobalVariable.COMPANY_ID
                        select new
                        DriverVariableSalaryReportViewModel
                        {
                            DriverId = user.Id,
                            DriverName = user.LastName + " " + user.FirstName,
                            DriverNRIC = user.NRIC,
                            PayItemCat = checklist.ShortText1, //PaymentItemCode
                            Amount = checklistItem.ChecklistPrice,
                            PickUpDate = workOrder.PickUpdateDate,
                            Salary = serviceJob.TripFees + serviceJob.MSFees,
                            ServiceJobId = serviceJob.Id
                        };

            if(aCri.Date.HasValue)
            {
                query = query.Where(x => x.PickUpDate >= aCri.Date.Value);
            }

            if(aCri.ToDate.HasValue)
            {
                query = query.Where(x => x.PickUpDate <= aCri.ToDate.Value);
            }

            return query.OrderBy(x => x.PickUpDate);
        }
    }
}
