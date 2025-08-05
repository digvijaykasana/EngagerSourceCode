using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.IRepository.Jobs;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EngagerMark4.Common;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;

namespace EngagerMark4.Infrastructure.SOP.Repository.Jobs
{
    public class ServiceJobRepository : GenericRepository<ApplicationDbContext, ServiceJobCri, ServiceJob>, IServiceJobRepository
    {
        int count;

        public ServiceJobRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        #region Load References

        public async Task<IEnumerable<ServiceJob>> GetServiceJobsByWorkOrderId(long workOrderId)
        {
            var serviceJobs = context.ServiceJobs.Include(x => x.User).AsNoTracking().Where(x => x.WorkOrderId == workOrderId);

            return serviceJobs;
        }

        public async override Task<ServiceJob> GetById(object id)
        {
            var serviceJob = await context.ServiceJobs.AsNoTracking().Include(x => x.WorkOrder.Customer).Include(x => x.WorkOrder.User).FirstOrDefaultAsync(x => x.Id == (Int64)id);

            var vessel = context.CommonConfigurations.FirstOrDefault(x => x.Id == serviceJob.WorkOrder.VesselId);
            serviceJob.WorkOrder.VesselName = vessel == null ? string.Empty : vessel.Name;

            var boardType = context.CommonConfigurations.FirstOrDefault(x => x.Id == serviceJob.WorkOrder.BoardTypeId);
            serviceJob.WorkOrder.BoardType = boardType == null ? string.Empty : boardType.Name;
            serviceJob.WorkOrder.WorkOrderLocationList = context.WorkOrderLocations.Include(x => x.Location).AsNoTracking().Where(x => x.WorkOrderId == serviceJob.WorkOrderId).ToList();
            serviceJob.WorkOrder.WorkOrderPassengerList = context.WorkOrderPassengers.AsNoTracking().Where(x => x.WorkOrderId == serviceJob.WorkOrderId).ToList();
            serviceJob.WorkOrder.MeetingServiceList = context.WorkOrderMeetingServices.AsNoTracking().Where(x => x.WorkOrderId == serviceJob.WorkOrderId).ToList();
            serviceJob.Vehicle = context.Vehicles.AsNoTracking().Where(x => x.Id == serviceJob.VehicleId).FirstOrDefault();
            var company = context.Companies.FirstOrDefault(x => x.Id == GlobalVariable.COMPANY_ID);
            serviceJob.CompanyName = company == null ? string.Empty : company.Name;
            return serviceJob;
        }

        public async override Task<IEnumerable<ServiceJob>> GetByCri(ServiceJobCri cri)
        {
            var queryableData = await base.GetByCri(cri);

            if (cri.WorkOrderId != 0)
            {
                queryableData = queryableData.Where(x => x.WorkOrderId == cri.WorkOrderId);
            }

            return queryableData;
        }

        public async Task<ServiceJob> GetByIdWithTracking(object id)
        {
            var serviceJob = await base.GetById(id);

            return serviceJob;
        }

        #endregion

        #region Save Data

        //OLD VERSION
        public void UpdateSignature(long serviceJobId, string signatureName, long signatureId)
        {
            var serviceJob = context.ServiceJobs.FirstOrDefault(x => x.Id == serviceJobId);

            if (serviceJob != null)
            {
                serviceJob.SignatureName = signatureName;
                serviceJob.SignatureId = signatureId;
                this.context.SaveChanges();
            }
        }

        //PCR2021
        public void UpdateSignatureData(long serviceJobId, string signatureName, long signatureId)
        {
            var serviceJob = context.ServiceJobs.FirstOrDefault(x => x.Id == serviceJobId);

            if (serviceJob != null)
            {
                serviceJob.SignatureName = signatureName;
                this.context.SaveChanges();
            }
        }

        #endregion

        #region OBSOLETE

        //OBSOLETE - Aung Ye Kaung - 20191022
        //public void AcknowledgeByDriver(long serviceJobId)
        //{
        //    count = 0;

        //    var serviceJob = context.ServiceJobs.FirstOrDefault(x => x.Id == serviceJobId);

        //    serviceJob.Status = ServiceJob.ServiceJobStatus.Scheduled;

        //    var jobs = context.ServiceJobs.Where(x => x.WorkOrderId == serviceJob.WorkOrderId).ToList();

        //    var workOrder = context.WorkOrders.FirstOrDefault(x => x.Id == serviceJob.WorkOrderId);

        //    if (workOrder != null)
        //    {
        //        if (jobs.Count > 0)
        //        {
        //            foreach (ServiceJob job in jobs)
        //            {
        //                if (job.Status >= ServiceJob.ServiceJobStatus.Scheduled)
        //                {
        //                    count++;
        //                }
        //                else
        //                {
        //                    if (job.Id == serviceJobId)
        //                    {
        //                        count++;
        //                    }
        //                }
        //            }

        //            if (count == jobs.Count())
        //            {
        //                if (workOrder != null)
        //                    workOrder.Status = WorkOrder.OrderStatus.Scheduled;
        //            }
        //        }
        //    }
        //}


