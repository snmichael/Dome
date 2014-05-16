using System.Web;

namespace Demo.Based
{
    /// <summary>
    /// 提供用户登陆的Session信息操作类
    /// </summary>
    public class Session
    {
        /// <summary>
        /// 获取指定的Keys
        /// </summary>
        /// <param name="Key">主键</param>
        /// <returns>指定的Keys</returns>
        private static string GetKeys(string Key)
        {
            return Base.Key_Cache + Key;
        }
        /// <summary>
        /// 设置Session信息
        /// </summary>
        /// <param name="Key">主键</param>
        /// <param name="Value">值</param>
        public static void Set(string Key, object Value)
        {
            HttpContext.Current.Session.Add(Session.GetKeys(Key), Value);
        }
        /// <summary>
        /// 获取Session信息
        /// </summary>
        /// <param name="Key">主键</param>
        /// <returns>值</returns>
        public static object Get(string Key)
        {
            return HttpContext.Current.Session[Session.GetKeys(Key)];
        }
        /// <summary>
        /// 移除Session信息
        /// </summary>
        /// <param name="Key">主键</param>
        public static void Remove(string Key)
        {
            if (Session.Get(Key) != null)
            {
                Session.RemoveKey(Key);
            }
        }
        /// <summary>
        /// 私有的删除Session对象
        /// </summary>
        /// <param name="Key">主键</param>
        private static void RemoveKey(string Key)
        {
            HttpContext.Current.Session.Remove(Session.GetKeys(Key));
        }
        /// <summary>
        /// 移除所有Session信息
        /// </summary>
        public static void RemoveAll()
        {
            HttpContext.Current.Session.RemoveAll();
        }
    }
}
