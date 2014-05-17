using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Demo.Web.Utility.Globalization;
using Demo.Web.Utility.Helper;

namespace Demo.Web.Utility
{
    public class BaseController:GlobalizationBaseController
    {
        private const string Title = "CRM系统";

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ViewBag.Title = Title + "-" + ViewBag.Title;
            base.OnResultExecuting(filterContext);
            
        }
    }
}
