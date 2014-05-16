using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Error
{
    /// <summary>
    /// 系统错误编码
    /// </summary>
    public class ErrorNum
    {
        /// <summary>
        /// 读取 SqlServer 文件错误
        /// </summary>
        public static int Rkey_SqlServer = -900001;
        /// <summary>
        /// 数据库出错
        /// </summary>
        public static int Rkey_SqlData = -900002;
        /// <summary>
        /// 重写地址出错
        /// </summary>
        public static int Rkey_ReWrite = -800001;
        /// <summary>
        /// 系统错误
        /// </summary>
        public static int Rkey_System = -700001;
        /// <summary>
        /// 验证码错误
        /// </summary>
        public static int Rkey_Verify = -600001;
    }
}
