using EngagerMark4.Service.ApplicationCore;
using EngagerMark4.ApplicationCore.Dummy.IRepository.MeetingServices;
using EngagerMark4.ApplicationCore.Dummy.Cris.MeetingServices;
using EngagerMark4.ApplicationCore.Dummy.Entities.MeetingServices;
using EngagerMark4.ApplicationCore.Dummy.IService.MeetingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.Dummy.ApplicationCore.MeetingServices
{
    public class MeetingServiceService : AbstractService<IMeetingServiceRepository, MeetingServiceCri, MeetingService>, IMeetingServiceService
    {
        public MeetingServiceService(IMeetingServiceRepository repository) : base(repository)
        { }
    }
}
