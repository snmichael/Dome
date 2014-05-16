using System;
using Demo.Based;

namespace Demo.Cached
{
    /// <summary>
    /// 缓存模式接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 获取当前缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <returns>object 对象</returns>
        object Get(string Key);
        /// <summary>
        /// 移除当前缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        void Remove(string Key);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        void SetCache(string Key, object Value);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="eCache">缓存类型</param>
        void SetCache(string Key, object Value, ECache eCache);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="Time">到期时间上限 分钟</param>
        void SetCache(string Key, object Value, int Time);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="Time">到期时间上限 时间</param>
        void SetCache(string Key, object Value, DateTime Time);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="eCache">缓存类型</param>
        /// <param name="Time">到期时间上限 分钟</param>
        void SetCache(string Key, object Value, ECache eCache, int Time);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="eCache">缓存类型</param>
        /// <param name="Time">到期时间上限 时间</param>
        void SetCache(string Key, object Value, ECache eCache, DateTime Time);
    }
}
