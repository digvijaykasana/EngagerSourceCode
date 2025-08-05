using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.Infrasturcture.ExpressionBuilders;
using EngagerMark4.Common;
using EngagerMark4.ApplicationCore.SOP.Excels;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.Common.Utilities;

namespace EngagerMark4.Infrastructure.SOP.Repository.SalesInvoices
{
    /// <summary>
    /// 
    /// </summary>
    public class SalesInvoiceRepository : GenericRepository<ApplicationDbContext, SalesInvoiceCri, SalesInvoice>, ISalesInvoiceRepository
    {
        public SalesInvoiceRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public List<InvoiceExcel> GetData(long[] ids)
        {
            try
            {
                var entities = from invoice in this.context.SalesInvoices.AsNoTracking()
                               join invoiceDetails in this.context.SalesInvoiceDetails.AsNoTracking() on invoice.Id equals invoiceDetails.SalesInvoiceId
                               join price in this.context.Prices.AsNoTracking() on invoiceDetails.PriceId equals price.Id
                               join gl in this.context.GeneralLedgers.AsNoTracking() on price.GLCodeId equals gl.Id
                               join workOrder in this.context.WorkOrders.AsNoTracking() on invoice.WorkOrderId equals workOrder.Id
                               join customer in this.context.Customers.AsNoTracking() on workOrder.CustomerId equals customer.Id
                               join vessels in this.context.CommonConfigurations.AsNoTracking() on workOrder.VesselId equals vessels.Id
                               where invoice.ParentCompanyId == GlobalVariable.COMPANY_ID && ids.Contains(invoice.Id) &&
                               invoice.TotalNetAmount > 0 && invoiceDetails.Price > 0 && invoiceDetails.Qty > 0
                               && invoice.SalesInvoiceSummaryId != 0
                               select new InvoiceExcel
                               {
                                   INV_NO = invoice.ShortText1,
                                   AC_NO = customer.AccNo,
                                   AC_NAME = customer.Name,
                                   PRODUCT_NO = gl.Code,
                                   PRODUCT_NAME = invoiceDetails.Code,
                                   QTY = invoiceDetails.Qty,
                                   PRICE = invoiceDetails.Price,
                                   AMOUNT = invoiceDetails.TotalAmt,
                                   InvoiceDate = invoice.InvoiceDate,
                                   GL_CODE = gl.Code,
                                   PROJ_NO = workOrder.RefereneceNo,
                                   Vessel = vessels.Name,
                                   Taxable = gl.Taxable,
                                   TaxPercent = invoice.GSTPercent
                               };
                return entities.OrderBy(x => x.INV_NO).ToList();
            }
            catch(Exception ex)
            {
                return new List<InvoiceExcel>();
            }
        }

