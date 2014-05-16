using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Demo.Based
{
    /// <summary>
    /// 提供 自定义操作的 HttpContext.Current
    /// </summary>
    public class Current
    {
        /// <summary>
        /// HttpResponse 对象
        /// </summary>
        public static HttpResponse Response
        {
            get
            {
                return HttpContext.Current.Response;
            }
        }
        /// <summary>
        /// HttpRequest 对象
        /// </summary>
        public static HttpRequest Request
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }
        /// <summary>
        /// HttpServerUtility 对象
        /// </summary>
        public static HttpServerUtility Server
        {
            get
            {
                return HttpContext.Current.Server;
            }
        }
        /// <summary>
        /// 获取来自Url
        /// </summary>
        public static string ReFerer
        {
            get
            {
                return Current.Request.ServerVariables["HTTP_REFERER"];
            }
        }
        /// <summary>
        /// 当前服务器IP
        /// </summary>
        public static string ServerIP
        {
            get
            {
                return Current.Request.ServerVariables.Get("Local_Addr").ToString();
            }
        }
        /// <summary>
        /// 当前服务器URL 主地址 无 / 结尾
        /// </summary>
        public static string ThisServerUrl
        {
            get
            {
                string text = Current.Request.ServerVariables["Server_Port"];
                if (Base.IsNull(text))
                {
                    text = "";
                }
                else
                {
                    text = ((text == "80") ? "" : (":" + text));
                }
                return "http://" + Current.Request.ServerVariables["SERVER_NAME"] + text;
            }
        }
        /// <summary>
        /// 当前URL不含参数
        /// </summary>
        public static string ThisPageUrlNoQuery
        {
            get
            {
                return Current.ThisServerUrl + Current.Request.ServerVariables["SCRIPT_NAME"];
            }
        }
        /// <summary>
        /// 获取当前页的全部绝对路径
        /// </summary>
        public static string ThisPageFillPath
        {
            get
            {
                string thisPageUrlNoQuery = Current.ThisPageUrlNoQuery;
                int num = thisPageUrlNoQuery.LastIndexOf("/");
                string a = Base.Right(thisPageUrlNoQuery, thisPageUrlNoQuery.Length - num);
                string result;
                if (a == "/")
                {
                    result = thisPageUrlNoQuery;
                }
                else
                {
                    result = Base.Left(thisPageUrlNoQuery, num + 1);
                }
                return result;
            }
        }
        /// <summary>
        /// 当前URL 含参数
        /// </summary>
        public static string ThisPageUrl
        {
            get
            {
                string text = Current.Request.ServerVariables["QUERY_STRING"];
                return Current.ThisPageUrlNoQuery + ((!Base.IsNull(text)) ? ("?" + text) : "");
            }
        }
        /// <summary>
        /// 当前URL 不含服务器URL
        /// </summary>
        public static string ThisQuery
        {
            get
            {
                string text = Current.Request.ServerVariables["QUERY_STRING"];
                return Base.IsNull(text) ? "" : text;
            }
        }
        /// <summary>
        /// 服务器域
        /// </summary>
        public static string ServerDomain
        {
            get
            {
                string text = Current.Request.Url.Host.ToLower();
                string result;
                if (Current.IsIP(text))
                {
                    result = text;
                }
                else
                {
                    string[] array = text.Split(new char[]
					{
						'.'
					});
                    int num = array.Length;
                    if (num < 3)
                    {
                        result = text;
                    }
                    else
                    {
                        bool flag = text.IndexOf(".com.") > 0 || text.IndexOf(".net.") > 0 || text.IndexOf(".org.") > 0 || text.IndexOf(".gov.") > 0;
                        int num2 = num - (flag ? 1 : 0) - 2;
                        string text2 = "";
                        for (int i = num2; i < num; i++)
                        {
                            text2 = text2 + array[i] + ((i == num - 1) ? "" : ".");
                        }
                        result = text2;
                    }
                }
                return result;
            }
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="Remove"></param>
        /// <returns></returns>
        public static string GetQuery(string Remove)
        {
            string result;
            if (Base.IsNull(Remove))
            {
                result = Current.ThisQuery;
            }
            else
            {
                result = Current.ThisQuery.Replace(Remove + "=", "");
            }
            return result;
        }
        /// <summary>
        /// 打印当前字符
        /// </summary>
        /// <param name="Str">字符</param>
        public static void Write(string Str)
        {
            Current.Response.Write(Str);
        }
        /// <summary>
        /// 打印当前字符
        /// </summary>
        /// <param name="Sb">StringBuilder</param>
        public static void Write(StringBuilder Sb)
        {
            Current.Response.Write(Sb.ToString());
        }
        /// <summary>
        /// 结束打印
        /// </summary>
        public static void End()
        {
            Current.Response.End();
        }
        /// <summary>
        /// 获取所有方式的参数值
        /// 包含GET 和 POST 方式
        /// </summary>
        /// <param name="Key">名称</param>
        /// <returns>字符</returns>
        public static string Query(string Key)
        {
            return Base.DoSqlReplace(Current.Request[Key]);
        }
        /// <summary>
        /// 获取 GET 方式参数值
        /// </summary>
        /// <param name="Key">名称</param>
        /// <returns>字符</returns>
        public static string Get(string Key)
        {
            return Base.DoSqlReplace(Current.Request.QueryString[Key]);
        }
        /// <summary>
        /// 获取 POST 方式参数值
        /// </summary>
        /// <param name="Key">名称</param>
        /// <returns>字符</returns>
        public static string Post(string Key)
        {
            return Base.DoSqlReplace(Current.Request.Form[Key]);
        }
        /// <summary>
        /// 是否IP
        /// </summary>
        /// <param name="TIP">IP</param>
        /// <returns>BOOL</returns>
        public static bool IsIP(string TIP)
        {
            return Regex.IsMatch(TIP, "^\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}$");
        }
        /// <summary>
        /// 获取IP
        /// </summary>
        /// <returns></returns>
        public static string IP()
        {
            string text = Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string text2 = Current.Request.ServerVariables["REMOTE_ADDR"];
            string result;
            if (Base.IsNull(text))
            {
                result = text2;
            }
            else
            {
                result = text;
            }
            return result;
        }
        /// <summary>
        /// IP对应数据段
        /// </summary>
        /// <param name="TIP">IP</param>
        /// <returns></returns>
        public static float IP(string TIP)
        {
            float result;
            if (!Current.IsIP(TIP))
            {
                result = -1f;
            }
            else
            {
                string[] array = TIP.Split(new char[]
				{
					','
				});
                result = (float)(Convert.ToInt32(array[0]) * 16777216 + Convert.ToInt32(array[1]) * 65536 + Convert.ToInt32(array[2]) * 256 + Convert.ToInt32(array[3]));
            }
            return result;
        }
        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="Url">Url地址</param>
        public static void Redirect(string Url)
        {
            Current.Response.Redirect(Url, true);
        }
        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="Url">Url地址</param>
        /// <param name="IsUelEnCode">是否使用自定义的URL地址编码</param>
        public static void Redirect(string Url, bool IsUelEnCode)
        {
            if (IsUelEnCode)
            {
                Current.Redirect(Current.UrlEnCode(Url));
            }
            Current.Redirect(Url);
        }
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="Url">URL</param>
        /// <returns></returns>
        public static string ServerUrlEnCode(string Url)
        {
            string result;
            if (Base.IsNull(Url))
            {
                result = "";
            }
            else
            {
                result = Current.Server.UrlEncode(Url);
            }
            return result;
        }
        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string ServerUrlDecode(string Url)
        {
            string result;
            if (Base.IsNull(Url))
            {
                result = "";
            }
            else
            {
                result = Current.Server.UrlDecode(Url);
            }
            return result;
        }
        /// <summary>
        /// 自定义URL编码
        /// </summary>
        /// <param name="Url">URL</param>
        /// <returns></returns>
        public static string UrlEnCode(string Url)
        {
            string result;
            if (Base.IsNull(Url))
            {
                result = "";
            }
            else
            {
                result = Url.Replace("?", "%3F").Replace("&", "%26").Replace("=", "%3D");
            }
            return result;
        }
        /// <summary>
        /// 自定义URL解码
        /// </summary>
        /// <param name="Url">URL</param>
        /// <returns></returns>
        public static string UrlDeCode(string Url)
        {
            string result;
            if (Base.IsNull(Url))
            {
                result = "";
            }
            else
            {
                result = Url.Replace("%3F", "?").Replace("%26", "&").Replace("%3D", "=");
            }
            return result;
        }
        /// <summary>
        /// 转换格式化站点路径 最末带 /
        /// </summary>
        /// <param name="Site">原路径</param>
        /// <returns>新路径</returns>
        public static string FormatWebSite(string Site)
        {
            string result;
            if (Base.IsNull(Site))
            {
                result = Current.ThisServerUrl + "/";
            }
            else
            {
                if (Base.Right(Site, 1) == "/")
                {
                    result = Site;
                }
                else
                {
                    result = Site + "/";
                }
            }
            return result;
        }
        /// <summary>
        /// 输入错误日志 并 跳转到错误页面
        /// </summary>
        /// <param name="IsToUrl">是否发生跳转</param>
        /// <param name="ErrID">错误编号</param>
        /// <param name="ErrMsg">错误信息</param>
        public static void Error(bool IsToUrl, string ErrID, string ErrMsg)
        {
            Logs.SLog.WriteE(ErrMsg);
            if (IsToUrl)
            {
                Current.Redirect(Base.GetKeyValue("Key.ErrorPage", Current.ThisServerUrl + "/License.aspx") + "?ID=" + ErrID);
            }
            Current.Response.End();
        }
        /// <summary>
        /// 获取所有参数,并排除指定的参数
        /// </summary>
        /// <param name="Excludes">排除指定的参数 以 , 分割</param>
        /// <returns>string</returns>
        public static string GetParameters(string Excludes)
        {
            if (!Base.IsNull(Excludes))
            {
                Excludes = Excludes.Replace(" ", "");
            }
            StringBuilder stringBuilder = new StringBuilder();
            Excludes = ("," + Excludes + ",").ToLower();
            string[] allKeys = Current.Request.QueryString.AllKeys;
            int num = (allKeys == null) ? 0 : allKeys.Length;
            for (int i = 0; i < num; i++)
            {
                string text = allKeys[i];
                if (!Base.IsNull(text))
                {
                    if (Excludes.IndexOf("," + text.ToLower() + ",") < 0)
                    {
                        if (stringBuilder.Length >= 1)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append(text + "=" + Current.Get(text));
                    }
                }
            }
            return stringBuilder.ToString();
        }
    }
}
