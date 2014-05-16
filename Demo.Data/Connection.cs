using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Demo.Based;
using Demo.Error;

namespace Demo.Data
{
    /// <summary>
    /// 数据库操作基类 只支持 SqlServer 数据库
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// 添加事务的时候的超时限制
        /// </summary>
        private int TransactionTimeOut = 240;
        /// <summary>
        /// 是否成功连接数据库
        /// </summary>
        public bool IsConn = true;
        /// <summary>
        /// SQL 连接字符串
        /// </summary>
        public string DataString = "";
        /// <summary>
        ///是否成功执行指定的操作
        /// </summary>
        public bool Executed = false;
        /// <summary>
        /// 是否打开数据库
        /// </summary>
        public bool Connected = false;
        /// <summary>
        /// 查询数据库次数
        /// </summary>
        public int Queries = 0;
        /// <summary>
        /// SqlConnection对象
        /// </summary>
        protected SqlConnection Conn = null;
        /// <summary>
        /// 是否支持跳转到错误页面
        /// </summary>
        private bool IsGoError = false;
        /// <summary>
        /// 是否建立事务回滚机制
        /// </summary>
        public bool IsSqlTransaction = false;
        /// <summary>
        /// 数据库的事务回滚机制
        /// </summary>
        private SqlTransaction Tran;
        /// <summary>
        /// 构造新的Sql数据库连接对象
        /// 使用事务,必须显式指明 IsSqlTransaction = true
        /// </summary>
        public Connection() { this.Initialize(Base.Data_Config); }
        /// <summary>
        /// 构造新的Sql数据库连接对象
        /// 使用事务,必须显式指明 IsSqlTransaction = true
        /// </summary>
        /// <param name="IsGoError">是否跳转到错误页面</param>
        public Connection(bool IsGoError) { this.IsGoError = IsGoError; this.Initialize(Base.Data_Config); }
        /// <summary>
        /// 构造新的Sql数据库连接对象
        /// 使用事务,必须显式指明 IsSqlTransaction = true
        /// </summary>
        /// <param name="DataKey">连接数据库的字符串 或 是指定的Based.Dkey_</param>
        public Connection(string DataKey) { this.Initialize(DataKey); }
        /// <summary>
        /// 构造新的Sql数据库连接对象
        /// 使用事务,必须显式指明 IsSqlTransaction = true
        /// </summary>
        /// <param name="DataKey">连接数据库的字符串 或 是指定的Based.Dkey_</param>
        /// <param name="IsGoError">是否跳转到错误页面</param>
        public Connection(string DataKey, bool IsGoError) { this.IsGoError = IsGoError; this.Initialize(DataKey); }
        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <param name="DataKey">连接数据库的字符串 或 是指定的Based.Dkey_</param>
        private void Initialize(string DataKey)
        {

            //开始处理操作
            this.IsConn = true;
            this.Queries = 0;
            this.Connected = false;
            if (Base.IsNull(DataKey)) { this.Error("请联系管理员以便得到正确的数据库配置."); return; }
            if (DataKey.ToLower().IndexOf("database=") >= 0) { this.DataString = DataKey; return; }
            this.DataString = SqlServer.Get(DataKey);
            if (Base.IsNull(this.DataString)) { this.Error("请联系管理员以便得到正确的数据库配置[" + this.DataString + "]."); return; }
            if (this.DataString.ToLower().IndexOf("database=") < 0) { this.Error("请联系管理员以便得到正确的数据库配置[" + this.DataString + "]."); return; }
        }
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public void Open()
        {
            if (!this.Connected && this.IsConn)
            {
                try
                {
                    this.Conn = new SqlConnection(this.DataString); this.Conn.Open(); this.Connected = true;
                    this.Tran = this.IsSqlTransaction ? this.Conn.BeginTransaction() : null;
                }
                catch (Exception Ex) { this.IsConn = false; this.Error(Ex); }
            }
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            this.Connected = false;
            this.ClearSqlTransaction();
            if (this.Conn != null) { this.Conn.Close(); this.Conn.Dispose(); }
        }
        /// <summary>
        /// 清除事务
        /// </summary>
        private void ClearSqlTransaction() { if (this.Tran != null) { this.Tran.Dispose(); this.Tran = null; } }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit() { if (this.Tran != null) { this.Tran.Commit(); } }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback() { if (this.Tran != null) { this.Tran.Rollback(); } }
        /// <summary>
        /// 消息错误处理
        /// </summary>
        /// <param name="ErrorMsg">错误消息</param>
        private void Error(string ErrorMsg) { this.IsConn = false; Current.Error(this.IsGoError, ErrorNum.Rkey_SqlData.ToString(), "错误告警:\r\n异常路径:" + this.DataString + "\r\n" + ErrorMsg); }
        /// <summary>
        /// 消息错误处理
        /// </summary>
        /// <param name="Exceptions">Exception错误机制</param>
        private void Error(Exception Exceptions) { this.Error(Exceptions, ""); }
        /// <summary>
        /// 消息错误处理
        /// </summary>
        /// <param name="Exceptions">Exception错误机制</param>
        /// <param name="ProcName">存贮过程名</param>
        private void Error(Exception Exceptions, string ProcName)
        {
            this.IsConn = false;
            this.Rollback();//事务回滚
            this.Close();
            StringBuilder ErrorMsg = new StringBuilder("错误告警:\r\n");
            ErrorMsg.Append("异常参数:" + ProcName + "\r\n");
            ErrorMsg.Append("异常路径:" + this.DataString + "\r\n");
            ErrorMsg.Append("异常消息:" + Exceptions.Message);
            Current.Error(this.IsGoError, ErrorNum.Rkey_SqlData.ToString(), ErrorMsg.ToString());
        }
        /// <summary>
        /// 执行 SQL语句
        /// </summary>
        /// <param name="SqlString">SqlString SQL查询语句</param>
        public void Execute(string SqlString)
        {
            this.Executed = false; this.Open();
            if (!this.IsConn) { this.Error("请联系管理员以便得到正确的数据库配置."); return; }
            SqlCommand Command = new SqlCommand(SqlString, this.Conn);
            Command.CommandTimeout = this.TransactionTimeOut;
            if (this.Tran != null) Command.Transaction = this.Tran;
            try { this.Queries++; Command.ExecuteNonQuery(); this.Executed = true; }
            catch (Exception Ex) { this.Error(Ex, SqlString); }
            finally { Command.Dispose(); Command = null; }
        }
        /// <summary>
        /// 执行SQL过程
        /// </summary>
        /// <param name="ProcedureName">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        public void Execute(string ProcedureName, SqlParameter[] Parameters)
        {
            this.Executed = false; this.Open();
            if (!this.IsConn) { this.Error("请联系管理员以便得到正确的数据库配置."); return; }
            SqlCommand Command = new SqlCommand(ProcedureName, this.Conn);
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandTimeout = this.TransactionTimeOut;
            if (this.Tran != null) Command.Transaction = this.Tran;
            if (Parameters != null) Command.Parameters.AddRange(Parameters);
            try { this.Queries++; Command.ExecuteNonQuery(); this.Executed = true; }
            catch (Exception Ex) { this.Error(Ex, ProcedureName); }
            finally { Command.Dispose(); Command = null; }
        }
        /// <summary>
        /// 执行SQL过程并返回指定的一个整数值
        /// </summary>
        /// <param name="ProcedureName">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Arg">指定单个需要返回值的索引 无 请使用 -1</param>
        /// <returns>int</returns>
        public object Execute(string ProcedureName, SqlParameter[] Parameters, int Arg)
        {
            List<object> Values = this.Execute(ProcedureName, Parameters, new List<int>() { Arg });
            if (Values == null) return null;
            return Values[0];
        }
        /// <summary>
        /// 执行SQL过程 并返回所需要的参数列表值
        /// </summary>
        /// <param name="ProcedureName">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Args">具有返回值的 SqlParameter  对象的索引的 List&gt;int&lt;  数组对象</param>
        /// <returns>string[]</returns>
        public List<object> Execute(string ProcedureName, SqlParameter[] Parameters, List<int> Args)
        {
            List<object> Values = null;
            this.Execute(ProcedureName, Parameters);//计算参数
            int IArgs = Args == null ? 0 : Args.Count;
            if (IArgs > 0)
            {
                int ILen = Parameters.Length;
                Values = new List<object>();
                for (int i = 0; i < IArgs; i++)
                {
                    int Index = Args[i];
                    Values.Add((Index >= 0 && Index < ILen) ? Parameters[Index].Value : null);
                }
            }
            return Values;
        }
        /// <summary>
        /// 执行返回的 DataTable 的语句
        /// </summary>
        /// <param name="SqlString">SqlString SQL查询语句</param>
        /// <returns>执DataTable</returns>
        public DataTable GetDataTable(string SqlString)
        {
            DataTableCollection Dts = this.GetDataTables(SqlString);
            if (Dts == null) return null;
            return Dts[0];
        }

