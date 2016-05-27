using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JR.Common;
 
namespace SZ.Aisino.CMS.Filters {
    public class ExceptionLogAttribute : FilterAttribute, IExceptionFilter {

        [Dependency]
        public ILog Log {
            get;
            set;
        }

        public void OnException(ExceptionContext filterContext) {
            if (!filterContext.ExceptionHandled) {

                HandleErrorInfo model = null;
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];

                if (filterContext.Exception is HttpRequestValidationException) {

                    if (filterContext.IsChildAction) {
                        ViewContext par = filterContext.ParentActionViewContext;
                        while (null != par) {
                            var wtr = (StringWriter)par.Writer;
                            wtr.GetStringBuilder().Clear();
                            par = par.ParentActionViewContext;
                        }
                    }

                    model = new HandleErrorInfo(new Exception("非法字符串"), controllerName, actionName);
                } else {
                    var ex = filterContext.Exception.GetBaseException();
                    this.Log.Log(ex);

                    model = new HandleErrorInfo(ex, controllerName, actionName);
                }


                ViewResult result = new ViewResult {
                    ViewName = "Exception",
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = filterContext.Controller.TempData
                };
                filterContext.Result = result;
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
        }
    }
}