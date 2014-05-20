using System.Web.Mvc;
using Demo.Based.Globalization;

namespace Demo.Core.Helper
{
    public static class ResourceExtensions
    {
        public static string Resource(this Controller controller, string expression, params object[] args)
        {
            return LanuagePack.GetGlobalResource(expression, args);
        }

        public static string Resource(this HtmlHelper htmlHelper, string expression, params object[] args)
        {
            return LanuagePack.GetGlobalResource(expression, args);
        }
    }
}
