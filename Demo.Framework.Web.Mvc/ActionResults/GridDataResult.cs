using System;
using System.Web;
using System.Web.Mvc;
using Demo.Framework.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Demo.Framework.Web.Mvc.ActionResults
{
    public class GridDataResult : ActionResult
    {
        public JsonSerializerSettings SerializerSettings { get; set; }

        private  object data;
        private  object userdata;

        public GridDataResult(object data)
        {
            this.data = data;
        }

        public GridDataResult(object data,object userData):this(data)
        {
            this.userdata = userData;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");


            if (context.RequestContext.HttpContext.Request.IsFileDownLoad())
            {
                var fileName = context.RequestContext.HttpContext.Request["filename"];
                (new ObjectToExcelResult(fileName, data)).ExecuteResult(context);    
            }
            else
            {
                var pList = data as IPagedList;
                if (pList != null)
                {
                    data = new JqGridPagedList()
                               {
                                    page =  pList.PageIndex,
                                    total = pList.TotalPages,
                                    records = pList.TotalCount,
                                    rows = data,
                                    userdata = userdata
                               };
                }

                (new JsonNetResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = data }).ExecuteResult(context);    
            }
        }

       
    }

    public class JqGridPagedList
    {
        public int page;
        public int total;
        public int records;
        public Object userdata;
        public Object rows;
        
    }

    public static class HttpContextExtensions
    {
        /// <summary>
        /// ≤ø√≈
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsFileDownLoad(this HttpRequestBase request)
        {
            var fileName = request["filename"];
            return (!string.IsNullOrEmpty(fileName));
        }


    }
}