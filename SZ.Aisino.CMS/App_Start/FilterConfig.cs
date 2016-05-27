using System.Web.Mvc;
using SZ.Aisino.CMS.Filters;

namespace SZ.Aisino.CMS {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            //filters.Add(new HandleErrorAttribute(), 0);
            //filters.Add(new ExceptionLogAttribute(), 1);
            //filters.Add(DependencyResolver.Current.GetService<ExceptionLogAttribute>(), 1);
            //filters.Add(DependencyResolver.Current.GetService<NoPermissionExceptionHandlerAttribute>(), 2);
        }
    }
}
