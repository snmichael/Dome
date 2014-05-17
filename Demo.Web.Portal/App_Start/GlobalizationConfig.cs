using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Demo.Based.Globalization;

namespace Demo.Web.Portal
{
    public class GlobalizationConfig
    {
        public static void RegisterGlobalizationRoutes()
        {
            //判断Routes 是否为空
            if (RouteTable.Routes == null)
                return;
            //路由集合
            var routeCollection = new RouteCollection();

            //这里需要跳过routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //由于IgnoreRouteInternal是个私有类，所以这里只能反射
            //skip IgnoreRouteInternal
            var routes = RouteTable.Routes.SkipWhile(p => (p.GetType().Name == "IgnoreRouteInternal"));

            int insertpoint = RouteTable.Routes.Count() - routes.Count();
            //遍历所有需要处理的路由
            foreach (var route in routes)
            {
                var item = (route as Route);
                //下面的代码创建一个新的路由对象，在url规则前面加上lang参数，并拷贝其他设置
                var newRoute = new Route(
                    //string.Format(@"{lang}/{0}",item.Url),
                    @"{lang}/" + item.Url,
                    new MvcRouteHandler()
                    );
                newRoute.Defaults = new RouteValueDictionary(item.Defaults);
                newRoute.Constraints = new RouteValueDictionary(item.Constraints);
                //lang参数需要验证，因为只有合法的culture才能被接受
                newRoute.Constraints.Add("lang", new CulturePrefixRule());
                newRoute.DataTokens = new RouteValueDictionary();
                newRoute.DataTokens["Namespaces"] = item.DataTokens["Namespaces"];
                routeCollection.Add(newRoute);
            }
            foreach (var routeBase in routeCollection)
            {
                RouteTable.Routes.Insert(insertpoint++, routeBase);
            }
        }
    }
}