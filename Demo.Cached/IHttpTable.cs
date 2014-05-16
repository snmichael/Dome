using System;
using System.Data;
using Demo.Based;
using Demo.Data;

namespace Demo.Cached
{
    /// <summary>
    /// 底层缓存DataTable操作基类
    /// 线性安全
    /// </summary>
    public class IHttpTable : IHttpBase
    {
        /// <summary>
        /// 原子锁
        /// </summary>
        private static object Locker = new object();
        /// <summary>
        /// 当前缓存中的DataTable对象
        /// </summary>
        public DataTable Table;
        /// <summary>
        /// 底层缓存操作基类
        /// </summary>
        /// <param name="Cached">缓存的参数结构体</param>
        public IHttpTable(TCached Cached)
            : base(Cached)
        {
        }
        /// <summary>
        /// 提供虚拟的初始化操作
        /// </summary>
        protected override void Set_Initialize()
        {
            this.Table = (DataTable)base.Get_Cache();
            this.IsHave = (this.Table != null);
        }
        /// <summary>
        /// 加载并保存数据源
        /// </summary>
        protected override void Set_LoadCache()
        {
            if (!Base.IsNull(this.CACHED))
            {
                base.Set_SqlStatement();
                lock (IHttpTable.Locker)
                {
                    base.Set_Locker();
                    this.Table = SqlExecute.GetTable(this.SQLDATA, this.SQLSTATEMENT);
                    base.Save(this.Table, ECache.Elasticity);
                }
            }
        }
        /// <summary>
        /// 释放资源操作
        /// </summary>
        public override void Close()
        {
            if (this.Table != null)
            {
                this.Table.Dispose();
                this.Table = null;
            }
            base.Close();
        }
        /// <summary>
        /// 更新当前缓存中的某个值
        /// </summary>
        /// <param name="tAttribute">对象</param>
        public override void Update(TAttribute tAttribute)
        {
            if (!Base.IsNull(this.CACHED))
            {
                this.Pv_Update(tAttribute, this.GetETypeFiled(tAttribute[this.KEY_FIELD, "0"]));
            }
        }
        /// <summary>
        /// 更新当前缓存中的某个值
        /// </summary>
        /// <param name="tAttribute">对象</param>
        /// <param name="Search">提供筛选的Sql语句</param>
        private void Pv_Update(TAttribute tAttribute, string Search)
        {
            if (!this.ISLOCK)
            {
                this.Pv_Update2(tAttribute, Search);
            }
            else
            {
                lock (IHttpBase.GetLocker(this.CACHEID).Locker)
                {
                    this.Pv_Update2(tAttribute, Search);
                }
            }
        }
        /// <summary>
        /// 更新当前缓存中的某个值
        /// </summary>
        /// <param name="tAttribute">对象</param>
        /// <param name="Search">提供筛选的Sql语句</param>
        private void Pv_Update2(TAttribute tAttribute, string Search)
        {
            ESqlAction eSqlAction = (ESqlAction)tAttribute["ACTION", -2];
            if (eSqlAction != ESqlAction.None)
            {
                if (!this.IsHave)
                {
                    base.Log();
                }
                else
                {
                    DataRow dataRow;
                    if (eSqlAction == ESqlAction.Add)
                    {
                        dataRow = this.Table.NewRow();
                    }
                    else
                    {
                        dataRow = this.GetRow(Search);
                    }
                    if (dataRow != null)
                    {
                        if (eSqlAction == ESqlAction.Delete)
                        {
                            this.Table.Rows.Remove(dataRow);
                            base.Save(this.Table);
                        }
                        else
                        {
                            this.Set_TAttributeToRow(tAttribute, ref dataRow);
                            if (eSqlAction == ESqlAction.Update)
                            {
                                base.Save(this.Table);
                            }
                            else
                            {
                                this.Table.Rows.Add(dataRow);
                                base.Save(this.Table);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 根据一定条件获取DataRow[]
        /// </summary>
        /// <param name="Searchs">条件</param>
        /// <returns>DataRow[]</returns>
        public DataRow[] GetRows(string Searchs)
        {
            return this.GetRows(Searchs, string.Empty);
        }
        /// <summary>
        /// 根据一定条件以及排序规则获取DataRow[]
        /// </summary>
        /// <param name="Searchs">条件</param>
        /// <param name="Sort">条件排序</param>
        /// <returns>DataRow[]</returns>
        public DataRow[] GetRows(string Searchs, string Sort)
        {
            return Base.GetRows(this.Table, Searchs, Sort);
        }
        /// <summary>
        /// 根据一定条件以及排序规则获取DataRow
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>DataRow</returns>
        public DataRow GetRow(int ID)
        {
            return this.GetRow("ID = " + ID.ToString());
        }
        /// <summary>
        /// 根据一定条件以及排序规则获取DataRow
        /// </summary>
        /// <param name="Searchs">条件</param>
        /// <returns>DataRow</returns>
        public DataRow GetRow(string Searchs)
        {
            return this.GetRow(Searchs, string.Empty);
        }
        /// <summary>
        /// 根据一定条件以及排序规则获取DataRow
        /// </summary>
        /// <param name="Searchs">条件</param>
        /// <param name="Sort">条件排序</param>
        /// <returns>DataRow</returns>
        public DataRow GetRow(string Searchs, string Sort)
        {
            return Base.GetRow(this.Table, Searchs, Sort);
        }
        /// <summary>
        /// 根据一定条件以及排序规则获取Object
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>TAttribute</returns>
        public TAttribute GetTAttribute(int ID)
        {
            return this.GetTAttributeBySearchKey(this.GetETypeFiled(ID));
        }
        /// <summary>
        /// 根据一定条件以及排序规则获取Object
        /// </summary>
        /// <param name="Key">条件</param>
        /// <returns>TAttribute</returns>
        public TAttribute GetTAttribute(string Key)
        {
            return this.GetTAttributeBySearchKey(this.GetETypeFiled(Key));
        }
        /// <summary>
        /// 根据一定条件以及排序规则获取Object
        /// </summary>
        /// <param name="Key">条件</param>
        /// <param name="Sort">条件排序</param>
        /// <returns>TAttribute</returns>
        public TAttribute GetTAttribute(string Key, string Sort)
        {
            return this.GetTAttributeBySearchKey(this.GetETypeFiled(Key), Sort);
        }
        /// <summary>
        /// 根据一定条件以及排序规则获取Object
        /// </summary>
        /// <param name="Searchs">条件</param>
        /// <returns>TAttribute</returns>
        public TAttribute GetTAttributeBySearchKey(string Searchs)
        {
            return this.GetTAttributeBySearchKey(Searchs, string.Empty);
        }
        /// <summary>
        /// 根据一定条件以及排序规则获取Object
        /// </summary>
        /// <param name="Searchs">条件</param>
        /// <param name="Sort">条件排序</param>
        /// <returns>TAttribute</returns>
        public TAttribute GetTAttributeBySearchKey(string Searchs, string Sort)
        {
            DataRow row = this.GetRow(Searchs, Sort);
            TAttribute result;
            if (row == null)
            {
                result = new TAttribute(this.SQLCOLUMNS);
            }
            else
            {
                result = this.Set_RowToTAttribute(row);
            }
            return result;
        }
        /// <summary>
        /// 获取当前的筛选搜索条件
        /// </summary>
        /// <param name="KeyValue">当前筛选的某个字段的值</param>
        /// <returns>string</returns>
        private string GetETypeFiled(object KeyValue)
        {
            string result;
            switch (this.FIELDTYPE)
            {
                case SqlDbType.Bit:
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                case SqlDbType.Int:
                case SqlDbType.Money:
                    result = this.KEY_FIELD + " = " + KeyValue.ToString();
                    return result;
            }
            result = this.KEY_FIELD + " = '" + KeyValue.ToString() + "'";
            return result;
        }
        /// <summary>
        /// 实现DataRow转TAttribute的方法
        /// </summary>
        /// <param name="Rs">DataRow</param>
        /// <returns>TAttribute</returns>
        private TAttribute Set_RowToTAttribute(DataRow Rs)
        {
            TAttribute tAttribute = new TAttribute(this.SQLCOLUMNS);
            int num = this.SQLCOLUMNS.Length;
            for (int i = 0; i < num; i++)
            {
                tAttribute.Set(this.SQLCOLUMNS[i], Rs[this.SQLCOLUMNS[i]]);
            }
            return tAttribute;
        }
        /// <summary>
        /// 实现TAttribute转DataRow的方法
        /// </summary>
        /// <param name="Attribute">TAttribute</param>
        /// <param name="Rs">DataRow</param>
        private void Set_TAttributeToRow(TAttribute Attribute, ref DataRow Rs)
        {
            int num = this.SQLCOLUMNS.Length;
            for (int i = 0; i < num; i++)
            {
                try
                {
                    Rs[this.SQLCOLUMNS[i]] = Attribute[this.SQLCOLUMNS[i], null, false];
                }
                catch
                {
                    Rs[this.SQLCOLUMNS[i]] = DBNull.Value;
                }
            }
        }
    }
}
