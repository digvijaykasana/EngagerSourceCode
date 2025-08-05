using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.IRepository.Jobs;
using EngagerMark4.ApplicationCore.SOP.IService.Jobs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.SOP.Jobs
{
    public class ServiceJobService : AbstractService<IServiceJobRepository, ServiceJobCri, ServiceJob>, IServiceJobService
    {
        ApplicationDbContext context;
        public ServiceJobService(IServiceJobRepository repository, ApplicationDbContext _context) : base(repository)
        {
            this.context = _context;
        }
                              
        #region Load References

        public async Task<ServiceJob> GetByIdWithTracking(long id)
        {
            var serviceJob = await this.repository.GetByIdWithTracking(id);

            return serviceJob;
        }

        public async Task<IEnumerable<ServiceJob>> GetServiceJobsByWorkOrderId(long workOrderId)
        {
            return await this.repository.GetServiceJobsByWorkOrderId(workOrderId);
        }

        #endregion

        #region Save Data
        public void SaveChanges()
        {
            this.repository.SaveChanges();
        }

        //OLD VERSION
        public void UpdateSignature(long serviceJobId, string signatureName, long signatureId)
        {
            this.repository.UpdateSignature(serviceJobId, signatureName, signatureId);
        }

        //PCR2021
        public void UpdateSignatureData(long serviceJobId, string signatureName, long signatureId)
        {
            this.repository.UpdateSignatureData(serviceJobId, signatureName, signatureId);
        }

        public async Task UpdateCopySign(Int64 serviceJobId, bool isCopySignNeeded)
        {
            var serviceJob = await this.repository.GetById(serviceJobId);

            if (serviceJob != null)
            {
                serviceJob.YesNo10 = isCopySignNeeded;
                serviceJob.WorkOrder = null;
                context.Entry(serviceJob).State = System.Data.Entity.EntityState.Modified;
            }

            this.repository.SaveChanges();
        }

        public async Task UpdateCreatedNotificationSentStatus(long serviceJobId)
        {
            try
            {
                var serviceJob = await this.repository.GetByIdWithTracking(serviceJobId);

                if (serviceJob != null)
                {
                    serviceJob.YesNo9 = true;
                }

                this.repository.SaveChanges();
            }
            catch (Exception ex)
            { }
        }

        #endregion

        #region OBSOLETE

        //OBSOLETE - Aung Ye Kaung - 20191022
        //public void AcknowledgeByDriver(long serviceJobId)
        //{
        //    this.repository.AcknowledgeByDriver(serviceJobId);
        //    this.repository.SaveChanges();
        //}

        //OBSOLETE - Aung Ye Kaung - 20191022
        //public void BeginExecution(long serviceJobId, double longitude, double latitude)
        //{
        //var serviceJob = this.repository.BeginExecution(serviceJobId);

        //var address = GoogleMapUtil.GenerateAddressFromLongAndLati(longitude, latitude);

        //serviceJob.StartExecutionTime = TimeUtil.GetLocalTime();
        //serviceJob.StartExecutionPlace = address;

        //this.repository.SaveChanges();
        //}

        //OBSOLETE - Aung Ye Kaung - 20191022
        //public void EndExecution(long serviceJobId, double longitude, double latitude,string standByDate,string standByTime, string reason, int pickupPax)
        //{
        //    var serviceJob = this.repository.EndExecution(serviceJobId, standByDate, standByTime, reason);

        //    var address = GoogleMapUtil.GenerateAddressFromLongAndLati(longitude, latitude);

        //    serviceJob.EndExecutionTime = TimeUtil.GetLocalTime();
        //    serviceJob.EndExecutionPlace = address;
        //    serviceJob.PickUpPax = pickupPax;

        //    this.repository.SaveChanges();
        //}

        //OBSOLETE - Aung Ye Kaung - 20191023
        //public void SubmitTask(long serviceJobId, string checkListIds, string additionStops, string disposals, string waitingTime, string meetingServiceIds, long customDetentionId, string remark, int pickupPax)
        //{
        //    this.repository.SubmitTask(serviceJobId, checkListIds, additionStops, disposals, waitingTime, meetingServiceIds, customDetentionId, remark, pickupPax);
        //}
        #endregion
    }
}
