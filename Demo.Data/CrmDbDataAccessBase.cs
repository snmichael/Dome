using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.Framework.Core.Infrastructure;
using Demo.Framework.Data;
using Dome.DataBase;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Demo.Data
{
    

    /// <summary>
    /// 管理HTCRM数据库连接对象
    /// </summary>
    public class CrmDbDataAccessBase : DataAccessBase
    {
        /// <summary>
        /// HTCRM数据库连接对象
        /// </summary>
        protected override Database CurrentDatabase
        {
            get { return _database ?? (_database = new SqlDatabase(DbConnection.CrmDb.ConnectionString)); }
        }

        private CrmDbEntities _htcrmDbContext;
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        protected CrmDbEntities HtcrmDbContext
        {
            get { return _htcrmDbContext ?? (_htcrmDbContext = BaseEngine.Resolve<CrmDbEntities>()); }
        }
    }
}
