using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes;
using EngagerMark4.Infrasturcture;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EngagerMark4.ApplicationCore.SOP.Excels;
using EngagerMark4.Common;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using System.Data.SqlClient;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Common.Configs;

namespace EngagerMark4.Infrastructure.SOP.Repository.CreditNotes
{
    public class CreditNoteRepository : GenericRepository<ApplicationDbContext, CreditNoteCri, CreditNote>, ICreditNoteRepository
    {
        public CreditNoteRepository(ApplicationDbContext aContext) : base(aContext)
        { }

        public async override Task<CreditNote> GetById(object id)
        {
            var creditNote = await base.GetById(id);

            creditNote.Details = context.CreditNoteDetails.AsNoTracking().Where(x => x.CreditNoteId == creditNote.Id).ToList().OrderBy(x=>x.Id).ToList();

            return creditNote;
        }

        public async Task<CreditNote> GetByWorkOrderId(long workOrderId)
        {
            var creditNote = await context.CreditNotes.FirstOrDefaultAsync(x => x.OrderId == workOrderId);

            if(creditNote !=null)
            {
                creditNote.Details = await context.CreditNoteDetails.Where(x => x.CreditNoteId == creditNote.Id).ToListAsync();
            }

            return creditNote;
        }

        public async Task<CreditNoteReportPDF> GetCreditNoteReportViewModel(CreditNoteCri aCri)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            string query = "SELECT " +
                            "Summary.ReferenceNo InvoiceNo, " +
                            "Summary.InvoiceDate InvoiceDate, " +
                            "CN.VesselName, " +
                            "SUM(CNDetails.TotalAmount) TotalAmount, " +
                            "MAX(SI.GSTPercent) GSTPercent "+
                            "FROM " +
                            "SOP.Tb_SalesInvoiceSummary Summary " +
                            "INNER JOIN SOP.Tb_SalesInvoiceSummaryDetails Details ON Summary.Id = Details.SalesInvoiceSummaryId " +
                            "INNER JOIN SOP.Tb_CreditNote CN ON Details.CreditNoteId = CN.Id " +
                            "INNER JOIN SOP.Tb_CreditNoteDetails CNDetails ON CN.Id = CNDetails.CreditNoteId " +
                            "INNER JOIN SOP.Tb_SalesInvoice SI ON Details.SalesInvoiceId = SI.Id WHERE 1=1 ";

            if (aCri.CustomerId != 0)
            {
                query += "AND SI.CustomerId = @CustomerId ";
                parameters.Add(new SqlParameter("CustomerId", aCri.CustomerId));
            }

            if(!string.IsNullOrEmpty(aCri.FromDate))
            {
                string fromDate = Util.ConvertDateToString(Util.ConvertStringToDateTime(aCri.FromDate, DateConfig.CULTURE));
                query += "AND Summary.InvoiceDate >= @FromDate ";
                parameters.Add(new SqlParameter("FromDate", fromDate));
            }

            if(!string.IsNullOrEmpty(aCri.ToDate))
            {
                string toDate = Util.ConvertDateToString(Util.ConvertStringToDateTime(aCri.ToDate, DateConfig.CULTURE));
                query += "AND Summary.InvoiceDate <= @ToDate ";
                parameters.Add(new SqlParameter("ToDate", toDate));
            }
            
            query += " GROUP BY Summary.ReferenceNo,Summary.InvoiceDate, CN.VesselName";

            
            var results = await context.Database.SqlQuery<CreditNoteDetailsReportViewModel>(query, parameters.ToArray()).ToListAsync();

            return new CreditNoteReportPDF { Details = results };
        }

