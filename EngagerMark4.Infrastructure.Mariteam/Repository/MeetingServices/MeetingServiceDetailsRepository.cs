using EngagerMark4.Infrasturcture.Repository;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.ApplicationCore.Dummy.Cris.MeetingServices;
using EngagerMark4.ApplicationCore.Dummy.Entities.MeetingServices;
using EngagerMark4.ApplicationCore.Dummy.IRepository.MeetingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.Dummy.Repository.MeetingServices
{
    public class MeetingServiceDetailsRepository : GenericRepository<ApplicationDbContext, MeetingServiceDetailsCri, MeetingServiceDetails>, IMeetingServiceDetailsRepository
    {
        public MeetingServiceDetailsRepository(ApplicationDbContext aContext) : base(aContext)
        { }
    }
}
