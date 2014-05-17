using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Demo.Based.Globalization
{
    public class GlobalizationBaseController : Controller
    {
        protected string RedirectUrl;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            
            object cultureValue;
            //检测lang
            if (requestContext.RouteData.Values.TryGetValue("lang", out cultureValue))
            {
                //设置当前线程的culture
                try
                {
                    Thread.CurrentThread.CurrentUICulture = CultureProvider.GetCultureInfo(cultureValue.ToString());
                    Thread.CurrentThread.CurrentCulture = CultureProvider.GetCultureInfo(cultureValue.ToString());
                    var cultureHttpCookie = new HttpCookie(CultureProvider.CultureCookieKey, cultureValue.ToString());
                    Response.Cookies.Add(cultureHttpCookie);
                }
                catch (Exception)
                {
                    throw new Exception("Culture Error!");
                }
            }
            else//如果没有lang参数
            {
                //check cookie
                var langHttpCookie = requestContext.HttpContext.Request.Cookies[CultureProvider.CultureCookieKey];
                if (langHttpCookie != null)
                {
                    cultureValue = langHttpCookie.Value;
                }
                else//check brower setting
                {
                    var langs = requestContext.HttpContext.Request.UserLanguages;
                    if (langs != null && langs.Any())
                    {
                        cultureValue = langs[0].Split(',').First();
                    }
                }

                if (cultureValue == null)
                {
                    cultureValue = CultureProvider.CultureDefault;
                }
                RedirectUrl = string.Format(@"/{0}/{1}",
                    cultureValue.ToString(),
                    requestContext.HttpContext.Request.RawUrl);
            }
        }

        protected override IActionInvoker CreateActionInvoker()
        {
            return new CustomControllerActionInvoker(RedirectUrl);
        }
    }
}
