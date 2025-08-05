using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.Job.IService;
using EngagerMark4.ApplicationCore.ReportViewModels;
using EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs;
using EngagerMark4.Common;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.DocumentProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Report
{
    public class DriverDailyReportController : BaseController<UserCri, User, IUserService>
    {
        IUserService _userService;
        IDriverDailyReportRepository _reportRepository;
        ICompanyService _companyService;
        ICheckListService _checkListService;

        public DriverDailyReportController(IUserService service,
            IDriverDailyReportRepository reportRepository,
            ICompanyService companyService,
            ICheckListService checkListService) : base(service)
        {
            this._userService = service;
            this._reportRepository = reportRepository;
            this._companyService = companyService;
            this._checkListService = checkListService;
        }

        protected async override Task LoadReferencesForList(UserCri aCri)
        {
            var users = _userService.GetDrivers();

            ViewBag.DriverId = new SelectList(users, "Id", "Name", aCri.DriverId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(DriverDailyReportCri aCri)
        {
            try
            {

                if (aCri.DriverId == null)
                {
                    await LoadReferencesForList(new UserCri { DriverId = aCri.DriverId == null ? 0 : aCri.DriverId.Value });
                    ModelState.AddModelError("", "Please select a driver!");
                }

                var driver = await _userService.GetById(aCri.DriverId);

                var company = await _companyService.GetById(GlobalVariable.COMPANY_ID);

                var checkLists = await _checkListService.GetByCri(new ApplicationCore.Job.Cris.CheckListCri());

                if (driver == null) driver = new ApplicationCore.Entities.Users.User();
                var driverJobs = _reportRepository.GetDriverDailyReport(aCri).ToList();

                foreach (var job in driverJobs)
                {
                    if (!string.IsNullOrEmpty(job.CheckListIds))
                    {
                        var checklistItems = job.GetChecklistItemList();

                        if (checklistItems != null && checklistItems.Count > 0)
                        {
                            //Parking Fees
                            var parkFeeChecklist = checkLists.Where(x => x.Code == ChecklistConfig.CarParkCode).FirstOrDefault();
                            if (parkFeeChecklist != null)
                            {
                                var parkFeeItem = checklistItems.Where(x => x.ChecklistId == parkFeeChecklist.Id).FirstOrDefault();

                                if (parkFeeItem != null) job.ParkFee = parkFeeItem.ChecklistPrice;
                            }

                            //ERP
                            var erpChecklist = checkLists.Where(x => x.Code == ChecklistConfig.ERPCode).FirstOrDefault();
                            if (erpChecklist != null)
                            {
                                var erpItem = checklistItems.Where(x => x.ChecklistId == erpChecklist.Id).FirstOrDefault();

                                if (erpItem != null) job.ERP = erpItem.ChecklistPrice;
                            }

                            //Meal
                            var mealChecklist = checkLists.Where(x => x.Code == ChecklistConfig.MealsCode).FirstOrDefault();
                            if (mealChecklist != null)
                            {
                                var mealItem = checklistItems.Where(x => x.ChecklistId == mealChecklist.Id).FirstOrDefault();

                                if (mealItem != null) job.Meal = mealItem.ChecklistPrice;
                            }

                            //NEA Cert
                            var neaCertChecklist = checkLists.Where(x => x.Code == ChecklistConfig.NEACertFeeCode).FirstOrDefault();
                            if (neaCertChecklist != null)
                            {
                                var neaCertItem = checklistItems.Where(x => x.ChecklistId == neaCertChecklist.Id).FirstOrDefault();

                                if (neaCertItem != null) job.NEACert = neaCertItem.ChecklistPrice;
                            }

                            //Ferry Ticket
                            var ferryTicketChecklist = checkLists.Where(x => x.Code == ChecklistConfig.FerryTicketCode).FirstOrDefault();
                            if (ferryTicketChecklist != null)
                            {
                                var ferryTicketFeeItem = checklistItems.Where(x => x.ChecklistId == ferryTicketChecklist.Id).FirstOrDefault();

                                if (ferryTicketFeeItem != null) job.FerryTicket = ferryTicketFeeItem.ChecklistPrice;
                            }

                            //JP Pass Fees
                            var jpPassChecklist = checkLists.Where(x => x.Code == ChecklistConfig.JPPassFeeCode).FirstOrDefault();
                            if (jpPassChecklist != null)
                            {
                                var jpPassFeeItem = checklistItems.Where(x => x.ChecklistId == jpPassChecklist.Id).FirstOrDefault();

                                if (jpPassFeeItem != null) job.JPPass = jpPassFeeItem.ChecklistPrice;
                            }

                            //PSA Pass Fees
                            var psaPassChecklist = checkLists.Where(x => x.Code == ChecklistConfig.PSAPassFeeCode).FirstOrDefault();
                            if (psaPassChecklist != null)
                            {
                                var psaPassFeeItem = checklistItems.Where(x => x.ChecklistId == psaPassChecklist.Id).FirstOrDefault();

                                if (psaPassFeeItem != null) job.PSAPass = psaPassFeeItem.ChecklistPrice;
                            }

                            //Others
                            var othersChecklist = checkLists.Where(x => x.Code == ChecklistConfig.OthersCode).FirstOrDefault();
                            if (othersChecklist != null)
                            {
                                var othersItem = checklistItems.Where(x => x.ChecklistId == othersChecklist.Id).FirstOrDefault();

                                if (othersItem != null) job.Others = othersItem.ChecklistPrice;
                            }
                        }
                    }
                }

                var driverJob = driverJobs.FirstOrDefault();

                if (driverJob == null) driverJob = new DriverDailyReportViewModel();

                DriverDailyPDF pdf = new DriverDailyPDF
                {
                    CompanyName = company.Name,
                    CompanyLogo = company.Logo,
                    DriverName = driver.Name,
                    FromDate = aCri.DateStr,
                    ToDate = aCri.ToDateStr,
                    VehicleNo = driverJob.VehicleNo,
                    Models = driverJobs
                };

                if (aCri.GeneratedReportType == ApplicationCore.Cris.BaseCri.ReportType.ByJob)
                {
                    switch (aCri.GeneratedReportFormat)
                    {
                        case ApplicationCore.Cris.BaseCri.ReportFormat.Excel:
                            ExcelProcessor<DriverDailyReportViewModel> processor = new ExcelProcessor<DriverDailyReportViewModel>();
                            string path = processor.ExportToExcel(pdf.Models, Server.MapPath(FileConfig.ExportExcelDriverDaily), KeyUtil.GenerateKey());
                            return base.File(path, CONTENT_DISPOSITION, $"{driver.Name}_DriverDailyReport.xls");

                        case ApplicationCore.Cris.BaseCri.ReportFormat.PDF:
                            return base.File(base.GeneratePDF<DriverDailyPDF>(pdf, FileConfig.DRIVER_DAILY_REPORT, $"{driver.Name}_DriverDailyReport"), CONTENT_DISPOSITION, $"{driver.Name}_DriverDailyReport.pdf");

                        default:
                            return base.File(base.GeneratePDF<DriverDailyPDF>(pdf, FileConfig.DRIVER_DAILY_REPORT, $"{driver.Name}_DriverDailyReport"), CONTENT_DISPOSITION, $"{driver.Name}_DriverDailyReport.pdf");
                    }
                }
                else
                {
                    //PCR2021 - Preparing Title
                    List<CustomTitle> customTitles = new List<CustomTitle>();
                    customTitles.Add(new CustomTitle() { ColumnNo = 0, Value = "DRIVER MONTHLY SALARY FOR THE MONTH OF:" });
                    customTitles.Add(new CustomTitle() { ColumnNo = 1, Value = aCri.DateStr + "-" + aCri.ToDateStr });
                    customTitles.Add(new CustomTitle() { ColumnNo = 2, Value = "NAME OF DRIVER:" });
                    customTitles.Add(new CustomTitle() { ColumnNo = 3, Value = driver.Name + " - (" + driver.NRIC + ")" });

                    //PCR2021 - Preparing Data
                    var jobDays = driverJobs.Where(x => x.PickUpDate.HasValue).OrderBy(x => x.PickUpDate).Select(x => x.PickUpDate.Value.Date).Distinct();
                    
                    //Get Date Range from Criteria if exists
                    List<DateTime> aCriDays = new List<DateTime>();
                    if(aCri.Date.HasValue || aCri.ToDate.HasValue)
                    {
                        var aCriStartDate = aCri.Date.HasValue ? aCri.Date.Value : TimeUtil.GetLocalTime();
                        var aCriEndDate = aCri.ToDate.HasValue ? aCri.ToDate.Value : TimeUtil.GetLocalTime();

                        aCriDays = Util.GetDates(aCriStartDate, aCriEndDate);
                    }
                    List<int> months = new List<int>();
                    List<int> years = new List<int>();
                    if (aCriDays.Count > 0)
                    {
                        months = aCriDays.Select(x => x.Month).Distinct().ToList();
                        years = aCriDays.Select(x => x.Year).Distinct().ToList();
                    }
                    else
                    {
                        months = jobDays.Select(x => x.Month).Distinct().ToList();
                        years = jobDays.Select(x => x.Year).Distinct().ToList();
                    }


                    List<DriverDailyReportByDayViewModel> data = new List<DriverDailyReportByDayViewModel>();
                    foreach (var year in years)
                    {
                        foreach (var month in months)
                        {
                            var currentMonthFirstDay = new DateTime(year, month, 1);

                            //Add Month Title Row
                            DriverDailyReportByDayViewModel monthModel = new DriverDailyReportByDayViewModel()
                            {
                                DateStr = currentMonthFirstDay.ToString("MMM") + "'" + currentMonthFirstDay.ToString("yy")
                            };

                            data.Add(monthModel);

                            //Add Day Rows
                            var daysInMonth = Util.GetDates(year, month);

                            foreach (var day in daysInMonth)
                            {
                                if (aCri.Date.HasValue && day.Date < aCri.Date.Value) continue;
                                if (aCri.ToDate.HasValue && day.Date > aCri.ToDate.Value) continue;

                                var currentDayJobs = driverJobs.Where(x => x.PickUpDate.HasValue && x.PickUpDate.Value.Date == day.Date);

                                if (currentDayJobs.Any())
                                {
                                    DriverDailyReportByDayViewModel dayModel = new DriverDailyReportByDayViewModel()
                                    {
                                        DateStr = day.Date.ToString("dd"),
                                        TripFees = currentDayJobs.Sum(x => x.Trip),
                                        MSFees = currentDayJobs.Sum(x => x.MS),
                                        ParkFees = currentDayJobs.Sum(x => x.ParkFee),
                                        ERPFees = currentDayJobs.Sum(x => x.ERP),
                                        MealFees = currentDayJobs.Sum(x => x.Meal),
                                        NEACertFees = currentDayJobs.Sum(x => x.NEACert),
                                        FerryTicketFees = currentDayJobs.Sum(x => x.FerryTicket),
                                        JPPassFees = currentDayJobs.Sum(x => x.JPPass),
                                        PSAPassFees = currentDayJobs.Sum(x => x.PSAPass),
                                        OtherFees = currentDayJobs.Sum(x => x.Others)
                                    };

                                    data.Add(dayModel);
                                }
                                else
                                {
                                    data.Add(new DriverDailyReportByDayViewModel()
                                    {
                                        DateStr = day.Date.ToString("dd")
                                    });
                                }
                            }
                        }
                    }

                    switch (aCri.GeneratedReportFormat)
                    {
                        case ApplicationCore.Cris.BaseCri.ReportFormat.Excel:
                            ExcelProcessor<DriverDailyReportByDayViewModel> byDayProcessor = new ExcelProcessor<DriverDailyReportByDayViewModel>();
                            string byDayPath = byDayProcessor.ExportToExcel(data, Server.MapPath(FileConfig.ExportExcelDriverDaily), KeyUtil.GenerateKey(), false, customTitles);
                            return base.File(byDayPath, CONTENT_DISPOSITION, $"{driver.Name}_DriverDailyReport.xls");

                        case ApplicationCore.Cris.BaseCri.ReportFormat.PDF:

                            DriverDailyPDFByDay pdfByDay = new DriverDailyPDFByDay
                            {
                                DriverName = driver.Name,
                                DriverNRIC = driver.NRIC,
                                FromDate = aCri.DateStr,
                                ToDate = aCri.ToDateStr,
                                Models = data
                            };

                            return base.File(base.GeneratePDF<DriverDailyPDFByDay>(pdfByDay, FileConfig.DRIVER_DAILY_REPORT, $"{driver.Name}_DriverDailyReport"), CONTENT_DISPOSITION, $"{driver.Name}_DriverDailyReport.pdf");

                        default:
                            ExcelProcessor<DriverDailyReportByDayViewModel> defaultProcessor = new ExcelProcessor<DriverDailyReportByDayViewModel>();
                            string defaultPath = defaultProcessor.ExportToExcel(data, Server.MapPath(FileConfig.ExportExcelDriverDaily), KeyUtil.GenerateKey(), false, customTitles);
                            return base.File(defaultPath, CONTENT_DISPOSITION, $"{driver.Name}_DriverDailyReport.xls");

                    }
                }
            }
            catch(Exception ex)
            {
                return View("Error");
            }          
        }
    }
}