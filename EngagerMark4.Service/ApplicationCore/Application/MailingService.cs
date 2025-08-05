using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.IService.Application;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.ApplicationCore.Application
{
    /// <summary>
    /// FUJI XEROX INTERNAL USE ONLY<<RESTRICTED>>
    /// Disclose to : PSTG T&T Team
    /// Protected until:
    /// Author: Kyaw Min Htut
    /// Prepared on: 2-12-2015
    /// </summary>
    public class MailingService : IMailingService
    {

        ISMTPService smtpService;

        public MailingService(ISMTPService smtpService)
        {
            this.smtpService = smtpService;
        }

        /// <summary>
        /// Send a simple text smtp email 
        /// Created by: Kyaw Min Htut
        /// Created date: 31-03-2015
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool SendMail(string fromAddress, string toAddress, string subject, string body)
        {
            SMTP smtp = new SMTP();
            List<SMTP> smpts = smtpService.GetByCri(null).Result.ToList();
            if (smpts.Count > 0)
            {
                smtp = smpts[0];
            }
            else
            {
                smtp.SMTPServer = MailConfig.SMTPADDRESS;
                smtp.Port = MailConfig.PORT;
                smtp.Credential = MailConfig.CREDENTIAL;
                smtp.Password = MailConfig.PASSWORD;
            }
            SMTPEmail mail = new SMTPEmail(smtp.SMTPServer, smtp.Credential, Cryptography.Decrypt(smtp.Password, true), smtp.Port);
            try
            {
                if (fromAddress == null)
                    fromAddress = smtp.AdminMail;
                mail.SendMail(fromAddress, toAddress, subject, body, smtp.SSL);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Send an HTML body email
        /// Created by: Kyaw Min Htut
        /// Created date: 31-03-2015
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool SendHTMLMail(string fromAddress, string toAddress, string subject, string body)
        {
            SMTP smtp = new SMTP();
            List<SMTP> smtps = smtpService.GetByCri(null).Result.ToList();
            if (smtps.Count > 0)
            {
                smtp = smtps[0];
            }
            else
            {
                smtp.SMTPServer = MailConfig.SMTPADDRESS;
                smtp.Port = MailConfig.PORT;
                smtp.Credential = MailConfig.CREDENTIAL;
                smtp.Password = MailConfig.PASSWORD;
            }
            try
            {
                if (fromAddress == null)
                    fromAddress = smtp.AdminMail;
                SMTPEmail mail = new SMTPEmail(smtp.SMTPServer, smtp.Credential, Cryptography.Decrypt(smtp.Password, true), smtp.Port);
                mail.SendHTMLMail(fromAddress, toAddress, subject, body, smtp.SSL);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool SendHTMLMailWithAttachment(string fromAddress, string toAddress, string subject, string body, string filePath)
        {
            SMTP smtp = new SMTP();
            List<SMTP> smtps = smtpService.GetByCri(null).Result.ToList();
            if (smtps.Count > 0)
            {
                smtp = smtps[0];
            }
            else
            {
                smtp.SMTPServer = MailConfig.SMTPADDRESS;
                smtp.Port = MailConfig.PORT;
                smtp.Credential = MailConfig.CREDENTIAL;
                smtp.Password = MailConfig.PASSWORD;
            }
            try
            {
                if (fromAddress == null)
                    fromAddress = smtp.AdminMail;
                SMTPEmail smtpemail = new SMTPEmail(smtp.SMTPServer, smtp.Credential, smtp.Password, smtp.Port, smtp.SSL);
                return smtpemail.SendHtmlMailWithAttachment(fromAddress, toAddress, subject, body, filePath);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendHTMLMailWithAttachmentName(string fromAddress, string toAddress, string subject, string body, string filePath, string fileName)
        {
            SMTP smtp = new SMTP();
            List<SMTP> smtps = smtpService.GetByCri(null).Result.ToList();
            if (smtps.Count > 0)
            {
                smtp = smtps[0];
            }
            else
            {
                smtp.SMTPServer = MailConfig.SMTPADDRESS;
                smtp.Port = MailConfig.PORT;
                smtp.Credential = MailConfig.CREDENTIAL;
                smtp.Password = MailConfig.PASSWORD;
            }
            try
            {
                if (fromAddress == null)
                    fromAddress = smtp.AdminMail;
                SMTPEmail smtpemail = new SMTPEmail(smtp.SMTPServer, smtp.Credential, smtp.Password, smtp.Port, smtp.SSL);
                return smtpemail.SendHtmlMailWithAttachmentName(fromAddress, toAddress, subject, body, filePath, fileName);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendHTMLMailWithMultipleAttachments(string fromAddress, string toAddress, string subject, string body, List<string> filePath)
        {
            SMTP smtp = new SMTP();
            List<SMTP> smtps = smtpService.GetByCri(null).Result.ToList();
            if (smtps.Count > 0)
            {
                smtp = smtps[0];
            }
            else
            {
                smtp.SMTPServer = MailConfig.SMTPADDRESS;
                smtp.Port = MailConfig.PORT;
                smtp.Credential = MailConfig.CREDENTIAL;
                smtp.Password = MailConfig.PASSWORD;
            }
            try
            {
                if (fromAddress == null)
                    fromAddress = smtp.AdminMail;
                SMTPEmail smtpemail = new SMTPEmail(smtp.SMTPServer, smtp.Credential, smtp.Password, smtp.Port, smtp.SSL);
                return smtpemail.SendHtmlMailWithMultipleAttachment(fromAddress, toAddress, subject, body, filePath);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendHTMLMailWithMultipleAttachmentsWithName(string fromAddress, string toAddress, string subject, string body, Dictionary<string, string> filePath)
        {
            SMTP smtp = new SMTP();
            List<SMTP> smtps = smtpService.GetByCri(null).Result.ToList();
            if (smtps.Count > 0)
            {
                smtp = smtps[0];
            }
            else
            {
                smtp.SMTPServer = MailConfig.SMTPADDRESS;
                smtp.Port = MailConfig.PORT;
                smtp.Credential = MailConfig.CREDENTIAL;
                smtp.Password = MailConfig.PASSWORD;
            }
            try
            {
                if (fromAddress == null)
                    fromAddress = smtp.AdminMail;
                SMTPEmail smtpemail = new SMTPEmail(smtp.SMTPServer, smtp.Credential, smtp.Password, smtp.Port, smtp.SSL);
                return smtpemail.SendHtmlMailWithMultipleAttachmentWithName(fromAddress, toAddress, subject, body, filePath);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
