using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Demo.Based;
using Demo.Error;

namespace Demo.Data
{
    /// <summary>
    /// ִ�����ݿ�������м��
    /// </summary>
    public class SqlExecute
    {
        #region ���ر����ӵķ���
        /// <summary>
        /// ֻ����һ�� int ���͵ô�������
        /// </summary>
        /// <param name="Conn">Connection����������ַ���</param>
        /// <param name="Procedure">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Args">���з���ֵ�� SqlParameter  ����������� int ����</param>
        /// <returns>���� int ����ֵ</returns>
        public static int Execute(Connection Conn, string Procedure, SqlParameter[] Parameters, int Arg)
        {
            List<object> Values = Conn.Execute(Procedure, Parameters, new List<int>() { Arg });
            if (Values == null) return ErrorNum.Rkey_SqlData;
            return Base.ToInt(Values[0], ErrorNum.Rkey_SqlData);
        }
        /// <summary>
        /// ����һ��DataTable ֧�� ���ص�������
        /// </summary>
        /// <param name="Conn">Connection ����������ַ���</param>
        /// <param name="Procedure">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <returns>DataTable ����</returns>
        public static DataTable GetTable(Connection Conn, string Procedure, SqlParameter[] Parameters)
        {
            DataTable Dt = Conn.GetDataTable(Procedure, Parameters);
            return Dt;
        }
        #endregion

        /// <summary>
        /// ִ��һ�����
        /// </summary>
        /// <param name="Conns">Connection����������ַ���</param>
        /// <param name="Sql">���</param>
        public static bool Execute(string Conns, string Sql)
        {
            Connection Conn = new Connection(Conns);
            Conn.Execute(Sql);
            bool rBool = Conn.Executed;
            Conn.Close();
            return rBool;
        }
        /// <summary>
        /// ֻ����һ�� int ���͵ô�������
        /// </summary>
        /// <param name="Conns">Connection����������ַ���</param>
        /// <param name="Procedure">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Args">���з���ֵ�� SqlParameter  ����������� int ����</param>
        /// <returns>���� int ����ֵ</returns>
        public static int Execute(string Conns, string Procedure, SqlParameter[] Parameters, int Arg)
        {
            Connection Conn = new Connection(Conns);
            List<object> Values = Conn.Execute(Procedure, Parameters, new List<int>() { Arg });
            Conn.Close();
            if (Values == null) return ErrorNum.Rkey_SqlData;
            return Base.ToInt(Values[0], ErrorNum.Rkey_SqlData);
        }
        /// <summary>
        /// ����һ�� int[]  ���͵ô�������
        /// </summary>
        /// <param name="Conns">Connection����������ַ���</param>
        /// <param name="Procedure">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Args">���з���ֵ�� SqlParameter  ����������� List&gt;int&lt;  �������</param>
        /// <returns>���� List&gt;object&lt; ����ֵ</returns>
        public static List<object> Execute(string Conns, string Procedure, SqlParameter[] Parameters, List<int> Args)
        {
            Connection Conn = new Connection(Conns);
            List<object> Values = Conn.Execute(Procedure, Parameters, Args);
            Conn.Close();
            return Values;
        }
        /// <summary>
        /// ����һ��DataTable ֧�� ���ص�������
        /// </summary>
        /// <param name="Conns">Connection ����������ַ���</param>
        /// <param name="Procedure">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <returns>DataTable ����</returns>
        public static DataTable GetTable(string Conns, string Procedure, SqlParameter[] Parameters)
        {
            Connection Conn = new Connection(Conns);
            DataTable Dt = Conn.GetDataTable(Procedure, Parameters);
            Conn.Close();
            return Dt;
        }
        /// <summary>
        /// ����һ��DataTable ֧�� ���ص�������
        /// </summary>
        /// <param name="Conns">Connection ����������ַ���</param>
        /// <param name="Procedure">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Arg">����</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>DataTable ����</returns>
        public static DataTable GetTable(string Conns, string Procedure, SqlParameter[] Parameters, int Arg, ref  object Value)
        {
            Connection Conn = new Connection(Conns);
            DataTable Table = Conn.GetDataTable(Procedure, Parameters, Arg, ref Value);// GetTable(Conns, Procedure, Parameters, Arg, ref Value);
            Conn.Close(); 
            return Table;
        }
        /// <summary>
        /// ����һ��DataTable ֧�� ��������
        /// </summary>
        /// <param name="Conns">Connection ����������ַ���</param>
        /// <param name="Procedure">������������</param>
        /// <param name="Parameters">SqlParameter[] ����</param>
        /// <param name="Args">���з���ֵ�� SqlParameter  ����������� List&gt;int&lt;  �������</param>
        /// <param name="Values">���ص� SqlParameter[]  �����  List&gt;object&lt; </param>
        /// <returns>DataTable ����</returns>
        public static DataTable GetTable(string Conns, string Procedure, SqlParameter[] Parameters, List<int> Args, ref List<object> Values)
        {
            Connection Conn = new Connection(Conns);
            DataTable Table = Conn.GetDataTable(Procedure, Parameters, Args, ref Values);
            Conn.Close();
            return Table;
        }

