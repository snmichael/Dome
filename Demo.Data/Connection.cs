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
    /// ���ݿ�������� ֻ֧�� SqlServer ���ݿ�
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// ��������ʱ��ĳ�ʱ����
        /// </summary>
        private int TransactionTimeOut = 240;
        /// <summary>
        /// �Ƿ�ɹ��������ݿ�
        /// </summary>
        public bool IsConn = true;
        /// <summary>
        /// SQL �����ַ���
        /// </summary>
        public string DataString = "";
        /// <summary>
        ///�Ƿ�ɹ�ִ��ָ���Ĳ���
        /// </summary>
        public bool Executed = false;
        /// <summary>
        /// �Ƿ�����ݿ�
        /// </summary>
        public bool Connected = false;
        /// <summary>
        /// ��ѯ���ݿ����
        /// </summary>
        public int Queries = 0;
        /// <summary>
        /// SqlConnection����
        /// </summary>
        protected SqlConnection Conn = null;
        /// <summary>
        /// �Ƿ�֧����ת������ҳ��
        /// </summary>
        private bool IsGoError = false;
        /// <summary>
        /// �Ƿ�������ع�����
        /// </summary>
        public bool IsSqlTransaction = false;
        /// <summary>
        /// ���ݿ������ع�����
        /// </summary>
        private SqlTransaction Tran;
        /// <summary>
        /// �����µ�Sql���ݿ����Ӷ���
        /// ʹ������,������ʽָ�� IsSqlTransaction = true
        /// </summary>
        public Connection() { this.Initialize(Base.Data_Config); }
        /// <summary>
        /// �����µ�Sql���ݿ����Ӷ���
        /// ʹ������,������ʽָ�� IsSqlTransaction = true
        /// </summary>
        /// <param name="IsGoError">�Ƿ���ת������ҳ��</param>
        public Connection(bool IsGoError) { this.IsGoError = IsGoError; this.Initialize(Base.Data_Config); }
        /// <summary>
        /// �����µ�Sql���ݿ����Ӷ���
        /// ʹ������,������ʽָ�� IsSqlTransaction = true
        /// </summary>
        /// <param name="DataKey">�������ݿ���ַ��� �� ��ָ����Based.Dkey_</param>
        public Connection(string DataKey) { this.Initialize(DataKey); }
        /// <summary>
        /// �����µ�Sql���ݿ����Ӷ���
        /// ʹ������,������ʽָ�� IsSqlTransaction = true
        /// </summary>
        /// <param name="DataKey">�������ݿ���ַ��� �� ��ָ����Based.Dkey_</param>
        /// <param name="IsGoError">�Ƿ���ת������ҳ��</param>
        public Connection(string DataKey, bool IsGoError) { this.IsGoError = IsGoError; this.Initialize(DataKey); }
        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <param name="DataKey">�������ݿ���ַ��� �� ��ָ����Based.Dkey_</param>
        private void Initialize(string DataKey)
        {

            //��ʼ�������
            this.IsConn = true;
            this.Queries = 0;
            this.Connected = false;
            if (Base.IsNull(DataKey)) { this.Error("����ϵ����Ա�Ա�õ���ȷ�����ݿ�����."); return; }
            if (DataKey.ToLower().IndexOf("database=") >= 0) { this.DataString = DataKey; return; }
            this.DataString = SqlServer.Get(DataKey);
            if (Base.IsNull(this.DataString)) { this.Error("����ϵ����Ա�Ա�õ���ȷ�����ݿ�����[" + this.DataString + "]."); return; }
            if (this.DataString.ToLower().IndexOf("database=") < 0) { this.Error("����ϵ����Ա�Ա�õ���ȷ�����ݿ�����[" + this.DataString + "]."); return; }
        }
        /// <summary>
        /// �����ݿ�����
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
        /// �ر����ݿ�����
        /// </summary>
        public void Close()
        {
            this.Connected = false;
            this.ClearSqlTransaction();
            if (this.Conn != null) { this.Conn.Close(); this.Conn.Dispose(); }
        }
        /// <summary>
        /// �������
        /// </summary>
        private void ClearSqlTransaction() { if (this.Tran != null) { this.Tran.Dispose(); this.Tran = null; } }
        /// <summary>
        /// �ύ����
        /// </summary>
        public void Commit() { if (this.Tran != null) { this.Tran.Commit(); } }
        /// <summary>
        /// �ع�����
        /// </summary>
        public void Rollback() { if (this.Tran != null) { this.Tran.Rollback(); } }
        /// <summary>
        /// ��Ϣ������
        /// </summary>
        /// <param name="ErrorMsg">������Ϣ</param>
        private void Error(string ErrorMsg) { this.IsConn = false; Current.Error(this.IsGoError, ErrorNum.Rkey_SqlData.ToString(), "����澯:\r\n�쳣·��:" + this.DataString + "\r\n" + ErrorMsg); }
        /// <summary>
        /// ��Ϣ������
        /// </summary>
        /// <param name="Exceptions">Exception�������</param>
        private void Error(Exception Exceptions) { this.Error(Exceptions, ""); }
        /// <summary>
        /// ��Ϣ������
        /// </summary>
        /// <param name="Exceptions">Exception�������</param>
        /// <param name="ProcName">����������</param>
        private void Error(Exception Exceptions, string ProcName)
        {
            this.IsConn = false;
            this.Rollback();//����ع�
            this.Close();
            StringBuilder ErrorMsg = new StringBuilder("����澯:\r\n");
            ErrorMsg.Append("�쳣����:" + ProcName + "\r\n");
            ErrorMsg.Append("�쳣·��:" + this.DataString + "\r\n");
            ErrorMsg.Append("�쳣��Ϣ:" + Exceptions.Message);
            Current.Error(this.IsGoError, ErrorNum.Rkey_SqlData.ToString(), ErrorMsg.ToString());
        }
        /// <summary>
        /// ִ�� SQL���
        /// </summary>
        /// <param name="SqlString">SqlString SQL��ѯ���</param>
        public void Execute(string SqlString)
        {
            this.Executed = false; this.Open();
            if (!this.IsConn) { this.Error("����ϵ����Ա�Ա�õ���ȷ�����ݿ�����."); return; }
            SqlCommand Command = new SqlCommand(SqlString, this.Conn);
            Command.CommandTimeout = this.TransactionTimeOut;
            if (this.Tran != null) Command.Transaction = this.Tran;
            try { this.Queries++; Command.ExecuteNonQuery(); this.Executed = true; }
            catch (Exception Ex) { this.Error(Ex, SqlString); }
            finally { Command.Dispose(); Command = null; }
        }
        /// <summary>
        /// ִ��SQL����
        /// </summary>
        /// <param name="ProcedureName">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        public void Execute(string ProcedureName, SqlParameter[] Parameters)
        {
            this.Executed = false; this.Open();
            if (!this.IsConn) { this.Error("����ϵ����Ա�Ա�õ���ȷ�����ݿ�����."); return; }
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
        /// ִ��SQL���̲�����ָ����һ������ֵ
        /// </summary>
        /// <param name="ProcedureName">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Arg">ָ��������Ҫ����ֵ������ �� ��ʹ�� -1</param>
        /// <returns>int</returns>
        public object Execute(string ProcedureName, SqlParameter[] Parameters, int Arg)
        {
            List<object> Values = this.Execute(ProcedureName, Parameters, new List<int>() { Arg });
            if (Values == null) return null;
            return Values[0];
        }
        /// <summary>
        /// ִ��SQL���� ����������Ҫ�Ĳ����б�ֵ
        /// </summary>
        /// <param name="ProcedureName">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Args">���з���ֵ�� SqlParameter  ����������� List&gt;int&lt;  �������</param>
        /// <returns>string[]</returns>
        public List<object> Execute(string ProcedureName, SqlParameter[] Parameters, List<int> Args)
        {
            List<object> Values = null;
            this.Execute(ProcedureName, Parameters);//�������
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
        /// ִ�з��ص� DataTable �����
        /// </summary>
        /// <param name="SqlString">SqlString SQL��ѯ���</param>
        /// <returns>ִDataTable</returns>
        public DataTable GetDataTable(string SqlString)
        {
            DataTableCollection Dts = this.GetDataTables(SqlString);
            if (Dts == null) return null;
            return Dts[0];
        }

        /// <summary>
        /// ִ�з��ص� DataTable �Ĵ�������
        /// </summary>
        /// <param name="ProcedureName">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string ProcedureName, SqlParameter[] Parameters)
        {
            int Count = 0;
            return this.GetDataTable(ProcedureName, Parameters, ref Count);
        }
        /// <summary>
        /// ִ�з��ص� DataTable �Ĵ�������
        /// </summary>
        /// <param name="ProcedureName">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Count">���ص�ǰ DataTable ���ܼ�¼��</param>
        /// <returns>ִ�з��ص� DataTable �����</returns>
        public DataTable GetDataTable(string ProcedureName, SqlParameter[] Parameters, ref int Count)
        {
            List<object> Values = null;
            return this.GetDataTable(ProcedureName, Parameters, null, ref Count, ref Values);
        }
        /// <summary>
        /// ִ�з��ص� DataTable �Ĵ�������
        /// </summary>
        /// <param name="ProcedureName">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Arg">����</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string ProcedureName, SqlParameter[] Parameters, int Arg, ref object Value)
        {
            List<object> Values = null; int Count = 0;
            DataTable Table = this.GetDataTable(ProcedureName, Parameters, new List<int>() { Arg }, ref Count, ref Values);
            if (Values != null) Value = Values[0];
            return Table;
        }
        /// <summary>
        /// ִ�з��ص� DataTable �Ĵ�������
        /// </summary>
        /// <param name="ProcedureName">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Args">���з���ֵ�� SqlParameter  ����������� List&gt;int&lt;  �������</param>
        /// <param name="Values">���ص� SqlParameter[]  �����  List&gt;object&lt; </param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string ProcedureName, SqlParameter[] Parameters, List<int> Args, ref List<object> Values)
        {
            int Count = 0;
            return this.GetDataTable(ProcedureName, Parameters, Args, ref Count, ref Values);
        }
        /// <summary>
        /// ִ�з��ص� DataTable �Ĵ�������
        /// </summary>
        /// <param name="ProcedureName">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Args">���з���ֵ�� SqlParameter  ����������� List&gt;int&lt;  �������</param>
        /// <param name="Count">���ص�ǰ DataTable ���ܼ�¼��</param>
        /// <param name="Values">���ص� SqlParameter[]  �����  List&gt;object&lt; </param>
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
        /// ִ�з��ص� DataTableCollection �����
        /// </summary>
        /// <param name="SqlString">SqlString SQL��ѯ���</param>
        /// <returns>ִDataTable</returns>
        public DataTableCollection GetDataTables(string SqlString)
        {
            DataSet iDataSet = this.GetDataSet(SqlString);
            if (iDataSet == null) return null;
            return ((iDataSet.Tables.Count <= 0) ? null : iDataSet.Tables);
        }
        /// <summary>
        /// ִ�з��ص� DataTableCollection �Ĵ�������
        /// </summary>
        /// <param name="ProcedureName">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <returns>DataTable[]</returns>
        public DataTableCollection GetDataTables(string ProcedureName, SqlParameter[] Parameters)
        {
            DataSet iDataSet = this.GetDataSet(ProcedureName, Parameters);
            if (iDataSet == null) return null;
            return ((iDataSet.Tables.Count <= 0) ? null : iDataSet.Tables);
        }

        /// <summary>
        /// ִ�з��ص� DataTableCollection �Ĵ�������
        /// </summary>
        /// <param name="ProcedureName">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Arg">����</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>DataTable[]</returns>
        public DataTableCollection GetDataTables(string ProcedureName, SqlParameter[] Parameters, int Arg, ref  object Value)
        {
            DataSet iDataSet = this.GetDataSet(ProcedureName, Parameters, Arg, ref Value);
            if (iDataSet == null) return null;
            return ((iDataSet.Tables.Count <= 0) ? null : iDataSet.Tables);
        }
        /// <summary>
        /// ִ�з��ص� DataTableCollection �Ĵ�������
        /// </summary>
        /// <param name="ProcedureName">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Args">���з���ֵ�� SqlParameter  ����������� List&gt;int&lt;  �������</param>
        /// <param name="Values">���ص� SqlParameter[]  �����  List&gt;object&lt; </param>
        /// <returns>DataTable[]</returns>
        public DataTableCollection GetDataTables(string ProcedureName, SqlParameter[] Parameters, List<int> Args, ref List<object> Values)
        {
            DataSet iDataSet = this.GetDataSet(ProcedureName, Parameters, Args, ref Values);
            if (iDataSet == null) return null;
            return ((iDataSet.Tables.Count <= 0) ? null : iDataSet.Tables);
        }
        /// <summary>
        /// ִ�з��ص� DataSet �����
        /// </summary>
        /// <param name="SqlString">SqlString SQL��ѯ���</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string SqlString)
        {
            this.Executed = false; this.Open();
            if (!this.IsConn) { this.Error("����ϵ����Ա�Ա�õ���ȷ�����ݿ�����."); return null; }
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
        /// /ִ�з��ص� DataSet �Ĵ�������
        /// </summary>
        /// <param name="ProcedureName">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string ProcedureName, SqlParameter[] Parameters)
        {
            List<object> Values = null;
            return this.GetDataSet(ProcedureName, Parameters, null, ref Values);
        }

        /// <summary>
        /// /ִ�з��ص� DataSet �Ĵ�������
        /// </summary>
        /// <param name="ProcedureName">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Args">���з���ֵ�� SqlParameter  ����������� int����</param>
        /// <param name="Value">���ص� SqlParameter[]  �����  object </param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string ProcedureName, SqlParameter[] Parameters, int Arg, ref object Value)
        {
            List<object> Values = null;
            DataSet iDataSet = this.GetDataSet(ProcedureName, Parameters, new List<int>() { Arg }, ref Values);
            if (Values != null) Value = Values[0];
            return iDataSet;
        }
        /// <summary>
        /// /ִ�з��ص� DataSet �Ĵ�������
        /// </summary>
        /// <param name="ProcedureName">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Args">���з���ֵ�� SqlParameter  ����������� List&gt;int&lt;  �������</param>
        /// <param name="Values">���ص� SqlParameter[]  �����  List&gt;object&lt; </param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string ProcedureName, SqlParameter[] Parameters, List<int> Args, ref List<object> Values)
        {
            this.Executed = false;
            this.Open();
            if (!this.IsConn) { this.Error("����ϵ����Ա�Ա�õ���ȷ�����ݿ�����."); return null; }
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
                //�������
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