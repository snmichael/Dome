using System.Globalization;

namespace Demo.Core.Globalization
{
    public static class CultureProvider
    {
        public const string CultureCookieKey = "Lang";
        public const string CultureDefault = "zh-cn";

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
