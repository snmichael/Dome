using System.Web;
using System.Web.Mvc;
using Demo.Web.Utility.Filters;

namespace Demo.Web.Portal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionAttribute());
        }
    }
}