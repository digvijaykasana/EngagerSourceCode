using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Common
{
    public class GeneralConfig
    {
        #region 

        public ConfigurationGrpCodes configGroupCode
        {
            get;
            set;
        }

        public enum ConfigurationGrpCodes
        {
            UserStatus = 1,
            RoleStatus,
            DiscountType,
            OrderLocationType,
            OrderStatus,
            Rank,
            BoardType,
            CustomDetention,
            DutyType,
            Task,
            PaymentSchedule,
            WorkType,
            TimeSlot,
            VesselId,
            WaitingTime,
            ConveyorType,
            SalaryPayItem,
            DriverMeetingServiceFee
        };

        public enum UserStatusCodes
        {
            Active,
            InActive
        }

        #endregion
    }
}
