using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Based
{
    /// <summary>
    /// 随机数产生的枚举
    /// </summary>
    public enum ERand
    {
        /// <summary>
        /// 数字
        /// </summary>
        Numeric,
        /// <summary>
        /// 字母
        /// </summary>
        Letter,
        /// <summary>
        /// 数字字母混合
        /// </summary>
        Blend,
        /// <summary>
        /// 汉字
        /// </summary>
        Chinese
    }


    /// <summary>
    /// 数据操作枚举
    /// </summary>
    public enum ESqlAction
    {
        /// <summary>
        /// 更新数据操作
        /// </summary>
        Update = 1,
        /// <summary>
        /// 新增数据操作
        /// </summary>
        Add = 0,
        /// <summary>
        /// 删除数据操作
        /// </summary>
        Delete = -1,
        /// <summary>
        /// 无操作
        /// </summary>
        None = -2
    }

    public enum ECache
    {
        /// <summary>
        /// 弹性缓存
        /// </summary>
        Elasticity = 1,
        /// <summary>
        /// 绝对缓存
        /// </summary>
        Absolutely = 0
    }

    /// <summary>
    /// 日志枚举类型
    /// </summary>
    public enum ELog
    {
        /// <summary>
        /// 正确的操作信息(#标识) 
        /// </summary>
        Right = 1,
        /// <summary>
        /// 带有警告操作的信息(!标识) 
        /// </summary>
        Waring,
        /// <summary>
        /// 错误的操作信息(~标识) 
        /// </summary>
        Error,
        /// <summary>
        /// 未定义的操作信息(^标识) 
        /// </summary>
        Deflault = 0
    }


}
