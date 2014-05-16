using System;
using System.Web;
using System.Web.Caching;

namespace Demo.Based
{
    /// <summary>
    /// .Net系统缓存系统操作类
    /// </summary>
    public class Caching
    {
        /// <summary>
        /// 设置 Cache 对象
        /// 现在使用  HttpRuntime.Cache
        /// 另外一种  HttpContext.Current.Cache
        /// </summary>
        private static Cache _Cache = HttpRuntime.Cache;
        /// <summary>
        /// 设置绝对过期的时间 秒级
        /// 默认2小时过期
        /// </summary>
        public static int Minute = 14400;
        /// <summary>
        /// 获取缓存KEY
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <returns>string</returns>
        public static string GetKey(string Key)
        {
            return Base.Key_Cache + Key;
        }
        /// <summary>
        /// 获取当前缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <returns>object 对象</returns>
        public static object Get(string Key)
        {
            Key = Caching.GetKey(Key);
            object result;
            if (Caching._Cache[Key] == null)
            {
                result = null;
            }
            else
            {
                result = Caching._Cache.Get(Key);
            }
            return result;
        }
        /// <summary>
        /// 移除当前缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        public static void Remove(string Key)
        {
            Key = Caching.GetKey(Key);
            if (Caching._Cache[Key] != null)
            {
                Caching._Cache.Remove(Key);
            }
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        public static void SetCache(string Key, object Value)
        {
            Caching.SetCache(Key, Value, ECache.Elasticity, Caching.Minute, null);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="Time">到期时间上限 分钟</param>
        public static void SetCache(string Key, object Value, int Time)
        {
            Caching.SetCache(Key, Value, ECache.Elasticity, Time, null);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="Time">到期时间上限 时间</param>
        public static void SetCache(string Key, object Value, DateTime Time)
        {
            Caching.SetCache(Key, Value, ECache.Elasticity, Time, null);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="eCache">缓存类型</param>
        /// <param name="Time">到期时间上限 分钟</param>
        public static void SetCache(string Key, object Value, ECache eCache, int Time)
        {
            Caching.SetCache(Key, Value, eCache, Time, null);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="eCache">缓存类型</param>
        /// <param name="Time">到期时间上限 时间</param>
        public static void SetCache(string Key, object Value, ECache eCache, DateTime Time)
        {
            Caching.SetCache(Key, Value, eCache, Time, null);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="eCache">缓存类型</param>
        /// <param name="Time">到期时间上限 分钟</param>
        /// <param name="iCacheDependency">缓存依赖项</param>
        public static void SetCache(string Key, object Value, ECache eCache, int Time, CacheDependency iCacheDependency)
        {
            if (eCache == ECache.Absolutely)
            {
                Caching.SetCacheAbsolutely(Key, Value, DateTime.Now.AddMinutes((double)Time), iCacheDependency);
            }
            else
            {
                Caching.SetCacheElasticity(Key, Value, Time, iCacheDependency);
            }
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="eCache">缓存类型</param>
        /// <param name="Time">到期时间上限 时间</param>
        /// <param name="iCacheDependency">缓存依赖项</param>
        public static void SetCache(string Key, object Value, ECache eCache, DateTime Time, CacheDependency iCacheDependency)
        {
            if (eCache == ECache.Absolutely)
            {
                Caching.SetCacheAbsolutely(Key, Value, Time, iCacheDependency);
            }
            else
            {
                Caching.SetCacheElasticity(Key, Value, Caching.Minute, iCacheDependency);
            }
        }
        /// <summary>
        /// 设置绝对缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="Time">绝对到期时间</param>
        /// <param name="iCacheDependency">缓存依赖项</param>
        private static void SetCacheAbsolutely(string Key, object Value, DateTime Time, CacheDependency iCacheDependency)
        {
            Caching._Cache.Insert(Caching.GetKey(Key), Value, iCacheDependency, Time, TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
        }
        /// <summary>
        /// 设置弹性缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="Time">时间(分钟) 当前时间上加上多少时间,整数</param>
        /// <param name="iCacheDependency">缓存依赖项</param>
        private static void SetCacheElasticity(string Key, object Value, int Time, CacheDependency iCacheDependency)
        {
            Caching._Cache.Insert(Caching.GetKey(Key), Value, iCacheDependency, DateTime.MaxValue, TimeSpan.FromMinutes((double)Time), CacheItemPriority.NotRemovable, null);
        }
    }
}
