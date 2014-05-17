using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using Demo.Web.Utility.Globalization;

namespace Demo.Web.Utility.Helper
{
    public static class ResourceExtensions
    {
        public static string Resource(this Controller controller, string expression, params object[] args)
        {
            //ResourceExpressionFields fields = GetResourceFields(expression, "~/");
            //return GetGlobalResource(fields, args);
            return GetGlobalResource(expression, args);
        }

        public static string Resource(this HtmlHelper htmlHelper, string expression, params object[] args)
        {
            //string path = "~/";
            //ResourceExpressionFields fields = GetResourceFields(string.Format("Resource,{0}", expression), path);
            //return GetGlobalResource(fields, args);
            return GetGlobalResource(expression, args);
        }

        //static string GetGlobalResource(ResourceExpressionFields fields, object[] args)
        //{
        //    return string.Format((string)HttpContext.GetGlobalResourceObject(fields.ClassKey, fields.ResourceKey, CultureInfo.CurrentUICulture), args);
        //}

        static string GetGlobalResource(string resourceKey, object[] args)
        {
            var value = resourceKey;
            var lanuage = LanuagePack.GetAvailableLanguages().FirstOrDefault(lang => String.Equals(lang.Code, CultureInfo.CurrentUICulture.ToString(), StringComparison.CurrentCultureIgnoreCase));
            if (lanuage != null)
            {
                var resource =
                    lanuage.Resources.FirstOrDefault(r => String.Equals(r.Name, resourceKey, StringComparison.CurrentCultureIgnoreCase));
                if (resource != null)
                {
                    value = resource.Value;
                }
            }

            return string.Format(value, args);
        }

        //static ResourceExpressionFields GetResourceFields(string expression, string virtualPath)
        //{
        //    var context = new ExpressionBuilderContext(virtualPath);
        //    var builder = new ResourceExpressionBuilder();
        //    return (ResourceExpressionFields)builder.ParseExpression(expression, typeof(string), context);
        //}
    }
}
