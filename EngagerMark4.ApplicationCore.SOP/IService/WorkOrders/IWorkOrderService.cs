using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IService.WorkOrders
{
    public interface IWorkOrderService : IBaseService<WorkOrderCri, WorkOrder>
    {
        #region Load References

        Task<IEnumerable<WorkOrder>> GetByInvoiceNo(string invoiceNo);

        Task<List<WorkOrder>> GetByIds(long[] invoiceIds);

        Task<WorkOrder> GetByCreditNoteId(long creditNoteId);

        Task<List<WorkOrderPassenger>> GetNonInchargePassengers(long workOrderId, long vehicleId);

        Task<List<WorkOrderPassenger>> GetNonInchargePassengersByServiceJobId(long workOrderId, long serviceJobId);

        List<WorkOrder> GetSimliarOrders(DateTime pickupDate, long CustomerId, long VesselId, List<WorkOrderLocation> locations);

        Task<IEnumerable<WorkOrder>> GetByInvoiceDateCri(WorkOrderCri cri);

        //PCR2021
        Task<IEnumerable<BilledOrderListInvoiceViewModel>> GetDataForBilledOrderList(WorkOrderCri cri);

        //PCR2021
        List<long> GetWorkOrderIdsFromInvoiceIds(long[] invoiceIds);

        Task<IEnumerable<WorkOrder>> GetOrdersForAgents(WorkOrderCri cri);
        
        #endregion

        #region Save / Update Work Order

        Task MovetoBill(Int64[] workOrderIds);

        Task MoveToWithAccounts(long[] workOrderIds);

        //PCR2011 - OBSOLETE
        Task SavePassengers(long workOrderId, long vehicleId, List<WorkOrderPassenger> passengers);
               
        Task UpdatePassengerInCharge(WorkOrder workOrder, string applicationUserId, WorkOrderPassenger inChargePassenger, CommonConfiguration rank);

        Task RemoveUnselectedOrdersFromInvoice(Int64[] workOrderIds, string invoiceSummaryNo);

        Task<Int64> SaveAuditHistoryForWorkOrder(WorkOrder workOrder, bool IsNewEntity);

        Task SaveServiceJobChecklistItem(WorkOrder workOrder);

        #endregion
    }
}
