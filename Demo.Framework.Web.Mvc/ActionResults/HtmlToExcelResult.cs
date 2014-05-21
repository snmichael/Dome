using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Demo.Framework.Web.Mvc.ActionResults {
    public class HtmlToExcelResult : FileResult {

        public HtmlToExcelResult(string fileName, string html)
            : base("application/vnd.ms-excel")
        {
            if (String.IsNullOrEmpty(fileName)) {
                throw new ArgumentException("fileName is requied");
            }

            FileName = fileName;
            Html = html;
        }

        public string FileName {
            get;
            private set;
        }
        public string Html
        {
            get;
            private set;
        }

        protected override void WriteFile(HttpResponseBase response) {
            
            //设定编码方式，若输出的excel有乱码，可优先从编码方面解决
            //response.Charset = "gb2312";
            response.ContentEncoding = Encoding.UTF8;
            //filenames是自定义的文件名
            response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, Encoding.UTF8));
            //content是步骤1的html，注意是string类型
            response.Write(Html);
            response.End();
        }

    }
}
