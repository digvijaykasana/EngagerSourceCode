using EngagerMark4.ApplicationCore.Inventory.Cris;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Inventory.IRepository.Price
{
    public interface IPriceLocationRepository : IBaseRepository<PriceLocationCri, PriceLocation>
    {
        IEnumerable<PriceLocation> GetByPriceId(Int64 priceId);
    }
}
