using System;
using Demo.Based;

namespace Demo.Cached
{
    /// <summary>
    /// 实现 缓存模式接口 系统缓存
    /// </summary>
    public class ICSystem : ICache
    {
        /// <summary>
        /// 获取当前缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <returns>object 对象</returns>
        public object Get(string Key)
        {
            return Caching.Get(Key);
        }
        /// <summary>
        /// 移除当前缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        public void Remove(string Key)
        {
            Caching.Remove(Key);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        public void SetCache(string Key, object Value)
        {
            Caching.SetCache(Key, Value);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="eCache">缓存类型</param>
        public void SetCache(string Key, object Value, ECache eCache)
        {
            Caching.SetCache(Key, Value, eCache, Caching.Minute);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="Time">到期时间上限 分钟</param>
        public void SetCache(string Key, object Value, int Time)
        {
            Caching.SetCache(Key, Value, Time);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="Time">到期时间上限 时间</param>
        public void SetCache(string Key, object Value, DateTime Time)
        {
            Caching.SetCache(Key, Value, Time);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="eCache">缓存类型</param>
        /// <param name="Time">到期时间上限 分钟</param>
        public void SetCache(string Key, object Value, ECache eCache, int Time)
        {
            Caching.SetCache(Key, Value, eCache, Time);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="eCache">缓存类型</param>
        /// <param name="Time">到期时间上限 时间</param>
        public void SetCache(string Key, object Value, ECache eCache, DateTime Time)
        {
            Caching.SetCache(Key, Value, eCache, Time);
        }
    }
}
