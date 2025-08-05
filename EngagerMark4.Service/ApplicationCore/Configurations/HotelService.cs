using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.ApplicationCore.Configurations
{
    public class HotelService : AbstractService<IHotelRepository, HotelCri, Hotel>, IHotelService
    {
        public HotelService(IHotelRepository repository) : base(repository)
        {
        }
    }
}
