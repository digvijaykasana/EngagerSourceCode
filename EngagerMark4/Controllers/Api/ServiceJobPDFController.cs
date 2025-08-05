using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.IService.Jobs;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using EngagerMark4.Common;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Api
{
    public class ServiceJobPDFController : BaseController<ServiceJobCri, ServiceJob, IServiceJobService>
    {
        ICompanyService _companyService;
        ICustomerService _customerService;
        IWorkOrderService _workOrderService;
        ILetterheadService _letterheadService;

        public ServiceJobPDFController(IServiceJobService service,
            ICompanyService companyService, ICustomerService customerService, IWorkOrderService workOrderService, ILetterheadService letterheadService) : base(service)
        {
            this._companyService = companyService;
            this._customerService = customerService;
            this._workOrderService = workOrderService;
            this._letterheadService = letterheadService;
        }
        

        public async Task<ActionResult> DownloadVoucher(Int64 serviceJobId)
        {
            var serviceJob = await this._service.GetById(serviceJobId);

            if (serviceJob == null) return HttpNotFound();

            var customer = await this._customerService.GetById(serviceJob.CustomerId);

            var defaultLetterhead = this._letterheadService.GetDefaultLetterhead();

            switch(customer.TransferVoucherFormat)
            {
                case TransferVoucherConfig.TransferVoucherFormat.InchcapeFormat:

                    var inchapeSJViewModel = new ServiceJobInchcapeViewModel
                    {
                        ReferenceNo = serviceJob.ReferenceNo,
                        ReferenceNoNumber = serviceJob.ReferenceNoNumber,
                        SignatureId = serviceJob.SignatureId,
                        WorkOrder = serviceJob.WorkOrder,
                        VehicleId = serviceJob.VehicleId,
                        Vehicle = serviceJob.Vehicle,
                        StartTimeStr = TimeUtil.ConvertTo24HrsFormat(serviceJob.WorkOrder.PickUpdateDate),
                        EndExecutionTimeStr = TimeUtil.ConvertTo24HrsFormat(serviceJob.EndExecutionTime),
                        LetterheadId = customer.LetterheadId.HasValue ? customer.LetterheadId.Value : 0
                    };

                    if(inchapeSJViewModel.LetterheadId == 0 && defaultLetterhead != null)
                    {
                        inchapeSJViewModel.LetterheadId = defaultLetterhead.Id;
                    }

                    return base.File(base.GeneratePDF<ServiceJobInchcapeViewModel>(inchapeSJViewModel, FileConfig.SERVICE_JOBS, inchapeSJViewModel.ReferenceNo), CONTENT_DISPOSITION, inchapeSJViewModel.ReferenceNo + ".pdf");

                case TransferVoucherConfig.TransferVoucherFormat.WallemFormat:

                    var wallemSJViewModel = new ServiceJobWallemViewModel
                    {
                        ReferenceNo = serviceJob.ReferenceNo,
                        ReferenceNoNumber = serviceJob.ReferenceNoNumber,
                        SignatureId = serviceJob.SignatureId,
                        WorkOrder = serviceJob.WorkOrder,
                        VehicleId = serviceJob.VehicleId,
                        Vehicle = serviceJob.Vehicle,
                        StartDateTimeStr = TimeUtil.ConvertToDateTime24HrsFormat(serviceJob.WorkOrder.PickUpdateDate),
                        LetterheadId = customer.LetterheadId.HasValue ? customer.LetterheadId.Value : 0,
                        ServiceJobId = serviceJob.Id
                    };

                    if (wallemSJViewModel.LetterheadId == 0 && defaultLetterhead != null)
                    {
                        wallemSJViewModel.LetterheadId = defaultLetterhead.Id;
                    }

                    return base.File(base.GeneratePDF<ServiceJobWallemViewModel>(wallemSJViewModel, FileConfig.SERVICE_JOBS, wallemSJViewModel.ReferenceNo), CONTENT_DISPOSITION, wallemSJViewModel.ReferenceNo + ".pdf");

                default:

                    var company = await _companyService.GetById(GlobalVariable.COMPANY_ID);

                    if (company == null) company = new ApplicationCore.Entities.Company();

                    serviceJob.ShortText10 = company.TransferVoucherLogo;
                    serviceJob.LetterheadId = customer.LetterheadId.HasValue ? customer.LetterheadId.Value : 0;

                    if (serviceJob.LetterheadId == 0 && defaultLetterhead != null)
                    {
                        serviceJob.LetterheadId = defaultLetterhead.Id;
                    }

                    return base.File(base.GeneratePDF<ServiceJob>(serviceJob, FileConfig.SERVICE_JOBS, serviceJob.ReferenceNo), CONTENT_DISPOSITION, serviceJob.ReferenceNo + ".pdf");
            }            
        }
    }
}