using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.IService.Application;
using EngagerMark4.Common.Configs;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using EngagerMark4.Infrasturcture.Repository.Application;
using EngagerMark4.Service.ApplicationCore;
using EngagerMark4.Service.ApplicationCore.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EngagerMark4.MvcFilters
{
    public class ExceptionHandlerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            try
            {
                //using (ApplicationDbContext dbContext = new ApplicationDbContext())
                //{
                //    ISMTPService smtpService = new SMTPService(new SMTPRepository(dbContext));
                //    IMailingService mailingService = new MailingService(smtpService);
                //    Company company = new Company();


                //    SMTP smtp = smtpService.GetByCri(null).Result.FirstOrDefault();
                //    string imageUrl = "\\Image\\Company?img=" + company.Logo;
                //    string message = MailConfig.GetErrorDetailsTemplate(System.Web.Hosting.HostingEnvironment.MapPath(MailConfig.ERROR_DETAILS_TEMPLATE), imageUrl, Util.ConvertDateToString(TimeUtil.GetLocalTime(), DateConfig.CULTURE), filterContext.Exception.Message, filterContext.Exception.Source, filterContext.Exception.StackTrace);
                //    try
                //    {
                       
                //        Audit audit = new Audit();
                //        audit.StartProcessingTime = TimeUtil.GetLocalTime();
                //        audit.Description = message;
                //        audit.Type = Audit.AuditType.Error;
                //        audit.EndProcessingTime = TimeUtil.GetLocalTime();
                //        dbContext.Audits.Add(audit);
                //        dbContext.SaveChanges();
                //    }
                //    catch
                //    {
                //    }
                //    if (smtp != null)
                //        mailingService.SendHTMLMail(null, smtp.SupportMail, MailConfig.ENGAGER_ERROR_SUBJECT, message);
                //}
            }
            catch (Exception ex)
            {
            }

            filterContext.Result = new RedirectToRouteResult(new
                                    RouteValueDictionary
                                {
                                    { "action", "Index"},
                                    { "controller", "Error"}
                                });
        }
    }
}