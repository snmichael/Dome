using System;
using Demo.Based;

namespace Demo.Data
{
    public class DataBase
    {
        /// <summary>
        /// Ka8系统数据库库
        /// </summary>
        public static string KaSystem_Config = Base.GetKeyValue("DataForConfig", "DataForConfig");
    }
}