using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IService.Application
{
    public interface IMailingService
    {
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
        bool SendMail(string fromAddress, string toAddress, string subject, string body);

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
        bool SendHTMLMail(string fromAddress, string toAddress, string subject, string body);

        /// <summary>
        /// Send an HTML body email with attachment
        /// Created by: Kyaw Min Htut
        /// Created date: 12-11-2015
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        bool SendHTMLMailWithAttachment(string fromAddress, string toAddress, string subject, string body, string filePath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        bool SendHTMLMailWithAttachmentName(string fromAddress, string toAddress, string subject, string body, string filePath, string fileName);

        bool SendHTMLMailWithMultipleAttachments(string fromAddress, string toAddress, string subject, string body, List<string> filePath);

        bool SendHTMLMailWithMultipleAttachmentsWithName(string fromAddress, string toAddress, string subject, string body, Dictionary<string, string> filePath);
    }
}
