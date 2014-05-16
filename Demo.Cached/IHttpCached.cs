using System;
using System.Collections.Generic;
using System.Data;
using Demo.Based;
using Demo.Data;

namespace Demo.Cached
{
    /// <summary>
    /// 处理缓存列表项
    /// 保证2分钟循环取一次缓存对象
    /// </summary>
    public class IHttpCached
    {
        /// <summary>
        /// 处理缓存列表项数据
        /// </summary>
        private static List<TCached> Items;
        /// <summary>
        /// 原子锁
        /// </summary>
        private static object Locker;
        /// <summary>
        /// 是否已经加载过数据
        /// </summary>
        private static bool IsLoaded;
        /// <summary>
        /// 计时器
        /// </summary>
        private static long Ticks;
        /// <summary>
        /// 根据缓存主键处理缓存列表项获取指定对象
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <returns>TCached</returns>
        public static TCached Get(string CKey)
        {
            return Items.Find((TCached Item) => Item.CACHED == CKey);
        }
        /// <summary>
        /// 处理缓存列表项
        /// </summary>
        static IHttpCached()
        {
            Locker = new object();
            Ticks = DateTime.Now.Ticks;
            if (!IsLoaded || (DateTime.Now.Ticks - Ticks) / 1000000L > 120L)
            {
                lock (Locker)
                {
                    Load();
                }
            }
        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        private static void Log(string CKey)
        {
            Logs.CLog.WriteE("缓存主键 [" + CKey + "] 不存在.");
        }
        /// <summary>
        /// 加载所有的缓存服务器
        /// </summary>
        private static void Load()
        {
            IsLoaded = true;
            Ticks = DateTime.Now.Ticks;
            if (Items != null)
            {
                Items.Clear();
            }
            else
            {
                Items = new List<TCached>();
            }
            string text = "SELECT * FROM [MS_T_CACHE](NOLOCK) ORDER BY ID ASC";
            DataTable dataTable = SqlExecute.GetTable(Base.Data_Config, text);
            if (dataTable == null)
            {
                Logs.SLog.WriteE("加载缓存配置表失败!");
            }
            else
            {
                int count = dataTable.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    DataRow dataRow = dataTable.Rows[i];
                    TCached Cached = new TCached();
                    Cached.ID = Base.ToInt(dataRow["ID"].ToString(), 0);
                    Cached.CACHED = dataRow["CACHED"].ToString();
                    if (!(Base.Left(Cached.CACHED, 3) == "---"))
                    {
                        if (Items.Find((TCached Item) => Item.CACHED == Cached.CACHED) == null)
                        {
                            try
                            {
                                Cached.ISLOCK = Base.IsBool(dataRow["ISLOCK"].ToString());
                                Cached.USECACHED = (EUseCached)Base.ToInt(dataRow["USECACHED"].ToString(), 0);
                                Cached.CACHETYPE = (ECacheTyped)Base.ToInt(dataRow["CACHETYPE"].ToString(), 2);
                                Cached.EXTIME = Base.ToInt(dataRow["EXTIME"].ToString(), 0);
                                Cached.FIELDTYPE = (SqlDbType)Base.ToInt(dataRow["FIELDTYPE"].ToString(), 22);
                                Cached.KEY_FIELD = dataRow["KEY_FIELD"].ToString();
                                Cached.KEY_VALUE = dataRow["KEY_VALUE"].ToString();
                                Cached.MEMCACHED = dataRow["MEMCACHED"].ToString();
                                Cached.SQLDATA = dataRow["SQLDATA"].ToString();
                                Cached.SQLSTATEMENT = dataRow["SQLSTATEMENT"].ToString();
                                Cached.ASQLSTATEMENT = dataRow["ASQLSTATEMENT"].ToString();
                                Cached.SQLCOLUMNS = GetColumns(Cached.SQLSTATEMENT, Cached.SQLDATA);
                                Items.Add(Cached);
                            }
                            catch
                            {
                            }
                            Cached = null;
                        }
                    }
                }
                dataTable.Dispose();
                dataTable = null;
                Ticks = DateTime.Now.Ticks;
            }
        }
        /// <summary>
        /// 根据Sql语句或者是DataTable结构 获取当前所有的列名
        /// </summary>
        /// <param name="SqlStatement">Sql语句</param>
        /// <param name="Conns">当前Sql语句的连接Conn对象</param>
        /// <returns>string[]</returns>
        private static string[] GetColumns(string SqlStatement, string Conns)
        {
            string text = SqlStatement.ToUpper();
            string[] result;
            if (text.IndexOf(" * ") > 0)
            {
                result = _GetColumns(text, Conns);
            }
            else
            {
                int num = text.IndexOf("SELECT ");
                int num2 = text.LastIndexOf(" FROM ");
                if (num < 0 || num2 < 0)
                {
                    result = _GetColumns(text, Conns);
                }
                else
                {
                    result = _GetColumns(Base.Mid(text, num + 7, num2 - num - 7));
                }
            }
            return result;
        }
        /// <summary>
        /// 排除杂项干扰 , 获取 不带任何 修饰符 的字段名集合
        /// 去除 [  ]  . 等一系列干扰符号
        /// </summary>
        /// <param name="sColumn">列名集合</param>
        /// <returns>string[] </returns>
        private static string[] _GetColumns(string sColumn)
        {
            List<string> list = new List<string>();
            string[] array = sColumn.Split(",".ToCharArray(), StringSplitOptions.None);
            int num = array.Length;
            for (int i = 0; i < num; i++)
            {
                string cName = array[i];
                int num2 = cName.LastIndexOf(" AS ");
                if (num2 < 0)
                {
                    num2 = cName.LastIndexOf(".");
                }
                if (num2 > 0)
                {
                    cName = Base.Mid(cName, num2 + 1, cName.Length - num2 - 1);
                }
                cName = cName.Replace(" ", "");
                if (!(list.Find((string Item) => Item == cName) == cName))
                {
                    list.Add(cName);
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 根据Sql语句或者是DataTable结构 获取当前所有的列名
        /// </summary>
        /// <param name="SqlStatement">Sql语句</param>
        /// <param name="Conns">当前Sql语句的连接Conn对象</param>
        /// <returns>string[]</returns>
        private static string[] _GetColumns(string SqlStatement, string Conns)
        {
            DataTable table = SqlExecute.GetTable(Conns, SqlStatement);
            DataColumnCollection columns = table.Columns;
            table.Dispose();
            return _GetColumns(ref columns);
        }
        /// <summary>
        /// 根据Sql语句或者是DataTable结构 获取当前所有的列名
        /// </summary>
        /// <param name="Columns">DataColumnCollection</param>
        /// <returns>string[]</returns>
        private static string[] _GetColumns(ref DataColumnCollection Columns)
        {
            List<string> list = new List<string>();
            int count = Columns.Count;
            for (int i = 0; i < count; i++)
            {
                string cName = Columns[i].ColumnName;
                if (!(list.Find((string Item) => Item == cName) == cName))
                {
                    list.Add(cName);
                }
            }
            Columns = null;
            return list.ToArray();
        }
    }
}
