using System.Collections.Generic;
using System.Web.Mvc;

namespace Demo.Core.Globalization
{
    internal class CustomControllerActionInvoker : ControllerActionInvoker
    {
        readonly string _redirectUrl;
        public CustomControllerActionInvoker(string url)
            : base()
        {
            _redirectUrl = url;
        }
        protected override ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
        {
            object returnValue;
            //ChildAction内部不能重定向
            if (!string.IsNullOrEmpty(_redirectUrl) && !controllerContext.IsChildAction)
                returnValue = new RedirectResult(_redirectUrl);
            else
                returnValue = actionDescriptor.Execute(controllerContext, parameters);
            ActionResult result = CreateActionResult(controllerContext, actionDescriptor, returnValue);
            return result;
        }
    }
}