using System;
using System.Collections.Generic;
using Demo.Based;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using Miracle.Cached;

namespace Demo.Cached
{
    /// <summary>
    /// 操作远程缓存服务器客户端类
    /// </summary>
    internal class MemClient
    {
        /// <summary>
        /// 是否含有数据源
        /// </summary>
        private static bool IsHave;
        /// <summary>
        /// 原子锁
        /// </summary>
        private static object Locker;
        /// <summary>
        /// 是否已经加载过数据
        /// </summary>
        private static bool IsLoaded;
        /// <summary>
        /// 远程缓存服务器列表
        /// </summary>
        private static List<TMemClient> Items;
        /// <summary>
        /// 获取指定主键的TMemClient对象
        /// </summary>
        /// <param name="MKey">指定主键</param>
        /// <returns>TMemClient对象</returns>
        public static TMemClient Get(string MKey)
        {
            TMemClient result;
            if (Base.IsNull(MKey))
            {
                result = null;
            }
            else
            {
                if (!MemClient.IsHave)
                {
                    result = null;
                }
                else
                {
                    result = MemClient.Items.Find((TMemClient Item) => Item.MKey == MKey);
                }
            }
            return result;
        }
        /// <summary>
        /// 判断是否建立过当前实例对象
        /// </summary>
        /// <returns>bool</returns>
        private static bool Have()
        {
            bool result;
            if (MemClient.IsLoaded)
            {
                result = MemClient.IsHave;
            }
            else
            {
                result = (MemClient.Items != null && MemClient.Items.Count > 0);
            }
            return result;
        }
        /// <summary>
        /// 静态实例
        /// </summary>
        static MemClient()
        {
            MemClient.Locker = new object();
            if (!MemClient.Have())
            {
                if (!MemClient.IsLoaded)
                {
                    lock (MemClient.Locker)
                    {
                        MemClient.Load();
                    }
                }
            }
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="Text">日志</param>
        /// <param name="eLog">日志类型</param>
        private static void Log(string Text, ELog eLog)
        {
            Logs.CLog.Write(Text, eLog);
        }
        /// <summary>
        /// 加载所有的缓存服务器
        /// </summary>
        private static void Load()
        {
            MemClient.IsLoaded = true;
            if (MemClient.Items != null)
            {
                MemClient.Items.Clear();
            }
            else
            {
                MemClient.Items = new List<TMemClient>();
            }
            List<TMemIPServer> items = Memcached.Items;
            if (items == null)
            {
                MemClient.Log("Memcached 远程服务器地址配置文件不正确!", ELog.Error);
            }
            else
            {
                foreach (TMemIPServer _IPItem in items)
                {
                    TMemClient tMemClient = new TMemClient();
                    tMemClient.MKey = _IPItem.MKey;
                    if (MemClient.Items.Find((TMemClient Item) => Item.MKey == _IPItem.MKey) == null)
                    {
                        MemcachedClientConfiguration memcachedClientConfiguration = new MemcachedClientConfiguration();
                        int count = _IPItem.Items.Count;
                        string text = "";
                        for (int i = 0; i < count; i++)
                        {
                            memcachedClientConfiguration.AddServer(_IPItem.Items[i]);
                            text = text + _IPItem.Items[i] + ";";
                        }
                        tMemClient.Client = new MemcachedClient(memcachedClientConfiguration);
                        if (tMemClient.Client.Store(StoreMode.Set, "Mc.Cache.IsConnection", "Connectioned", DateTime.Now.AddMinutes(1.0)))
                        {
                            MemClient.Items.Add(tMemClient);
                        }
                        else
                        {
                            MemClient.Log(string.Concat(new string[]
							{
								"Memcached 不能连接远程服务器[ Key : ",
								_IPItem.MKey,
								", IP : ",
								text,
								"]!"
							}), ELog.Error);
                        }
                    }
                }
                MemClient.IsHave = (MemClient.Items.Count > 0);
            }
        }
    }
}
