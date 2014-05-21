using System;
using System.Linq;

namespace Demo.Framework.Web.Mvc.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paged<T>(
                this IQueryable<T> query, int page, int pageSize)
        {
            int skip = Math.Max(pageSize * page, 0);
            return query.Skip(skip).Take(pageSize);
        }
    }
}
