using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.Repository.Configurations
{
    public class HotelRepository : GenericRepository<ApplicationDbContext, HotelCri, Hotel>, IHotelRepository
    {
        public HotelRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        protected override void ApplyCri(HotelCri cri)
        {
            if (cri != null && !string.IsNullOrEmpty(cri.SearchValue))
                base.queryableData = queryableData.Where(x => x.Name.ToLower().Contains(cri.SearchValue.ToLower()) || x.PostalCode.ToLower().Contains(cri.SearchValue.ToLower()) || x.Address.ToLower().Contains(cri.SearchValue.ToLower()));
        }

        public async Task Saves(List<Hotel> hotels)
        {
            foreach (var hotel in hotels.Distinct())
            {
                var dbHotel = context.Hotels.FirstOrDefault(x => x.Name.Equals(hotel.Name));
                if (dbHotel == null)
                    context.Hotels.Add(hotel);
            }

            await this.context.SaveChangesAsync();
        }
    }
}
