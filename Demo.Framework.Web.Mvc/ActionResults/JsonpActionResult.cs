using System.Web.Mvc;

namespace Demo.Framework.Web.Mvc.ActionResults
{
    public class JsonpResult : JsonResult
    {
        public string Callback { get; set; }

        public JsonpResult()
        {
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = context.HttpContext;
            var callback = Callback;

            if (string.IsNullOrWhiteSpace(callback))
                callback = httpContext.Request["callback"];

            httpContext.Response.Write(callback + "(");
            base.ExecuteResult(context);
            httpContext.Response.Write(");");
        }
    }
}