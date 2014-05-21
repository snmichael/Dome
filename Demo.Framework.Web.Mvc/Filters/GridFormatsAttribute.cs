using System.Web.Mvc;

namespace Demo.Framework.Web.Mvc.Filters
{
    public class GridFormatsAttribute : ActionFilterAttribute
    {
        
        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    var request = filterContext.HttpContext.Request;
        //    var viewResult = filterContext.Result as IPagedList<object>;

        //    var grid = request["grid"];
        //    object data = null;
        //    if (string.IsNullOrEmpty(grid)
        //        || string.Compare(grid, GridType.Default.ToString(),true) == 0)
        //    {
        //        data = viewResult;
        //    }
        //    else if (string.Compare(grid, GridType.JqGrid.ToString(), true) == 0)
        //    {
        //        var model = new JqGridModel();
        //        model.PageIndex = viewResult.PageIndex;
        //        model.TotalPages = viewResult.TotalPages;
        //        model.TotalCount = viewResult.TotalCount;
        //        model.List = viewResult;
        //        data = model;
        //    }

        //    filterContext.Result = new JsonNetResult
        //    {
        //        Data = data,
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };
        //}

        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    var request = filterContext.HttpContext.Request;
        //    var jsonNetResult = filterContext.Result as JsonNetResult;

            
        //    if (jsonNetResult != null)
        //    {
        //        var grid = request["grid"];
        //        var data = jsonNetResult.Data as PagedList<object>;
        //        object rtn = null;
        //        if (string.IsNullOrEmpty(grid)
        //            || string.Compare(grid, GridType.Default.ToString(), true) == 0)
        //        {
        //            rtn = data;
        //        }
        //        else if (string.Compare(grid, GridType.JqGrid.ToString(), true) == 0)
        //        {
                    
        //            var model = new JqGridModel();
        //            model.PageIndex = data.PageIndex;
        //            model.TotalPages = data.TotalPages;
        //            model.TotalCount = data.TotalCount;
        //            model.List = data.Select();
        //            rtn = model;
        //        }

        //        jsonNetResult.Data = rtn;
        //    }
        //}

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            
      
        }
    }

    public  enum  GridType
    {
        Default = 0,
        JqGrid  = 1,
        FlexiGrid = 2
    }
}