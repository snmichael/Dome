using Enyim.Caching;

namespace Demo.Cached
{
    /// <summary>
    /// 远程缓存的结构体
    /// </summary>
    internal class TMemClient
    {
        /// <summary>
        /// 缓存主键
        /// </summary>
        public string MKey;
        /// <summary>
        /// 远程缓存服务器对象
        /// </summary>
        public MemcachedClient Client;
    }
}