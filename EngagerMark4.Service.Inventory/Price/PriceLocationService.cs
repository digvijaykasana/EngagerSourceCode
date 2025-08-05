using EngagerMark4.ApplicationCore.Inventory.Cris;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.Inventory.IRepository.Price;
using EngagerMark4.ApplicationCore.Inventory.IService.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.Inventory.Price
{
    public class PriceLocationService : AbstractService<IPriceLocationRepository, PriceLocationCri, PriceLocation>, IPriceLocationService
    {
        public PriceLocationService(IPriceLocationRepository repository) : base(repository)
        { }
    }
}
