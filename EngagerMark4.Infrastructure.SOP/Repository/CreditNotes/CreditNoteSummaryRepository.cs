using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Common.Configs;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;

namespace EngagerMark4.Infrastructure.SOP.Repository.CreditNotes
{
    public class CreditNoteSummaryRepository : GenericRepository<ApplicationDbContext, CreditNoteCri, CreditNoteSummary>, ICreditNoteSummaryRepository
    {
        public CreditNoteSummaryRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        protected override void ApplyCri(CreditNoteCri cri)
        {
            if (!string.IsNullOrEmpty(cri.ReferenceNo))
                queryableData = queryableData.Where(x => x.ReferenceNo.Contains(cri.ReferenceNo));

            if (cri.CustomerId > 0)
                queryableData = queryableData.Where(x => x.CustomerId == cri.CustomerId);

            if (!string.IsNullOrEmpty(cri.FromDate))
            {
                var fromDateTime = Util.ConvertStringToDateTime(cri.FromDate, DateConfig.CULTURE);
                queryableData = queryableData.Where(x => x.CreditNoteDate >= fromDateTime);
            }

            if (!string.IsNullOrEmpty(cri.ToDate))
            {
                var toDateTime = Util.ConvertStringToDateTime(cri.ToDate, DateConfig.CULTURE);
                queryableData = queryableData.Where(x => x.CreditNoteDate <= toDateTime);
            }

            if (cri.Status != null && cri.Status != CreditNoteSummary.CreditNoteStatus.All)
            {
                queryableData = queryableData.Where(x => x.Status == cri.Status);
            }
        }


        public async override Task<CreditNoteSummary> GetById(object id)
        {
            return await this.dbSet.Include(x => x.Details).FirstOrDefaultAsync(x => x.Id == (long)id);
        }

        public override void Save(CreditNoteSummary model)
        {
            if (model.Id == 0)
            {
                model.Customer = null;
                dbSet.Add(model);

                context.SaveChanges();

                model.SetReferenceNo(new SerialNoRepository<CreditNoteReportSerialNo>(context).GetSerialNoByMonth(model.Id, model.CreditNoteDate.Value));
            }
            else
            {
                foreach (var details in context.CreditNoteSummaryDetails.Where(x => x.CreditNoteSummaryId == model.Id))
                {
                    context.CreditNoteSummaryDetails.Remove(details);
                }

                foreach (var toSaveDetail in model.Details)
                {
                    toSaveDetail.Created = TimeUtil.GetLocalTime();
                    toSaveDetail.CreditNoteSummaryId = model.Id;
                    context.CreditNoteSummaryDetails.Add(toSaveDetail);
                }

                model.Customer = null;

                context.Entry(model).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// https://www.entityframeworktutorial.net/EntityFramework4.3/raw-sql-query-in-entity-framework.aspx
        /// </summary>
        /// <param name="aCri"></param>
        /// <returns></returns>
        public IEnumerable<CreditNoteSummaryViewModel> GetSummaryViewModel(CreditNoteCri aCri)
        {
            string query = "SELECT Cus.Name Customer,Cus.Address,SISummary.InvoiceDate,SISummary.ReferenceNo,CN.VesselName,SUM(CN.Subtotal) SubTotal, SUM(CN.GrandTotal) TotalAmt, CN.GSTPercent GSTPercent FROM SOP.Tb_SalesInvoiceSummary SISummary " +
                           "INNER JOIN SOP.Tb_SalesInvoiceSummaryDetails Details ON SISummary.Id = Details.SalesInvoiceSummaryId " +
                           "INNER JOIN SOP.Tb_CreditNote CN ON Details.CreditNoteId = CN.Id " +
                           "INNER JOIN Customer.Tb_Customer Cus ON CN.CustomerId = Cus.Id " +
                           "WHERE SISummary.Id in " + aCri.SalesInvoiceSummaryIdsStr + " " +
                           "GROUP BY SISummary.InvoiceDate,SISummary.ReferenceNo, CN.VesselName,Cus.Name,Cus.Address, CN.GSTPercent " +
                           "ORDER BY SISummary.ReferenceNo;";
            return context.Database.SqlQuery<CreditNoteSummaryViewModel>(query);
        }

        public IEnumerable<SalesInvoiceSummary> GetAlreadySavedSalesInvoiceSummaries(CreditNoteCri aCri)
        {
            var query = from salesInvoiceSummary in context.SalesInvoiceSummaries.AsNoTracking()
                        join inDetails in context.CreditNoteSummaryDetails.AsNoTracking() on salesInvoiceSummary.Id equals inDetails.SalesInvoiceSummaryId
                        where salesInvoiceSummary.InvoiceDate >= aCri.FromDateTime && salesInvoiceSummary.InvoiceDate <= aCri.ToDateTime && salesInvoiceSummary.CustomerId == aCri.CustomerId
                        select salesInvoiceSummary;


            return query;

        }

        public IEnumerable<CreditNoteSummaryDetails> GetCreditNoteSummariesBySalesInvoiceIds(List<Int64> salesInvoiceSummaryIds)
        {
            var query = from creditNoteSummaryDetails in context.CreditNoteSummaryDetails.Include(x => x.CreditNoteSummary).Where(x => salesInvoiceSummaryIds.Contains(x.SalesInvoiceSummaryId))
                        select creditNoteSummaryDetails;

            return query;
        }
    }
}
