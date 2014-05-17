using System;
using System.Collections.Generic;
using System.Globalization;
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
            InitLangs();
            base.OnResultExecuting(filterContext);
            
        }

        private void InitLangs()
        {
            var lanuagePackages = LanuagePack.GetAvailableLanguages();
            var langs = lanuagePackages.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Code,
                Selected = (String.Equals(item.Code, CultureInfo.CurrentUICulture.ToString(), StringComparison.CurrentCultureIgnoreCase))
            }).ToList();

            ViewBag.LangView = langs;
        }
    }
}
