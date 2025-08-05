using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices;
using EngagerMark4.Common;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EngagerMark4.Infrasturcture.ExpressionBuilders;
using EngagerMark4.ApplicationCore.Cris;

namespace EngagerMark4.Infrastructure.SOP.Repository.SalesInvoices
{
    public class SalesInvoiceSummaryRepository : GenericRepository<ApplicationDbContext, SalesInvoiceSummaryCri, SalesInvoiceSummary>, ISalesInvoiceSummaryRepository
    {
        public SalesInvoiceSummaryRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        protected override void ApplyCri(SalesInvoiceSummaryCri cri)
        {

            if (cri != null && !string.IsNullOrEmpty(cri.SearchValue))
                base.queryableData = queryableData.Where(x => x.ReferenceNo.Contains(cri.SearchValue) || x.CreatedByName.Contains(cri.SearchValue));

            if (cri != null && cri.CustomerId > 0)
                base.queryableData = queryableData.Where(x => x.CustomerId == cri.CustomerId);

            if (cri != null && cri.VesselId > 0)
                base.queryableData = queryableData.Where(x => x.VesselId == cri.VesselId);


            if (cri != null && cri.FromDate != null)
            {
                TimeSpan ts = new TimeSpan(0, 0, 0);
                cri.FromDate = cri.FromDate.Value.Date + ts;
                queryableData = queryableData.Where(x => x.InvoiceDate >= cri.FromDate);
            }


            if (cri != null && cri.ToDate != null)
            {
                TimeSpan newts = new TimeSpan(23, 59, 59);
                cri.ToDate = cri.ToDate.Value.Date + newts;
                //cri.ToDate = cri.ToDate.Value.AddDays(1);
                queryableData = queryableData.Where(x => x.InvoiceDate <= cri.ToDate);
            }

            if(cri.CustomerId!=0)
            {
                queryableData = queryableData.Where(x => x.CustomerId == cri.CustomerId);
            }
        }

        public async Task<SalesInvoiceSummary> GetByInvoiceNo(string invoiceNo)
        {
            return await context.SalesInvoiceSummaries.AsNoTracking().FirstOrDefaultAsync(x => x.ReferenceNo.ToLower().Trim().Contains(invoiceNo.ToLower().Trim()));
        }

        public override void Save(SalesInvoiceSummary model)
        {
            base.Save(model);

            foreach(var detail in context.SalesInvoiceSummaryDetails.Where(x => x.SalesInvoiceSummaryId == model.Id && x.ParentCompanyId == GlobalVariable.COMPANY_ID))
            {
                context.SalesInvoiceSummaryDetails.Remove(detail);
            }

            foreach(var detail in model.GetDetails())
            {
                context.SalesInvoiceSummaryDetails.Add(detail);
            }
        }

        public bool IsWorkOrderUnderInvoice(long woId, long invoiceSummaryId)
        {
            var workOrders = this.context.SalesInvoiceSummaryDetails.Where(x => x.SalesInvoiceSummaryId == invoiceSummaryId && x.WorkOrderId == woId);

            if(workOrders.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public long? GetInvoiceVesselId(long Id)
        {
            try
            {
                var invoiceVesselId = this.context.SalesInvoiceSummaries.Where(x => x.Id == Id).AsNoTracking().Select(x => x.VesselId).FirstOrDefault();

                return invoiceVesselId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void RemoveInvoiceSummaryDetailsByWorkOrderId(long invoiceId, List<long> workOrderIds)
        {
            try
            {
                foreach (var woId in workOrderIds)
                {
                    SalesInvoiceSummaryDetails detailToDelete = this.context.SalesInvoiceSummaryDetails.AsNoTracking().SingleOrDefault(x => x.SalesInvoiceSummaryId == invoiceId && x.WorkOrderId == woId);

                    if(detailToDelete != null)
                    {
                        this.context.SalesInvoiceSummaryDetails.Remove(detailToDelete);
                    }
                }
            }
            catch(Exception ex)
            {

            }

        }
    }
}
