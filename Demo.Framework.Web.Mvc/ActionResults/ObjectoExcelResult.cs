using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Demo.Framework.Web.Mvc.ActionResults {
    public class ObjectToExcelResult : FileResult {

        public ObjectToExcelResult(string fileName, object obj)
            : base("application/excel")
        {
            if (String.IsNullOrEmpty(fileName)) {
                throw new ArgumentException("fileName is requied");
            }

            FileName = fileName;
            Object = obj;
        }

        public string FileName {
            get;
            private set;
        }
        public object Object
        {
            get;
            private set;
        }

        protected override void WriteFile(HttpResponseBase response) {

            var grid = new GridView();
            grid.DataSource = Object;
            grid.DataBind();
            response.ClearContent();
            response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            response.Charset = "gb2312";
            response.ContentEncoding = Encoding.GetEncoding("gb2312");
            response.ContentType = "application/excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            
            grid.RenderControl(htw);
            response.Write( sw.ToString());
            response.End();
        }

    }
}
