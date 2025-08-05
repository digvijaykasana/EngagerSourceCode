using EngagerMark4.Filters;
using EngagerMark4.MvcFilters;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionHandlerAttribute());
            //filters.Add(new LoadCompanyInfoAttribute());
            filters.Add(new LoadRolePermissionAttribute());
            filters.Add(new RBACAttribute());
            filters.Add(new LoadConfigurationsAttribute());
            filters.Add(new LoadUserAttribute());
            //filters.Add(new AuditAttribute());
            
        }
    }
}
