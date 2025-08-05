using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.Dummy.Cris.MeetingServices;
using EngagerMark4.ApplicationCore.Dummy.Entities.MeetingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Dummy.IRepository.MeetingServices
{
    public interface IMeetingServiceDetailsRepository : IBaseRepository<MeetingServiceDetailsCri, MeetingServiceDetails>
    {
    }
}
