using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using Unity.Mvc5;
using JR.Common.Unity.MVC;
using SZ.Aisino.CMS.Filters;
using API = JR.Common.Unity.WebApi;

namespace SZ.Aisino.CMS
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {

            var container = new UnityContainer();

            container.LoadConfiguration();

            //对 ActionFilter 进行注入
            container.RegisterType<IFilterProvider, UnityFilterAttributeFilterProvider>();

            //对全局异常处理程序自注入
            container.RegisterType<ExceptionLogAttribute, ExceptionLogAttribute>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            //设置WebApi 的 Resolver
            GlobalConfiguration.Configuration.DependencyResolver = new API.UnityDependencyResolver(container);
        }
    }
}