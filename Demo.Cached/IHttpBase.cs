using System;
using System.Collections.Generic;
using System.Data;
using Demo.Based;

namespace Demo.Cached
{
    /// <summary>
    /// 底层缓存Object操作基类
    /// 必须重写方法 Set_Initialize Set_LoadCache Close
    /// 子类中在Set_LoadCache方法中指定当前object
    /// </summary>
    public class IHttpBase
    {
        /// <summary>
        /// 保存系统中所有的缓存对象
        /// </summary>
        public static List<TLocked> Locked = new List<TLocked>();
        /// <summary>
        /// 是否含有当前缓存
        /// </summary>
        public bool IsHave;
        /// <summary>
        /// 真实的缓存Key
        /// </summary>
        protected string CACHEID;
        /// <summary>
        /// 是否为具有附加ID的单系列缓存
        /// </summary>
        protected bool IsAppend;
        /// <summary>
        /// 当前缓存存放的Memcached服务器主键
        /// 和Config/MemcachedFile.config中的Key对应
        /// </summary>
        protected string MEMCACHED;
        /// <summary>
        /// 当前缓存主键名
        /// </summary>
        protected string CACHED;
        /// <summary>
        /// 在更新缓存的时候,是否需要锁定对象,以保证线性安全
        /// 一般 在 DataTable 和 DataTable 转 TAttribute 对象的缓存 操作的时候 需要锁定对象
        /// </summary>
        public bool ISLOCK;
        /// <summary>
        /// 当前缓存的对应数据库主键名
        /// Miracle.Data.SqlServer 值关联,
        /// 也可以直接输入具体的数据库连接字符串
        /// </summary>
        protected string SQLDATA;
        /// <summary>
        /// 当前缓存的主键附属加载Sql语句
        /// 当前缓存的条件语法, 不含 WHERE 筛选语法
        /// 条件必须用{0}, {1},{2} .... 以此类推表示
        /// 例如: ID = {0} AND NAME ='{1}' AND OTHER LIKE '%{2}%'
        /// </summary>
        protected string ASQLSTATEMENT;
        /// <summary>
        /// 当前缓存的主键附属ID
        /// 提供一个缓存条件为多个单个缓存提供服务的时候产生的ID
        /// </summary>
        protected string APPENDID;
        /// <summary>
        /// 需要选择的列名集合
        /// </summary>
        protected string[] SQLCOLUMNS;
        /// <summary>
        /// 当前加载缓存的Sql语句
        /// </summary>
        protected string SQLSTATEMENT;
        /// <summary>
        /// 使用的缓存类型
        /// </summary>
        public EUseCached USECACHED;
        /// <summary>
        /// 当前缓存类型
        /// </summary>
        protected ECacheTyped CACHETYPE;
        /// <summary>
        /// 过期失效时间,0为不设置
        /// </summary>
        public int EXTIME;
        /// <summary>
        /// 当前对应数据值字段名
        /// </summary>
        protected string KEY_FIELD;
        /// <summary>
        /// 当前对应数据值字段名
        /// 在 CACHETYPE == ECacheTyped.Attribute | ECacheTyped.Attributed 有效
        /// </summary>
        protected string KEY_VALUE;
        /// <summary>
        /// 当前对应数据字段名
        /// 在 CACHETYPE == ECacheTyped.Table | ECacheTyped.Tabled 有效
        /// </summary>
        protected SqlDbType FIELDTYPE;
        /// <summary>
        /// 当使用 Attributed, Tabled 有效时,数据必须用字段 , 分割
        /// 以便在使用时候直接 Split 分割成相关数组
        /// </summary>
        protected string ARGUMENT;
        /// <summary>
        /// 获取当前指定的真实缓存Key的TLocker对象
        /// </summary>
        /// <param name="CACHEID">真实缓存Key</param>
        /// <returns>TLocker</returns>
        public static TLocked GetLocker(string CACHEID)
        {
            return IHttpBase.Locked.Find((TLocked _Locked) => _Locked.CACHEID == CACHEID);
        }
        public static List<TLocked> GetLockers(string CACHED)
        {
            return IHttpBase.Locked.FindAll((TLocked _Locked) => _Locked.CACHED == CACHED);
        }
        /// <summary>
        /// 底层缓存操作基类
        /// </summary>
        /// <param name="Cached">缓存TCached</param>
        public IHttpBase(TCached Cached)
        {
            this.Clone(Cached);
            this.Initialize();
        }
        /// <summary>
        /// 从参数克隆属性
        /// 因为 在内部的使用会造成TCached属性随应的更改
        /// </summary>
        /// <param name="ICached">TCached</param>
        private void Clone(TCached ICached)
        {
            if (ICached != null)
            {
                this.APPENDID = ICached.APPENDID;
                this.ARGUMENT = ICached.ARGUMENT;
                this.ASQLSTATEMENT = ICached.ASQLSTATEMENT;
                this.CACHED = ICached.CACHED;
                this.CACHEID = ICached.CACHED;
                this.USECACHED = ICached.USECACHED;
                this.CACHETYPE = ICached.CACHETYPE;
                this.EXTIME = ICached.EXTIME;
                this.FIELDTYPE = ICached.FIELDTYPE;
                this.ISLOCK = ICached.ISLOCK;
                this.KEY_FIELD = ICached.KEY_FIELD;
                this.KEY_VALUE = ICached.KEY_VALUE;
                this.MEMCACHED = ICached.MEMCACHED;
                this.SQLCOLUMNS = ICached.SQLCOLUMNS;
                this.SQLDATA = ICached.SQLDATA;
                this.SQLSTATEMENT = ICached.SQLSTATEMENT;
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            if (!Base.IsNull(this.CACHED))
            {
                this.IsAppend = this.InitAppend();
                if (this.IsAppend)
                {
                    this.CACHEID += this.APPENDID;
                }
                this.Set_Initialize();
                if (!this.IsHave)
                {
                    this.Set_LoadCache();
                }
            }
        }
        /// <summary>
        /// 判断是否为具有附加ID的单系列缓存
        /// </summary>
        /// <returns>bool</returns>
        protected bool InitAppend()
        {
            return this.SQLSTATEMENT.IndexOf("{WHERE}") >= 0 && !Base.IsNull(this.APPENDID) && !Base.IsNull(this.ASQLSTATEMENT) && !Base.IsNull(this.ARGUMENT);
        }
        /// <summary>
        /// 获取当前缓存
        /// </summary>
        /// <returns>object</returns>
        protected object Get_Cache()
        {
            return Cache.GetIntance(this.MEMCACHED, this.USECACHED).Get(this.CACHEID);
        }
        /// <summary>
        /// 记录缓存日志信息
        /// </summary>
        protected void Log()
        {
            Logs.CLog.WriteE("缓存对象 [" + this.CACHEID + "] 不存在!");
        }
        /// <summary>
        /// 需要在继承类中现实调用
        /// 以节约加载的初始化资源
        /// 格式化Sql语句
        /// </summary>
        protected void Set_SqlStatement()
        {
            string text = "";
            if (this.IsAppend)
            {
                text += " WHERE ";
                text += string.Format(this.ASQLSTATEMENT, this.ARGUMENT.Split(",".ToCharArray(), StringSplitOptions.None));
                text += " ";
            }
            this.SQLSTATEMENT = this.SQLSTATEMENT.Replace("{WHERE}", text);
        }
        /// <summary>
        /// 设置相关对象的原子锁
        /// </summary>
        protected void Set_Locker()
        {
            if (this.ISLOCK)
            {
                if (IHttpBase.GetLocker(this.CACHEID) == null)
                {
                    IHttpBase.Locked.Add(new TLocked
                    {
                        CACHED = this.CACHED,
                        CACHEID = this.CACHEID,
                        Locker = new object()
                    });
                }
            }
        }
        /// <summary>
        /// 提供虚拟的初始化操作
        /// 该操作必须在子类重写
        /// </summary>
        protected virtual void Set_Initialize()
        {
        }
        /// <summary>
        /// 提供虚拟的加载缓存操作
        /// 该操作必须在子类重写
        /// 必须在此方法中显式指定当前缓存对象
        /// </summary>
        protected virtual void Set_LoadCache()
        {
        }
        /// <summary>
        /// 释放资源操作
        /// </summary>
        public virtual void Close()
        {
        }
        /// <summary>
        /// 更新当前缓存中的某个值
        /// </summary>
        /// <param name="UAttribute">对象</param>
        public virtual void Update(TAttribute UAttribute)
        {
        }
        /// <summary>
        /// 保存当前缓存
        /// 统一提供IsHave属性的改变
        /// 子类中不需要额外定义
        /// </summary>
        /// <param name="Value">缓存</param>
        public void Save(object Value)
        {
            this.Save(Value, ECache.Elasticity);
        }
        /// <summary>
        /// 保存当前缓存
        /// 统一提供IsHave属性的改变
        /// 子类中不需要额外定义
        /// </summary>
        /// <param name="Value">缓存</param>
        /// <param name="eCahce">枚举</param>
        public void Save(object Value, ECache eCahce)
        {
            if (Value != null)
            {
                this.IsHave = true;
                if (this.EXTIME > 0)
                {
                    Cache.GetIntance(this.MEMCACHED, this.USECACHED).SetCache(this.CACHEID, Value, eCahce, this.EXTIME);
                }
                else
                {
                    Cache.GetIntance(this.MEMCACHED, this.USECACHED).SetCache(this.CACHEID, Value, eCahce);
                }
            }
        }
        /// <summary>
        /// 重载当前缓存
        /// </summary>
        public void ReLoad()
        {
            this.Set_LoadCache();
        }
    }
}
