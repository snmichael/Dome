namespace Demo.Based
{
    /// <summary>
    /// 记录日志的操作类
    /// </summary>
    public class Logs
    {
        /// <summary>
        /// 缓存类日志目录
        /// </summary>
        public static readonly string FLOG_CACHE = "~/LOG_CACHE";
        /// <summary>
        /// 系统类日志目录
        /// </summary>
        public static readonly string FLOG_SYSTEM = "~/LOG_SYSTEM";
        /// <summary>
        /// 缓存类操作日志
        /// </summary>
        public static ILogs CLog = new ILogs(Logs.FLOG_CACHE);
        /// <summary>
        /// 系统类操作日志
        /// </summary>
        public static ILogs SLog = new ILogs(Logs.FLOG_SYSTEM);
        /// <summary>
        /// 其他日志的记录方式
        /// </summary>
        /// <param name="Folder">日志放置的路径</param>
        /// <returns>ILogs</returns>
        public static ILogs Log(string Folder)
        {
            return new ILogs(Folder);
        }
    }
}
