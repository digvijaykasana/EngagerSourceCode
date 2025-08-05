using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Common.Configs
{
    public class MailConfig
    {
        public static string SMTPADDRESS = "sgpaphq-exccr01.dc01.fujixerox.net";
        public static int PORT = 25;
        public static string CREDENTIAL = "dc01\\sgkyawmh";
        public static string PASSWORD = "sensekmhmay@123";
        public static string ADMIN_MAIL = "admin@ciecmx.no-ip.org";

        public static string ENGAGER_ERROR_SUBJECT = "Engager Error";
        public static string ERROR_DETAILS_TEMPLATE = "\\App_Data\\Templates\\ErrorDetails.txt";

       public static string GetErrorDetailsTemplate(string templateLocation, string imageUrl, string occurTime, string message, string source, string stackTrace)
        {

            string body;
            using (var sr = new StreamReader(templateLocation))
            {
                body = sr.ReadToEnd();

                string messageBody = string.Format(body, occurTime, message, source, stackTrace, imageUrl);
                return messageBody;
            }

        }
    }
}
