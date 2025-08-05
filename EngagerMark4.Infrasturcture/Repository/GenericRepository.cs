using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.Common;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Infrasturcture.ExpressionBuilders;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EngagerMark4.Infrasturcture.Repository
{
    public abstract class GenericRepository<Context, Cri, Entity> : UOW<Context>
        where Context : DbContext
        where Entity : BasicEntity
        where Cri : BaseCri
    {
        protected DbSet<Entity> dbSet;

        public GenericRepository(Context aContext)
            : base(aContext)
        {
            this.dbSet = this.context.Set<Entity>();
        }

        protected IQueryable<Entity> queryableData;

        public async virtual Task<IEnumerable<Entity>> GetByCri(Cri cri)
        {
            queryableData = this.dbSet.AsNoTracking().Where(x => x.ParentCompanyId == GlobalVariable.COMPANY_ID);

            if (cri != null && cri.Includes != null) 
            {
                foreach(var include in cri.Includes)
                {
                    queryableData = queryableData.Include(include).AsNoTracking();
                }
            }

            ApplyCri(cri);

            if(cri!=null && cri.OrderBys!=null)
            {
                foreach(var columnName in cri.OrderBys.Keys)
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
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<Entity,String>(columnName));
                                    break;
                                case BaseCri.DataType.Int64:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpressionInt64<Entity>(columnName));
                                    break;
                                case BaseCri.DataType.DateTime:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<Entity, DateTime>(columnName));
                                    break;
                                default:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<Entity>(columnName, dataType));
                                    break;
                            }
                            break;
                        case BaseCri.EntityOrderBy.Dsc:
                            switch (dataType)
                            {
                                case BaseCri.DataType.String:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<Entity,String>(columnName));
                                    break;
                                case BaseCri.DataType.Int64:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpressionInt64<Entity>(columnName));
                                    break;
                                case BaseCri.DataType.DateTime:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<Entity, DateTime>(columnName));
                                    break;
                                default:
                                    queryableData = queryableData.OrderByDescending(ExpressionBuilder.GetExpression<Entity>(columnName, dataType));
                                    break;
                            }
                            break;
                        default:
                            switch (dataType)
                            {
                                case BaseCri.DataType.String:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<Entity, String>(columnName));
                                    break;
                                case BaseCri.DataType.Int64:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpressionInt64<Entity>(columnName));
                                    break;
                                case BaseCri.DataType.DateTime:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<Entity, DateTime>(columnName));
                                    break;
                                default:
                                    queryableData = queryableData.OrderBy(ExpressionBuilder.GetExpression<Entity>(columnName, dataType));
                                    break;
                            }
                            break;
                    }
                }
            }

            if (cri != null && cri.IsPagination)
                queryableData = queryableData.Skip(cri.NoOfPage * (cri.CurrentPage - 1)).Take(cri.NoOfPage);

            var result = await queryableData.ToListAsync();

            return result;
        }

        protected virtual void ApplyCri(Cri cri)
        {
            if (cri != null && cri.StringCris != null)
            {
                foreach (var column in cri.StringCris.Keys)
                {
                    StringValue strValue = cri.StringCris[column];

                    if (strValue != null && !string.IsNullOrEmpty(strValue.Value))
                    {
                        switch (strValue.ComparisonOperator)
                        {
                            case BaseCri.StringComparisonOperator.Equal:
                                queryableData = queryableData.Where(ExpressionBuilder.GetEqualExpression<Entity>(column, strValue.Value));
                                break;
                            case BaseCri.StringComparisonOperator.Contains:
                                queryableData = queryableData.Where(ExpressionBuilder.GetContainsExpression<Entity>(column, strValue.Value));
                                break;
                            case BaseCri.StringComparisonOperator.StartsWith:
                                queryableData = queryableData.Where(ExpressionBuilder.GetStartsWithExpression<Entity>(column, strValue.Value));
                                break;
                            case BaseCri.StringComparisonOperator.EndsWith:
                                queryableData = queryableData.Where(ExpressionBuilder.GetEndsWithExpression<Entity>(column, strValue.Value));
                                break;
                            default:
                                queryableData = queryableData.Where(ExpressionBuilder.GetEqualExpression<Entity>(column, strValue.Value));
                                break;
                        }
                    }
                }
            }

            if (cri != null && cri.NumberCris != null)
            {
                foreach (var columnName in cri.NumberCris.Keys)
                {
                    IntValue intValue = cri.NumberCris[columnName];

                    if (intValue != null)
                    {
                        switch (intValue.ComparisonOperator)
                        {
                            case BaseCri.NumberComparisonOperator.Equal:
                                queryableData = queryableData.Where(ExpressionBuilder.GetEqualExpression<Entity>(columnName, intValue.Value));
                                break;
                            case BaseCri.NumberComparisonOperator.GreaterThan:
                                queryableData = queryableData.Where(ExpressionBuilder.GetGreaterThanExpression<Entity>(columnName, intValue.Value));
                                break;
                            case BaseCri.NumberComparisonOperator.GreaterThanEqual:
                                queryableData = queryableData.Where(ExpressionBuilder.GetGreaterThanOrEqualExpression<Entity>(columnName, intValue.Value));
                                break;
                            case BaseCri.NumberComparisonOperator.LessThan:
                                queryableData = queryableData.Where(ExpressionBuilder.GetLessThanExpression<Entity>(columnName, intValue.Value));
                                break;
                            case BaseCri.NumberComparisonOperator.LessThanEqual:
                                queryableData = queryableData.Where(ExpressionBuilder.GetLessThanOrEqualExpression<Entity>(columnName, intValue.Value));
                                break;
                            default:
                                queryableData = queryableData.Where(ExpressionBuilder.GetEqualExpression<Entity>(columnName, intValue.Value));
                                break;
                        }
                    }
                }
            }

            if (cri != null && cri.DecimalCris != null)
            {
                foreach (var columnName in cri.DecimalCris.Keys)
                {
                    DecimalValue deciamlValue = cri.DecimalCris[columnName];

                    if (deciamlValue != null)
                    {
                        switch (deciamlValue.ComparisonOperator)
                        {
                            case BaseCri.NumberComparisonOperator.Equal:
                                queryableData = queryableData.Where(ExpressionBuilder.GetEqualExpression<Entity>(columnName, deciamlValue.Value));
                                break;
                            case BaseCri.NumberComparisonOperator.GreaterThan:
                                queryableData = queryableData.Where(ExpressionBuilder.GetGreaterThanExpression<Entity>(columnName, deciamlValue.Value));
                                break;
                            case BaseCri.NumberComparisonOperator.GreaterThanEqual:
                                queryableData = queryableData.Where(ExpressionBuilder.GetGreaterThanOrEqualExpression<Entity>(columnName, deciamlValue.Value));
                                break;
                            case BaseCri.NumberComparisonOperator.LessThan:
                                queryableData = queryableData.Where(ExpressionBuilder.GetLessThanExpression<Entity>(columnName, deciamlValue.Value));
                                break;
                            case BaseCri.NumberComparisonOperator.LessThanEqual:
                                queryableData = queryableData.Where(ExpressionBuilder.GetLessThanOrEqualExpression<Entity>(columnName, deciamlValue.Value));
                                break;
                            default:
                                queryableData = queryableData.Where(ExpressionBuilder.GetEqualExpression<Entity>(columnName, deciamlValue.Value));
                                break;
                        }
                    }
                }
            }
        }

        public async virtual Task<Entity> GetById(object id)
        {
            return await this.dbSet.SingleOrDefaultAsync(x => x.Id ==(Int64)id && x.ParentCompanyId == GlobalVariable.COMPANY_ID);
        }

        protected string userId = string.Empty;

        //protected string GetCurrentUserId()
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(this.userId))
        //            return this.userId;
        //        string userName = HttpContext.Current.User.Identity.Name;
        //        string userId = HttpContext.Current.User.Identity.GetUserId();
        //        return userId;
        //    }
        //    catch (Exception ex)
        //    {
        //        return userId;
        //    }
        //}

        protected string GetCurrentUserId()
        {
            try
            {
                if (!string.IsNullOrEmpty(GlobalVariable.mobile_userId) && HttpContext.Current.Request.IsAuthenticated == false)
                    return GlobalVariable.mobile_userId;

                if (!string.IsNullOrEmpty(this.userId))
                    return this.userId;

                string userName = HttpContext.Current.User.Identity.Name;
                string userId = HttpContext.Current.User.Identity.GetUserId();
                return userId;
            }
            catch (Exception ex)
            {
                return userId;
            }
        }

        public string userName = string.Empty;

        protected string GetUserName()
        {
            if (!string.IsNullOrEmpty(userName))
                return userName;
            try
            {
                if (!String.IsNullOrEmpty(GlobalVariable.mobile_userName) && HttpContext.Current.Request.IsAuthenticated == false)
                {
                    return GlobalVariable.mobile_userName;
                }
                else
                {
                    return GlobalVariable.USER_NAMES[GetCurrentUserId()];
                }
            }
            catch(Exception ex)
            {
                return string.Empty;
            }
        }

        public virtual void Save(Entity model)
        {
            if (this.dbSet == null)
                return;
            if (model.Id == 0)
            {
                model.Created = TimeUtil.GetLocalTime();
                model.Modified = TimeUtil.GetLocalTime();
                model.CreatedBy = GetCurrentUserId();
                model.CreatedByName = GetUserName();
                this.dbSet.Add(model);
            }
            else
            {
                this.dbSet.Attach(model);
                this.context.Entry(model).State = EntityState.Modified;
                this.context.Entry(model).Property(x => x.Created).IsModified = false;
                this.context.Entry(model).Property(x => x.CreatedBy).IsModified = false;
                this.context.Entry(model).Property(x => x.CreatedByName).IsModified = false;
                model.Modified = TimeUtil.GetLocalTime();
                model.ModifiedBy = GetCurrentUserId();
                model.ModifiedByName = GetUserName();
            }
        }

        public virtual void ExecuteWithGraph(Entity model)
        {
            if (this.dbSet == null)
                return;

            this.dbSet.Add(model);

            CheckForEntitiesWithoutBaseEntity(this.context);

            foreach (var entry in this.context.ChangeTracker.Entries<BasicEntity>())
            {
                entry.State = ConverState(entry.Entity.State);
                if (entry.State == EntityState.Added)
                {
                    entry.Property(x => x.Created).CurrentValue = TimeUtil.GetLocalTime();
                    entry.Property(x => x.CreatedBy).CurrentValue = GetCurrentUserId();
                    entry.Property(x => x.CreatedByName).CurrentValue = GetUserName();
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.Created).IsModified = false;
                    entry.Property(x => x.CreatedBy).IsModified = false;
                    entry.Property(x => x.CreatedByName).IsModified = false;
                    entry.Property(x => x.Modified).CurrentValue = TimeUtil.GetLocalTime();
                    entry.Property(x => x.ModifiedBy).CurrentValue = GetCurrentUserId();
                    entry.Property(x => x.ModifiedByName).CurrentValue = GetUserName();
                }
            }
        }

        private EntityState ConverState(BaseEntity.ModelState state)
        {
            switch (state)
            {
                case BaseEntity.ModelState.Added:
                    return EntityState.Added;
                case BaseEntity.ModelState.Modified:
                    return EntityState.Modified;
                case BaseEntity.ModelState.Deleted:
                    return EntityState.Deleted;
                case BaseEntity.ModelState.UnChanged:
                    return EntityState.Unchanged;
                default:
                    return EntityState.Unchanged;
            }
        }

        private void CheckForEntitiesWithoutBaseEntity(Context db)
        {
            var entitiesWithoutBase = from e in db.ChangeTracker.Entries()
                                      where !(e.Entity is BasicEntity)
                                      select e;

            if (entitiesWithoutBase.Any())
            {
                throw new NotSupportedException("All entities must implement Base Entity");
            }
        }

        public virtual void Delete(Entity model)
        {
            if (this.dbSet == null) return;

            if (model == null)
                return;
            Entity entityToDelete = this.dbSet.SingleOrDefault(x => x.Id == model.Id && x.ParentCompanyId == GlobalVariable.COMPANY_ID);
            if (model != null)
                this.dbSet.Remove(entityToDelete);
        }
        
    }
}
