using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Based;

namespace Demo.Cached
{
    /// <summary>
    /// 缓存操作类
    /// </summary>
    public class Cache
    {
        /// <summary>
        /// 当前系统使用的缓存类型
        /// </summary>
        private static EUseCached EUCached = EUseCached.System;
        /// <summary>
        /// 系统默认缓存
        /// </summary>
        private static ICSystem _ICSystem = new ICSystem();
        /// <summary>
        /// 根据缓存服务器地址主键 获取系统缓存 指向对象
        /// </summary>
        /// <param name="Key">缓存服务器地址主键</param>
        /// <returns>ICache</returns>
        public static ICache GetIntance(string Key, EUseCached UCached)
        {
            ICache result;
            if (Base.IsNull(Key))
            {
                result = Cache._ICSystem;
            }
            else
            {
                if (UCached != EUseCached.Memcached)
                {
                    result = Cache._ICSystem;
                }
                else
                {
                    result = ICMemcached.GetIntance(Key, Cache._ICSystem);
                }
            }
            return result;
        }
    }
}
