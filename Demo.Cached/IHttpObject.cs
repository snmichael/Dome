using System.Data;
using Demo.Based;
using Demo.Data;

namespace Demo.Cached
{
    /// <summary>
    /// 底层缓存Object操作基类
    /// 线性安全
    /// </summary>
    public class IHttpObject : IHttpBase
    {
        /// <summary>
        /// 原子锁
        /// </summary>
        private static object Locker = new object();
        /// <summary>
        /// 数据对象
        /// </summary>
        public TAttribute Attribute;
        /// <summary>
        /// 底层缓存操作基类
        /// </summary>
        /// <param name="Cached">缓存TCached</param>
        /// <param name="MKey">远程缓存服务器列表主键</param>
        /// <param name="Args">提供外部调用接口的参数</param>
        public IHttpObject(TCached Cached)
            : base(Cached)
        {
        }
        /// <summary>
        /// 提供虚拟的初始化操作
        /// </summary>
        protected override void Set_Initialize()
        {
            this.Attribute = (TAttribute)base.Get_Cache();
            this.IsHave = (this.Attribute != null);
        }
        /// <summary>
        /// 加载并保存数据源
        /// </summary>
        protected override void Set_LoadCache()
        {
            if (!Base.IsNull(this.CACHED))
            {
                base.Set_SqlStatement();
                lock (IHttpObject.Locker)
                {
                    base.Set_Locker();
                    DataTable table = SqlExecute.GetTable(this.SQLDATA, this.SQLSTATEMENT);
                    if (table != null)
                    {
                        int count = table.Rows.Count;
                        this.Attribute = new TAttribute();
                        for (int i = 0; i < count; i++)
                        {
                            DataRow dataRow = table.Rows[i];
                            this.Attribute.Set(dataRow[this.KEY_FIELD].ToString(), dataRow[this.KEY_VALUE]);
                        }
                        table.Dispose();
                        base.Save(this.Attribute, ECache.Elasticity);
                    }
                }
            }
        }
        /// <summary>
        /// 更新当前缓存中的某个值
        /// </summary>
        /// <param name="tAttribute">对象</param>
        public override void Update(TAttribute tAttribute)
        {
            if (!Base.IsNull(this.CACHED))
            {
                if (!this.ISLOCK)
                {
                    this.Pv_Update(tAttribute);
                }
                else
                {
                    lock (IHttpBase.GetLocker(this.CACHEID).Locker)
                    {
                        this.Pv_Update(tAttribute);
                    }
                }
            }
        }
        /// <summary>
        /// 更新当前缓存中的某个值
        /// </summary>
        /// <param name="tAttribute">对象</param>
        private void Pv_Update(TAttribute tAttribute)
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
                    if (eSqlAction == ESqlAction.Delete)
                    {
                        this.Attribute.Remove(tAttribute[this.KEY_FIELD, ""]);
                    }
                    else
                    {
                        this.Attribute.Set(tAttribute[this.KEY_FIELD, ""], tAttribute[this.KEY_VALUE, null, true]);
                    }
                    base.Save(this.Attribute, ECache.Elasticity);
                }
            }
        }
    }
}
