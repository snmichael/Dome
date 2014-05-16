using System.Collections.Generic;
using System.Data;
using System.Text;
using Demo.Based;

namespace Demo.Data
{

    /// <summary>
    /// 数据源文件缓存
    /// </summary>
    public class SqlServer
    {
        /// <summary>
        /// 静态初始化
        /// </summary>
        static SqlServer()
        {
            //加载数据源
            if (IsHave()) return;
            lock (Locker) { if (IsHave()) return; Load(); }
        }
        /// <summary>
        /// 系统锁
        /// </summary>
        private static object Locker = new object();
        /// <summary>
        /// 数据库连接泛型
        /// </summary>
        private static List<SqlItems> Items;
        /// <summary>
        /// 数据源文件路径
        /// </summary>
        private static string SqlServerFile = Base.GetKeyValue("Key.SqlServerFile", "~/Config/SqlServer.config");
        /// <summary>
        /// 是否含有数据
        /// </summary>
        /// <returns>bool</returns>
        private static bool IsHave()
        {
            if (Items == null) return false;
            if (Items.Count <= 0) return false;
            return true;
        }
        /// <summary>
        /// 记载数据源
        /// </summary>
        private static void Load()
        {
            //
            if (Items == null) Items = new List<SqlItems>();
            else Items.Clear();
            //
            DataTable Table = XmlToData.XmlFileToDataTable(Files.ServerPath(SqlServerFile));
            if (Table == null) { Logs.SLog.WriteE("加载数据源配置文件 SqlServer.config 失败!"); return; }
            int Count = Table.Rows.Count;
            for (int i = 0; i < Count; i++)
            {
                DataRow Rs = Table.Rows[i];
                SqlItems Item = new SqlItems();
                StringBuilder Sb = new StringBuilder();
                Item.Key = EnCode.DeToCode(Rs["DataCache"].ToString())
                ;
                string Value = "";
                //DataService
                Sb.Append("Data Source=" + EnCode.DeToCode(Rs["DataService"].ToString()) + ",");
                //DataPort
                Sb.Append(Base.ToInt(EnCode.DeToCode(Rs["DataPort"].ToString()), 1433).ToString() + ";");
                //DataName
                Sb.Append("Database=" + EnCode.DeToCode(Rs["DataName"].ToString()) + ";");
                //DataUser
                Value = EnCode.DeToCode(Rs["DataUser"].ToString());
                Sb.Append("UID=" + (Base.IsNull(Value) ? "" : Value) + ";");
                //DataPassword
                Value = EnCode.DeToCode(Rs["DataPassword"].ToString());
                Sb.Append("Password=" + (Base.IsNull(Value) ? "" : Value) + ";");
                //DataTimeOut
                Sb.Append("Connection Lifetime=" + Base.ToInt(EnCode.DeToCode(Rs["DataTimeOut"].ToString()), 60).ToString() + ";");
                //MinPoolSize
                Sb.Append("Min Pool Size=0;");
                //DataMaxPool
                Sb.Append("Max Pool Size=" + Base.ToInt(EnCode.DeToCode(Rs["DataMaxPool"].ToString()), 800).ToString() + ";");
                //
                Item.Source = Sb.ToString();
                Sb = null;
                Items.Add(Item);
                Item = null; Rs = null;
            }
            Table.Dispose(); Table = null;
        }
        /// <summary>
        /// 获取对应的数据源
        /// </summary>
        /// <param name="Key">源主键</param>
        /// <returns>连接串</returns>
        public static string Get(string Key)
        {
            //if (!IsHave()) return "连接主键  " + Key + "  数据源不存在!";
            //foreach (SqlItems Item in Items)
            //{
            //    if (Item.Key == Key) return Item.Source;
            //}
            //return "连接主键  " + Key + "  数据源不存在!";
            if (!IsHave())
            {
                return ("连接主键  " + Key + "  数据源不存在!");
            }
            SqlItems items = null;
            items = Items.Find(delegate(SqlItems item)
            {
                return item.Key == Key;
            });
            if (items == null)
            {
                return ("连接主键  " + Key + "  数据源不存在!");
            }
            if (Base.IsNull(items.Source))
            {
                return ("连接主键  " + Key + "  数据源不存在!");
            }
            return items.Source;

        }
    }
}