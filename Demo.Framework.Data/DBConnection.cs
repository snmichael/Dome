using System.Configuration;

namespace Demo.Framework.Data
{
    /// <summary>
    /// 管理项目中使用的连接字符串。
    /// </summary>
    public class DbConnection
    {
        public static ConnectionStringSettings CrmDb
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["CrmDbConnection"];
            }
        }
   
    }
}
