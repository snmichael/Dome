using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace Demo.Framework.Web.Mvc.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            Contract.Requires(pageIndex >= 0, "Page index cannot be negative");
            Contract.Requires(pageSize >= 0, "Page size cannot be negative");

            int skip = pageIndex * pageSize;

            if (skip > 0)
                source = source.Skip(skip);

            source = source.Take(pageSize);

            return source;
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            Contract.Requires(pageIndex >= 0, "Page index cannot be negative");
            Contract.Requires(pageSize >= 0, "Page size cannot be negative");

            int skip = pageIndex * pageSize;

            if (skip > 0)
                source = source.Skip(skip);

            source = source.Take(pageSize);

            return source;
        }


       
    }
    public static class ExtensionMethods
    {
        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }
    }

}
