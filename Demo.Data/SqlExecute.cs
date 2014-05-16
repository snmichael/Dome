using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Demo.Based;
using Demo.Error;

namespace Demo.Data
{
    /// <summary>
    /// 执行数据库操作的中间层
    /// </summary>
    public class SqlExecute
    {
        #region 不关闭连接的方法
        /// <summary>
        /// 只返回一个 int 类型得存贮过程
        /// </summary>
        /// <param name="Conn">Connection对象的连接字符串</param>
        /// <param name="Procedure">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Args">具有返回值的 SqlParameter  对象的索引的 int 对象</param>
        /// <returns>返回 int 类型值</returns>
        public static int Execute(Connection Conn, string Procedure, SqlParameter[] Parameters, int Arg)
        {
            List<object> Values = Conn.Execute(Procedure, Parameters, new List<int>() { Arg });
            if (Values == null) return ErrorNum.Rkey_SqlData;
            return Base.ToInt(Values[0], ErrorNum.Rkey_SqlData);
        }
        /// <summary>
        /// 返回一个DataTable 支持 返回单个数字
        /// </summary>
        /// <param name="Conn">Connection 对象的连接字符串</param>
        /// <param name="Procedure">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable GetTable(Connection Conn, string Procedure, SqlParameter[] Parameters)
        {
            DataTable Dt = Conn.GetDataTable(Procedure, Parameters);
            return Dt;
        }
        #endregion

        /// <summary>
        /// 执行一条语句
        /// </summary>
        /// <param name="Conns">Connection对象的连接字符串</param>
        /// <param name="Sql">语句</param>
        public static bool Execute(string Conns, string Sql)
        {
            Connection Conn = new Connection(Conns);
            Conn.Execute(Sql);
            bool rBool = Conn.Executed;
            Conn.Close();
            return rBool;
        }
        /// <summary>
        /// 只返回一个 int 类型得存贮过程
        /// </summary>
        /// <param name="Conns">Connection对象的连接字符串</param>
        /// <param name="Procedure">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Args">具有返回值的 SqlParameter  对象的索引的 int 对象</param>
        /// <returns>返回 int 类型值</returns>
        public static int Execute(string Conns, string Procedure, SqlParameter[] Parameters, int Arg)
        {
            Connection Conn = new Connection(Conns);
            List<object> Values = Conn.Execute(Procedure, Parameters, new List<int>() { Arg });
            Conn.Close();
            if (Values == null) return ErrorNum.Rkey_SqlData;
            return Base.ToInt(Values[0], ErrorNum.Rkey_SqlData);
        }
        /// <summary>
        /// 返回一个 int[]  类型得存贮过程
        /// </summary>
        /// <param name="Conns">Connection对象的连接字符串</param>
        /// <param name="Procedure">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Args">具有返回值的 SqlParameter  对象的索引的 List&gt;int&lt;  数组对象</param>
        /// <returns>返回 List&gt;object&lt; 类型值</returns>
        public static List<object> Execute(string Conns, string Procedure, SqlParameter[] Parameters, List<int> Args)
        {
            Connection Conn = new Connection(Conns);
            List<object> Values = Conn.Execute(Procedure, Parameters, Args);
            Conn.Close();
            return Values;
        }
        /// <summary>
        /// 返回一个DataTable 支持 返回单个数字
        /// </summary>
        /// <param name="Conns">Connection 对象的连接字符串</param>
        /// <param name="Procedure">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable GetTable(string Conns, string Procedure, SqlParameter[] Parameters)
        {
            Connection Conn = new Connection(Conns);
            DataTable Dt = Conn.GetDataTable(Procedure, Parameters);
            Conn.Close();
            return Dt;
        }
        /// <summary>
        /// 返回一个DataTable 支持 返回单个数字
        /// </summary>
        /// <param name="Conns">Connection 对象的连接字符串</param>
        /// <param name="Procedure">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Arg">索引</param>
        /// <param name="Value">返回值</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable GetTable(string Conns, string Procedure, SqlParameter[] Parameters, int Arg, ref  object Value)
        {
            Connection Conn = new Connection(Conns);
            DataTable Table = Conn.GetDataTable(Procedure, Parameters, Arg, ref Value);// GetTable(Conns, Procedure, Parameters, Arg, ref Value);
            Conn.Close(); 
            return Table;
        }
        /// <summary>
        /// 返回一个DataTable 支持 返回数组
        /// </summary>
        /// <param name="Conns">Connection 对象的连接字符串</param>
        /// <param name="Procedure">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Args">具有返回值的 SqlParameter  对象的索引的 List&gt;int&lt;  数组对象</param>
        /// <param name="Values">返回的 SqlParameter[]  对象的  List&gt;object&lt; </param>
        /// <returns>DataTable 对象</returns>
        public static DataTable GetTable(string Conns, string Procedure, SqlParameter[] Parameters, List<int> Args, ref List<object> Values)
        {
            Connection Conn = new Connection(Conns);
            DataTable Table = Conn.GetDataTable(Procedure, Parameters, Args, ref Values);
            Conn.Close();
            return Table;
        }

