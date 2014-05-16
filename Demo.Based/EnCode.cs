using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Demo.Based
{
    /// <summary>
    /// 提供自定义系统字符的加密和解密操作
    /// 支持指定和随机的加密方法
    /// 系统加密支持字符 [6位长度](支持 WEB.CONFIG 文件配置 节点名 Key.EnCodeString)
    /// JS获取验证码名称(支持 WEB.CONFIG 文件配置 节点名 Key.EnScript)
    /// </summary>
    public class EnCode
    {
        /// <summary>
        /// 设置初始Key
        /// 系统加密支持字符 [6位长度](支持 WEB.CONFIG 文件配置 节点名 Key.EnCodeString)
        /// </summary>
        public static string EnKey = Base.GetKeyValue("Key.EnCodeString", "BOMBZH");
        /// <summary>
        /// JS获取验证码名称(支持 WEB.CONFIG 文件配置 节点名 Key.EnScript)
        /// </summary>
        public static string Skey_EnScript = Base.GetKeyValue("Key.EnScript", "EnScript");
        /// <summary>
        /// 是否对还原Cooke或者Session进行加密
        /// </summary>
        public static bool IsEncodeCookie = Base.IsBool(Base.GetKeyValue("Key.IsEncodeCookie", "true"), true);
        /// <summary>
        /// 基础的加密Key
        /// </summary>
        private static string Skey_EnScriptKeys = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// 附加的加密Key
        /// </summary>
        private static string Skey_EnScriptKey = "abcde";
        /// <summary>
        /// 获取加密主键值
        /// </summary>
        /// <returns>string</returns>
        public static string GetKey()
        {
            Session.Remove(EnCode.Skey_EnScript);
            string text = EnCode.Skey_EnScriptKeys + Base.GetGuid(ERand.Letter, 5).Replace("z", "h");
            string text2 = "0";
            for (int i = 1; i < 41; i++)
            {
                int startIndex = new Random(Base.GetGuid(9)).Next(0, text2.Length + 1);
                text2 = text2.Insert(startIndex, text.Substring(i, 1));
            }
            Session.Set(EnCode.Skey_EnScript, text2);
            return text2;
        }
        /// <summary>
        /// 设置获取KEY函数
        /// </summary>
        /// <param name="EnStr">输入的随机字符</param>
        /// <param name="LeftKey">左字符</param>
        /// <param name="RightKey">右字符</param>
        /// <returns>string</returns>
        private static string GetKey(string EnStr, out string LeftKey, out string RightKey)
        {
            LeftKey = "";
            RightKey = "";
            if (!Base.IsNull(EnStr))
            {
                EnStr = EnStr.ToUpper();
                LeftKey = Base.Left(EnStr, 1);
                RightKey = Base.Right(EnStr, 1);
            }
            else
            {
                LeftKey = Base.GetGuid().ToUpper();
                RightKey = Base.Right(LeftKey, 1);
                LeftKey = Base.Left(LeftKey, 1);
            }
            return LeftKey + EnCode.EnKey + RightKey;
        }
        /// <summary>
        /// 加密方法 随机加密
        /// </summary>
        /// <param name="Value">要加密的字符</param>
        /// <returns></returns>
        public static string EnToCode(string Value)
        {
            return EnCode.EnToCode(Value, "");
        }
        /// <summary>
        /// 加密方法 指定加密
        /// </summary>
        /// <param name="Value">要加密的字符</param>
        /// <param name="CodeKey">指定的加密字符</param>
        /// <returns>string</returns>
        public static string EnToCode(string Value, string CodeKey)
        {
            string result;
            if (Base.IsNull(Value))
            {
                result = Value;
            }
            else
            {
                string str;
                string str2;
                string key = EnCode.GetKey(CodeKey, out str, out str2);
                try
                {
                    DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
                    byte[] bytes = Encoding.Default.GetBytes(Value);
                    dESCryptoServiceProvider.Key = Encoding.Default.GetBytes(key);
                    dESCryptoServiceProvider.IV = Encoding.Default.GetBytes(key);
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(bytes, 0, bytes.Length);
                    cryptoStream.FlushFinalBlock();
                    StringBuilder stringBuilder = new StringBuilder();
                    byte[] array = memoryStream.ToArray();
                    for (int i = 0; i < array.Length; i++)
                    {
                        byte b = array[i];
                        stringBuilder.AppendFormat("{0:X2}", b);
                    }
                    result = str + stringBuilder.ToString() + str2;
                }
                catch
                {
                    result = Value;
                }
            }
            return result;
        }
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="Value">要解密的字符</param>
        /// <returns>string</returns>
        public static string DeToCode(string Value)
        {
            string result;
            if (Base.IsNull(Value))
            {
                result = Value;
            }
            else
            {
                if (Value.Length <= 8)
                {
                    result = Value;
                }
                else
                {
                    string text;
                    string text2;
                    string key = EnCode.GetKey(Value, out text, out text2);
                    try
                    {
                        string text3 = Value.Substring(1, Value.Length - 2);
                        DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
                        byte[] array = new byte[text3.Length / 2];
                        for (int i = 0; i < text3.Length / 2; i++)
                        {
                            int num = Convert.ToInt32(text3.Substring(i * 2, 2), 16);
                            array[i] = (byte)num;
                        }
                        dESCryptoServiceProvider.Key = Encoding.Default.GetBytes(key);
                        dESCryptoServiceProvider.IV = Encoding.Default.GetBytes(key);
                        MemoryStream memoryStream = new MemoryStream();
                        CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
                        cryptoStream.Write(array, 0, array.Length);
                        cryptoStream.FlushFinalBlock();
                        StringBuilder stringBuilder = new StringBuilder();
                        result = Encoding.Default.GetString(memoryStream.ToArray());
                    }
                    catch
                    {
                        result = Value;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 外部对应JS加密的解密方法
        /// </summary>
        /// <param name="Value">要解密的字符</param>
        /// <param name="Key">加密的Key</param>
        /// <returns>string</returns>
        public static string DeToCodeForScript(string Value, string Key)
        {
            string text = "";
            string result;
            if (Base.IsNull(Value))
            {
                result = text;
            }
            else
            {
                bool flag = Value.Substring(0, 1) == "z";
                int num = flag ? ((Value.Length - 1) / 2) : (Value.Length / 3);
                int num2 = 0;
                int length = Key.Length;
                try
                {
                    for (int i = 0; i < num; i++)
                    {
                        int num3 = flag ? 0 : Key.IndexOf(Value.Substring(num2, 1));
                        num2++;
                        int num4 = Key.IndexOf(Value.Substring(num2, 1));
                        num2++;
                        int num5 = Key.IndexOf(Value.Substring(num2, 1));
                        if (!flag)
                        {
                            num2++;
                        }
                        int @int = num3 * 1681 + num4 * length + num5;
                        text += Base.IntToString(@int);
                    }
                }
                catch
                {
                }
                result = text;
            }
            return result;
        }
        /// <summary>
        /// 直接处理外部解密的方法
        /// </summary>
        /// <param name="HiddenName">提交的已加密的数据字段</param>
        /// <returns>string</returns>
        public static string DeToCodeForScript(string HiddenName)
        {
            string text = Current.Query(HiddenName);
            object obj = Session.Get(EnCode.Skey_EnScript);
            string key = (obj == null) ? (EnCode.Skey_EnScriptKeys + EnCode.Skey_EnScriptKey) : obj.ToString();
            Session.Remove(EnCode.Skey_EnScript);
            string result;
            if (Base.IsNull(text))
            {
                result = "";
            }
            else
            {
                result = EnCode.DeToCodeForScript(text, key);
            }
            return result;
        }
    }
}
