using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.ApplicationCore.Dummy.IService.MeetingServices;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using EngagerMark4.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api
{
    public class MeetingServicePDFController : BaseController<WorkOrderCri, WorkOrder, IWorkOrderService>
    {
        IMeetingServiceService _meetingServiceService;
        IGSTService _gstService;

        public MeetingServicePDFController(IWorkOrderService service, IMeetingServiceService meetingServiceService, IGSTService gstService) : base(service)
        {
            this._meetingServiceService = meetingServiceService;
            this._gstService = gstService;
        }

        public async Task<ActionResult> DownloadMeetingServiceForm(Int64 workOrderId, Int64 workOrderMeetingServiceId)
        {
            MeetingServiceFormViewModel viewModel = new MeetingServiceFormViewModel();

            var workOrder = await this._service.GetById(workOrderId);

            if(workOrder == null)
            {
                return null;
            }

            viewModel.WorkOrder = workOrder;

            WorkOrderAirportMeetingService workOrderMeetingService = null;

            if (workOrder.MeetingServiceList.Count() > 0)
            {
                workOrderMeetingService = workOrder.MeetingServiceList.Where(x => x.Id == workOrderMeetingServiceId).FirstOrDefault();
            }
            
            if(workOrderMeetingService == null)
            {
                return null;
            }

            viewModel.WorkOrderMeetingServiceEntity = workOrderMeetingService;

            var meetingService = await _meetingServiceService.GetById(workOrderMeetingService.AirportMeetingServiceId);

            if(meetingService == null)
            {
                return null;
            }

            viewModel.MeetingServiceEntity = meetingService;

            var gsts = (await _gstService.GetByCri(null)).OrderBy(x => x.Name);

            if (gsts == null || gsts.ToList().Count == 0)
            {
                return null;
            }

            viewModel.GSTAmount = viewModel.WorkOrderMeetingServiceEntity.Charges * (gsts.Where(x=>x.isDefault).FirstOrDefault().GSTPercent / 100);

            return base.File(base.GeneratePDF<MeetingServiceFormViewModel>(viewModel, FileConfig.MEETING_SERVICES, DateTime.Now.ToString("ddMMyyyy") + "_" + workOrder.RefereneceNo + "_MeetingServiceForm"), CONTENT_DISPOSITION, DateTime.Now.ToString("ddMMyyyy") + "_" + workOrder.RefereneceNo + ".pdf");

        }
    }
}