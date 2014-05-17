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
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            
        }
    }
}
