using Microsoft.ApplicationServer.Caching;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using JR.Common;
using JR.Common.MVC;
using JR.Common.MVC.Validations;
using SZ.Aisino.CMS.DbEntity.Annotation;

namespace SZ.Aisino.CMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            HttpRuntimeSetting.SameAppDomainAppId();

            UnityConfig.RegisterComponents();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngingSetting.Config();
            DisplayModelSetting.Config();
 
            DependencyResolver.Current.GetService<ILog>().Config();

            AnnotationHelper.AutoMap();
            ModelBinders.Binders.DefaultBinder = new SmartModelBinder();
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(DataTypeAttribute), typeof(DataTypeValidator));
            //AppFabric Caching
            //DataCache m_cache = CacheUtil.GetCache();


        }
    }
}
