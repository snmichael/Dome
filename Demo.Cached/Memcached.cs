using Demo.Based;
using System;
using System.Collections.Generic;
using System.Data;
using Demo.Cached;

namespace Miracle.Cached
{
    /// <summary>
    /// 提供操作当前远程缓存服务器的操作类
    /// </summary>
    internal class Memcached
    {
        /// <summary>
        /// 原子锁
        /// </summary>
        private static object Locker;
        /// <summary>
        /// 存放目录
        /// </summary>
        private static string IPServerFile;
        /// <summary>
        /// 远程服务器缓存列表
        /// </summary>
        public static List<TMemIPServer> Items;
        /// <summary>
        /// 获取指定的远程缓存服务器IP地址列表
        /// </summary>
        /// <param name="MKey">远程缓存服务器主键</param>
        /// <returns>TMemIPServer</returns>
        public static TMemIPServer Get(string MKey)
        {
            TMemIPServer result;
            if (!Memcached.IsHave())
            {
                result = null;
            }
            else
            {
                result = Memcached.Items.Find((TMemIPServer Item) => Item.MKey == MKey);
            }
            return result;
        }
        /// <summary>
        /// 是否含有当前对象
        /// </summary>
        /// <returns>bool</returns>
        private static bool IsHave()
        {
            return Memcached.Items != null && Memcached.Items.Count > 0;
        }
        /// <summary>
        /// 静态实例
        /// </summary>
        static Memcached()
        {
            Memcached.Locker = new object();
            Memcached.IPServerFile = Base.GetKeyValue("Key.MemcachedFile", "~/Config/MemcachedFile.config");
            if (!Memcached.IsHave())
            {
                lock (Memcached.Locker)
                {
                    if (!Memcached.IsHave())
                    {
                        Memcached.Load();
                    }
                }
            }
        }
        /// <summary>
        /// 加载远程缓存文件
        /// </summary>
        private static void Load()
        {
            if (Memcached.Items == null)
            {
                Memcached.Items = new List<TMemIPServer>();
            }
            else
            {
                Memcached.Items.Clear();
            }
            DataTable dataTable = XmlToData.XmlFileToDataTable(Files.ServerPath(Memcached.IPServerFile));
            if (dataTable == null)
            {
                Logs.SLog.WriteE("加载远程缓存服务器配置文件 MemcachedFile.config 失败!");
            }
            else
            {
                int count = dataTable.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    DataRow dataRow = dataTable.Rows[i];
                    string Key = dataRow["KEY"].ToString();
                    if (!Base.IsNull(Key))
                    {
                        if (Memcached.Items.Find((TMemIPServer _Item) => _Item.MKey == Key) == null)
                        {
                            List<string> iPServer = Memcached.GetIPServer(dataRow["IPLIST"].ToString());
                            if (iPServer != null)
                            {
                                TMemIPServer tMemIPServer = new TMemIPServer();
                                tMemIPServer.MKey = Key;
                                tMemIPServer.Items = iPServer;
                                Memcached.Items.Add(tMemIPServer);
                            }
                        }
                    }
                }
                dataTable.Dispose();
            }
        }
        /// <summary>
        /// 根据字符串获取指定的IP列表
        /// </summary>
        /// <param name="Text">字符串</param>
        /// <returns><![CDATA[List<string>]]></returns>
        private static List<string> GetIPServer(string Text)
        {
            List<string> result;
            if (Base.IsNull(Text))
            {
                result = null;
            }
            else
            {
                List<string> list = new List<string>();
                string[] array = Text.Split(new char[]
				{
					';'
				});
                int num = array.Length;
                for (int i = 0; i < num; i++)
                {
                    list.Add(array[i]);
                }
                if (list.Count <= 0)
                {
                    list = null;
                }
                result = list;
            }
            return result;
        }
    }
}
