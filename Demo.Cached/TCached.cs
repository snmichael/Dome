using System.Data;

namespace Demo.Cached
{
    /// <summary>
    /// 统一的缓存数据结构体
    /// 若此处发生变化,
    /// 请修改IHttpBase的对应的克隆的属性
    /// </summary>
    public class TCached
    {
        /// <summary>
        /// 对应数据库ID
        /// </summary>
        public int ID;
        /// <summary>
        /// 当前缓存存放的Memcached服务器主键
        /// 和Config/MemcachedFile.config中的Key对应
        /// </summary>
        public string MEMCACHED;
        /// <summary>
        /// 当前缓存主键名
        /// </summary>
        public string CACHED;
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
        public string SQLDATA;
        /// <summary>
        /// 当前缓存的主键附属加载Sql语句
        /// 当前缓存的条件语法, 不含 WHERE 筛选语法
        /// 条件必须用{0}, {1},{2} .... 以此类推表示
        /// 例如: ID = {0} AND NAME ='{1}' AND OTHER LIKE '%{2}%'
        /// </summary>
        public string ASQLSTATEMENT;
        /// <summary>
        /// 当前缓存的主键附属ID
        /// 提供一个缓存条件为多个单个缓存提供服务的时候产生的ID
        /// </summary>
        public string APPENDID;
        /// <summary>
        /// 需要选择的列名集合
        /// </summary>
        public string[] SQLCOLUMNS;
        /// <summary>
        /// 当前加载缓存的Sql语句
        /// </summary>
        public string SQLSTATEMENT;
        /// <summary>
        /// 使用的缓存类型
        /// </summary>
        public EUseCached USECACHED;
        /// <summary>
        /// 当前缓存类型
        /// </summary>
        public ECacheTyped CACHETYPE;
        /// <summary>
        /// 过期失效时间,0为不设置
        /// </summary>
        public int EXTIME;
        /// <summary>
        /// 当前对应数据值字段名
        /// </summary>
        public string KEY_FIELD;
        /// <summary>
        /// 当前对应数据值字段名
        /// 在 CACHETYPE == ECacheTyped.Attribute | ECacheTyped.Attributed 有效
        /// </summary>
        public string KEY_VALUE;
        /// <summary>
        /// 当前对应数据字段名
        /// 在 CACHETYPE == ECacheTyped.Table | ECacheTyped.Tabled 有效
        /// </summary>
        public SqlDbType FIELDTYPE;
        /// <summary>
        /// 当使用 Attributed, Tabled 有效时,数据必须用字段 , 分割
        /// 以便在使用时候直接 Split 分割成相关数组
        /// </summary>
        public string ARGUMENT;
    }
}
