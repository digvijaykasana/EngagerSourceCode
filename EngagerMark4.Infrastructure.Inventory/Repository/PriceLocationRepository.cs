using EngagerMark4.ApplicationCore.Inventory.Cris;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.Inventory.IRepository.Price;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.Inventory.Repository
{
    public class PriceLocationRepository : GenericRepository<ApplicationDbContext, PriceLocationCri, PriceLocation>, IPriceLocationRepository
    {
        public PriceLocationRepository(ApplicationDbContext aContext) : base(aContext)
        { }

        /// <summary>
        /// Added - 10/07/2019 - Aung Ye Kaung
        /// Returns list of price locations based on Price Id
        /// </summary>
        /// <param name="priceId"></param>
        /// <returns></returns>
        public IEnumerable<PriceLocation> GetByPriceId(Int64 priceId)
        {
            var priceLocations = context.PriceLocations.Where(x => x.PriceId == priceId);

            return priceLocations;
        }
    }
}
