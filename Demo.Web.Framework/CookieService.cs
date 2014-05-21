#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2012 Luca Cammarata Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/Documentation/License.txt

#endregion

using System;
using System.Web;

namespace Demo.Framework.Core
{

    public class CookieService
    {

        #region Private Methods

        private static string GetCookieValue(string key)
        {
            if ((HttpContext.Current.Request.Cookies[key] != null))
            {
                return HttpContext.Current.Request.Cookies[key].Value;
            }
            return string.Empty;
        }

        #endregion

        #region Public Methods

        public static void SetCookieValue(string key, string value, int expiration)
        {
            if ((HttpContext.Current.Request.Cookies[key] != null))
            {
                HttpContext.Current.Response.Cookies[key].Value = value;
                HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddDays(Convert.ToDouble(expiration));
            }
            else
            {
                HttpCookie Cookie = new HttpCookie(key, value);
                Cookie.Expires = DateTime.Now.AddDays(Convert.ToDouble(expiration));
                HttpContext.Current.Response.Cookies.Add(Cookie);
            }
        }

        public static string GetCookieAsString(string key, string DefaultValue)
        {
            string Result = GetCookieValue(key);
            if (Result != string.Empty)
            {
                return Result;
            }
            return DefaultValue;
        }

        public static void SetCookieValue(string key, string value)
        {
            SetCookieValue(key, value, 0);
        }

        public static bool GetCookieAsBool(string key, bool DefaultValue)
        {
            string Result = GetCookieValue(key);
            if (Result != string.Empty)
            {
                return Convert.ToBoolean(Result);
            }
            return DefaultValue;
        }

        public static int GetCookieAsInt(string key, int DefaultValue)
        {
            string Result = GetCookieValue(key);
            if (Result != string.Empty)
            {
                return Convert.ToInt32(Result);
            }
            return DefaultValue;
        }

        /// <summary>
        /// Çå³ýCookieValue
        /// </summary>
        /// <param name="cookieName">CookieÃû³Æ</param>
        public static void ClearCookie(string cookieName)
        {
            HttpCookie myCookie = new HttpCookie(cookieName);
            DateTime now = DateTime.Now;
            myCookie.Expires = now.AddYears(-2);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        #endregion

    }

}