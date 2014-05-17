using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Web.Utility.Globalization
{
    public static class CultureProvider
    {
        public const string CultureCookieKey = "Lang";
        public const string CultureDefault = "en-US";

        public static CultureInfo GetCultureInfo(string ci)
        {
            try
            {
                return new CultureInfo(ci);
            }
            catch
            {
                return null;
            }
        }
    }
}
