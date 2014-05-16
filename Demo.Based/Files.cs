using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace Demo.Based
{
    /// <summary>
    /// 提供 自定义的文件操作类
    /// </summary>
    public class Files
    {
        /// <summary>
        /// 判断是否属于本地窗体还是web结构
        /// </summary>
        public static bool IsAppForm = HttpContext.Current == null;
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>创建是否成功</returns>
        [DllImport("dbgHelp", SetLastError = true)]
        private static extern bool MakeSureDirectoryPathExists(string name);
        /// <summary>
        /// 获取服务器路径
        /// </summary>
        /// <param name="Path">原路径</param>
        /// <returns>系统路径 相对 或 绝对</returns>
        public static string ServerPath(string Path)
        {
            string result;
            if (Path.IndexOf(":\\") >= 0)
            {
                result = Path;
            }
            else
            {
                if (!Files.IsAppForm)
                {
                    result = HttpContext.Current.Server.MapPath(Path);
                }
                else
                {
                    Path = Path.Replace("~/", "").Replace("/", "\\");
                    result = Application.StartupPath + "\\" + Path;
                }
            }
            return result;
        }
        /// <summary>
        /// 检测文件是否存在
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns>bool 值</returns>
        public static bool CheckFile(string Path)
        {
            bool result;
            try
            {
                result = File.Exists(Files.ServerPath(Path));
            }
            catch
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 检测文件夹是否存在
        /// </summary>
        /// <param name="Path">文件夹路径</param>
        /// <returns>bool 值</returns>
        public static bool CheckFolder(string Path)
        {
            bool result;
            try
            {
                result = Directory.Exists(Files.ServerPath(Path));
            }
            catch
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 建立新文件
        /// 默认编码 GB2312
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Text">内容</param>
        /// <param name="Ovrrived">是否覆盖旧文件 如果文件存在</param>
        /// <returns>bool</returns>
        public static bool CreateFile(string Path, string Text, bool Ovrrived)
        {
            return Files.CreateFile(Path, Text, Ovrrived, "");
        }
        /// <summary>
        /// 建立新文件
        /// 默认编码 默认覆盖
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Text">内容</param>
        /// <returns>bool</returns>
        public static bool CreateFile(string Path, string Text)
        {
            return Files.CreateFile(Path, Text, true, "");
        }
        /// <summary>
        /// 建立新文件
        /// 默认覆盖
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Text">内容</param>
        /// <param name="Encode">文件编码</param>
        /// <returns>bool</returns>
        public static bool CreateFile(string Path, string Text, string Encode)
        {
            return Files.CreateFile(Path, Text, true, Encode);
        }
        /// <summary>
        /// 建立新文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Text">内容</param>
        /// <param name="Ovrrived">是否覆盖旧文件 如果文件存在</param>
        /// <param name="Encode">文件编码</param>
        /// <returns>bool</returns>
        public static bool CreateFile(string Path, string Text, bool Ovrrived, string Encode)
        {
            bool flag = Ovrrived || !Files.CheckFile(Path);
            bool result = true;
            if (flag)
            {
                FileStream fileStream = null;
                try
                {
                    fileStream = File.Create(Files.ServerPath(Path));
                    byte[] bytes = (Base.IsNull(Encode) ? Encoding.Default : Encoding.GetEncoding(Encode)).GetBytes(Text);
                    fileStream.Write(bytes, 0, bytes.Length);
                }
                catch
                {
                    result = false;
                    Logs.SLog.WriteE("创建文件 " + Path + " 失败!");
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 建立新文件夹
        /// </summary>
        /// <param name="Path">文件夹路径,完整路径</param>
        /// <returns>bool</returns>
        public static bool CreateFolder(string Path)
        {
            return Files.MakeSureDirectoryPathExists(Path);
        }
        /// <summary>
        /// 返回完整的目录路径 包含尾部 \ 或 / 符号
        /// </summary>
        /// <param name="Path">目录路径</param>
        /// <returns>完整的目录路径 </returns>
        public static string GetFolder(string Path)
        {
            string text = Files.IsAppForm ? "\\" : ((Path.IndexOf(":\\") >= 0) ? "\\" : "/");
            if (Base.Right(Path, 1) != text)
            {
                Path += text;
            }
            return Path;
        }
        /// <summary>
        /// 读取文件信息
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Ovrrived">是否覆盖旧文件 如果文件存在</param>
        /// <returns>返回已读取的信息</returns>
        public static string ReadFile(string Path, bool Ovrrived)
        {
            return Files.ReadFile(Path, Ovrrived, "");
        }
        /// <summary>
        /// 读取文件信息
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns>返回已读取的信息</returns>
        public static string ReadFile(string Path)
        {
            return Files.ReadFile(Path, true, "");
        }
        /// <summary>
        /// 读取文件信息
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Encode">文件编码</param>
        /// <returns>返回已读取的信息</returns>
        public static string ReadFile(string Path, string Encode)
        {
            return Files.ReadFile(Path, true, Encode);
        }
        /// <summary>
        /// 读取文件信息
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Ovrrived">是否覆盖旧文件 如果文件存在</param>
        /// <param name="Encode">文件编码</param>
        /// <returns>返回已读取的信息</returns>
        public static string ReadFile(string Path, bool Ovrrived, string Encode)
        {
            string result;
            if (!Files.CheckFile(Path))
            {
                Files.CreateFile(Path, "", Ovrrived);
                result = "";
            }
            else
            {
                StreamReader streamReader = null;
                string text = "";
                try
                {
                    streamReader = new StreamReader(Files.ServerPath(Path), Base.IsNull(Encode) ? Encoding.Default : Encoding.GetEncoding(Encode));
                    text = streamReader.ReadToEnd();
                }
                catch
                {
                    text = "";
                }
                finally
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                }
                result = text;
            }
            return result;
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns>bool</returns>
        public static bool DelFile(string Path)
        {
            bool result;
            if (!Files.CheckFile(Path))
            {
                result = false;
            }
            else
            {
                try
                {
                    File.Delete(Files.ServerPath(Path));
                    result = true;
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="Path">文件夹路径</param>
        /// <returns>bool</returns>
        public bool DelFolder(string Path)
        {
            bool result;
            if (!Files.CheckFolder(Path))
            {
                result = false;
            }
            else
            {
                try
                {
                    Directory.Delete(Files.ServerPath(Path), true);
                    result = true;
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