        /// <summary>
        /// ����һ��DataTable ֧�� ��������
        /// </summary>
        /// <param name="Conns">Connection ����������ַ���</param>
        /// <param name="Sql">Sql���</param>
        /// <returns>DataTable ����</returns>
        public static DataTable GetTable(string Conns, string Sql)
        {
            Connection Conn = new Connection(Conns);
            DataTable Dt = Conn.GetDataTable(Sql);
            Conn.Close();
            return Dt;
        }
        /// <summary>
        /// ����һ��DataTable[]����
        /// </summary>
        /// <param name="ProcName">������������</param>
        /// <param name="SqlParameters">SqlParameter[] ����</param>
        /// <returns>DataTableCollection ����</returns>
        public static DataTableCollection GetTables(string Conns, string Sql)
        {
            Connection Conn = new Connection(Conns);
            DataTableCollection Dtc = Conn.GetDataTables(Sql);
            Conn.Close();
            return Dtc;
        }
        /// <summary>
        /// ����һ��DataTable[]����
        /// </summary>
        /// <param name="ProcName">������������</param>
        /// <param name="SqlParameters">SqlParameter[] ����</param>
        /// <returns>DataTableCollection ����</returns>
        public static DataTableCollection GetTables(string ProcName, SqlParameter[] SqlParameters)
        {
            Connection Conn = new Connection();
            DataTableCollection Dtc = Conn.GetDataTables(ProcName, SqlParameters);
            Conn.Close();
            return Dtc;
        }
        /// <summary>
        /// ����һ��DataTable[]����
        /// </summary>
        /// <param name="ProcName">������������</param>
        /// <param name="SqlParameters">SqlParameter[] ����</param>
        /// <param name="Conn">Connection ����</param>
        /// <returns>DataTableCollection ����</returns>
        public static DataTableCollection GetTables(string ProcName, SqlParameter[] SqlParameters, out Connection Conn)
        {
            Conn = new Connection();
            return Conn.GetDataTables(ProcName, SqlParameters);
        }
        /// <summary>
        /// ����һ��DataTable[]����
        /// </summary>
        /// <param name="Conns">Connection ���������ַ���</param>
        /// <param name="ProcName">������������</param>
        /// <param name="SqlParameters">SqlParameter[] ����</param>
        /// <returns>DataTableCollection ����</returns>
        public static DataTableCollection GetTables(string Conns, string ProcName, SqlParameter[] SqlParameters)
        {
            Connection Conn = new Connection(Conns);
            DataTableCollection Dtc = Conn.GetDataTables(ProcName, SqlParameters);
            Conn.Close();
            return Dtc;
        }

        /// <summary>
        /// ����һ��DataTable[]����
        /// </summary>
        /// <param name="Conns">Connection ���������ַ���</param>
        /// <param name="ProcName">������������</param>
        /// <param name="SqlParameters">SqlParameter[] ����</param>
        /// <param name="Arg">����</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>DataTableCollection ����</returns>
        public static DataTableCollection GetTables(string Conns, string ProcName, SqlParameter[] SqlParameters, int Arg, ref  object Value)
        {
            Connection Conn = new Connection(Conns);
            DataTableCollection Dtc = Conn.GetDataTables(ProcName, SqlParameters, Arg, ref Value);
            Conn.Close();
            return Dtc;
        }
        /// <summary>
        /// ����һ��DataTable[]����
        /// </summary>
        /// <param name="Conns">Connection ���������ַ���</param>
        /// <param name="ProcName">������������</param>
        /// <param name="SqlParameters">SqlParameter[] ����</param>
        /// <param name="Args">���з���ֵ�� SqlParameter  ����������� List&gt;int&lt;  �������</param>
        /// <param name="Values">���ص� SqlParameter[]  �����  List&gt;object&lt; </param>
        /// <returns>DataTableCollection ����</returns>
        public static DataTableCollection GetTables(string Conns, string ProcName, SqlParameter[] SqlParameters, List<int> Args, ref List<object> Values)
        {
            Connection Conn = new Connection(Conns);
            DataTableCollection Dtc = Conn.GetDataTables(ProcName, SqlParameters, Args, ref Values);
            Conn.Close();
            return Dtc;
        }
    }
}