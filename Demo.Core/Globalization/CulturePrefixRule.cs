using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Demo.Core.Globalization
{
    //实现IRouteConstraint的一个类
    public class CulturePrefixRule : IRouteConstraint
    {

        private readonly IEnumerable<string> _cultureEnumerable =
            CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(p => p.Name.ToLower());
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if (values[parameterName]!=null)
                return _cultureEnumerable.Contains(values[parameterName].ToString().ToLower());
            return false;
        }
    }
}
