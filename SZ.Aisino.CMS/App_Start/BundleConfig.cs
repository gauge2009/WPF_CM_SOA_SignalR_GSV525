using System.Web.Optimization;

namespace SZ.Aisino.CMS {
    public class BundleConfig {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));


            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/jquery.colorbox.js",
                "~/Scripts/Common.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
                "~/Scripts/Chart.js",
                "~/Scripts/ichart.1.2.1.min.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/knockout_debug").Include(
                   "~/Scripts/knockout-3.1.0.debug.js"
                   ));
            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-3.1.0.js"
                 ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/themes/base/jquery-ui.css",
                "~/Content/colorbox.css",
                 "~/Content/site.css"
                ));
            bundles.Add(new StyleBundle("~/Content/bootcss").Include(
                "~/Content/bootstrap.min.css"
                 ));
            
 

            bundles.Add(new StyleBundle("~/Content/maintain").Include(
                "~/Content/maintain/index.css",
                "~/Content/maintain/main.css"
                ));
            //bundles.Add(new StyleBundle("~/Content/gtm").Include(
            //    "~/Content/gtm/index.css",
            //    "~/Content/gtm/main.css"));

            bundles.Add(new StyleBundle("~/Content/cms").Include(
                "~/Content/cms/index.css",
                "~/Content/cms/news.css",
                "~/Content/cms/support.css"
                ));
          





            // 将 EnableOptimizations 设为 false 以进行调试。有关详细信息，
            // 请访问 http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
