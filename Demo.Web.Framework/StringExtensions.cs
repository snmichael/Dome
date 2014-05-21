using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace Demo.Framework.Core
{
    using System.Collections;
    using System.Linq.Expressions;

    public  static class StringExtensions
    {
        /// <summary> 
        /// 获得枚举类型数据项（不包括空项）
        /// </summary> 
        /// <param name="enumType">枚举类型</param> 
        /// <returns></returns> 
        public static void AppendLine(this StringBuilder obj,string value)
        {
            obj.Append(value);
            obj.Append(Environment.NewLine);
        }

        /// <summary> 
        /// 可空类型转字符串
        /// </summary> 
        /// <param name="enumType">枚举类型</param> 
        /// <returns></returns> 
        public static string GetValueOrDefault(this int? obj, string defalutvalue = "")
        {
            if (obj == null)
            {
                return defalutvalue;
            }
            return obj.ToString();
        }

        /// 
        /// left截取字符串
        /// 
        public static string Left(this  string sSource, int iLength)
        {
            return sSource.Substring(0, iLength > sSource.Length ? sSource.Length : iLength);
        }
        /// 
        /// Right截取字符串
        /// 
        public static string Right(string sSource, int iLength)
        {
            return sSource.Substring(iLength > sSource.Length ? 0 : sSource.Length - iLength);
        }

        /// 
        /// Right截取字符串
        /// 
        public static string NullToEmpty(this string sSource)
        {
            if (sSource == null)
            {
                return "";
            }
            return sSource;
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="len">判断的长度</param>
        /// <param name="flag">true:加省略号(...) </param>
        /// <returns></returns>
        public static string GetOmitStr(this string str, int len, bool flag)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            string content = str.Substring(0, Math.Min(str.Length, len));
            if (flag && len < str.Length)
            {
                content += "...";
            }
            return content;
        }

        public static IEnumerable<T> Split<T>(this string str, string separator)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return Enumerable.Empty<T>();
            }
            var arr = str.Split(separator[0]);

            return arr.Select(ParseBuilder<T>.ParseFun);
        }


        class ParseBuilder<T>
        {
            public static readonly Func<string, T> ParseFun;
            static ParseBuilder()
            {
                MethodInfo mi = typeof(T).GetMethod("Parse", new Type[] { typeof(string) }); 
                ParseFun = (Func<string, T>)Delegate.CreateDelegate(typeof(Func<string, T>), null, mi);
            }
        }
    }
  
}