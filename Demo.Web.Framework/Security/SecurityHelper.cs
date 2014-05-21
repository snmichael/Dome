using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Demo.Framework.Core.Security
{
	/// <summary>
	/// 安全帮助类
	/// </summary>
	public sealed class SecurityHelper
	{
		private SecurityHelper()
        {
		}

		#region Base64
		/// <summary>
		/// 对数据进行Base64编码
		/// </summary>
		/// <param name="data">数据</param>
		/// <returns>Base64编码数据</returns>
		public static string EncodeBase64(string data)
		{
			try
			{
				UnicodeEncoding ByteConverter = new UnicodeEncoding();
				byte[] bytes = ByteConverter.GetBytes(data);
				return Convert.ToBase64String(bytes);
			}
			catch
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// 对数据进行Base64解码
		/// </summary>
		/// <param name="data">数据</param>
		/// <returns>Base64解码数据</returns>
		public static string DecodeBase64(string data)
		{
			try
			{
				UnicodeEncoding ByteConverter = new UnicodeEncoding();
				byte[] bytes = Convert.FromBase64String(data);
				return ByteConverter.GetString(bytes);
			}
			catch
			{
				return string.Empty;
			}
		}
		#endregion

		#region MD5
		/// <summary>
		/// MD5加密
		/// </summary>
		/// <param name="source">源串</param>
		/// <returns>密串</returns>
		public static string MD5(string source)
		{
			return MD5(source, string.Empty);
		}

		/// <summary>
		/// MD5加密
		/// </summary>
		/// <param name="source">源串</param>
		/// <param name="key">密钥</param>
		/// <returns>密串</returns>
		public static string MD5(string source, string key)
		{
			Byte[] data1ToHash = (new UnicodeEncoding()).GetBytes(string.Concat(source, key));
			Byte[] hashvalue1 = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(data1ToHash);
			string md5 = BitConverter.ToString(hashvalue1).Replace("-", "").ToLower(CultureInfo.InvariantCulture);
			return md5;
		}
		#endregion
	}
}
