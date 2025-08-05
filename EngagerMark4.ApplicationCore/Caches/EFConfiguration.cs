using EFCache;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Common;

namespace EngagerMark4.ApplicationCore.Caches
{
    public class EFConfiguration : DbConfiguration
    {
        public EFConfiguration()
        {
            bool useCahce = true;
            try
            {
                useCahce = Convert.ToBoolean(ConfigurationManager.AppSettings["UseCache"]);
            }
            catch (Exception ex)
            {

            }

            if (useCahce)
            {
                try
                {
                    var transactionHandler = new CacheTransactionHandler(new InMemoryCache());

                    AddInterceptor(transactionHandler);

                    var cachingPolicy = new CachingPolicy();

                    Loaded += (sender, args) => args.ReplaceService<DbProviderServices>(
                        (s, _) => new CachingProviderServices(s, transactionHandler, cachingPolicy));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
