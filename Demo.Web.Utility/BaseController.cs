using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Demo.Based.Globalization;

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
            //var langs = new List<SelectListItem>() { new SelectListItem() { Text = "English", Value = "en-us" }, new SelectListItem() { Text = "中文简体", Value = "zh-cn" } };
            //foreach (SelectListItem t in langs)
            //{
            //    if (t.Value == CultureInfo.CurrentUICulture.ToString().ToLower())
            //    {
            //        t.Selected = true;
            //    }
            //}
            ViewBag.LangView = langs;
        }
    }
}
