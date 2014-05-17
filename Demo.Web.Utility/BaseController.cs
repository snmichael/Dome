using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Demo.Web.Utility.Globalization;

namespace Demo.Web.Utility
{
    public class BaseController : Controller
    {
        protected string RedirectUrl;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            object cultureValue;
            //检测ci
            if (requestContext.RouteData.Values.TryGetValue("ci", out cultureValue))
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
            else//如果没有ci参数
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
