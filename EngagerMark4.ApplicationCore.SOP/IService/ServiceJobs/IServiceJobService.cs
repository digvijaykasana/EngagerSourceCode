using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IService.Jobs
{
    public interface IServiceJobService : IBaseService<ServiceJobCri, ServiceJob>
    {
        #region Load References

        Task<ServiceJob> GetByIdWithTracking(long id);

        Task<IEnumerable<ServiceJob>> GetServiceJobsByWorkOrderId(long workOrderId);

        #endregion


        #region Save Data

        //OLD VERSION
        void UpdateSignature(Int64 serviceJobId, string signatureName, Int64 signatureId);

        //PCR2021
        void UpdateSignatureData(Int64 serviceJobId, string signatureName, Int64 signatureId);

        void SaveChanges();

        Task UpdateCreatedNotificationSentStatus(long serviceJobId);

        Task UpdateCopySign(Int64 serviceJobId, bool isCopySignNeeded);

        #endregion



        #region OBSOLETE
        //void BeginExecution(long serviceJobId, double longitude, double latitude);

        //void EndExecution(long serviceJobId, double longitude, double latitude, string standByDate, string standByTime, string reason, int pickupPax);

        //void AcknowledgeByDriver(long serviceJobId);

        //void SubmitTask(Int64 serviceJobId, string checkListIds, string additionStops, string disposals, string waitingTime, string meetingServiceIds, Int64 customDetentionId, string remark, int pickupPax);
        #endregion
    }
}
