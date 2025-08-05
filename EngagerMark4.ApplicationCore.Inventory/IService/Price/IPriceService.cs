using EngagerMark4.ApplicationCore.Inventory.Cris;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Inventory.IService.Price
{
    public interface IPriceService : IBaseService<PriceCri, Entities.Price>
    {
        Task<Entities.Price> FindByPickUpAndDropOff(Int64 pickUpLocationId, Int64 dropOffLocationId, Int64 customerId);

        Task<Entities.Price> GetByGLCodeId(Int64 glCodeId, Int64 customerId);

        Task<Int32> ImportReturnTripChargesByCustomerId(Int64 customerId);
    }
}
