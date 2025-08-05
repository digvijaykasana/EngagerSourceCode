using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Common.Configs
{
    public class AppSettingKey
    {
        public const string DOMAIN = "domain";
        public const string TIME_ZONE_MINUTES = "timezoneminutes";
        public const string GOOGLE_MAP_KEY = "GoogleMapKey";
        public const string GOOGLE_MAP_URL = "GoogleMapUrl";

        public enum Key
        {
            DOMAIN,
            TIME_ZONE_MINUTES,
            GOOGLE_MAP_KEY,
            GOOGLE_MAP_URL,
            INVOICE_LENGTH,
            PRICE_CODE_NUM_COUNT,
            FCM_SERVER_KEY,
            FCM_SERVER_SENDERID,
            FCM_URL,
            WORKORDER_PUSH_NOTIFIED_DAY,
            WORKORDER_CANCELLEDJOB_REMOVAL_INTERVAL_MIN
        }
    }
}