        /// <summary>
        /// 执行返回的 DataTable 的存贮过程
        /// </summary>
        /// <param name="ProcedureName">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string ProcedureName, SqlParameter[] Parameters)
        {
            int Count = 0;
            return this.GetDataTable(ProcedureName, Parameters, ref Count);
        }
        /// <summary>
        /// 执行返回的 DataTable 的存贮过程
        /// </summary>
        /// <param name="ProcedureName">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Count">返回当前 DataTable 的总记录数</param>
        /// <returns>执行返回的 DataTable 的语句</returns>
        public DataTable GetDataTable(string ProcedureName, SqlParameter[] Parameters, ref int Count)
        {
            List<object> Values = null;
            return this.GetDataTable(ProcedureName, Parameters, null, ref Count, ref Values);
        }
        /// <summary>
        /// 执行返回的 DataTable 的存贮过程
        /// </summary>
        /// <param name="ProcedureName">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Arg">索引</param>
        /// <param name="Value">返回值</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string ProcedureName, SqlParameter[] Parameters, int Arg, ref object Value)
        {
            List<object> Values = null; int Count = 0;
            DataTable Table = this.GetDataTable(ProcedureName, Parameters, new List<int>() { Arg }, ref Count, ref Values);
            if (Values != null) Value = Values[0];
            return Table;
        }
        /// <summary>
        /// 执行返回的 DataTable 的存贮过程
        /// </summary>
        /// <param name="ProcedureName">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Args">具有返回值的 SqlParameter  对象的索引的 List&gt;int&lt;  数组对象</param>
        /// <param name="Values">返回的 SqlParameter[]  对象的  List&gt;object&lt; </param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string ProcedureName, SqlParameter[] Parameters, List<int> Args, ref List<object> Values)
        {
            int Count = 0;
            return this.GetDataTable(ProcedureName, Parameters, Args, ref Count, ref Values);
        }
        /// <summary>
        /// 执行返回的 DataTable 的存贮过程
        /// </summary>
        /// <param name="ProcedureName">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Args">具有返回值的 SqlParameter  对象的索引的 List&gt;int&lt;  数组对象</param>
        /// <param name="Count">返回当前 DataTable 的总记录数</param>
        /// <param name="Values">返回的 SqlParameter[]  对象的  List&gt;object&lt; </param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string ProcedureName, SqlParameter[] Parameters, List<int> Args, ref int Count, ref List<object> Values)
        {
            DataTableCollection Tables = this.GetDataTables(ProcedureName, Parameters, Args, ref Values);
            if (Tables == null) return null;
            DataTable Table = Tables.Count > 0 ? Tables[0] : null;
            Count = Table == null ? 0 : Table.Rows.Count;
            return Table;
        }

