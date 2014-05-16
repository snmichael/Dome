using System;
using Demo.Based;
using Enyim.Caching.Memcached;

namespace Demo.Cached
{
    /// <summary>
    /// 实现 缓存模式接口 Memcached
    /// </summary>
    internal class ICMemcached : ICache
    {
        /// <summary>
        /// 选定的远程缓存服务器
        /// </summary>
        private TMemClient TClient = null;
        /// <summary>
        /// 远程缓存模式 Memcached
        /// </summary>
        /// <param name="Client">TMemClient</param>
        public ICMemcached(TMemClient Client)
        {
            this.TClient = Client;
        }
        /// <summary>
        /// 获取当前缓存系统
        /// </summary>
        /// <param name="Key">缓存主键</param>
        /// <param name="_ICache">若无远程缓存,则使用的默认缓存</param>
        /// <returns>ICache</returns>
        public static ICache GetIntance(string MKey, ICache _ICache)
        {
            TMemClient tMemClient = MemClient.Get(MKey);
            ICache result;
            if (tMemClient == null)
            {
                result = _ICache;
            }
            else
            {
                result = new ICMemcached(tMemClient);
            }
            return result;
        }
        /// <summary>
        /// 获取当前缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <returns>获取当前缓存 object</returns>
        public object Get(string Key)
        {
            object result;
            try
            {
                result = this.TClient.Client.Get(Caching.GetKey(Key));
            }
            catch
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// 移除当前缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        public void Remove(string Key)
        {
            this.TClient.Client.Remove(Caching.GetKey(Key));
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        public void SetCache(string Key, object Value)
        {
            this.TClient.Client.Store(StoreMode.Set, Caching.GetKey(Key), Value);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="eCache">缓存类型</param>
        public void SetCache(string Key, object Value, ECache eCache)
        {
            this.SetCache(Key, Value);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="Time">到期时间上限 分钟</param>
        public void SetCache(string Key, object Value, int Time)
        {
            this.SetCache(Key, Value, ECache.Elasticity, Time);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Value">缓存对象</param>
        /// <param name="Time">到期时间上限 时间</param>
        public void SetCache(string Key, object Value, DateTime Time)
        {
            this.SetCache(Key, Value, ECache.Elasticity, Time);
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
            this.SetCache(Key, Value, eCache, DateTime.Now.AddMinutes((double)Time));
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
            this.TClient.Client.Store(StoreMode.Set, Caching.GetKey(Key), Value, Time);
        }
    }
}