        /// <summary>
        /// 返回一个DataTable 支持 返回数组
        /// </summary>
        /// <param name="Conns">Connection 对象的连接字符串</param>
        /// <param name="Sql">Sql语句</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable GetTable(string Conns, string Sql)
        {
            Connection Conn = new Connection(Conns);
            DataTable Dt = Conn.GetDataTable(Sql);
            Conn.Close();
            return Dt;
        }
        /// <summary>
        /// 返回一个DataTable[]对象
        /// </summary>
        /// <param name="ProcName">存贮过程名称</param>
        /// <param name="SqlParameters">SqlParameter[] 对象</param>
        /// <returns>DataTableCollection 对象</returns>
        public static DataTableCollection GetTables(string Conns, string Sql)
        {
            Connection Conn = new Connection(Conns);
            DataTableCollection Dtc = Conn.GetDataTables(Sql);
            Conn.Close();
            return Dtc;
        }
        /// <summary>
        /// 返回一个DataTable[]对象
        /// </summary>
        /// <param name="ProcName">存贮过程名称</param>
        /// <param name="SqlParameters">SqlParameter[] 对象</param>
        /// <returns>DataTableCollection 对象</returns>
        public static DataTableCollection GetTables(string ProcName, SqlParameter[] SqlParameters)
        {
            Connection Conn = new Connection();
            DataTableCollection Dtc = Conn.GetDataTables(ProcName, SqlParameters);
            Conn.Close();
            return Dtc;
        }
        /// <summary>
        /// 返回一个DataTable[]对象
        /// </summary>
        /// <param name="ProcName">存贮过程名称</param>
        /// <param name="SqlParameters">SqlParameter[] 对象</param>
        /// <param name="Conn">Connection 对象</param>
        /// <returns>DataTableCollection 对象</returns>
        public static DataTableCollection GetTables(string ProcName, SqlParameter[] SqlParameters, out Connection Conn)
        {
            Conn = new Connection();
            return Conn.GetDataTables(ProcName, SqlParameters);
        }
        /// <summary>
        /// 返回一个DataTable[]对象
        /// </summary>
        /// <param name="Conns">Connection 对象连接字符串</param>
        /// <param name="ProcName">存贮过程名称</param>
        /// <param name="SqlParameters">SqlParameter[] 对象</param>
        /// <returns>DataTableCollection 对象</returns>
        public static DataTableCollection GetTables(string Conns, string ProcName, SqlParameter[] SqlParameters)
        {
            Connection Conn = new Connection(Conns);
            DataTableCollection Dtc = Conn.GetDataTables(ProcName, SqlParameters);
            Conn.Close();
            return Dtc;
        }

        /// <summary>
        /// 返回一个DataTable[]对象
        /// </summary>
        /// <param name="Conns">Connection 对象连接字符串</param>
        /// <param name="ProcName">存贮过程名称</param>
        /// <param name="SqlParameters">SqlParameter[] 对象</param>
        /// <param name="Arg">索引</param>
        /// <param name="Value">返回值</param>
        /// <returns>DataTableCollection 对象</returns>
        public static DataTableCollection GetTables(string Conns, string ProcName, SqlParameter[] SqlParameters, int Arg, ref  object Value)
        {
            Connection Conn = new Connection(Conns);
            DataTableCollection Dtc = Conn.GetDataTables(ProcName, SqlParameters, Arg, ref Value);
            Conn.Close();
            return Dtc;
        }
        /// <summary>
        /// 返回一个DataTable[]对象
        /// </summary>
        /// <param name="Conns">Connection 对象连接字符串</param>
        /// <param name="ProcName">存贮过程名称</param>
        /// <param name="SqlParameters">SqlParameter[] 对象</param>
        /// <param name="Args">具有返回值的 SqlParameter  对象的索引的 List&gt;int&lt;  数组对象</param>
        /// <param name="Values">返回的 SqlParameter[]  对象的  List&gt;object&lt; </param>
        /// <returns>DataTableCollection 对象</returns>
        public static DataTableCollection GetTables(string Conns, string ProcName, SqlParameter[] SqlParameters, List<int> Args, ref List<object> Values)
        {
            Connection Conn = new Connection(Conns);
            DataTableCollection Dtc = Conn.GetDataTables(ProcName, SqlParameters, Args, ref Values);
            Conn.Close();
            return Dtc;
        }
    }
}