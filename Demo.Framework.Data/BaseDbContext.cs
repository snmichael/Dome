using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Demo.Framework.Data
{
    public abstract class BaseDbContext : DbContext
    {
        private static readonly Dictionary<ConnectionStringSettings, string> ConnMap =
            new Dictionary<ConnectionStringSettings, string>();

        protected BaseDbContext(string connectstring)
            : base(connectstring)
        {

        }

        protected BaseDbContext(ConnectionStringSettings connsetting)
            : base(GetEntityConnString(connsetting))
        {

        }   

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:检查 SQL 查询是否存在安全漏洞")]
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            //HACK: Entity Framework Code First doesn't support doesn't support output parameters
            //That's why we have to manually create command and execute it.
            //just wait until EF Code First starts support them
            //
            //More info: http://weblogs.asp.net/dwahlin/archive/2011/09/23/using-entity-framework-code-first-with-stored-procedures-that-have-output-parameters.aspx

            bool hasOutputParameters = false;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    var outputP = p as DbParameter;
                    if (outputP == null)
                        continue;

                    if (outputP.Direction == ParameterDirection.InputOutput ||
                        outputP.Direction == ParameterDirection.Output)
                        hasOutputParameters = true;
                }
            }



            var context = ((IObjectContextAdapter)(this)).ObjectContext;
            if (!hasOutputParameters)
            {
                //no output parameters
                var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();
                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);

                return result;

                //var result = context.ExecuteStoreQuery<TEntity>(commandText, parameters).ToList();
                //foreach (var entity in result)
                //    Set<TEntity>().Attach(entity);
                //return result;
            }
            else
            {

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution


                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    //command to execute
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // move parameters to command object
                    if (parameters != null)
                        foreach (var p in parameters)
                            cmd.Parameters.Add(p);

                    //database call
                    var reader = cmd.ExecuteReader();
                    //return reader.DataReaderToObjectList<TEntity>();
                    var result = context.Translate<TEntity>(reader).ToList();
                    for (int i = 0; i < result.Count; i++)
                        result[i] = AttachEntityToContext(result[i]);
                    //close up the reader, we're done saving results
                    reader.Close();
                    return result;
                }

            }
        }

        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            Set<TEntity>().Attach(entity);
            return entity;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected static string GetEntityConnString(ConnectionStringSettings connsetting)
        {
            if (ConnMap.ContainsKey(connsetting))
            {
                return ConnMap[connsetting];
            }

            var entityBuilder = new EntityConnectionStringBuilder
                                    {
                                        Metadata =
                                            string.Format(
                                                "res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl",
                                                connsetting.Name),
                                        ProviderConnectionString =
                                            connsetting.ConnectionString,
                                        Provider = connsetting.ProviderName
                                    };
            var connstr = entityBuilder.ToString();
            ConnMap.Add(connsetting, connstr);
            return connstr;
        }

        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        public DataTable ExecuteDataTable(string sql, params SqlParameter[] parameters)
        {
            
         //conn.ConnectionString = db.Connection.ConnectionString;
         //if (conn.State != ConnectionState.Open)
         //{
         //    conn.Open();
         //}
            System.Data.Common.DbConnection conn = (((IObjectContextAdapter)this).ObjectContext.Connection as EntityConnection).StoreConnection;
            SqlConnection sqlConn = null;
            if (conn is SqlConnection)
            {
                sqlConn = conn as SqlConnection;
            }
            else
            {
               var tmpConnection = conn.GetType().GetProperty("InnerConnection").GetValue(conn, null);
               sqlConn = tmpConnection as SqlConnection;
            }

         SqlCommand cmd = new SqlCommand();
         cmd.Connection = sqlConn;
         cmd.CommandText = sql;
         foreach (var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.ParameterName, param.Value);

            }
         SqlDataAdapter adapter = new SqlDataAdapter(cmd);
         DataTable table = new DataTable();
         adapter.Fill(table);
        
         return table;

        }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public int ExecuteSqlCommand(string sql, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var result = this.Database.ExecuteSqlCommand(sql, parameters);

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            //return result
            return result;
        }

        public static void NoLockInvokeDB(Action action)
        {
            var transactionOptions = new System.Transactions.TransactionOptions();
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;
            using (var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, transactionOptions))
            {
                try
                {
                    action();
                }
                finally
                {
                    transactionScope.Complete();
                }
            }
        }

        public DateTime GetNow()
        {
            return System.DateTime.Now;
            //return SqlQuery<DateTime>("select getdate(); ").FirstOrDefault();
        }

    }
}
