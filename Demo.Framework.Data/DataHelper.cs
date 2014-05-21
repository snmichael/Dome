using System;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Demo.Framework.Data
{
    public static class DataHelper
    {

        /// <summary>
        /// 获取db时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDbTime()
        {
            return
                Convert.ToDateTime(new SqlDatabase(DbConnection.CrmDb.ConnectionString).ExecuteScalar(new SqlCommand("select getdate();")));
        }
    }
}
