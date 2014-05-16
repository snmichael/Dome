using System;
using System.IO;
using System.Text;

namespace Demo.Based
{
    /// <summary>
    /// 日志操作
    /// </summary>
    public class ILogs
    {
        /// <summary>
        /// 所在日志的文件夹
        /// </summary>
        private string FileFolder;
        /// <summary>
        /// 日志操作
        /// </summary>
        /// <param name="FileFolder">所在日志的文件夹</param>
        public ILogs(string FileFolder)
        {
            this.FileFolder = FileFolder;
            this.Init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            this.FileFolder = Files.GetFolder(Files.ServerPath(this.FileFolder));
            Files.CreateFolder(this.FileFolder);
        }
        /// <summary>
        /// 返回日志记录的类型
        /// </summary>
        /// <param name="eLog">日志类型枚举类型</param>
        /// <returns>返回日志记录的类型</returns>
        private string GetELogs(ELog eLog)
        {
            string result;
            switch (eLog)
            {
                case ELog.Right:
                    result = "日志 : 正确";
                    break;
                case ELog.Waring:
                    result = "日志 : 警告";
                    break;
                case ELog.Error:
                    result = "日志 : 错误";
                    break;
                default:
                    result = "日志 : 未定义";
                    break;
            }
            return result;
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="Text">日志内容</param>
        /// <param name="eLog">日志枚举</param>
        public void Write(string Text, ELog eLog)
        {
            string path = this.FileFolder + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            StringBuilder stringBuilder = new StringBuilder("------------------------------------------------------------\r\n");
            stringBuilder.Append(string.Concat(new string[]
			{
				"--",
				this.GetELogs(eLog),
				"\r\n--时间 : ",
				DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"),
				"\r\n"
			}));
            stringBuilder.Append("--" + new StringBuilder(Text).Replace("\r\n", "\r\n--") + "\r\n");
            try
            {
                File.AppendAllText(path, stringBuilder.ToString(), Encoding.Default);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 记录正确的日志内容
        /// </summary>
        /// <param name="Text">日志内容</param>
        public void WriteR(string Text)
        {
            this.Write(Text, ELog.Right);
        }
        /// <summary>
        /// 记录报警的日志内容
        /// </summary>
        /// <param name="Text">日志内容</param>
        public void WriteW(string Text)
        {
            this.Write(Text, ELog.Waring);
        }
        /// <summary>
        /// 记录错误的日志内容
        /// </summary>
        /// <param name="Text">日志内容</param>
        public void WriteE(string Text)
        {
            this.Write(Text, ELog.Error);
        }
    }
}
