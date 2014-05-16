using System.Data;
using Demo.Based;
using Demo.Cached;

namespace Demo.Caching
{
    /// <summary>
    /// 获取当前数据中的缓存列表数据
    /// </summary>
    public class Cache
    {
        /// <summary>
        /// 通用的保存缓存对象的方法
        /// 包含新增,更新和删除
        /// 适用于 单个缓存 的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <param name="Attribute">TAttribute</param>
        public static void Update(string CKey, TAttribute Attribute)
        {
            Cache.Update(CKey, Attribute, "");
        }
        /// <summary>
        /// 通用的保存缓存对象的方法
        /// 包含新增,更新和删除
        /// 适用于 筛选条件等于单个键值的 单系列多缓存 的单个键值的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <param name="Attribute">TAttribute</param>
        /// <param name="AppendID">缓存主键附加键值</param>
        public static void Update(string CKey, TAttribute Attribute, string AppendID)
        {
            Cache.Update(CKey, Attribute, AppendID, AppendID);
        }
        /// <summary>
        /// 通用的保存缓存对象的方法
        /// 包含新增,更新和删除
        /// 适用于 多个筛选条件的 单系列多缓存 的单个键值的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <param name="Attribute">TAttribute</param>
        /// <param name="AppendID">缓存主键附加键值</param>
        /// <param name="Argument">需要格式化的参数集合 以 , 分割</param>
        public static void Update(string CKey, TAttribute Attribute, string AppendID, string Argument)
        {
            var eSqlAction = (ESqlAction)Attribute["ACTION", -2];
            if (eSqlAction != ESqlAction.None)
            {
                TCached tCached = null;
                if (Cache.Pv_GetCached(ref tCached, CKey, AppendID, Argument))
                {
                    IHttpBase httpBase;
                    if (tCached.CACHETYPE == ECacheTyped.Attribute)
                    {
                        httpBase = Cache.Pv_GetHttpObject(tCached);
                    }
                    else
                    {
                        httpBase = Cache.Pv_GetHttpTable(tCached);
                    }
                    httpBase.Update(Attribute);
                    httpBase.Close();
                }
            }
        }
        /// <summary>
        /// 重设指定缓存
        /// 适用于 单个缓存 的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        public static void ReLoad(string CKey)
        {
            Cache.ReLoad(CKey, "");
        }
        /// <summary>
        /// 重设指定缓存
        /// 适用于 筛选条件等于单个键值的 单系列多缓存 的单个键值的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <param name="AppendID">缓存主键附加键值</param>
        public static void ReLoad(string CKey, string AppendID)
        {
            Cache.ReLoad(CKey, AppendID, AppendID);
        }
        /// <summary>
        /// 重设指定缓存
        /// 适用于 多个筛选条件的 单系列多缓存 的单个键值的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <param name="AppendID">缓存主键附加键值</param>
        /// <param name="Argument">需要格式化的参数集合 以 , 分割</param>
        public static void ReLoad(string CKey, string AppendID, string Argument)
        {
            TCached tCached = null;
            if (Cache.Pv_GetCached(ref tCached, CKey, AppendID, Argument))
            {
                IHttpBase httpBase;
                if (tCached.CACHETYPE == ECacheTyped.Attribute)
                {
                    httpBase = new IHttpObject(tCached);
                }
                else
                {
                    httpBase = new IHttpTable(tCached);
                }
                httpBase.ReLoad();
                httpBase.Close();
            }
        }
        /// <summary>
        /// 通用的获取缓存TAttribute对象的方法
        /// 适用于 单个缓存 的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <returns>TAttribute</returns>
        public static TAttribute GetAttribute(string CKey)
        {
            return Cache.GetAttribute(CKey, "");
        }
        /// <summary>
        /// 通用的获取缓存TAttribute对象的方法
        /// 适用于 筛选条件等于单个键值的 单系列多缓存 的单个键值的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <param name="AppendID">缓存主键附属键值</param>
        /// <returns>TAttribute</returns>
        public static TAttribute GetAttribute(string CKey, string AppendID)
        {
            return Cache.GetAttribute(CKey, AppendID, AppendID);
        }
        /// <summary>
        /// 通用的获取缓存TAttribute对象的方法
        /// 适用于 多个筛选条件的 单系列多缓存 的单个键值的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <param name="AppendID">缓存主键附加键值</param>
        /// <param name="Argument">需要格式化的参数结果</param>
        /// <returns>TAttribute</returns>
        public static TAttribute GetAttribute(string CKey, string AppendID, string Argument)
        {
            TCached cached = null;
            TAttribute result;
            if (!Cache.Pv_GetCached(ref cached, CKey, AppendID, Argument))
            {
                result = null;
            }
            else
            {
                result = Cache.Pv_GetHttpObject(cached).Attribute;
            }
            return result;
        }
        /// <summary>
        /// 通用的获取缓存DataTable对象的方法
        /// 适用于 单个缓存 的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <param name="AppendID">缓存主键附加键值</param>
        /// <returns>DataTable</returns>
        public static DataTable GetTable(string CKey)
        {
            return Cache.GetTable(CKey, "");
        }
        /// <summary>
        /// 通用的获取缓存DataTable对象的方法
        /// 适用于 筛选条件等于单个键值的 单系列多缓存 的单个键值的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <param name="AppendID">缓存主键附加键值</param>
        /// <returns>DataTable</returns>
        public static DataTable GetTable(string CKey, string AppendID)
        {
            return Cache.GetTable(CKey, AppendID, AppendID);
        }
        /// <summary>
        /// 通用的获取缓存DataTable对象的方法
        /// 适用于 多个筛选条件的 单系列多缓存 的单个键值的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <param name="AppendID">缓存主键附加键值</param>
        /// <param name="Argument">需要格式化的参数结果</param>
        /// <returns>DataTable</returns>
        public static DataTable GetTable(string CKey, string AppendID, string Argument)
        {
            TCached cached = null;
            DataTable result;
            if (!Cache.Pv_GetCached(ref cached, CKey, AppendID, Argument))
            {
                result = null;
            }
            else
            {
                IHttpTable httpTable = Cache.Pv_GetHttpTable(cached);
                DataTable table = httpTable.Table;
                httpTable.Close();
                result = table;
            }
            return result;
        }
        /// <summary>
        /// 已过时 : 通用的获取缓存对象的方法
        /// 适用于 单个缓存 的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <returns>object</returns>
        public static object Get(string CKey)
        {
            return Cache.Get(CKey, "");
        }
        /// <summary>
        /// 已过时 : 通用的获取缓存对象的方法
        /// 适用于 筛选条件等于单个键值的 单系列多缓存 的单个键值的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <param name="AppendID">缓存主键附加键值</param>
        /// <returns>object</returns>
        public static object Get(string CKey, string AppendID)
        {
            return Cache.Get(CKey, AppendID, AppendID);
        }
        /// <summary>
        /// 已过时 : 通用的获取缓存对象的方法
        /// 适用于 多个筛选条件的 单系列多缓存 的单个键值的缓存
        /// </summary>
        /// <param name="CKey">缓存主键</param>
        /// <param name="AppendID">缓存主键附加键值</param>
        /// <param name="Argument">需要格式化的参数结果</param>
        /// <returns>object</returns>
        public static object Get(string CKey, string AppendID, string Argument)
        {
            TCached tCached = null;
            object result;
            if (!Cache.Pv_GetCached(ref tCached, CKey, AppendID, Argument))
            {
                result = null;
            }
            else
            {
                if (tCached.CACHETYPE == ECacheTyped.Attribute)
                {
                    result = Cache.Pv_GetHttpObject(tCached).Attribute;
                }
                else
                {
                    IHttpTable httpTable = Cache.Pv_GetHttpTable(tCached);
                    DataTable table = httpTable.Table;
                    httpTable.Close();
                    result = table;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取IHttpObject对象
        /// </summary>
        /// <param name="Cached">TCached</param>
        /// <returns>IHttpObject</returns>
        private static IHttpObject Pv_GetHttpObject(TCached Cached)
        {
            return new IHttpObject(Cached);
        }
        /// <summary>
        /// 获取IHttpTable对象
        /// </summary>
        /// <param name="Cached">TCached</param>
        /// <returns>IHttpTable</returns>
        private static IHttpTable Pv_GetHttpTable(TCached Cached)
        {
            return new IHttpTable(Cached);
        }
        /// <summary>
        /// 获取缓存
        /// 适用于 多个筛选条件的 单系列多缓存 的单个键值的缓存
        /// </summary>
        /// <param name="Cached">TCached</param>
        /// <param name="CKey">缓存主键</param>
        /// <param name="AppendID">缓存主键附加键值</param>
        /// <param name="Argument">需要格式化的参数集合 以 , 分割</param>
        /// <returns>bool</returns>
        private static bool Pv_GetCached(ref TCached Cached, string CKey, string AppendID, string Argument)
        {
            Cached = IHttpCached.Get(CKey);
            bool result;
            if (Cached == null)
            {
                result = false;
            }
            else
            {
                if (Cached.ID <= 0)
                {
                    result = false;
                }
                else
                {
                    Cached.APPENDID = AppendID;
                    Cached.ARGUMENT = Argument;
                    result = true;
                }
            }
            return result;
        }
    }
}
