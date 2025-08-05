using EngagerMark4.Infrasturcture.Repository;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.ApplicationCore.Inventory.Cris;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.Inventory.IRepository.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngagerMark4.ApplicationCore.IRepository.Users;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using EngagerMark4.Common;
using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.Infrasturcture.ExpressionBuilders;
using EngagerMark4.ApplicationCore.Cris.Application;
using EngagerMark4.Common.Configs;
using EngagerMark4.ApplicationCore.IRepository.Application;

namespace EngagerMark4.Infrastructure.Inventory.Repository
{
    public class PriceRepository : GenericRepository<ApplicationDbContext, PriceCri, Price>, IPriceRepository
    {
        IRolePermissionRepository _rolePermissionRepository;
        ISystemSettingRepository _systemSettingRepository;

        public PriceRepository(ApplicationDbContext aContext, IRolePermissionRepository rolePermissionRepository,
            ISystemSettingRepository systemSettingRepository) : base(aContext)
        {
            this._rolePermissionRepository = rolePermissionRepository;
            this._systemSettingRepository = systemSettingRepository;
        }

        public async override Task<IEnumerable<Price>> GetByCri(PriceCri cri)
        {
            var queryableData = context.Prices.AsNoTracking().Include(c => c.Customer).Include(c => c.GeneralLedger).AsNoTracking().Where(x => x.ParentCompanyId == GlobalVariable.COMPANY_ID);

            if (cri == null) cri = new PriceCri();

            if (cri.CustomerId > 0)
                queryableData = queryableData.Where(x => x.CustomerId == cri.CustomerId);

            if (cri.GLCodeId > 0)
                queryableData = queryableData.Where(x => x.GLCodeId == cri.GLCodeId);

            if (!string.IsNullOrEmpty(cri.ItemCode))
                queryableData = queryableData.Where(x => x.Code.Trim().ToLower().Contains(cri.ItemCode.Trim().ToLower()) || x.Name.Trim().ToLower().Contains(cri.ItemCode.Trim().ToLower()));

            if (!string.IsNullOrEmpty(cri.PickupPoint))
                queryableData = queryableData.Where(x => x.PickUpPoint.Trim().ToLower().Contains(cri.PickupPoint.Trim().ToLower()));

            if (!string.IsNullOrEmpty(cri.DropPoint))
                queryableData = queryableData.Where(x => x.DropPoint.Trim().ToLower().Contains(cri.DropPoint.Trim().ToLower()));

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
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<Price>(columnName, dataType));
                                    break;
                                case BaseCri.DataType.Int64:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpressionInt64<Price>(columnName));
                                    break;
                                default:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<Price>(columnName, dataType));
                                    break;
                            }
                            break;
                        case BaseCri.EntityOrderBy.Dsc:
                            switch (dataType)
                            {
                                case BaseCri.DataType.String:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<Price>(columnName, dataType));
                                    break;
                                case BaseCri.DataType.Int64:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpressionInt64<Price>(columnName));
                                    break;
                                default:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<Price>(columnName, dataType));
                                    break;
                            }
                            break;
                        default:
                            switch (dataType)
                            {
                                case BaseCri.DataType.String:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<Price>(columnName, dataType));
                                    break;
                                case BaseCri.DataType.Int64:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpressionInt64<Price>(columnName));
                                    break;
                                default:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<Price>(columnName, dataType));
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

        public async Task<Price> FindByPickUpAndDropOff(long pickUpLocationId, long dropOffLocationId,long customerId)
        {
            if (pickUpLocationId == 0 || dropOffLocationId == 0)
                return null;

            var price = await context.Prices.Include(x=> x.GeneralLedger).AsNoTracking().FirstOrDefaultAsync(x => x.PickUpPointId == pickUpLocationId && x.DropPointId == dropOffLocationId && x.CustomerId == customerId && x.ParentCompanyId == GlobalVariable.COMPANY_ID);

            return price;
        }

        public async Task<Price> GetByGLCodeId(long glCodeId, long customerId)
        {
            return await context.Prices.Include(x => x.GeneralLedger).AsNoTracking().FirstOrDefaultAsync(x => x.GLCodeId == glCodeId && x.CustomerId == customerId && x.ParentCompanyId == GlobalVariable.COMPANY_ID);
        }

        public IEnumerable<Price> GetTripChargesByGLCodeAndCustomerId(Int64 glCodeId, Int64 customerId)
        {
            var queryableData = context.Prices.AsNoTracking().Where(x => x.ParentCompanyId == GlobalVariable.COMPANY_ID);

            if(customerId != 0)
            {
                queryableData = queryableData.Where(x => x.CustomerId == customerId);
            }

            if(glCodeId != 0)
            {
                queryableData = queryableData.Where(x => x.GLCodeId == glCodeId);
            }

            return queryableData;
        }

        public async override Task<Price> GetById(object id)
        {
            var price = await base.GetById(id);

            price.PriceLocationList = context.PriceLocations.Include(c => c.Location).AsNoTracking().Where(x => x.PriceId == price.Id).ToList();

            foreach (var location in price.PriceLocationList)
            {
                if (location.Location == null) location.Location = new ApplicationCore.Entities.Configurations.Location { PostalCode = "TBD", Code = "TBD", Name = "TBD" };
            }

            return price;
        }

        public override void Save(Price model)
        {
            base.Save(model);

            var hasPermissionForLocation = _rolePermissionRepository.HasPermission("PriceLocationController", HttpContext.Current.User.Identity.GetUserId());

            if (hasPermissionForLocation)
            {
                if (model.Id != 0)
                {
                    foreach (var detail in context.PriceLocations.Where(x => x.PriceId == model.Id))
                    {
                        context.PriceLocations.Remove(detail);
                    }
                }

                foreach (var detail in model.GetLocations())
                {
                    if(detail.Type == PriceLocation.PriceLocationType.PickUp)
                    {
                        var location = context.Locations.AsNoTracking().FirstOrDefault(x => x.Id == detail.LocationId);
                        if (location != null)
                            model.PickUpPoint = location.Display;
                        model.PickUpPointId = location.Id;
                    }
                    if(detail.Type == PriceLocation.PriceLocationType.DropOff)
                    {
                        var location = context.Locations.AsNoTracking().FirstOrDefault(x => x.Id == detail.LocationId);
                        if (location != null)
                            model.DropPoint = location.Display;
                        model.DropPointId = location.Id;
                    }
                    context.PriceLocations.Add(detail);
                }
            }
        }

        public async Task Saves(List<Price> priceList)
        {
            try
            {
                SerialNoRepository<PriceSerialNo> serialNoRepoitory = new SerialNoRepository<PriceSerialNo>(context);

                #region SystemSettingCri
                SystemSettingCri sysCri = new SystemSettingCri()
                {
                    StringCris = new Dictionary<string, StringValue>()
                };

                sysCri.StringCris["Code"] = new StringValue { ComparisonOperator = BaseCri.StringComparisonOperator.Equal, Value = AppSettingKey.Key.PRICE_CODE_NUM_COUNT.ToString() };

                var setting = await _systemSettingRepository.GetByCri(sysCri);
                #endregion
                foreach (var price in priceList)
                {
                    //var dbPrice = context.Prices.FirstOrDefault(x => x.CustomerId == price.CustomerId && x.Name.Equals(price.Name));

                    if (price.Id == 0)
                    {
                        context.Prices.Add(price);

                        foreach (var priceLocation in price.PriceLocationList)
                        {
                            priceLocation.Price = price;
                            context.PriceLocations.Add(priceLocation);
                        }
                        await context.SaveChangesAsync();

                        price.Code = price.GetPriceCode(serialNoRepoitory.GetSerialNoWithNoTimeConstraint(price.Id), Convert.ToInt32(setting.FirstOrDefault().Value));
                    }
                    else
                    {
                        base.Save(price);
                    }


                    await context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {

            }            
        }

        public IEnumerable<Price> GetByCustomerId(long customerId)
        {
            var priceList = context.Prices.Where(x => x.CustomerId == customerId);

            if (priceList == null) return new List<Price>();

            return priceList;           
        }

        public override void Delete(Price model)
        {
            if (this.dbSet == null) return;

            if (model == null)
                return;
            Price entityToDelete = this.dbSet.SingleOrDefault(x => x.Id == model.Id && x.ParentCompanyId == GlobalVariable.COMPANY_ID);
            if (model != null)
                this.dbSet.Remove(entityToDelete);
        }

        public bool IsPriceReferenced(Int64 priceId)
        {
            var saleInvoiceDetailsItems = context.SalesInvoiceDetails.Where(x => x.PriceId == priceId);

            if(saleInvoiceDetailsItems == null || saleInvoiceDetailsItems.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
