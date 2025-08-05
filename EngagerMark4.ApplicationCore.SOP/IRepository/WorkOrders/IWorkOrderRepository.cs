using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IRepository.WorkOrders
{
    public interface IWorkOrderRepository : IBaseRepository<WorkOrderCri, Entities.WorkOrders.WorkOrder>
    {

        #region Load References

        Task<IEnumerable<Entities.WorkOrders.WorkOrder>> GetByInvoiceNo(string invoiceNo);

        Task<List<Entities.WorkOrders.WorkOrder>> GetByIds(long[] workOrderIds);

        Task<Entities.WorkOrders.WorkOrder> GetByCreditNoteId(long creditNoteId);

        Task<List<WorkOrderPassenger>> GetNonInchargePassengers(long workOrderId, long vehicleId);

        Task<List<WorkOrderPassenger>> GetNonInchargePassengersByServiceJobId(long workOrderId, long serviceJobId);

        List<Entities.WorkOrders.WorkOrder> GetSimliarOrders(DateTime pickupDate, long CustomerId, long VesselId, List<WorkOrderLocation> locations);

        Task<IEnumerable<Entities.WorkOrders.WorkOrder>> GetByInvoiceDateCri(WorkOrderCri cri);

        //PCR2021
        Task<IEnumerable<BilledOrderListInvoiceViewModel>> GetDataForBilledOrderList(WorkOrderCri cri);

        //PCR2021
        List<long> GetWorkOrderIdsFromInvoiceIds(long[] invoiceIds);

        Task<IEnumerable<Entities.WorkOrders.WorkOrder>> GetOrdersForAgents(WorkOrderCri cri);
    
        #endregion

        #region Save / Update Work Data

        void UpdatePassengerInCharge(Entities.WorkOrders.WorkOrder workOrder, string applicationUserId);

        //PCR2011 - OBSOLETE
        Task SavePassengers(long workOrderId, long vehicleId, List<WorkOrderPassenger> passengers);

        #endregion
    }
}
