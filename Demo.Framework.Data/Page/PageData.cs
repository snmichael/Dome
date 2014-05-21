using System;
using System.Data;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Demo.Framework.Data.Page
{
    public class PageData : DataAccessBase,IPaging
    {
       
        //表名
        private string _tableName;
        //唯一列名，可做为索引列
        private string _uniqueColumnName;
        //列名数组
        private string[] _columnNames;
        //条件
        private string _strWhere;
        //排序
        private string _sort;
        //当前页
        private int _pageIndex;
        //总页数
        private int _pageCount;
        //总条数
        private int _count;
        //每页显示的条数
        private int _pagesize;
        //是否执行自定义SQL语句
        private bool _state = false;
        //SQL语句
        private string _sql;

        /// <summary>
        /// 输出页码
        /// </summary>
        /// <param name="ClassName">样式名</param>
        /// <param name="divId">divID</param>
        /// <param name="jsmethodName">Js方法名 默认：ReturnView</param>
        /// <param name="controlActionName">控制器名</param>
        /// <returns></returns>
        public string PrintPage(string ClassName,string divId, string jsmethodName, string controlActionName)
        {
            var pag = new Page();
            pag.ClassName = ClassName;
            pag.DivID = divId;
            return pag.PrintPage(_pageIndex, _pageCount, _count, jsmethodName, controlActionName, true);
        }
        /// <summary>
        /// 设置表名
        /// </summary>
        public string TableName
        {
            set { _tableName = value; }
        }

        /// <summary>
        /// 设置唯一列名，可做为索引列
        /// </summary>
        public string UniqueColumnName
        {
            set { _uniqueColumnName = value; }
        }

        /// <summary>
        /// 设置列名数组
        /// </summary>
        public string[] ColumnNames
        {
            set { _columnNames = value; }
        }

        /// <summary>
        /// 设置条件
        /// </summary>
        public string StrWhere
        {
            set { _strWhere = value; }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public string Sort
        {
            set { _sort = value; }
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            set { _pageIndex = value; }
            get { return _pageIndex; }
        }

        /// <summary>
        /// 获取总页数
        /// </summary>
        public int PageCount
        {
            get { return _pageCount; }
        }

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize
        {
            set { _pagesize = value; }
            get { return _pagesize; }
        }

        /// <summary>
        /// 获取条数
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// 设置Sql语句
        /// </summary>
        public string Sql
        {
            set { _sql = value; }
        }

        /// <summary>
        /// 设置State
        /// </summary>
        public bool State
        {
            set { _state = value; }
        }

        #region IPaging 成员

        /// <summary>
        /// 返回DataSet数据源
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataSet()
        {
            if (_state)
            {
                if (_sql != null && _sql != "")
                {
                    string tempsql = string.Format("select * from (select temp.*,row_number() over(order by {0}) as rownum"
                                                                + " from ({1}) as temp) as tt where rownum between {2} and {3}"
                                                                , _sort, _sql, _pagesize * _pageIndex + 1, EndIndex());
                    
                    
                    //DataSet dt = SqlHelper.ExecuteDataset(SqlHelper._connectionString, CommandType.Text, tempsql);
                    var command = CurrentDatabase.GetSqlStringCommand(tempsql);
                    DataSet dt = CurrentDatabase.ExecuteDataSet(command);
                    _count =
                        Convert.ToInt32(CurrentDatabase.ExecuteScalar(CommandType.Text,
                            string.Format("SELECT COUNT(1) FROM ( {0} ) as Temp", _sql)));
                   
                    _pageCount = _count % PageSize == 0 ? _count / _pagesize : Count / PageSize + 1;
                    return dt;
                }
            }
            else
            {
                string sql = GetSql();
                if (sql != "")
                {
                    _count = Convert.ToInt32(CurrentDatabase.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM {0} {1}", _tableName, GetStrWhere())));
                    //_count = int.Parse(DBUtility.DbHelperSQL.GetSingle(string.Format("SELECT COUNT(*) FROM {0} {1}",
                    //                                    _tableName, GetStrWhere())).ToString());
                    _pageCount = _count % PageSize == 0 ? _count / _pagesize : Count / PageSize + 1;
                    if (_pageIndex < 0)
                    {
                        _pageIndex = 0;
                    }
                    else if (_pageIndex > _pageCount)
                    {
                        _pageIndex = _pageCount;
                    }
                    //DataSet dt = SqlHelper.ExecuteDataset(SqlHelper._connectionString, CommandType.Text, sql);
                    //DataSet dt = DBUtility.DbHelperSQL.Query(sql);
                    var command = CurrentDatabase.GetSqlStringCommand(sql);
                    DataSet dt = CurrentDatabase.ExecuteDataSet(command);
                    return dt;
                }
            }
            return null;

        }

        /// <summary>
        /// 返回DataTable数据源
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTable()
        {
            DataSet dt = GetDataSet();
            if (dt != null)
            {
                return dt.Tables[0];
            }
            return null;
        }

        #endregion

        private string GetSql()
        {
            StringBuilder sSQL = new StringBuilder();

            sSQL.AppendFormat("select * from("
                            + "select {0},row_number() over(order by {2}) as rownum from {1} {5}"
                            + ") as Temp "
                            + "where rownum between {3} and {4}",
                        GetColumns(),
                        _tableName,
                        _sort,
                        _pagesize * _pageIndex + 1,
                        EndIndex(),
                        GetStrWhere());

            return sSQL.ToString();
        }

        /// <summary>
        /// 获取数据的起点
        /// </summary>
        /// <returns></returns>
        private int StartIndex()
        {
            if (_pageIndex < 0)
            {
                _pageIndex = 0;
            }
            if (_pagesize < 1)
            {
                _pagesize = 20;
            }
            return _pageIndex * _pagesize;
        }

        /// <summary>
        /// 获取数据的结束点
        /// </summary>
        /// <returns></returns>
        private int EndIndex()
        {
            if (_pageIndex < 0)
            {
                _pageIndex = 0;
            }
            if (_pagesize < 1)
            {
                _pagesize = 20;
            }
            return (_pageIndex + 1) * _pagesize;
        }

        /// <summary>
        /// 获取列名组成的一串字符串
        /// </summary>
        /// <returns></returns>
        private string GetColumns()
        {
            StringBuilder sb = new StringBuilder();
            if (_columnNames == null || _columnNames.Length <= 0)
            {
                sb.AppendFormat("*");
            }
            else
            {
                sb.AppendFormat("{0}", _columnNames[0]);
                for (int i = 1; i < _columnNames.Length; i++)
                {
                    sb.AppendFormat(",{0}", _columnNames[i]);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取条件
        /// </summary>
        /// <returns></returns>
        private string GetStrWhere()
        {
            if (_strWhere == null || _strWhere.Trim() == "")
            {
                return "";
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(_strWhere, "^order by", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                return _strWhere;
            }
            return string.Format(" where {0}", System.Text.RegularExpressions.Regex.Replace(_strWhere, "Where", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase));
        }

        protected override Database CurrentDatabase
        {
            get { return _database ?? (_database = new SqlDatabase(DbConnection.CrmDb.ConnectionString)); }
        }


    }
}
