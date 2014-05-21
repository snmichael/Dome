using System;
using System.Web.Mvc;
using Demo.Framework.Core;

namespace Demo.Framework.Web.Mvc.ModelBinders
{

    public class PageConditionModelBinder :IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            var rtn = new PageCondition();
            var request = controllerContext.HttpContext.Request;
            rtn.PageIndex = Convert.ToInt32(request["page"]) - 1;
            rtn.PageSize = Convert.ToInt32(request["rows"]);
            rtn.SortFiled = request["sidx"];
            rtn.OrderAsc = request["sord"] == "asc" || request["sord"] == string.Empty;
            return rtn;
        }


    }

}
