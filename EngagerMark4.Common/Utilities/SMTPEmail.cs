using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Common.Utilities
{
    /// <summary>
    /// FUJI XEROX INTERNAL USE ONLY<<RESTRICTED>>
    /// Disclose to : PSTG T&T Team
    /// Protected until:
    /// Author: Kyaw Min Htut
    /// Prepared on: 2-12-2015
    /// </summary>
    public class SMTPEmail
    {
        public string Domain
        {
            get;
            set;
        }

        public string Credential
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public int PortNumber
        {
            get;
            set;
        }

        public bool SSL
        {
            get;
            set;
        }

        public SMTPEmail(string domain, string credential, string password, int portNumber)
        {
            this.Domain = domain;
            this.Credential = credential;
            this.Password = password;
            this.PortNumber = portNumber;
        }

        public SMTPEmail(string domain, string credential, string password, int portNumber, bool ssl)
            : this(domain, credential, password, portNumber)
        {
            this.SSL = ssl;
        }

        /// <summary>
        /// Send a simple smtp mail
        /// Created by: Kyaw Min Htut
        /// Created date: 31-03-2015
        /// </summary>
        /// <param name="fromaddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="ssl"></param>
        public void SendMail(string fromaddress, string toAddress, string subject, string message, bool ssl)
        {
            MailAddress fromMailAddress = new MailAddress(fromaddress);
            MailAddress toMailAddress = new MailAddress(toAddress);
            MailMessage mailMessage = new MailMessage(fromaddress, toAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = this.Domain;
            smtp.Port = this.PortNumber;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential(this.Credential, this.Password);
            smtp.EnableSsl = ssl;
            smtp.Send(mailMessage);
        }

        /// <summary>
        /// Send a HTML email
        /// Created by: Kyaw Min Htut
        /// Created date: 31-03-2015
        /// </summary>
        /// <param name="fromaddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="ssl"></param>
        public void SendHTMLMail(string fromaddress, string toAddress, string subject, string message, bool ssl)
        {
            MailAddress fromMailAddress = new MailAddress(fromaddress);
            MailAddress toMailAddress = new MailAddress(toAddress);
            MailMessage mailMessage = new MailMessage(fromaddress, toAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(Domain, PortNumber);
            smtp.Host = this.Domain;
            smtp.Port = this.PortNumber;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential(this.Credential, this.Password);
            smtp.EnableSsl = ssl;
            smtp.Send(mailMessage);
        }

        public bool SendHtmlMailWithAttachment(string fromAddress, string toAddress, string subject, string body, string filePath)
        {
            MailMessage message = new MailMessage(
               fromAddress,
               toAddress,
               subject,
               body);
            message.IsBodyHtml = true;
            // Create  the file attachment for this e-mail message.
            Attachment data = new Attachment(filePath, MediaTypeNames.Application.Octet);
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(filePath);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(filePath);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(filePath);
            // Add the file attachment to this e-mail message.
            message.Attachments.Add(data);

            //Send the message.
            SmtpClient client = new SmtpClient(Domain, PortNumber);
            // Add credentials if the SMTP server requires them.
            client.Credentials = new System.Net.NetworkCredential(Credential, Cryptography.Decrypt(Password, true));
            client.EnableSsl = SSL;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool SendHtmlMailWithAttachmentName(string fromAddress, string toAddress, string subject, string body, string filePath, string fileName)
        {
            MailMessage message = new MailMessage(
               fromAddress,
               toAddress,
               subject,
               body);
            message.IsBodyHtml = true;
            // Create  the file attachment for this e-mail message.
            Attachment data = new Attachment(filePath, MediaTypeNames.Application.Octet);
            data.Name = fileName + ".pdf";
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(filePath);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(filePath);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(filePath);
            // Add the file attachment to this e-mail message.
            message.Attachments.Add(data);

            //Send the message.
            SmtpClient client = new SmtpClient(Domain, PortNumber);
            // Add credentials if the SMTP server requires them.
            client.Credentials = new System.Net.NetworkCredential(Credential, Cryptography.Decrypt(Password, true));
            client.EnableSsl = SSL;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool SendHtmlMailWithMultipleAttachment(string fromAddress, string toAddress, string subject, string body, List<string> filePaths)
        {
            MailMessage message = new MailMessage(
               fromAddress,
               toAddress,
               subject,
               body);
            message.IsBodyHtml = true;

            foreach (var attachedFilePath in filePaths)
            {
                Attachment data = new Attachment(attachedFilePath, MediaTypeNames.Application.Octet);
                // Add time stamp information for the file.
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(attachedFilePath);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(attachedFilePath);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(attachedFilePath);
                // Add the file attachment to this e-mail message.
                message.Attachments.Add(data);
            }
            // Create  the file attachment for this e-mail message.


            //Send the message.
            SmtpClient client = new SmtpClient(Domain, PortNumber);
            // Add credentials if the SMTP server requires them.
            client.Credentials = new System.Net.NetworkCredential(Credential, Cryptography.Decrypt(Password, true));
            client.EnableSsl = SSL;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool SendHtmlMailWithMultipleAttachmentWithName(string fromAddress, string toAddress, string subject, string body, Dictionary<string, string> filePaths)
        {
            MailMessage message = new MailMessage(
               fromAddress,
               toAddress,
               subject,
               body);
            message.IsBodyHtml = true;

            foreach (string attachedFilePath in filePaths.Keys)
            {
                Attachment data = new Attachment(attachedFilePath, MediaTypeNames.Application.Octet);
                data.Name = filePaths[attachedFilePath];
                // Add time stamp information for the file.
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(attachedFilePath);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(attachedFilePath);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(attachedFilePath);
                // Add the file attachment to this e-mail message.
                message.Attachments.Add(data);
            }
            // Create  the file attachment for this e-mail message.


            //Send the message.
            SmtpClient client = new SmtpClient(Domain, PortNumber);
            // Add credentials if the SMTP server requires them.
            client.Credentials = new System.Net.NetworkCredential(Credential, Cryptography.Decrypt(Password, true));
            client.EnableSsl = SSL;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}
