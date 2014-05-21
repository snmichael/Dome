using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Demo.Framework.Core
{
    using System.Web.Hosting;

    public static class SiteUtility
    {
        public static string GetRootURI()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            HttpRequest Req;
            if (HttpCurrent != null)
            {
                Req = HttpCurrent.Request;

                string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
                if (Req.ApplicationPath == null || Req.ApplicationPath == "/")
                    //直接安装在   Web   站点   
                    AppPath = UrlAuthority;
                else
                    //安装在虚拟子目录下   
                    AppPath = UrlAuthority + Req.ApplicationPath;
            }
            return AppPath;
        }



        ////获取客户端IP地址   
        //public string getIP()
        //{
        //    string result = String.Empty;
        //    result = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //    if (null == result || result == String.Empty)
        //    { result = HttpContext.Request.ServerVariables["REMOTE_ADDR"]; }
        //    if (null == result || result == String.Empty)
        //    { result = HttpContext.Request.UserHostAddress; }
        //    if (null == result || result == String.Empty) { return "0.0.0.0"; } return result;
        //}

        public static string GetLocalIp()
        {
            return
                Dns.GetHostAddresses("")
                   .First(a => !a.IsIPv6LinkLocal && a.AddressFamily == AddressFamily.InterNetwork)
                   .ToString();
        }

        /// <summary>
        /// Gets a physical disk path of \Bin directory
        /// </summary>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public static string GetBinDirectory()
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HttpRuntime.BinDirectory;
            }
            else
            {
                //not hosted. For example, run either in unit tests
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}
