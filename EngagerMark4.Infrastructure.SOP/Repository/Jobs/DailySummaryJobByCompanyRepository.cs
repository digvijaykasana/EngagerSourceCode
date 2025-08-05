using EngagerMark4.ApplicationCore.Cris.Jobs;
using EngagerMark4.ApplicationCore.ReportViewModels;
using EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using System.Data.Entity.SqlServer;
using EngagerMark4.Common;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;

namespace EngagerMark4.Infrastructure.SOP.Repository.Jobs
{
    public class DailySummaryJobByCompanyRepository : IDailySummaryJobByCompanyRepository
    {
        public string DAY = "DAY";
        public string MONTH = "MONTH";
        public string YEAR = "YEAR";

        ApplicationDbContext _context;

        public DailySummaryJobByCompanyRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<DailySummaryJobByCompanyViewModel> GetReport(DailySummaryJobByCompanyCri aCri)
        {
            var dateTime = aCri.GetDateTime();


            List<ServiceJob> serviceJobs = _context.ServiceJobs.Include(x => x.WorkOrder).AsNoTracking()
                                                               .Where(x => SqlFunctions.DateDiff("MONTH", x.WorkOrder.PickUpdateDate, dateTime) == 0
                                                                           && SqlFunctions.DateDiff("YEAR", x.WorkOrder.PickUpdateDate, dateTime) == 0
                                                                           && x.WorkOrder.Status != ApplicationCore.SOP.Entities.WorkOrders.WorkOrder.OrderStatus.Cancelled
                                                                           && x.ParentCompanyId == GlobalVariable.COMPANY_ID).ToList();
            //List<WorkOrder> workOrders = _context.WorkOrders.AsNoTracking().Where(x => SqlFunctions.DateDiff("MONTH", x.PickUpdateDate, dateTime) == 0 && SqlFunctions.DateDiff("YEAR", x.PickUpdateDate, dateTime) == 0 && x.ParentCompanyId == GlobalVariable.COMPANY_ID ).ToList();

            List<DailySummaryJobByCompanyViewModel> viewModels = new List<DailySummaryJobByCompanyViewModel>();

            int days = DateTime.DaysInMonth(aCri.Year, aCri.Month);

            foreach(var company in _context.Customers.AsNoTracking().OrderBy(x => x.Name))
            {
                DailySummaryJobByCompanyViewModel viewModel = new DailySummaryJobByCompanyViewModel
                {
                    CustomerName = company.Name,
                };

                int TotalWorkOrderByCompany = 0;

                for (int i = 1; i <= days; i++)
                {
                    DateTime date = new DateTime(aCri.Year, aCri.Month, i);
                    int numberOfServiceJobs = serviceJobs.Where(x => x.WorkOrder.PickUpdateDate.Value.Date == date.Date && x.CustomerId == company.Id).ToList().Count;

                    switch (i)
                    {
                        case 1:
                            viewModel._1 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 2:
                            viewModel._2 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 3:
                            viewModel._3 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 4:
                            viewModel._4 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 5:
                            viewModel._5 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 6:
                            viewModel._6 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 7:
                            viewModel._7 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 8:
                            viewModel._8 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 9:
                            viewModel._9 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 10:
                            viewModel._10 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 11:
                            viewModel._11 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 12:
                            viewModel._12 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 13:
                            viewModel._13 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 14:
                            viewModel._14 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 15:
                            viewModel._15 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 16:
                            viewModel._16 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 17:
                            viewModel._17 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 18:
                            viewModel._18 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 19:
                            viewModel._19 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 20:
                            viewModel._20 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 21:
                            viewModel._21 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 22:
                            viewModel._22 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 23:
                            viewModel._23 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 24:
                            viewModel._24 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 25:
                            viewModel._25 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 26:
                            viewModel._26 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 27:
                            viewModel._27 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 28:
                            viewModel._28 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 29:
                            viewModel._29 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 30:
                            viewModel._30 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        case 31:
                            viewModel._31 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                        default:
                            viewModel._1 = numberOfServiceJobs;
                            TotalWorkOrderByCompany += numberOfServiceJobs;
                            break;
                    }

                }

                viewModel.Total = TotalWorkOrderByCompany;

                viewModels.Add(viewModel);
            }

            //Adding Last Total Row
            DailySummaryJobByCompanyViewModel totalViewModel = new DailySummaryJobByCompanyViewModel();
            totalViewModel.CustomerName = "Total";

            List<long> excludedCustomerIds = _context.Customers.AsNoTracking().Where(x => x.YesNo2).Select(x => x.Id).ToList();

            for (int i = 1; i <= days; i++)
            {
                DateTime date = new DateTime(aCri.Year, aCri.Month, i);
                int numberOfExcludedServiceJobs = serviceJobs.Where(x => x.WorkOrder.PickUpdateDate.Value.Date == date.Date && excludedCustomerIds.Contains(x.CustomerId.Value)).ToList().Count;

                switch (i)
                {
                    case 1:
                        totalViewModel._1 = viewModels.Sum(x => x._1);
                        totalViewModel._1 = totalViewModel._1 - numberOfExcludedServiceJobs;
                        break;
                    case 2:
                        totalViewModel._2 = viewModels.Sum(x => x._2);
                        totalViewModel._2 = totalViewModel._2 - numberOfExcludedServiceJobs;
                        break;
                    case 3:
                        totalViewModel._3 = viewModels.Sum(x => x._3);
                        totalViewModel._3 = totalViewModel._3 - numberOfExcludedServiceJobs;
                        break;
                    case 4:
                        totalViewModel._4 = viewModels.Sum(x => x._4);
                        totalViewModel._4 = totalViewModel._4 - numberOfExcludedServiceJobs;
                        break;
                    case 5:
                        totalViewModel._5 = viewModels.Sum(x => x._5);
                        totalViewModel._5 = totalViewModel._5 - numberOfExcludedServiceJobs;
                        break;
                    case 6:
                        totalViewModel._6 = viewModels.Sum(x => x._6);
                        totalViewModel._6 = totalViewModel._6 - numberOfExcludedServiceJobs;
                        break;
                    case 7:
                        totalViewModel._7 = viewModels.Sum(x => x._7);
                        totalViewModel._7 = totalViewModel._7 - numberOfExcludedServiceJobs;
                        break;
                    case 8:
                        totalViewModel._8 = viewModels.Sum(x => x._8);
                        totalViewModel._8 = totalViewModel._8 - numberOfExcludedServiceJobs;
                        break;
                    case 9:
                        totalViewModel._9 = viewModels.Sum(x => x._9);
                        totalViewModel._9 = totalViewModel._9 - numberOfExcludedServiceJobs;
                        break;
                    case 10:
                        totalViewModel._10 = viewModels.Sum(x => x._10);
                        totalViewModel._10 = totalViewModel._10 - numberOfExcludedServiceJobs;
                        break;
                    case 11:
                        totalViewModel._11 = viewModels.Sum(x => x._11);
                        totalViewModel._11 = totalViewModel._11 - numberOfExcludedServiceJobs;
                        break;
                    case 12:
                        totalViewModel._12 = viewModels.Sum(x => x._12);
                        totalViewModel._12 = totalViewModel._12 - numberOfExcludedServiceJobs;
                        break;
                    case 13:
                        totalViewModel._13 = viewModels.Sum(x => x._13);
                        totalViewModel._13 = totalViewModel._13 - numberOfExcludedServiceJobs;
                        break;
                    case 14:
                        totalViewModel._14 = viewModels.Sum(x => x._14);
                        totalViewModel._14 = totalViewModel._14 - numberOfExcludedServiceJobs;
                        break;
                    case 15:
                        totalViewModel._15 = viewModels.Sum(x => x._15);
                        totalViewModel._15 = totalViewModel._15 - numberOfExcludedServiceJobs;
                        break;
                    case 16:
                        totalViewModel._16 = viewModels.Sum(x => x._16);
                        totalViewModel._16 = totalViewModel._16 - numberOfExcludedServiceJobs;
                        break;
                    case 17:
                        totalViewModel._17 = viewModels.Sum(x => x._17);
                        totalViewModel._17 = totalViewModel._17 - numberOfExcludedServiceJobs;
                        break;
                    case 18:
                        totalViewModel._18 = viewModels.Sum(x => x._18);
                        totalViewModel._18 = totalViewModel._18 - numberOfExcludedServiceJobs;
                        break;
                    case 19:
                        totalViewModel._19 = viewModels.Sum(x => x._19);
                        totalViewModel._19 = totalViewModel._19 - numberOfExcludedServiceJobs;
                        break;
                    case 20:
                        totalViewModel._20 = viewModels.Sum(x => x._20);
                        totalViewModel._20 = totalViewModel._20 - numberOfExcludedServiceJobs;
                        break;
                    case 21:
                        totalViewModel._21 = viewModels.Sum(x => x._21);
                        totalViewModel._21 = totalViewModel._21 - numberOfExcludedServiceJobs;
                        break;
                    case 22:
                        totalViewModel._22 = viewModels.Sum(x => x._22);
                        totalViewModel._22 = totalViewModel._22 - numberOfExcludedServiceJobs;
                        break;
                    case 23:
                        totalViewModel._23 = viewModels.Sum(x => x._23);
                        totalViewModel._23 = totalViewModel._23 - numberOfExcludedServiceJobs;
                        break;
                    case 24:
                        totalViewModel._24 = viewModels.Sum(x => x._24);
                        totalViewModel._24 = totalViewModel._24 - numberOfExcludedServiceJobs;
                        break;
                    case 25:
                        totalViewModel._25 = viewModels.Sum(x => x._25);
                        totalViewModel._25 = totalViewModel._25 - numberOfExcludedServiceJobs;
                        break;
                    case 26:
                        totalViewModel._26 = viewModels.Sum(x => x._26);
                        totalViewModel._26 = totalViewModel._26 - numberOfExcludedServiceJobs;
                        break;
                    case 27:
                        totalViewModel._27 = viewModels.Sum(x => x._27);
                        totalViewModel._27 = totalViewModel._27 - numberOfExcludedServiceJobs;
                        break;
                    case 28:
                        totalViewModel._28 = viewModels.Sum(x => x._28);
                        totalViewModel._28 = totalViewModel._28 - numberOfExcludedServiceJobs;
                        break;
                    case 29:
                        totalViewModel._29 = viewModels.Sum(x => x._29);
                        totalViewModel._29 = totalViewModel._29 - numberOfExcludedServiceJobs;
                        break;
                    case 30:
                        totalViewModel._30 = viewModels.Sum(x => x._30);
                        totalViewModel._30 = totalViewModel._30 - numberOfExcludedServiceJobs;
                        break;
                    case 31:
                        totalViewModel._31 = viewModels.Sum(x => x._31);
                        totalViewModel._31 = totalViewModel._31 - numberOfExcludedServiceJobs;
                        break;
                    default:
                        totalViewModel._1 = viewModels.Sum(x => x._1);

                        break;
                }
            }

            totalViewModel.Total = viewModels.Sum(x => x.Total);

            //PCR2021 - Exclude Counts for Customer (MTS) with ExcludeInCustomerDailySummaryReport = true
            var excludeJobs = from serviceJob in serviceJobs
                               join customer in _context.Customers.AsNoTracking() on serviceJob.CustomerId equals customer.Id
                               where customer.YesNo2 == true //ExcludeInCustomerDailySummaryReport
                               select serviceJob;

            if(excludeJobs.Any())
            {
                totalViewModel.Total = totalViewModel.Total - excludeJobs.Count();
            }

            viewModels.Add(totalViewModel);

            return viewModels;
        }
    }
}