        /// <summary>
        /// 执行返回的 DataTableCollection 的语句
        /// </summary>
        /// <param name="SqlString">SqlString SQL查询语句</param>
        /// <returns>执DataTable</returns>
        public DataTableCollection GetDataTables(string SqlString)
        {
            DataSet iDataSet = this.GetDataSet(SqlString);
            if (iDataSet == null) return null;
            return ((iDataSet.Tables.Count <= 0) ? null : iDataSet.Tables);
        }
        /// <summary>
        /// 执行返回的 DataTableCollection 的存贮过程
        /// </summary>
        /// <param name="ProcedureName">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <returns>DataTable[]</returns>
        public DataTableCollection GetDataTables(string ProcedureName, SqlParameter[] Parameters)
        {
            DataSet iDataSet = this.GetDataSet(ProcedureName, Parameters);
            if (iDataSet == null) return null;
            return ((iDataSet.Tables.Count <= 0) ? null : iDataSet.Tables);
        }

        /// <summary>
        /// 执行返回的 DataTableCollection 的存贮过程
        /// </summary>
        /// <param name="ProcedureName">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Arg">索引</param>
        /// <param name="Value">返回值</param>
        /// <returns>DataTable[]</returns>
        public DataTableCollection GetDataTables(string ProcedureName, SqlParameter[] Parameters, int Arg, ref  object Value)
        {
            DataSet iDataSet = this.GetDataSet(ProcedureName, Parameters, Arg, ref Value);
            if (iDataSet == null) return null;
            return ((iDataSet.Tables.Count <= 0) ? null : iDataSet.Tables);
        }
        /// <summary>
        /// 执行返回的 DataTableCollection 的存贮过程
        /// </summary>
        /// <param name="ProcedureName">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Args">具有返回值的 SqlParameter  对象的索引的 List&gt;int&lt;  数组对象</param>
        /// <param name="Values">返回的 SqlParameter[]  对象的  List&gt;object&lt; </param>
        /// <returns>DataTable[]</returns>
        public DataTableCollection GetDataTables(string ProcedureName, SqlParameter[] Parameters, List<int> Args, ref List<object> Values)
        {
            DataSet iDataSet = this.GetDataSet(ProcedureName, Parameters, Args, ref Values);
            if (iDataSet == null) return null;
            return ((iDataSet.Tables.Count <= 0) ? null : iDataSet.Tables);
        }
        /// <summary>
        /// 执行返回的 DataSet 的语句
        /// </summary>
        /// <param name="SqlString">SqlString SQL查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string SqlString)
        {
            this.Executed = false; this.Open();
            if (!this.IsConn) { this.Error("请联系管理员以便得到正确的数据库配置."); return null; }
            try
            {
                this.Queries++;
                SqlDataAdapter DataAdapter = new SqlDataAdapter();
                SqlCommand Command = new SqlCommand(SqlString, this.Conn);
                Command.CommandTimeout = this.TransactionTimeOut;
                if (this.Tran != null) Command.Transaction = this.Tran;
                DataAdapter.SelectCommand = Command;
                DataSet iDataSet = new DataSet();
                DataAdapter.Fill(iDataSet);
                this.Executed = true;
                return iDataSet;
            }
            catch (Exception Ex) { this.Error(Ex, SqlString); return null; }
        }
        /// <summary>
        /// /执行返回的 DataSet 的存贮过程
        /// </summary>
        /// <param name="ProcedureName">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string ProcedureName, SqlParameter[] Parameters)
        {
            List<object> Values = null;
            return this.GetDataSet(ProcedureName, Parameters, null, ref Values);
        }

