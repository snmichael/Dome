using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Demo.Based.Globalization;
using Demo.Core.Globalization;

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
