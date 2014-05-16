namespace Demo.Cached
{
    /// <summary>
    /// 记录保存系统中所有的缓存对象的结构体
    /// </summary>
    public class TLocked
    {
        /// <summary>
        /// 当前缓存主键名
        /// 和数据库中的  CACHED  对应
        /// </summary>
        public string CACHED;
        /// <summary>
        /// 当前缓存的真实的CacheName
        /// </summary>
        public string CACHEID;
        /// <summary>
        /// 当前缓存的原子锁
        /// </summary>
        public object Locker;
    }
}
