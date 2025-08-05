using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.Common.Configs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Cris
{
    public class WorkOrderCri : BaseCri
    {
        public string Drivers
        {
            get;
            set;
        }

        public Int64 CustomerId
        {
            get;
            set;
        }

        public Int64 VesselId
        {
            get;
            set;
        }

        public Int64 DriverId
        {
            get;
            set;
        }

        public DateTime? FromDate
        {
            get;
            set;
        }

        public DateTime? ToDate
        {
            get;
            set;
        }

        public bool IncludeTax
        {
            get;
            set;
        }

        public bool IsExcel
        {
            get;
            set;
        } = false;

        public bool IncludeVesselInFrontOfCompanyName
        {
            get;
            set;
        }

        public WorkOrder.OrderStatus Status
        {
            get;
            set;
        } = WorkOrder.OrderStatus.All;

        public bool SearchByRange
        {
            get;
            set;
        } = false;

        public bool IsSearchByRange
        {
            get;
            set;
        } = false;

        public string SalesInvoiceSummaryNo
        {
            get;
            set;
        }

        public string SalesInvoiceSummaryStartingNo
        {
            get;
            set;
        } = string.Empty;

        public int StartingRefYearMonth
        {
            get;
            set;
        } = 0;

        public int StartingRefSerial
        {
            get;
            set;
        } = 0;

        public string SalesInvoiceSummaryEndingNo
        {
            get;
            set;
        } = string.Empty;

        public int EndingRefYearMonth
        {
            get;
            set;
        } = 0;

        public int EndingRefSerial
        {
            get;
            set;
        } = 0;


        public string InvoiceDate
        {
            get;
            set;
        }

        public string DNNo
        {
            get;
            set;
        }

        public enum Action
        {
            [Display(Name = "EZ Invoice")]
            EZInvoice,
            [Display(Name = "EZ Credit Note")]
            EZCN
        }

        public CommonEnumConfig.DownloadFileFormat DownloadFileFormat
        {
            get;
            set;
        }

        public Action ActionThing
        {
            get;
            set;
        }

        public bool IsComeFromAccount
        { get; set; }
       

        public static List<CommonConfiguration> GetOrderStatuses()
        {
            List<CommonConfiguration> configurations = new List<CommonConfiguration>();
            CommonConfiguration all = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.All, Name = WorkOrder.OrderStatus.All.ToString() };
            configurations.Add(all);
            CommonConfiguration draft = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Draft, Name = WorkOrder.OrderStatus.Draft.ToString() };
            configurations.Add(draft);
            CommonConfiguration ordered = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Ordered, Name = WorkOrder.OrderStatus.Ordered.ToString() };
            configurations.Add(ordered);
            CommonConfiguration pending = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Pending, Name = WorkOrder.OrderStatus.Pending.ToString() };
            configurations.Add(pending);
            CommonConfiguration assigned = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Assigned, Name = WorkOrder.OrderStatus.Assigned.ToString() };
            configurations.Add(assigned);
            CommonConfiguration scheduled = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Scheduled, Name = WorkOrder.OrderStatus.Scheduled.ToString() };
            configurations.Add(scheduled);
            CommonConfiguration inProgress = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.In_Progress, Name = WorkOrder.OrderStatus.In_Progress.ToString() };
            configurations.Add(inProgress);
            CommonConfiguration submitted = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Submitted, Name = WorkOrder.OrderStatus.Submitted.ToString() };
            configurations.Add(submitted);
            CommonConfiguration verified = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Verified, Name = WorkOrder.OrderStatus.Verified.ToString() };
            configurations.Add(verified);
            CommonConfiguration withAccounts = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.With_Accounts, Name = WorkOrder.OrderStatus.With_Accounts.ToString() };
            configurations.Add(withAccounts);
            CommonConfiguration billed = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Billed, Name = WorkOrder.OrderStatus.Billed.ToString() };
            configurations.Add(billed);
            CommonConfiguration cancelled = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Cancelled, Name = WorkOrder.OrderStatus.Cancelled.ToString() };
            configurations.Add(cancelled);
            return configurations;
        }


        public static List<CommonConfiguration> GetOrderStatusesForAccounting()
        {
            List<CommonConfiguration> configurations = new List<CommonConfiguration>();
            //CommonConfiguration all = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.All, Name = WorkOrder.OrderStatus.All.ToString() };
            //configurations.Add(all);
            CommonConfiguration withAccounts = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.With_Accounts, Name = WorkOrder.OrderStatus.With_Accounts.ToString() };
            configurations.Add(withAccounts);
            //CommonConfiguration billed = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Billed, Name = WorkOrder.OrderStatus.Billed.ToString() };
            //configurations.Add(billed);
            return configurations;
        }


        public static List<CommonConfiguration> GetOrderStatusesForBilled()
        {
            List<CommonConfiguration> configurations = new List<CommonConfiguration>();
            CommonConfiguration billed = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Billed, Name = WorkOrder.OrderStatus.Billed.ToString() };
            configurations.Add(billed);
            return configurations;
        }

        public static List<CommonConfiguration> GetOrderStatusesForAgents()
        {
            List<CommonConfiguration> configurations = new List<CommonConfiguration>();
            CommonConfiguration all = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.All, Name = WorkOrder.OrderStatus.All.ToString() };
            configurations.Add(all);
            CommonConfiguration draft = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Draft, Name = WorkOrder.OrderStatus.Draft.ToString() };
            configurations.Add(draft);
            CommonConfiguration ordered = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Ordered, Name = WorkOrder.OrderStatus.Ordered.ToString() };
            configurations.Add(ordered);
            CommonConfiguration pending = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Pending, Name = WorkOrder.OrderStatus.Pending.ToString() };
            configurations.Add(pending);
            CommonConfiguration scheduled = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Scheduled, Name = WorkOrder.OrderStatus.Scheduled.ToString() };
            configurations.Add(scheduled);
            CommonConfiguration assigned = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Assigned, Name = WorkOrder.OrderStatus.Assigned.ToString() };
            configurations.Add(assigned);
            CommonConfiguration inProgress = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.In_Progress, Name = WorkOrder.OrderStatus.In_Progress.ToString() };
            configurations.Add(inProgress);
            CommonConfiguration submitted = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Submitted, Name = WorkOrder.OrderStatus.Submitted.ToString() };
            configurations.Add(submitted);
            CommonConfiguration verified = new CommonConfiguration { Id = (int)WorkOrder.OrderStatus.Verified, Name = WorkOrder.OrderStatus.Verified.ToString() };
            configurations.Add(verified);
            return configurations;
        }

        //For the ATTN field in the header of invoices - used during invoice generation
        public string AttnStr
        {
            get;
            set;
        } = string.Empty;   
        
    }
}
