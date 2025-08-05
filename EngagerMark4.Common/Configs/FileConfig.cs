using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Common.Configs
{
    public static class FileConfig
    {
        public static string DEBUG_PATH = @"D:\01__Projects\02__Dummy\Source\DummyWeb\EngagerMark4\EngagerMark4";
        public static string ASPOSE_TOTAL_LICENSE = "\\Bin\\lib\\Aspose.Total.lic";
        public static string ASPOSE_PDF_LICENSE = "\\Bin\\lib\\Aspose.Pdf.lic";

        public static string SERVICE_JOBS = "\\PDFs\\ServiceJobs\\";
        public static string INVOICES = "\\PDFs\\Invoices\\";
        public static string CREDITNOTES = "\\PDFs\\CreditNotes\\";
        public static string DRIVER_DAILY_REPORT = "\\PDFs\\DriverDailyReports\\";
        public static string DAILY_SUMMARY_JOB_BY_COMPANY = "\\PDFs\\DailySummaryJobReportsByCompany\\";
        public static string DAILY_SUMMARY_JOB_BY_DRIVER = "\\PDFs\\DailySummaryJobReportsByDriver\\";
        public static string INVOICE_SUMMARY_REPORT = "\\PDFs\\InvoiceSummaryReport\\";
        public static string INVOICE_MONTHLY_REPORT = "\\PDFs\\MonthlyInvoiceReport\\";
        public static string CREDIT_NOTE_MONTHLY_REPORT = "\\PDFs\\MonthlyCreditNoteReport\\";
        public static string CREDIT_NOTE_SUMMARY_REPORT = "\\PDFs\\CreditNoteSummary\\";
        public static string MonthlyGeneralInvoiceReport = "\\PDFs\\MonthlyGeneralInvoiceReport";
        public static string MEETING_SERVICES = "\\PDFs\\MeetingServices\\";
        public static string DRIVER_VARIABLE_SALARY_REPORT = "\\PDFs\\DriverVariableSalaryReports\\";

        public static string EXPORT_EXCEL = "\\Excels\\";
        public static string EXPORT_EXCEL_INVOICES = "\\Excels\\Invoices\\";
        public static string EXPORT_EXCEL_CRNS = "\\Excels\\CreditNotes\\";
        public static string ExportExcelInvoiceSummary = "~/Excels/InvoiceSummaries";
        public static string ExportExcelDriverDaily = "~/Excels/DriverDaily";
        public static string ExportExcelDailySummaryJobByCompany = "~/Excels/DailySummaryJobByCompany";
        public static string ExportExcelDailySummaryJobByDriver = "~/Excels/DailySummaryJobByDriver";
        public static string ExportExcelMonthlyGeneralInvoiceReport = "~/Excels/MonthlyGeneralInvoiceReport";
        public static string ExportExcelDriverVariableSalary = "~/Excels/DriverVariableSalaryReport";

        public static string IMPORT_LOCATION = "~/Imports/Locations";
    }
}
