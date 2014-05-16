using System;
using System.Collections.Generic;

namespace Demo.Cached
{
    /// <summary>
    /// 远程缓存服务器IP列表结构体
    /// </summary>
    [Serializable]
    internal class TMemIPServer
    {
        /// <summary>
        /// 远程缓存服务器主键
        /// </summary>
        public string MKey;
        /// <summary>
        /// 远程缓存服务器IP列表
        /// </summary>
        public List<string> Items;
    }
}