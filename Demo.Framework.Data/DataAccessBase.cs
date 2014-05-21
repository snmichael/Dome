
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Demo.Framework.Data
{
    public abstract class DataAccessBase
    {
        protected Database _database;

        /// <summary>
        /// HTCRM的数据连接对象。
        /// </summary>
        protected abstract Database CurrentDatabase { get; }

        protected string CommandText { get; set; }

    }
}
