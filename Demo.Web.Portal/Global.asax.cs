

using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Demo.Framework.Core;
using Demo.Framework.Core.Infrastructure;
using Demo.Framework.Web;
using Demo.Web.Portal.Controllers;
using Elmah;
using WebGrease.Css.Extensions;

namespace Demo.Web.Portal
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //初始化依赖注入容器
            BaseEngine.Initialize();
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalizationConfig.RegisterGlobalizationRoutes();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();

            //读取日志  如果使用log4net,应用程序一开始的时候，都要进行初始化配置
            log4net.Config.XmlConfigurator.Configure();
        }


        public void Application_Error()
        {
            HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var routeData = urlHelper.RouteCollection.GetRouteData(currentContext);
            string action = routeData.Values["action"] == null ? "" : routeData.Values["action"] as string;
            string controller = routeData.Values["controller"] == null ? "" : routeData.Values["controller"] as string;

            var exception = Server.GetLastError();

            var httpException = new HttpException(null, exception);
            if (httpException.GetHttpCode() == 404 && WebHelper.IsStaticResource(this.Request))
            {
                return;
            }

            var errorId = "";
            if (httpException.GetHttpCode() == 500)
            {
                errorId = DateTime.Now.ToString("yyyyMMddhhmmss");
                var log = LogHelper.GetInstance("Error");
                var addition = "";
                if (exception is System.Data.Entity.Validation.DbEntityValidationException)
                {
                    foreach (var errorItem in (exception as System.Data.Entity.Validation.DbEntityValidationException).EntityValidationErrors)
                    {
                        errorItem.ValidationErrors.ForEach(_ => addition += "PropertyName:" + _.PropertyName + _.ErrorMessage);
                    }
                    ErrorSignal.FromCurrentContext().Raise(new Exception(addition, exception));
                }
                log.Error("Id:" + errorId + "||DateTime:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "||Action:" + action + "||Controller:" + controller + "|| " + httpException.Message + "||" + exception.Message + "||" + exception.StackTrace + "||Addition:" + addition);
            }

            //TODO: 记录Log（忽略404，403） 
            var errorrouteData = new RouteData();
            errorrouteData.Values.Add("controller", "Error");
            errorrouteData.Values.Add("action", "Index");
            errorrouteData.Values.Add("errorId", errorId);
            errorrouteData.Values.Add("httpException", httpException);
            Server.ClearError();

            //TODO: 跳转到错误页面
            IController errorController = DependencyResolver.Current.GetService<ErrorController>();
            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), errorrouteData));
        }

    }
}