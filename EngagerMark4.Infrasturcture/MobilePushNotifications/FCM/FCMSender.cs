using EngagerMark4.Common.Configs;
using EngagerMark4.Infrasturcture.EFContext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.MobilePushNotifications.FCM
{
    public class FCMSender
    {
        ApplicationDbContext _context;

        public FCMSender(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task Send(string to, string title, string body,string sound="default")
        {
            string key = AppSettingKey.Key.FCM_SERVER_KEY.ToString();
            string senderkey = AppSettingKey.Key.FCM_SERVER_SENDERID.ToString();
            string fcmSenderUrlKey = AppSettingKey.Key.FCM_URL.ToString();
            var serverKeyValue = this._context.SystemSettings.AsNoTracking().FirstOrDefault(x => x.Code.Equals(key));
            var senderIdValue = this._context.SystemSettings.AsNoTracking().FirstOrDefault(x => x.Code.Equals(senderkey));
            var fcmSenderUrlValue = this._context.SystemSettings.AsNoTracking().FirstOrDefault(x => x.Code.Equals(fcmSenderUrlKey));

            if (serverKeyValue == null) serverKeyValue = new ApplicationCore.Entities.Application.SystemSetting();
            if (senderIdValue == null) senderIdValue = new ApplicationCore.Entities.Application.SystemSetting();
            if (fcmSenderUrlValue == null) fcmSenderUrlValue = new ApplicationCore.Entities.Application.SystemSetting();

            // Get the server key from FCM console
            // server key
            var serverKey = string.Format("key={0}", serverKeyValue.Value);

            // Get the sender id from FCM console
            var senderId = string.Format("id={0}", senderIdValue.Value);

            var data = new
            {
                to, // Recipient device token
                notification = new { title, body, sound }
            };

            // Using Newtonsoft.Json
            var jsonBody = JsonConvert.SerializeObject(data);

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, fcmSenderUrlValue.Value))
            {
                httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);
                httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    var result = await httpClient.SendAsync(httpRequest);

                    if (result.IsSuccessStatusCode)
                    {
                    }
                    else
                    {
                        // Use result.StatusCode to handle failure
                        // Your custom error handler here
                    }
                }
            }
        }
    }
}
