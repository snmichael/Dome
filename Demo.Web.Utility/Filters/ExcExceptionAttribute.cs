using System.Web.Mvc;
using System.Web.Routing;
using Demo.Framework.Core;

namespace Demo.Web.Utility.Filters
{
    public class ExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            //处理错误消息，将其跳转到一个页面
            var log = LogHelper.GetInstance("Error");
            log.Error(filterContext.Exception.ToString());
            //页面跳转到错误页面
            filterContext.Result= new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    {"action","Index"},
                    {"controller","Error"},
                    {"HttpCode","403"}
                }
                );
        }
    }
}
