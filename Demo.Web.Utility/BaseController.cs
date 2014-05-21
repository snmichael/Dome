using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Demo.Framework.Core.Globalization;

namespace Demo.Web.Utility
{
    public class BaseController : GlobalizationBaseController
    {
        private List<SelectListItem> _langs;

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            InitLangs();
            base.OnResultExecuting(filterContext);
        }

        private void InitLangs()
        {
            if (_langs == null)
            {
                var lanuagePackages = LanuagePack.GetAvailableLanguages();
                _langs = lanuagePackages.Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Code,
                    Selected =
                        (String.Equals(item.Code, CultureInfo.CurrentUICulture.ToString(),
                            StringComparison.CurrentCultureIgnoreCase))
                }).ToList();
                ViewBag.LangView = null;
            }
            ViewBag.LangView = _langs;
        }
    }
}