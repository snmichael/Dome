using System;
using System.Globalization;
using System.IO;
using System.Security.Policy;
using System.Web.Mvc;
using System.Web.Routing;
using Demo.Core;

namespace Demo.Web.Utility.Filters
{
    public class ExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            //处理错误消息，将其跳转到一个页面
            LogHelper.WriteLog(filterContext.Exception.ToString());
            //页面跳转到错误页面
            string errorUrl = "/{0}/Error/";
            errorUrl = string.Format(errorUrl, CultureInfo.CurrentUICulture.ToString().ToLower());
            filterContext.HttpContext.Response.Redirect(errorUrl);
        }
    }
}
