using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SZ.Aisino.CMS.Filters
{
    public class ActionParameterCheckAttribute : ActionFilterAttribute
    {

        private bool IsNullableType(Type t)
        {
            return (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var inputs = filterContext.ActionParameters;
            filterContext.ActionDescriptor.GetParameters().ToList().ForEach(p =>
            {
                var input = inputs.FirstOrDefault(i => i.Key.Equals(p.ParameterName, StringComparison.OrdinalIgnoreCase));
                if (p.DefaultValue == null && input.Value == null && p.ParameterType.IsValueType && !IsNullableType(p.ParameterType))
                {
                    //throw new InvalidUrlException();

                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "InvalidUrl"
                    };
                    return;
                }
            });
            base.OnActionExecuting(filterContext);
        }

    }
}