using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
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
    public class MeetingServiceRepository : GenericRepository<ApplicationDbContext, MeetingServiceCri, MeetingService>, IMeetingServiceRepository
    {
        public MeetingServiceRepository(ApplicationDbContext aContext) : base(aContext)
        { }

        public async override Task<MeetingService> GetById(object id)
        {
            var meetingService = await base.GetById(id);

            meetingService.MeetingServiceDetails = context.MeetingServiceDetails.AsNoTracking().Where(x => x.MeetingServiceId == meetingService.Id).ToList();

            return meetingService;
        }

        public override void Save(MeetingService model)
        {
            base.Save(model);

            
            if (model.Id != 0)
            {
                foreach (var detail in context.MeetingServiceDetails.Where(x => x.MeetingServiceId == model.Id))
                {
                    var meetingServiceDetail = context.MeetingServiceDetails.FirstOrDefault(x => x.Id == detail.MeetingServiceId);
                    if (meetingServiceDetail != null)
                        context.MeetingServiceDetails.Remove(meetingServiceDetail);
                    context.MeetingServiceDetails.Remove(detail);
                }
            }

            foreach (var detail in model.GetMeetingServiceDetails())
            {
                context.MeetingServiceDetails.Add(detail);
            }
        }
    }
}
