using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.PDFs;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.DocumentProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;

namespace EngagerMark4.Controllers.Report
{
    public class DriverVariableSalaryReportController : BaseController<WorkOrderCri, WorkOrder, IWorkOrderService>
    {
        IDriverVariableSalarySummaryRepository _reportRepository;
        ICommonConfigurationService _commonConfigurationService;

        // GET: DriverVariableSalaryReport
        public DriverVariableSalaryReportController(IWorkOrderService service,
            IDriverVariableSalarySummaryRepository reportRepository,
            ICommonConfigurationService commonConfigurationService) : base(service)
        {
            this._reportRepository = reportRepository;
            this._commonConfigurationService = commonConfigurationService;
        }

        protected async override Task LoadReferencesForList(WorkOrderCri aCri)
        {
            string monthStr = Request["Month"];
            string yearStr = Request["Year"];

            int month = string.IsNullOrEmpty(monthStr) ? TimeUtil.GetLocalTime().Month : Convert.ToInt32(monthStr);
            int year = string.IsNullOrEmpty(yearStr) ? TimeUtil.GetLocalTime().Year : Convert.ToInt32(yearStr);
            ViewBag.Month = new SelectList(TimeUtil.GetMonths(), "Id", "Value", month);
            ViewBag.Year = new SelectList(TimeUtil.GetYears(), "Id", "Value", year);
            await base.LoadReferencesForList(aCri);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(DriverVariableSalaryReportCri aCri)
        {
            try
            {
                var fromDateStr =  aCri.ReportFromDateStr;
                var toDateStr = aCri.ReportToDateStr;

                //Get Salary Pay Items
                CommonConfigurationCri cri = new CommonConfigurationCri();
                cri.Includes = new List<string>();
                cri.Includes.Add("ConfigurationGroup");
                var configs = await _commonConfigurationService.GetByCri(cri);

                var payItemConfigs = configs.Where(x => x.ConfigurationGroup.Code == ConfigurationGrpCodes.SalaryPayItem.ToString()).ToList();

                var data = this._reportRepository.GetReport(aCri).ToList();

                List<DriverVariableSalaryReportViewModel> finalData = new List<DriverVariableSalaryReportViewModel>();

                if(data.Any() && data.Count() > 0)
                {
                    var driverIds = data.Select(x => x.DriverId).Distinct();
                    var payItemCodes = data.Select(x => x.PayItemCat).Distinct();

                    foreach(var driverId in driverIds)
                    {
                        var currentDriverDataSet = data.Where(x => x.DriverId == driverId);

                        foreach (var payItemCode in payItemCodes)
                        {
                            var currentDataSet = currentDriverDataSet.Where(x => !string.IsNullOrEmpty(payItemCode) && x.PayItemCat == payItemCode);

                            if(currentDataSet.Any() && currentDataSet.Count() > 0)
                            {
                                var currentData = new DriverVariableSalaryReportViewModel();

                                currentData.DriverNRIC = currentDataSet.First().DriverNRIC;
                                currentData.DriverName = currentDataSet.First().DriverName;

                                currentData.PayItemCat = currentDataSet.First().PayItemCat;

                                var payItemConfig = payItemConfigs.Where(x => x.Code == currentDataSet.First().PayItemCat).First();

                                if(payItemConfig != null)
                                {
                                    currentData.PayItemCatStr = payItemConfig.Name;
                                    currentData.SerialNo = payItemConfig.SerialNo;
                                }

                                currentData.Amount = currentDataSet.Sum(x => x.Amount);

                                finalData.Add(currentData);
                            }
                        }

                        var driverServiceJobIds = currentDriverDataSet.Select(x => x.ServiceJobId).Distinct();
                        decimal salary = 0;

                        foreach(var serviceJobId in driverServiceJobIds)
                        {
                            salary += currentDriverDataSet.Where(x => x.ServiceJobId == serviceJobId).First().Salary;
                        }

                        if(salary > 0)
                        {
                            var currentData = new DriverVariableSalaryReportViewModel();

                            currentData.DriverNRIC = currentDriverDataSet.First().DriverNRIC;
                            currentData.DriverName = currentDriverDataSet.First().DriverName;

                            currentData.PayItemCatStr = "Salary";
                            currentData.SerialNo = 0;

                            currentData.Amount = salary;

                            finalData.Add(currentData);
                        }
                    }
                }

                if(finalData.Any())
                {
                    finalData = finalData.OrderBy(x => x.DriverName).ThenBy(x => x.SerialNo).ToList();
                }

                switch (aCri.GeneratedReportFormat)
                {
                    case ApplicationCore.Cris.BaseCri.ReportFormat.Excel:
                        ExcelProcessor<DriverVariableSalaryReportViewModel> processor = new ExcelProcessor<DriverVariableSalaryReportViewModel>();
                        string path = processor.ExportToExcel(finalData, Server.MapPath(FileConfig.ExportExcelDriverVariableSalary), KeyUtil.GenerateKey());
                        return base.File(path, CONTENT_DISPOSITION, $"Driver_Variable_Salary_"+ fromDateStr + "_" + toDateStr +".xls");
                    case ApplicationCore.Cris.BaseCri.ReportFormat.PDF:

                        DriverVariableSalaryPDF pdfModel = new DriverVariableSalaryPDF
                        {
                            Month = aCri.Month,
                            Year = aCri.Year,
                            Models = finalData
                        };

                        return base.File(base.GeneratePDF<DriverVariableSalaryPDF>(pdfModel, FileConfig.DRIVER_VARIABLE_SALARY_REPORT, $"Driver_Variable_Salary_" + fromDateStr + "_" + toDateStr), CONTENT_DISPOSITION, $"Driver_Variable_Salary_" + fromDateStr + "_" + toDateStr + ".pdf");

                    default:
                        ExcelProcessor<DriverVariableSalaryReportViewModel> defprocessor = new ExcelProcessor<DriverVariableSalaryReportViewModel>();
                        string defpath = defprocessor.ExportToExcel(finalData, Server.MapPath(FileConfig.ExportExcelDriverVariableSalary), KeyUtil.GenerateKey());
                        return base.File(defpath, CONTENT_DISPOSITION, $"Driver_Variable_Salary_" + fromDateStr + "_" + toDateStr + ".xls");
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}