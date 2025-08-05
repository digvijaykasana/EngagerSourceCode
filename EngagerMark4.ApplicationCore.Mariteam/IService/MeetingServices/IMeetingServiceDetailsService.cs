using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.Dummy.Cris.MeetingServices;
using EngagerMark4.ApplicationCore.Dummy.Entities.MeetingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Dummy.IService.MeetingServices
{
    public interface IMeetingServiceDetailsService : IBaseService<MeetingServiceDetailsCri, MeetingServiceDetails>
    {
    }
}
