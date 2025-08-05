using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.IRepository.Jobs
{
    public interface IServiceJobRepository : IBaseRepository<ServiceJobCri, ServiceJob>
    {
        #region Load References

        #endregion

        #region Save Data

        #endregion

        #region OBSOLETE

        //Aung Ye Kaung - 20191022
        //void AcknowledgeByDriver(Int64 serviceJobId);
        //ServiceJob BeginExecution(Int64 serviceJobId);
        //ServiceJob EndExecution(Int64 serviceJobId, string standByDate, string standByTime, string reason);
        //void SubmitTask(Int64 serviceJobId, string checkListIds, string additionStops, string disposals, string waitingTime, string meetingServiceIds, Int64 customDetentionId, string remark, int pickupPax);

        #endregion

        //OLD VERSION
        void UpdateSignature(Int64 serviceJobId, string signatureName, Int64 signatureId);

        //PCR2021
        void UpdateSignatureData(Int64 serviceJobId, string signatureName, Int64 signatureId);

        Task<ServiceJob> GetByIdWithTracking(object Id);

        Task<IEnumerable<ServiceJob>> GetServiceJobsByWorkOrderId(long workOrderId);
    }
}