        /// <summary>
        /// /执行返回的 DataSet 的存贮过程
        /// </summary>
        /// <param name="ProcedureName">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Args">具有返回值的 SqlParameter  对象的索引的 int对象</param>
        /// <param name="Value">返回的 SqlParameter[]  对象的  object </param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string ProcedureName, SqlParameter[] Parameters, int Arg, ref object Value)
        {
            List<object> Values = null;
            DataSet iDataSet = this.GetDataSet(ProcedureName, Parameters, new List<int>() { Arg }, ref Values);
            if (Values != null) Value = Values[0];
            return iDataSet;
        }
        /// <summary>
        /// /执行返回的 DataSet 的存贮过程
        /// </summary>
        /// <param name="ProcedureName">存贮过程名称</param>
        /// <param name="Parameters">SqlParameter[] 对象</param>
        /// <param name="Args">具有返回值的 SqlParameter  对象的索引的 List&gt;int&lt;  数组对象</param>
        /// <param name="Values">返回的 SqlParameter[]  对象的  List&gt;object&lt; </param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string ProcedureName, SqlParameter[] Parameters, List<int> Args, ref List<object> Values)
        {
            this.Executed = false;
            this.Open();
            if (!this.IsConn) { this.Error("请联系管理员以便得到正确的数据库配置."); return null; }
            try
            {
                this.Queries++;
                SqlDataAdapter DataAdapter = new SqlDataAdapter();
                SqlCommand Command = new SqlCommand(ProcedureName, this.Conn);
                Command.CommandTimeout = this.TransactionTimeOut;
                if (this.Tran != null) Command.Transaction = this.Tran;
                Command.CommandType = CommandType.StoredProcedure;
                if (Parameters != null) Command.Parameters.AddRange(Parameters);
                DataAdapter.SelectCommand = Command;
                DataSet iDataSet = new DataSet();
                DataAdapter.Fill(iDataSet);
                this.Executed = true;
                //计算参数
                int IArgs = Args == null ? 0 : Args.Count;
                if (IArgs > 0)
                {
                    int ILen = Parameters.Length;
                    Values = new List<object>();
                    for (int i = 0; i < IArgs; i++)
                    { int Index = Args[i]; Values.Add((Index >= 0 && Index < ILen) ? Parameters[Index].Value : null); }
                }
                return iDataSet;
            }
            catch (Exception Ex) { this.Error(Ex, ProcedureName); return null; }
        }
    }
}