        //OBSOLETE - Aung Ye Kaung - 20191022
        //public ServiceJob BeginExecution(long serviceJobId)
        //{
        //    count = 0;

        //    var serviceJob = context.ServiceJobs.FirstOrDefault(x => x.Id == serviceJobId);

        //    serviceJob.Status = ServiceJob.ServiceJobStatus.In_Progress;

        //    var jobs = context.ServiceJobs.Where(x => x.WorkOrderId == serviceJob.WorkOrderId).ToList();

        //    var workOrder = context.WorkOrders.FirstOrDefault(x => x.Id == serviceJob.WorkOrderId);

        //    if (workOrder != null)
        //    {
        //        if (jobs.Count > 0)
        //        {
        //            foreach (ServiceJob job in jobs)
        //            {
        //                if (job.Status >= ServiceJob.ServiceJobStatus.In_Progress)
        //                {
        //                    count++;
        //                }
        //                else
        //                {
        //                    if (job.Id == serviceJobId)
        //                    {
        //                        count++;
        //                    }
        //                }
        //            }

        //            if (count == jobs.Count())
        //            {
        //                if (workOrder != null)
        //                    workOrder.Status = WorkOrder.OrderStatus.In_Progress;
        //            }
        //        }
        //    }

        //    return serviceJob;
        //}

        //OBSOLETE - Aung Ye Kaung -20191022
        //public ServiceJob EndExecution(long serviceJobId, string standByDate, string standByTime, string reason)
        //{
        //    var serviceJob = context.ServiceJobs.FirstOrDefault(x => x.Id == serviceJobId);

        //    serviceJob.Status = ServiceJob.ServiceJobStatus.Pending_Sign;

        //    if (!(standByDate == "" && standByTime == ""))
        //    {
        //        serviceJob.LongText1 = standByDate + " - " + standByTime;

        //        /**
        //         * Modified - Kaung [ 25-06-2018 ]
        //        workOrder.StandByDateBinding = standByDate;
        //        workOrder.StandByTimeBinding = standByTime;
        //        workOrder.SetStandByDateTime();**/
        //    }

        //    if (!(reason == ""))
        //    {
        //        serviceJob.LongText1 = reason;
        //    }

        //    var jobs = context.ServiceJobs.Where(x => x.WorkOrderId == serviceJob.WorkOrderId).ToList();

        //    var workOrder = context.WorkOrders.FirstOrDefault(x => x.Id == serviceJob.WorkOrderId);

        //    if (workOrder != null)
        //    {
        //        if (jobs.Count > 0)
        //        {
        //            foreach (ServiceJob job in jobs)
        //            {
        //                if (job.Status >= ServiceJob.ServiceJobStatus.In_Progress)
        //                {
        //                    count++;
        //                }
        //                else
        //                {
        //                    if (job.Id == serviceJobId)
        //                    {
        //                        count++;
        //                    }
        //                }
        //            }

        //            if (count == jobs.Count())
        //            {
        //                if (workOrder != null)
        //                    workOrder.Status = WorkOrder.OrderStatus.In_Progress;
        //            }
        //        }
        //    }

        //    return serviceJob;
        //}

        //OBSOLETE - Aung Ye Kaung- 20191022
        //public void SubmitTask(long serviceJobId, string checkListIds, string additionStops, string disposals, string waitingTime,
        //    string meetingServiceIds, long customDetentionId, string remark, int pickupPax)
        //{
        //    var serviceJob = context.ServiceJobs.FirstOrDefault(x => x.Id == serviceJobId);

        //    if (serviceJob != null)
        //    {
        //        serviceJob.CheckListIds = checkListIds;
        //        serviceJob.AdditionalStops = additionStops;
        //        serviceJob.Disposals = disposals;
        //        serviceJob.WaitingTime = waitingTime;
        //        serviceJob.MeetingServiceIds = meetingServiceIds;
        //        serviceJob.CustomDetentionId = customDetentionId;
        //        serviceJob.DriverRemark = remark;
        //        serviceJob.Status = ServiceJob.ServiceJobStatus.Submitted;

        //        count = 0;

        //        var jobs = context.ServiceJobs.Where(x => x.WorkOrderId == serviceJob.WorkOrderId).ToList();

        //        var workorder = context.WorkOrders.FirstOrDefault(x => x.Id == serviceJob.WorkOrderId);

        //        if (workorder != null)
        //        {
        //            workorder.PickUpPax = 0;

        //            if (jobs.Count > 0)
        //            {
        //                foreach (var job in jobs)
        //                {
        //                    if (job.Status == ServiceJob.ServiceJobStatus.Submitted)
        //                    {
        //                        count++;

        //                        workorder.PickUpPax += job.PickUpPax;
        //                    }
        //                    else
        //                    {
        //                        if (job.Id == serviceJobId)
        //                        {
        //                            count++;
        //                        }
        //                    }
        //                }

        //                if (count == jobs.Count())
        //                    workorder.Status = ApplicationCore.SOP.Entities.WorkOrders.WorkOrder.OrderStatus.Submitted;
        //            }
        //        }

        //        this.context.SaveChanges();
        //    }
        //}

        #endregion
    }
}
