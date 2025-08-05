using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.Repository
{
    public abstract class UOW<Context>
        where Context : DbContext
    {
        protected Context context;
        protected DbContextTransaction dbTransaction;

        public UOW(Context context)
        {
            this.context = context;
        }

        public virtual void BeginTransaction()
        {
            if (context != null)
                dbTransaction = context.Database.BeginTransaction();
        }

        public virtual void CommitTransaction()
        {
            if (dbTransaction != null) this.dbTransaction.Commit();
        }

        public virtual void RollbackTransaction()
        {
            if (dbTransaction != null) this.dbTransaction.Rollback();
        }

        public virtual void DisposeTransaction()
        {
            if (dbTransaction != null) this.dbTransaction.Dispose();
        }

        public virtual void SaveChanges()
        {
            if (context != null) this.context.SaveChanges();
        }

        public async virtual Task SaveChangesAsync()
        {
            if (context != null)
                try
                { 
                await this.context.SaveChangesAsync();
                }
                catch(Exception ex)
                { }
        }
    }
}
