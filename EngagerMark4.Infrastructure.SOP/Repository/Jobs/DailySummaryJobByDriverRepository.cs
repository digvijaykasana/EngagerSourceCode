using EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EngagerMark4.ApplicationCore.Cris.Jobs;
using EngagerMark4.ApplicationCore.ReportViewModels;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using System.Data.Entity.SqlServer;
using EngagerMark4.Common;
using EngagerMark4.ApplicationCore.IRepository.Users;

namespace EngagerMark4.Infrastructure.SOP.Repository.Jobs
{
    public class DailySummaryJobByDriverRepository : IDailySummaryJobByDriverRepository
    {
        ApplicationDbContext _context;
        IUserRepository _userRepository;

        public DailySummaryJobByDriverRepository(ApplicationDbContext context,
            IUserRepository userRepository)
        {
            this._context = context;
            this._userRepository = userRepository;
        }

        public IEnumerable<DailySummaryJobByDriverViewModel> GetReports(DailySummaryJobByDriverCri aCri)
        {
            var dateTime = aCri.GetDateTime();

            List<ServiceJob> serviceJobs = _context.ServiceJobs.Include(x => x.WorkOrder).AsNoTracking()
                                                               .Where(x => SqlFunctions.DateDiff("MONTH", x.WorkOrder.PickUpdateDate, dateTime) == 0 
                                                                           && SqlFunctions.DateDiff("YEAR", x.WorkOrder.PickUpdateDate, dateTime) == 0 
                                                                           && x.WorkOrder.Status != ApplicationCore.SOP.Entities.WorkOrders.WorkOrder.OrderStatus. Cancelled 
                                                                           && x.ParentCompanyId == GlobalVariable.COMPANY_ID).ToList();

            List<DailySummaryJobByDriverViewModel> viewModels = new List<DailySummaryJobByDriverViewModel>();
            //List<DailySummaryJobByDriverViewModel> finalViewModels = new List<DailySummaryJobByDriverViewModel>();

            int days = DateTime.DaysInMonth(aCri.Year, aCri.Month);

            foreach (var driver in _userRepository.GetDrivers())
            {
                DailySummaryJobByDriverViewModel viewModel = new DailySummaryJobByDriverViewModel
                {
                    DriverId = driver.Id,
                    DriverName = driver.Name
                };

                int TotalWorkOrderByDriver = 0;

                for (int i = 1; i <= days; i++)
                {
                    DateTime date = new DateTime(aCri.Year, aCri.Month, i);
                    int numberOfServiceJobs = serviceJobs.Where(x => x.WorkOrder.PickUpdateDate.Value.Date == date.Date && x.UserId == driver.Id).ToList().Count;

                    switch (i)
                    {
                        case 1:
                            viewModel._1 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 2:
                            viewModel._2 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 3:
                            viewModel._3 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 4:
                            viewModel._4 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 5:
                            viewModel._5 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 6:
                            viewModel._6 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 7:
                            viewModel._7 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 8:
                            viewModel._8 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 9:
                            viewModel._9 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 10:
                            viewModel._10 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 11:
                            viewModel._11 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 12:
                            viewModel._12 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 13:
                            viewModel._13 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 14:
                            viewModel._14 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 15:
                            viewModel._15 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 16:
                            viewModel._16 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 17:
                            viewModel._17 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 18:
                            viewModel._18 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 19:
                            viewModel._19 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 20:
                            viewModel._20 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 21:
                            viewModel._21 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 22:
                            viewModel._22 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 23:
                            viewModel._23 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 24:
                            viewModel._24 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 25:
                            viewModel._25 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 26:
                            viewModel._26 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 27:
                            viewModel._27 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 28:
                            viewModel._28 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 29:
                            viewModel._29 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 30:
                            viewModel._30 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        case 31:
                            viewModel._31 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                        default:
                            viewModel._1 = numberOfServiceJobs;
                            TotalWorkOrderByDriver += numberOfServiceJobs;
                            break;
                    }


                }

                viewModel.Total = TotalWorkOrderByDriver;
                viewModels.Add(viewModel);

                //finalViewModels = viewModels;
            }

            //Adding Last Total Row

            DailySummaryJobByDriverViewModel totalViewModel = new DailySummaryJobByDriverViewModel();

            totalViewModel.DriverName = "Total";

            for (int i = 1; i <= days; i++)
            {
                DateTime date = new DateTime(aCri.Year, aCri.Month, i);

                switch (i)
                {
                    case 1:
                        totalViewModel._1 = viewModels.Sum(x => x._1);
                        break;
                    case 2:
                        totalViewModel._2 = viewModels.Sum(x => x._2);
                        break;
                    case 3:
                        totalViewModel._3 = viewModels.Sum(x => x._3);
                        break;
                    case 4:
                        totalViewModel._4 = viewModels.Sum(x => x._4);
                        break;
                    case 5:
                        totalViewModel._5 = viewModels.Sum(x => x._5);
                        break;
                    case 6:
                        totalViewModel._6 = viewModels.Sum(x => x._6);
                        break;
                    case 7:
                        totalViewModel._7 = viewModels.Sum(x => x._7);
                        break;
                    case 8:
                        totalViewModel._8 = viewModels.Sum(x => x._8);
                        break;
                    case 9:
                        totalViewModel._9 = viewModels.Sum(x => x._9);
                        break;
                    case 10:
                        totalViewModel._10 = viewModels.Sum(x => x._10);
                        break;
                    case 11:
                        totalViewModel._11 = viewModels.Sum(x => x._11);
                        break;
                    case 12:
                        totalViewModel._12 = viewModels.Sum(x => x._12);
                        break;
                    case 13:
                        totalViewModel._13 = viewModels.Sum(x => x._13);
                        break;
                    case 14:
                        totalViewModel._14 = viewModels.Sum(x => x._14);
                        break;
                    case 15:
                        totalViewModel._15 = viewModels.Sum(x => x._15);
                        break;
                    case 16:
                        totalViewModel._16 = viewModels.Sum(x => x._16);
                        break;
                    case 17:
                        totalViewModel._17 = viewModels.Sum(x => x._17);
                        break;
                    case 18:
                        totalViewModel._18 = viewModels.Sum(x => x._18);
                        break;
                    case 19:
                        totalViewModel._19 = viewModels.Sum(x => x._19);
                        break;
                    case 20:
                        totalViewModel._20 = viewModels.Sum(x => x._20);
                        break;
                    case 21:
                        totalViewModel._21 = viewModels.Sum(x => x._21);
                        break;
                    case 22:
                        totalViewModel._22 = viewModels.Sum(x => x._22);
                        break;
                    case 23:
                        totalViewModel._23 = viewModels.Sum(x => x._23);
                        break;
                    case 24:
                        totalViewModel._24 = viewModels.Sum(x => x._24);
                        break;
                    case 25:
                        totalViewModel._25 = viewModels.Sum(x => x._25);
                        break;
                    case 26:
                        totalViewModel._26 = viewModels.Sum(x => x._26);
                        break;
                    case 27:
                        totalViewModel._27 = viewModels.Sum(x => x._27);
                        break;
                    case 28:
                        totalViewModel._28 = viewModels.Sum(x => x._28);
                        break;
                    case 29:
                        totalViewModel._29 = viewModels.Sum(x => x._29);
                        break;
                    case 30:
                        totalViewModel._30 = viewModels.Sum(x => x._30);
                        break;
                    case 31:
                        totalViewModel._31 = viewModels.Sum(x => x._31);
                        break;
                    default:
                        totalViewModel._1 = viewModels.Sum(x => x._1);

                        break;
                }
            }

            totalViewModel.Total = viewModels.Sum(x => x.Total);

            viewModels.Add(totalViewModel);

            return viewModels;
        }
    }
}