        public async override Task<IEnumerable<SalesInvoice>> GetByCri(SalesInvoiceCri cri)
        {
            var queryableData = context.SalesInvoices.Include(x => x.Customer).AsNoTracking().Where(x => x.ParentCompanyId == GlobalVariable.COMPANY_ID);

            if (cri.CustomerId > 0)
                queryableData = queryableData.Where(x => x.CustomerId == cri.CustomerId);

            if (cri.VesselId > 0)
                queryableData = queryableData.Where(x => x.VesselId == cri.VesselId);

            if (cri.Status > 0)
                queryableData = queryableData.Where(x => x.Status == cri.Status);

            if (cri.FromDate != null)
                queryableData = queryableData.Where(x => x.InvoiceDate >= cri.FromDate);

            if (cri.ToDate != null)
                queryableData = queryableData.Where(x => x.InvoiceDate <= cri.ToDate);

            if (!String.IsNullOrEmpty(cri.SalesInvoiceSummaryNo))
                queryableData = queryableData.Where(x => x.ShortText1.ToLower().Trim().Contains(cri.SalesInvoiceSummaryNo.ToLower().Trim()));

            if (cri != null && cri.OrderBys != null)
            {
                foreach (var columnName in cri.OrderBys.Keys)
                {
                    var value = cri.OrderBys[columnName];
                    var dataType = value.Keys.FirstOrDefault();
                    var orderType = value.Values.FirstOrDefault();

                    switch (orderType)
                    {
                        case BaseCri.EntityOrderBy.Asc:
                            switch (dataType)
                            {
                                case BaseCri.DataType.String:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<SalesInvoice, String>(columnName));
                                    break;
                                case BaseCri.DataType.Int64:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpressionInt64<SalesInvoice>(columnName));
                                    break;
                                case BaseCri.DataType.DateTime:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<SalesInvoice, DateTime>(columnName));
                                    break;
                                default:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<SalesInvoice>(columnName, dataType));
                                    break;
                            }
                            break;
                        case BaseCri.EntityOrderBy.Dsc:
                            switch (dataType)
                            {
                                case BaseCri.DataType.String:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<SalesInvoice, String>(columnName));
                                    break;
                                case BaseCri.DataType.Int64:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpressionInt64<SalesInvoice>(columnName));
                                    break;
                                case BaseCri.DataType.DateTime:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<SalesInvoice, DateTime>(columnName));
                                    break;
                                default:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<SalesInvoice>(columnName, dataType));
                                    break;
                            }
                            break;
                        default:
                            switch (dataType)
                            {
                                case BaseCri.DataType.String:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<SalesInvoice, String>(columnName));
                                    break;
                                case BaseCri.DataType.Int64:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpressionInt64<SalesInvoice>(columnName));
                                    break;
                                case BaseCri.DataType.DateTime:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<SalesInvoice, DateTime>(columnName));
                                    break;
                                default:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<SalesInvoice>(columnName, dataType));
                                    break;
                            }
                            break;
                    }
                }
            }


            if (cri != null && cri.IsPagination)
                queryableData = queryableData.Skip(cri.NoOfPage * (cri.CurrentPage - 1)).Take(cri.NoOfPage);

            return queryableData;
        }

        public async override Task<SalesInvoice> GetById(object id)
        {
            var invoice = await base.GetById(id);

            invoice.Details = context.SalesInvoiceDetails.AsNoTracking().Where(x => x.SalesInvoiceId == invoice.Id).OrderBy(x => x.Id).ToList();

            return invoice;
        }

        public async Task<IEnumerable<SalesInvoice>> GetByInvoiceNo(string invoiceNo)
        {
            //var invoice = context.SalesInvoiceSummaries.FirstOrDefault(x => x.ReferenceNo.Equals(invoiceNo));

            //if (invoice == null) return new List<SalesInvoice>();
            //var salesInvoiceIdList = new List<Int64>();

            //foreach (var detail in context.SalesInvoiceSummaryDetails.AsNoTracking().Where(x => x.SalesInvoiceSummaryId == invoice.Id))
            //{
            //    salesInvoiceIdList.Add(detail.SalesInvoiceId);
            //}
            //var salesInvoiceIds = salesInvoiceIdList.ToArray();

            var salesInvoices = context.SalesInvoices.Include(w => w.Customer).Where(x => x.ParentCompanyId == GlobalVariable.COMPANY_ID && x.ShortText1 == invoiceNo);

            //salesInvoices = salesInvoices.Where(x => salesInvoiceIds.Contains(x.Id));

            return  salesInvoices.OrderByDescending(x => x.ReferenceNoNumber);
        }

        public override void Save(SalesInvoice model)
        {
            base.Save(model);

            foreach (var detail in model.GetDetails())
            {
                if (detail.Id == 0 && detail.Delete == false)
                {
                    context.SalesInvoiceDetails.Add(detail);
                    continue;
                }

                if (detail.Id != 0 && detail.Delete)
                {
                    context.Entry(detail).State = System.Data.Entity.EntityState.Deleted;
                    continue;
                }

                if (detail.Id != 0)
                {
                    context.Entry(detail).State = System.Data.Entity.EntityState.Modified;
                    context.Entry(detail).Property(x => x.Created).IsModified = false;
                    context.Entry(detail).Property(x => x.CreatedBy).IsModified = false;
                }
            }

            if (model.Id == 0)
            {
                context.SaveChanges();
                SerialNoRepository<SalesInvoiceSerialNo> serialNoService = new SerialNoRepository<SalesInvoiceSerialNo>(context);
                model.ReferenceNo = model.GetReferenceNo(serialNoService.GetSerialNoByMonth(model.Id, model.Created));
                model.LongText1 = model.ReferenceNo;
            }

            var workOrder = context.WorkOrders.FirstOrDefault(x => x.Id == model.WorkOrderId);
            if (workOrder != null)
            {
                workOrder.InvoiceId = model.Id;
                model.WorkOrderNo = workOrder.RefereneceNo;
                workOrder.InvoiceNo = model.ReferenceNo;
            }

        }

        public long? SaveVersionedInvoice(SalesInvoice model)
        {
            try
            {
                long previousVersionId = model.Id;

                if (this.dbSet == null)
                    return null;

                model.Id = 0;

                context.Entry(model).State = System.Data.Entity.EntityState.Added;
                model.Created = TimeUtil.GetLocalTime();
                model.Modified = TimeUtil.GetLocalTime();
                model.CreatedBy = GetCurrentUserId();
                model.CreatedByName = GetUserName();
                this.dbSet.Add(model);


                context.SaveChanges();

                foreach (var detail in model.GetDetails())
                {
                    SalesInvoiceDetails newDetail = new SalesInvoiceDetails()
                    {
                        Delete = detail.Delete,
                        GSTEssentials = detail.GSTEssentials,
                        Taxable = detail.Taxable,
                        SalesInvoiceId = model.Id,
                        SerialNo = detail.SerialNo,
                        PriceId = detail.PriceId,
                        Code = detail.Code,
                        Description = detail.Description,
                        Qty = detail.Qty,
                        Price = detail.Price,
                        TotalAmt = detail.TotalAmt,
                        DiscountPercent = detail.DiscountPercent,
                        DiscountAmount = detail.DiscountAmount,
                        TotalNetAmount = detail.TotalNetAmount,
                        Type = detail.Type
                    };


                    if (newDetail.Id == 0 && newDetail.Delete == false)
                    {
                        context.SalesInvoiceDetails.Add(newDetail);
                        context.Entry(newDetail).State = System.Data.Entity.EntityState.Added;
                        continue;
                    }
                }

                context.SaveChanges();

                var version = model.GetVersionNo(model.VersionNumber);

                model.ReferenceNo = model.AddVersionNo(model.LongText1, version);

                var workOrder = context.WorkOrders.FirstOrDefault(x => x.Id == model.WorkOrderId);
                if (workOrder != null)
                {
                    workOrder.InvoiceId = model.Id;
                    model.WorkOrderNo = workOrder.RefereneceNo;
                    workOrder.InvoiceNo = model.ReferenceNo;
                }

                if(previousVersionId> 0)
                {
                    var salesInvoiceSummaryDetails = context.SalesInvoiceSummaryDetails.FirstOrDefault(x => x.SalesInvoiceSummaryId == model.SalesInvoiceSummaryId && x.SalesInvoiceId == previousVersionId);

                    if(salesInvoiceSummaryDetails != null)
                    {
                        salesInvoiceSummaryDetails.SalesInvoiceId = model.Id;
                    }
                }

                return version;

            }
            catch(Exception ex)
            {
                return null;
            }

            #region Obsolete 04/04/2019
            //if (this.dbSet == null)
            //    return null;

            //model.Id = 0;

            //context.Entry(model).State = System.Data.Entity.EntityState.Added;
            //model.Created = TimeUtil.GetLocalTime();
            //model.Modified = TimeUtil.GetLocalTime();
            //model.CreatedBy = GetCurrentUserId();
            //model.CreatedByName = GetUserName();
            //this.dbSet.Add(model);

            //foreach (var detail in model.GetDetails())
            //{
            //    if (detail.Id == 0 && detail.Delete == false)
            //    {
            //        context.SalesInvoiceDetails.Add(detail);
            //        continue;
            //    }

            //    if (detail.Id != 0 && detail.Delete)
            //    {
            //        context.Entry(detail).State = System.Data.Entity.EntityState.Deleted;
            //        continue;
            //    }

            //    if (detail.Id != 0)
            //    {
            //        context.Entry(detail).State = System.Data.Entity.EntityState.Modified;
            //        context.Entry(detail).Property(x => x.Created).IsModified = false;
            //        context.Entry(detail).Property(x => x.CreatedBy).IsModified = false;
            //    }
            //}

            //context.SaveChanges();

            //var version = model.GetVersionNo(model.VersionNumber);

            //model.ReferenceNo = model.AddVersionNo(model.LongText1, version);

            //var workOrder = context.WorkOrders.FirstOrDefault(x => x.Id == model.WorkOrderId);
            //if (workOrder != null)
            //{
            //    workOrder.InvoiceId = model.Id;
            //    model.WorkOrderNo = workOrder.RefereneceNo;
            //    workOrder.InvoiceNo = model.ReferenceNo;
            //}

            //return version;
            #endregion
        }

        public void UpdateVersions(long? workOrderId, long? VersionNumber)
        {

            if(workOrderId != null && VersionNumber != null)
            { 
            var salesInvoices = this.context.SalesInvoices.Where(x => x.WorkOrderId == workOrderId).ToList();

                foreach(SalesInvoice invoice in salesInvoices)
                {
                    invoice.VersionNumber = VersionNumber;
                }

                this.context.SaveChanges();
            }
        }

        public long? GetInvoiceDetailVesselId(long Id)
        {
            try
            {
                var invoiceDetailVesselId = this.context.SalesInvoices.Where(x => x.Id == Id).AsNoTracking().Select(x => x.VesselId).FirstOrDefault();

                return invoiceDetailVesselId;
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
                var invoiceDetail = await base.GetById(Id);

                invoiceDetail.VesselId = VesselId;
                invoiceDetail.VesselName = VesselName;

                invoiceDetail.Modified = TimeUtil.GetLocalTime();
                invoiceDetail.ModifiedBy = GetCurrentUserId();
                invoiceDetail.ModifiedByName = GetUserName();

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
