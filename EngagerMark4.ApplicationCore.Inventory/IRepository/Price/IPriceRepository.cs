using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.Inventory.Cris;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Inventory.IRepository.Price
{
    public interface IPriceRepository : IBaseRepository<PriceCri, Entities.Price>
    {
        Task<Entities.Price> FindByPickUpAndDropOff(Int64 pickUpLocationId, Int64 dropOffLocationId, Int64 customerId);

        Task<Entities.Price> GetByGLCodeId(Int64 glCodeId, Int64 customerId);

        IEnumerable<Entities.Price> GetTripChargesByGLCodeAndCustomerId(Int64 glCodeId, Int64 customerId);

        Task Saves(List<Entities.Price> priceList);

        IEnumerable<Entities.Price> GetByCustomerId(Int64 customerId);

        bool IsPriceReferenced(Int64 priceId);
    }
}
