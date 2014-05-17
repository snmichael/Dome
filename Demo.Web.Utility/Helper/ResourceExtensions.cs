using System.Globalization;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;

namespace Demo.Web.Utility.Helper
{
    public static class ResourceExtensions
    {
        public static string Resource(this Controller controller, string expression, params object[] args)
        {
            ResourceExpressionFields fields = GetResourceFields(expression, "~/");
            return GetGlobalResource(fields, args);
        }

        public static string Resource(this HtmlHelper htmlHelper, string expression, params object[] args)
        {
            string path = "~/";
            ResourceExpressionFields fields = GetResourceFields(string.Format("Resource,{0}", expression), path);
            return GetGlobalResource(fields, args);
        }

        public static string GetGlobalResource(ResourceExpressionFields fields, object[] args)
        {
            return string.Format((string)HttpContext.GetGlobalResourceObject(fields.ClassKey, fields.ResourceKey, CultureInfo.CurrentUICulture), args);
        }

        static ResourceExpressionFields GetResourceFields(string expression, string virtualPath)
        {
            var context = new ExpressionBuilderContext(virtualPath);
            var builder = new ResourceExpressionBuilder();
            return (ResourceExpressionFields)builder.ParseExpression(expression, typeof(string), context);
        }
    }
}