        public List<CreditNoteExcel> GetData(long[] creditNoteIds)
        {
            try
            {
                var tempCreditNotes = from creditNote in this.context.CreditNotes.AsNoTracking()
                               join creditNoteDetails in this.context.CreditNoteDetails.AsNoTracking() on creditNote.Id equals creditNoteDetails.CreditNoteId
                               join workOrder in this.context.WorkOrders.AsNoTracking() on creditNote.OrderId equals workOrder.Id
                               join invoice in this.context.SalesInvoiceSummaries.AsNoTracking() on workOrder.SalesInvoiceSummaryId equals invoice.Id
                               join customer in this.context.Customers.AsNoTracking() on workOrder.CustomerId equals customer.Id
                               join gl in this.context.GeneralLedgers.AsNoTracking() on creditNoteDetails.GLId equals gl.Id
                               where creditNote.ParentCompanyId == GlobalVariable.COMPANY_ID && creditNoteIds.Contains(creditNote.Id)
                               && creditNote.GrandTotal > 0 && creditNoteDetails.Price > 0
                               select new CreditNoteExcel
                               {
                                   SalesInvoiceSummaryId = invoice.Id,
                                   INV_NO = invoice.ReferenceNo,
                                   AC_NO = customer.AccNo,
                                   AC_NAME = customer.Name,
                                   DESCIRPTION = creditNoteDetails.Description,
                                   AMOUNT = creditNoteDetails.TotalAmount,
                                   InvoiceDate = invoice.InvoiceDate,
                                   GL_CODE = gl.Code,
                                   Taxable = gl.Taxable
                               };

                var tempList = tempCreditNotes.OrderBy(x => x.INV_NO).ToList();

                var invoiceIdList = tempList.Select(x => x.SalesInvoiceSummaryId).Distinct().ToList();

                if (invoiceIdList != null && invoiceIdList.Count() > 0)
                {
                    var tempSummaryList = from cnSummary in context.CreditNoteSummaries.AsNoTracking()
                                          join cnSummaryDetails in context.CreditNoteSummaryDetails.AsNoTracking() on cnSummary.Id equals cnSummaryDetails.CreditNoteSummaryId
                                          where invoiceIdList.Contains(cnSummaryDetails.SalesInvoiceSummaryId)
                                          select new
                                          {
                                              ReferenceNo = cnSummary.ReferenceNo,
                                              InvoiceSummaryId = cnSummaryDetails.SalesInvoiceSummaryId
                                          };

                    if (tempSummaryList.Any())
                    {
                        var cnSummaryList = tempSummaryList.ToList();

                        foreach (var cn in tempList)
                        {
                            var tempcnList = tempList.Where(x => x.SalesInvoiceSummaryId == 0).ToList();

                            if (invoiceIdList.Contains(cn.SalesInvoiceSummaryId))
                            {
                                var cnSummary = cnSummaryList.Where(x => x.InvoiceSummaryId == cn.SalesInvoiceSummaryId).FirstOrDefault();

                                if(cnSummary != null)
                                {
                                    cn.INV_NO = cnSummary.ReferenceNo;
                                }
                            }
                        }
                    }
                }

                var creditNotes = from cn in tempList
                                  group cn by cn.INV_NO into i
                                  select new CreditNoteExcel
                                  {
                                      SalesInvoiceSummaryId = i.FirstOrDefault().SalesInvoiceSummaryId,
                                      INV_NO = i.FirstOrDefault().INV_NO,
                                      AC_NO = i.FirstOrDefault().AC_NO,
                                      AC_NAME = i.FirstOrDefault().AC_NAME,
                                      DESCIRPTION = i.OrderBy(x => x.DESCIRPTION.Length).First().DESCIRPTION,
                                      AMOUNT = i.Sum(x => x.AMOUNT),
                                      InvoiceDate = i.FirstOrDefault().InvoiceDate,
                                      GL_CODE = i.FirstOrDefault().GL_CODE,
                                      Taxable = i.FirstOrDefault().Taxable
                                  };

                var cnList = creditNotes.ToList();

                return cnList;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public override void Save(CreditNote model)
        {
            base.Save(model);

            var gl = context.GeneralLedgers.AsNoTracking().FirstOrDefault(x => x.DiscountType == true);

            if (gl == null) gl = new ApplicationCore.Account.Entities.GeneralLedger();

            foreach(var detail in model.GetDetails())
            {
                detail.GLId = gl.Id;

                if (detail.Id ==  0 && detail.Delete ==false )
                {
                    context.CreditNoteDetails.Add(detail);
                    continue;
                }

                if(detail.Id != 0 && detail.Delete)
                {
                    context.Entry(detail).State = System.Data.Entity.EntityState.Deleted;
                    continue;
                }

                if(detail.Id != 0)
                {
                    context.Entry(detail).State = System.Data.Entity.EntityState.Modified;
                    context.Entry(detail).Property(x => x.Created).IsModified = false;
                    context.Entry(detail).Property(x => x.CreatedBy).IsModified = false;
                }
            }
        }

        public async override Task<IEnumerable<CreditNote>> GetByCri(CreditNoteCri cri)
        {
            var queryableData = await base.GetByCri(cri);

            if (cri.CustomerId > 0)
                queryableData = queryableData.Where(x => x.CustomerId == cri.CustomerId);

            if (cri.VesselId > 0)
                queryableData = queryableData.Where(x => x.VesselId == cri.VesselId);

            return queryableData;
        }

        public long? GetCreditNoteDetailVesselId(long Id)
        {
            try
            {
                var cnDetailVesselId = this.context.SalesInvoices.Where(x => x.Id == Id).AsNoTracking().Select(x => x.VesselId).FirstOrDefault();

                return cnDetailVesselId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateVesselIdAndVesselName(long VesselId, string VesselName, long? Id)
        {
            try
            {
                var cnDetail = await base.GetById(Id);

                cnDetail.VesselId = VesselId;
                cnDetail.VesselName = VesselName;

                cnDetail.Modified = TimeUtil.GetLocalTime();
                cnDetail.ModifiedBy = GetCurrentUserId();
                cnDetail.ModifiedByName = GetUserName();

                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
