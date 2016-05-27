using System.Web.Mvc;
using System.Web.Routing;

namespace SZ.Aisino.CMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Train",
                url: "Train/{action}/{code}",
                defaults: new
                {
                    controller = "Train",
                    action = "Index"
                },
                constraints: null,
                namespaces: new[] { "SZ.Aisino.CMS.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                constraints:null,
                namespaces: new[] { "SZ.Aisino.CMS.Controllers" }
            );
        }

        //public static void RegisterRoutes(RouteCollection routes)
        //{
        //    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        //    routes.MapRoute(
        //        name: "Default",
        //        url: "{controller}/{action}/{id}",
        //        defaults: new
        //        {
        //            controller = "Home",
        //            action = "Index",
        //            id = UrlParameter.Optional,
        //            userType = "U",
        //            isAdmin = false
        //        },
        //        constraints: new
        //        {
        //            userType = "U",
        //            controller = @"\w{2,}"
        //        }
        //    );

        //    routes.Add("Admin",
        //        new Route(
        //            "{userType}/{controller}/{action}/{id}",
        //            new RouteValueDictionary(new
        //            {
        //                isAdmin = true,
        //                controller = "Home",
        //                action = "Index",
        //                id = UrlParameter.Optional
        //            }), new RouteValueDictionary(new
        //            {
        //                userType = "A",
        //                controller = @"\w{2,}"
        //            }), new MvcRouteHandler())
        //        );
        //}
    }
}
