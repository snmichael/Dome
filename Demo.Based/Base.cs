using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Demo.Based
{

    public class Base
    {
        /// <summary>
        /// 实例化后的静态缓存过期时间
        /// 默认30秒
        /// </summary>
        public static int Second_Cache = Base.ToInt(Base.GetKeyValue("SecondForCache", "30"), 30);
        /// <summary>
        /// 数据系统配置库
        /// </summary>
        public static string Data_Config = Base.GetKeyValue("DataForConfig", "DataForConfig");
        /// <summary>
        /// 缓存的主键值
        /// </summary>
        public static string Key_Cache = Base.GetKeyValue("Mc.Cache", "Mc.Cache.");
        /// <summary>
        /// 随机码主键值
        /// </summary>
        public static string Key_Verify = Base.GetKeyValue("Mc.Verify", "Verify");
        /// <summary>
        /// 返回获取的属性值
        /// </summary>
        /// <param name="Key">Web.Config 中指定的 Key</param>
        /// <param name="Value">若Web.Config 中无指定的值 则显示此默认值</param>
        /// <returns>Value</returns>
        public static string GetKeyValue(string Key, string Value)
        {
            string text = ConfigurationManager.AppSettings[Key];
            if (Base.IsNull(text))
            {
                text = Value;
            }
            return text;
        }
        /// <summary>
        /// 根据一定条件以及排序规则获取DataRow
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>DataRow</returns>
        public static DataRow GetRow(DataTable Table, int ID)
        {
            return Base.GetRow(Table, "ID = " + ID.ToString());
        }
        /// <summary>
        /// 根据一定条件以及排序规则获取DataRow
        /// </summary>
        /// <param name="Searchs">条件</param>
        /// <returns>DataRow</returns>
        public static DataRow GetRow(DataTable Table, string Searchs)
        {
            return Base.GetRow(Table, Searchs, string.Empty);
        }
        /// <summary>
        /// 根据一定条件以及排序规则获取DataRow
        /// </summary>
        /// <param name="Searchs">条件</param>
        /// <param name="Sort">条件排序</param>
        /// <returns>DataRow</returns>
        public static DataRow GetRow(DataTable Table, string Searchs, string Sort)
        {
            DataRow[] rows = Base.GetRows(Table, Searchs, Sort);
            DataRow result;
            if (rows == null)
            {
                result = null;
            }
            else
            {
                if (rows.Length <= 0)
                {
                    result = null;
                }
                else
                {
                    result = rows[0];
                }
            }
            return result;
        }
        /// <summary>
        /// 根据一定条件获取DataRow[]
        /// </summary>
        /// <param name="Table">DataTable</param>
        /// <param name="Searchs">条件</param>
        /// <returns>DataRow[]</returns>
        public static DataRow[] GetRows(DataTable Table, string Searchs)
        {
            return Base.GetRows(Table, Searchs, string.Empty);
        }
        /// <summary>
        /// 根据一定条件以及排序规则获取DataRow[]
        /// </summary>
        /// <param name="Table">DataTable</param>
        /// <param name="Searchs">条件</param>
        /// <param name="Sort">条件排序</param>
        /// <returns>DataRow[]</returns>
        public static DataRow[] GetRows(DataTable Table, string Searchs, string Sort)
        {
            DataRow[] result;
            if (Base.IsNull(Searchs))
            {
                result = null;
            }
            else
            {
                if (Table == null)
                {
                    result = null;
                }
                else
                {
                    result = (Base.IsNull(Sort) ? Table.Select(Searchs) : Table.Select(Searchs, Sort));
                }
            }
            return result;
        }
        
        /// <summary>
        /// 从一个DataTable中按一定条件得到一个新的DataTable
        /// </summary>
        /// <param name="Table">DataTable</param>
        /// <param name="Searchs">一定条件</param>
        /// <returns>DataTable</returns>
        public static DataTable GetTable(DataTable Table, string Searchs)
        {
            return Base.GetTable(Table, Searchs, string.Empty);
        }
        /// <summary>
        /// 从一个DataTable中按一定条件得到一个新的DataTable
        /// </summary>
        /// <param name="Table">DataTable</param>
        /// <param name="Searchs">一定条件</param>
        /// <param name="Sort">一定条件</param>
        /// <returns>DataTable</returns>
        public static DataTable GetTable(DataTable Table, string Searchs, string Sort)
        {
            DataRow[] rows = Base.GetRows(Table, Searchs, Sort);
            int num = (rows == null) ? 0 : rows.Length;
            DataTable result;
            if (num <= 0)
            {
                result = null;
            }
            else
            {
                DataTable dataTable = Table.Clone();
                for (int i = 0; i < num; i++)
                {
                    dataTable.ImportRow(rows[i]);
                }
                result = dataTable;
            }
            return result;
        }
        /// <summary>
        /// 按字段获取string
        /// </summary>
        /// <param name="Rs">DataRow</param>
        /// <param name="Filed">string</param>
        /// <returns>string</returns>
        public static string GetDataRowValue(DataRow Rs, string Filed)
        {
            return Rs[Filed].ToString();
        }
        /// <summary>
        /// 按字段集获取string
        /// </summary>
        /// <param name="Rs">DataRow</param>
        /// <param name="Fileds">string[]</param>
        /// <returns>string</returns>
        public static string GetDataRowValue(DataRow Rs, string[] Fileds)
        {
            string text = "";
            int num = (Fileds == null) ? 0 : Fileds.Length;
            for (int i = 0; i < num; i++)
            {
                text = text + ((i > 0) ? "," : "") + Rs[Fileds[i]].ToString();
            }
            return text;
        }
        /// <summary>
        /// 对象是否不为空,或者空字符
        /// 本例基本为字符串操作设置
        /// </summary>
        /// <param name="Object">对象</param>
        /// <returns>bool</returns>
        public static bool NoNull(object Object)
        {
            return Base.NoNull(Object, false);
        }
        /// <summary>
        /// 对象是否不为空,或者空字符
        /// 本例基本为字符串操作设置
        /// </summary>
        /// <param name="Object">对象</param>
        /// <param name="IsRemoveSpace">是否移除空字符</param>
        /// <returns>bool</returns>
        public static bool NoNull(object Object, bool IsRemoveSpace)
        {
            return !Base.IsNull(Object, IsRemoveSpace);
        }
        /// <summary>
        /// 对象是否不为空,或者空字符
        /// 本例基本为字符串操作设置
        /// </summary>
        /// <param name="Object">对象</param>
        /// <returns>bool</returns>
        public static bool IsNull(object Object)
        {
            return Base.IsNull(Object, false);
        }
        /// <summary>
        /// 对象是否不为空,或者空字符
        /// 本例基本为字符串操作设置
        /// </summary>
        /// <param name="Object">对象</param>
        /// <param name="IsRemoveSpace">是否移除空字符</param>
        /// <returns>bool</returns>
        public static bool IsNull(object Object, bool IsRemoveSpace)
        {
            string empty = string.Empty;
            return Base.IsNull(Object, IsRemoveSpace, out empty);
        }
        /// <summary>
        /// 对象是否不为空,或者空字符
        /// 本例基本为字符串操作设置
        /// </summary>
        /// <param name="Object">对象</param>
        /// <param name="IsRemoveSpace">是否移除空字符</param>
        /// <param name="String">返回的字符</param>
        /// <returns>bool</returns>
        public static bool IsNull(object Object, bool IsRemoveSpace, out string String)
        {
            String = string.Empty;
            bool result;
            if (Object == null)
            {
                result = true;
            }
            else
            {
                String = Object.ToString();
                if (String == string.Empty)
                {
                    result = true;
                }
                else
                {
                    if (String.Length <= 0)
                    {
                        result = true;
                    }
                    else
                    {
                        if (IsRemoveSpace)
                        {
                            String = String.Replace(" ", "").Replace("\u3000", "");
                        }
                        result = (String.Length <= 0);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 对象是否为bool值
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <returns>bool值</returns>
        public static bool IsBool(object Object)
        {
            return Base.IsBool(Object, false);
        }
        /// <summary>
        /// 判断是否为bool值
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="Default">默认bool值</param>
        /// <returns>bool值</returns>
        public static bool IsBool(object Object, bool Default)
        {
            string empty = string.Empty;
            bool result;
            if (Base.IsNull(Object, false, out empty))
            {
                result = Default;
            }
            else
            {
                result = (empty.ToLower() == "true" || Default);
            }
            return result;
        }
        /// <summary>
        /// 判断
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <returns>bool</returns>
        public static bool IsInt(object Object)
        {
            bool result = false;
            int num = Base.IsInt(Object, out result);
            return result;
        }
        /// <summary>
        /// 对象是否为 int  类型数据
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>int值</returns>
        public static int IsInt(object Object, out bool IsTrue)
        {
            int result;
            if (Base.IsNull(Object))
            {
                IsTrue = false;
                result = 0;
            }
            else
            {
                try
                {
                    IsTrue = true;
                    result = int.Parse(Object.ToString());
                }
                catch
                {
                    IsTrue = false;
                    result = 0;
                }
            }
            return result;
        }
        /// <summary>
        /// 转换成为 int  数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>int 数据</returns>
        public static int ToInt(object Object)
        {
            return Base.ToInt(Object, 0);
        }
        /// <summary>
        /// 转换成为 int  数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <returns>int 数据</returns>
        public static int ToInt(object Object, int Default)
        {
            return Base.ToInt(Object, Default, -2147483648, 2147483647);
        }
        /// <summary>
        /// 转换成为 int  数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinInt"> 下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>int 数据</returns>
        public static int ToInt(object Object, int Default, int MinInt)
        {
            return Base.ToInt(Object, Default, MinInt, 2147483647);
        }
        /// <summary>
        /// 转换成为 int  数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinInt"> 下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <param name="MaxInt">上界限定的最大值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>int 数据</returns>
        public static int ToInt(object Object, int Default, int MinInt, int MaxInt)
        {
            bool flag = false;
            int num = Base.IsInt(Object, out flag);
            int result;
            if (!flag)
            {
                result = Default;
            }
            else
            {
                if (num < MinInt || num > MaxInt)
                {
                    result = Default;
                }
                else
                {
                    result = num;
                }
            }
            return result;
        }
        /// <summary>
        /// 判断
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <returns>bool</returns>
        public static bool IsLong(object Object)
        {
            bool result = false;
            long num = Base.IsLong(Object, out result);
            return result;
        }
        /// <summary>
        /// 对象是否为 long  类型数据
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>long值</returns>
        public static long IsLong(object Object, out bool IsTrue)
        {
            long result;
            if (Base.IsNull(Object))
            {
                IsTrue = false;
                result = 0L;
            }
            else
            {
                try
                {
                    IsTrue = true;
                    result = long.Parse(Object.ToString());
                }
                catch
                {
                    IsTrue = false;
                    result = 0L;
                }
            }
            return result;
        }
        /// <summary>
        /// 转换成为 Long 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>Long 数据</returns>
        public static long ToLong(object Object)
        {
            return Base.ToLong(Object, 0L);
        }
        /// <summary>
        /// 转换成为 Long 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <returns>Long 数据</returns>
        public static long ToLong(object Object, long Default)
        {
            return Base.ToLong(Object, Default, -9223372036854775808L, 9223372036854775807L);
        }
        /// <summary>
        /// 转换成为 long 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">转换不成功返回的默认值</param>
        /// <param name="MinLong">下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>long 数据</returns>
        public static long ToLong(object Object, long Default, long MinLong)
        {
            return Base.ToLong(Object, Default, MinLong, 9223372036854775807L);
        }
        /// <summary>
        /// 转换成为 long 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinLong">下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <param name="MaxLong">上界限定的最大值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>long 数据</returns>
        public static long ToLong(object Object, long Default, long MinLong, long MaxLong)
        {
            bool flag = false;
            long num = Base.IsLong(Object, out flag);
            long result;
            if (!flag)
            {
                result = Default;
            }
            else
            {
                if (num < MinLong || num > MaxLong)
                {
                    result = Default;
                }
                else
                {
                    result = num;
                }
            }
            return result;
        }
        /// <summary>
        /// 判断
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <returns>bool</returns>
        public static bool IsFloat(object Object)
        {
            bool result = false;
            float num = Base.IsFloat(Object, out result);
            return result;
        }
        /// <summary>
        /// 对象是否为 float  类型数据
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>float值</returns>
        public static float IsFloat(object Object, out bool IsTrue)
        {
            float result;
            if (Base.IsNull(Object))
            {
                IsTrue = false;
                result = 0f;
            }
            else
            {
                try
                {
                    IsTrue = true;
                    result = float.Parse(Object.ToString());
                }
                catch
                {
                    IsTrue = false;
                    result = 0f;
                }
            }
            return result;
        }
        /// <summary>
        /// 转换成为 float 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>float 数据</returns>
        public static float ToFloat(object Object)
        {
            return Base.ToFloat(Object, 0f);
        }
        /// <summary>
        /// 转换成为 float 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <returns>float 数据</returns>
        public static float ToFloat(object Object, float Default)
        {
            return Base.ToFloat(Object, Default, -3.40282347E+38f, 3.40282347E+38f);
        }
        /// <summary>
        /// 转换成为 float 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinFloat"> 小于等于 转换成功后,下界限定的最小值,若超过范围 则返回 默认值</param>
        /// <returns>float 数据</returns>
        public static float ToFloat(object Object, float Default, float MinFloat)
        {
            return Base.ToFloat(Object, Default, MinFloat, 3.40282347E+38f);
        }
        /// <summary>
        /// 转换成为 float 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinFloat"> 下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <param name="MaxFloat"> 上界限定的最大值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>float 数据</returns>
        public static float ToFloat(object Object, float Default, float MinFloat, float MaxFloat)
        {
            bool flag = false;
            float num = Base.IsFloat(Object, out flag);
            float result;
            if (!flag)
            {
                result = Default;
            }
            else
            {
                if (num < MinFloat || num > MaxFloat)
                {
                    result = Default;
                }
                else
                {
                    result = num;
                }
            }
            return result;
        }
        /// <summary>
        /// 对象是否为 decimal  类型数据
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>decimal值</returns>
        public static decimal IsDecimal(object Object, out bool IsTrue)
        {
            decimal result;
            if (Base.IsNull(Object))
            {
                IsTrue = false;
                result = 0m;
            }
            else
            {
                try
                {
                    IsTrue = true;
                    result = decimal.Parse(Object.ToString());
                }
                catch
                {
                    IsTrue = false;
                    result = 0m;
                }
            }
            return result;
        }
        /// <summary>
        /// 转换成为 decimal 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>decimal 数据</returns>
        public static decimal ToDecimal(object Object)
        {
            return Base.ToDecimal(Object, 0m);
        }
        /// <summary>
        /// 转换成为 decimal 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <returns>decimal 数据</returns>
        public static decimal ToDecimal(object Object, decimal Default)
        {
            return Base.ToDecimal(Object, Default, -79228162514264337593543950335m, 79228162514264337593543950335m);
        }
        /// <summary>
        /// 转换成为 decimal 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinFloat"> 小于等于 转换成功后,下界限定的最小值,若超过范围 则返回 默认值</param>
        /// <returns>decimal 数据</returns>
        public static decimal ToDecimal(object Object, decimal Default, decimal MinFloat)
        {
            return Base.ToDecimal(Object, Default, MinFloat, 79228162514264337593543950335m);
        }
        /// <summary>
        /// 转换成为 decimal 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinDecimal"> 下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <param name="MaxDecimal"> 上界限定的最大值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>decimal 数据</returns>
        public static decimal ToDecimal(object Object, decimal Default, decimal MinDecimal, decimal MaxDecimal)
        {
            bool flag = false;
            decimal num = Base.IsDecimal(Object, out flag);
            decimal result;
            if (!flag)
            {
                result = Default;
            }
            else
            {
                if (num < MinDecimal || num > MaxDecimal)
                {
                    result = Default;
                }
                else
                {
                    result = num;
                }
            }
            return result;
        }
        /// <summary>
        /// 判断
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <returns>bool</returns>
        public static bool IsTime(object Object)
        {
            bool result = false;
            DateTime dateTime = Base.IsTime(Object, out result);
            return result;
        }
        /// <summary>
        /// 是否为时间格式
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>DateTime</returns>
        public static DateTime IsTime(object Object, out bool IsTrue)
        {
            IsTrue = false;
            DateTime result;
            if (Base.IsNull(Object))
            {
                result = DateTime.Now;
            }
            else
            {
                try
                {
                    IsTrue = true;
                    result = DateTime.Parse(Object.ToString());
                }
                catch
                {
                    IsTrue = false;
                    result = DateTime.Now;
                }
            }
            return result;
        }
        /// <summary>
        /// 操作 DateTime  数据
        /// </summary>
        /// <param name="Object">要操作的字符</param>
        /// <returns>DateTime</returns>
        public static DateTime ToTime(string Object)
        {
            return Base.ToTime(Object, DateTime.Now);
        }
        /// <summary>
        /// 字符串转换为时间函数
        /// </summary>
        /// <param name="Object">要操作的字符</param>
        /// <param name="Default">默认时间</param>
        /// <returns>DateTime</returns>
        public static DateTime ToTime(string Object, DateTime Default)
        {
            DateTime result;
            if (Base.IsNull(Object))
            {
                result = Default;
            }
            else
            {
                bool flag = false;
                DateTime dateTime = Base.IsTime(Object, out flag);
                if (!flag)
                {
                    result = Default;
                }
                else
                {
                    result = dateTime;
                }
            }
            return result;
        }
        /// <summary>
        /// 重新获取ID列表
        /// </summary>
        /// <param name="OldIdList">旧的ID列表</param>
        /// <returns>string</returns>
        public static string GetIdList(string OldIdList)
        {
            string result;
            if (Base.IsNull(OldIdList))
            {
                result = "";
            }
            else
            {
                string[] array = OldIdList.Split(",".ToCharArray(), StringSplitOptions.None);
                int num = array.Length;
                int num2 = 0;
                string text = "";
                for (int i = 0; i < num; i++)
                {
                    int num3 = Base.ToInt(array[i], 0);
                    if (num3 > 0)
                    {
                        if (num2 > 0)
                        {
                            text += ",";
                        }
                        text += num3.ToString();
                        num2++;
                    }
                }
                result = text;
            }
            return result;
        }
        /// <summary>
        /// 字符 int  转换为 char
        /// </summary>
        /// <param name="Int">字符[int]</param>
        /// <returns>char</returns>
        public static char IntToChar(int Int)
        {
            return (char)Int;
        }
        /// <summary>
        /// 字符 int  转换为字符 string
        /// </summary>
        /// <param name="Int">字符 int</param>
        /// <returns>字符 string</returns>
        public static string IntToString(int Int)
        {
            return Base.IntToChar(Int).ToString();
        }
        /// <summary>
        /// 字符 string  转换为字符 int
        /// </summary>
        /// <param name="Strings">字符 string</param>
        /// <returns>字符 int</returns>
        public static int StringToInt(string Strings)
        {
            char[] array = Strings.ToCharArray();
            return (int)array[0];
        }
        /// <summary>
        /// 字符 string  转换为 char
        /// </summary>
        /// <param name="Strings">字符 string</param>
        /// <returns>char</returns>
        public static char StringToChar(string Strings)
        {
            return Base.IntToChar(Base.StringToInt(Strings));
        }
        /// <summary>
        /// 获取缩略图片路径
        /// </summary>
        /// <param name="Url">原始路径</param>
        /// <param name="ExecName">返回的文件名</param>
        /// <returns>string</returns>
        public static string GetPhoto(string Url, ref string ExecName)
        {
            int num = Url.LastIndexOf("\\");
            if (num < 0)
            {
                num = Url.LastIndexOf("/");
            }
            ExecName = Base.Right(Url, Url.Length - num - 1);
            Url = Url.Replace(ExecName, "");
            return Url;
        }
        /// <summary>
        /// 获取缩略图地址 1级
        /// </summary>
        /// <param name="Url">原始路径</param>
        /// <param name="Text">添加的标记</param>
        /// <returns>string</returns>
        public static string GetPhotoScale(string Url, string Text)
        {
            string str = "";
            string photo = Base.GetPhoto(Url, ref str);
            return photo + Text + str;
        }
        /// <summary>
        /// 判断文件名是否为浏览器可以直接显示的图片文件名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否可以直接显示</returns>
        public static bool IsImgFilename(string filename)
        {
            filename = filename.Trim();
            bool result;
            if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
            {
                result = false;
            }
            else
            {
                string a = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
                result = (a == "jpg" || a == "jpeg" || a == "png" || a == "bmp" || a == "gif");
            }
            return result;
        }
        /// <summary>
        /// 取字符左函数
        /// </summary>
        /// <param name="Object">要操作的 string  数据</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string Left(object Object, int MaxLength)
        {
            string result;
            if (Base.IsNull(Object))
            {
                result = "";
            }
            else
            {
                result = Object.ToString().Substring(0, Math.Min(Object.ToString().Length, MaxLength));
            }
            return result;
        }
        /// <summary>
        /// 取字符中间函数
        /// </summary>
        /// <param name="Object">要操作的 string  数据</param>
        /// <param name="StarIndex">开始的位置索引</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string Mid(string Object, int StarIndex, int MaxLength)
        {
            string result;
            if (Base.IsNull(Object))
            {
                result = "";
            }
            else
            {
                if (StarIndex >= Object.Length)
                {
                    result = "";
                }
                else
                {
                    result = Object.Substring(StarIndex, MaxLength);
                }
            }
            return result;
        }
        /// <summary>
        /// 取字符右函数
        /// </summary>
        /// <param name="Object">要操作的 string  数据</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string Right(object Object, int MaxLength)
        {
            string result;
            if (Base.IsNull(Object))
            {
                result = "";
            }
            else
            {
                int num = Object.ToString().Length;
                if (num < MaxLength)
                {
                    MaxLength = num;
                    num = 0;
                }
                else
                {
                    num -= MaxLength;
                }
                result = Object.ToString().Substring(num, MaxLength);
            }
            return result;
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="Object">要操作的 string  数据</param>
        /// <returns>string</returns>
        public static string MD5(string Object)
        {
            string result;
            if (Base.IsNull(Object))
            {
                result = "";
            }
            else
            {
                result = FormsAuthentication.HashPasswordForStoringInConfigFile(Object, "MD5");
            }
            return result;
        }
        /// <summary>
        /// 获取全局唯一GUID 值
        /// </summary>
        /// <returns>string</returns>
        public static string GetGuid()
        {
            return Base.GetGuid(100, true);
        }
        /// <summary>
        /// 获取当前时间的刻度值
        /// </summary>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>int</returns>
        public static int GetGuid(int MaxLength)
        {
            return Base.ToInt(Base.Right(DateTime.Now.Ticks.ToString(), MaxLength));
        }
        /// <summary>
        /// 获取全局唯一GUID 值
        /// </summary>
        /// <param name="MaxLength">最大长度</param>
        /// <param name="IsRemove">是否去掉特殊字符</param>
        /// <returns>string</returns>
        public static string GetGuid(int MaxLength, bool IsRemove)
        {
            string text = Guid.NewGuid().ToString();
            if (IsRemove)
            {
                text = text.Replace("{", "");
                text = text.Replace("}", "");
                text = text.Replace("-", "");
            }
            return Base.Left(text, MaxLength);
        }
        /// <summary>
        /// 获取指定类型的随机数
        /// </summary>
        /// <param name="Enum">产生随机数的枚举类型</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string GetGuid(ERand Enum, int MaxLength)
        {
            return Base.GetGuid(Enum, "GB2312", MaxLength);
        }
        /// <summary>
        /// 获取指定类型的随机数
        /// </summary>
        /// <param name="Enum">产生随机数的枚举类型</param>
        /// <param name="Encode">产生随机中文的编码</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string GetGuid(ERand Enum, string Encode, int MaxLength)
        {
            string result;
            if (Enum == ERand.Chinese)
            {
                result = Base.GetGuid(Encode, MaxLength);
            }
            else
            {
                string text = "";
                for (int i = 1; i <= MaxLength; i++)
                {
                    Random random = new Random(Base.GetGuid(9) + i * 1000);
                    int num = (int)((Enum == ERand.Blend) ? ((random.Next(0, 10) <= 4) ? ERand.Numeric : ERand.Letter) : Enum);
                    int minValue = (num == 0) ? 48 : 97;
                    int maxValue = (num == 0) ? 57 : 122;
                    text += Base.IntToString(random.Next(minValue, maxValue));
                }
                result = text;
            }
            return result;
        }
        /// <summary>
        /// 获取随机产生的中文
        /// </summary>
        /// <param name="Encode">产生随机中文的编码</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string GetGuid(string Encode, int MaxLength)
        {
            string text = "";
            Encoding encoding = Encoding.GetEncoding(Encode);
            for (int i = 1; i <= MaxLength; i++)
            {
                Random random = new Random(Base.GetGuid(9) + i * 1000);
                int num = random.Next(16, 56);
                int num2 = num;
                num = ((num2 == 55) ? 90 : 95);
                random = new Random(Base.GetGuid(9) + i * 3000);
                num = random.Next(1, num);
                int num3 = num;
                byte b = Convert.ToByte((num2 + 160).ToString("x"), 16);
                byte b2 = Convert.ToByte((num3 + 160).ToString("x"), 16);
                byte[] bytes = new byte[]
				{
					b,
					b2
				};
                text += encoding.GetString(bytes);
            }
            return text;
        }
        /// <summary>
        /// 是否汉字
        /// </summary>
        /// <param name="Object">单个字符</param>
        /// <returns>bool</returns>
        public static bool IsChinese(string Object)
        {
            int num = Base.StringToInt(Object);
            return num >= 19968 && num <= 40869;
        }
        /// <summary>
        /// 是否汉字
        /// </summary>
        /// <param name="Object">单个字符 char</param>
        /// <returns>bool</returns>
        private static bool IsChinese(char Object)
        {
            return Object >= '一' && Object <= '龥';
        }
        /// <summary>
        /// 获取字符串的长度
        /// </summary>
        /// <param name="Object">要检测的对象</param>
        /// <param name="IsChecked">是否判断汉字</param>
        /// <returns>int</returns>
        public static int GetLength(string Object, bool IsChecked)
        {
            int result;
            if (Base.IsNull(Object))
            {
                result = 0;
            }
            else
            {
                if (!IsChecked)
                {
                    result = Object.Length;
                }
                else
                {
                    int num = 0;
                    for (int i = 0; i < Object.Length; i++)
                    {
                        char @object = Object[i];
                        num++;
                        if (Base.IsChinese(@object))
                        {
                            num++;
                        }
                    }
                    result = num;
                }
            }
            return result;
        }
        /// <summary>
        /// 检测长度
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="MinInt">小于等于 最小长度</param>
        /// <param name="MaxInt">大于等于 最大长度</param>
        /// <returns>符合?true:false</returns>
        public static bool CheckLength(string Object, int MinInt, int MaxInt)
        {
            return Base.CheckLength(Object, MinInt, MaxInt, false);
        }
        /// <summary>
        /// 检测长度
        /// </summary>
        /// <param name="Object">要检测的对象</param>
        /// <param name="MinInt">小于等于 最小长度</param>
        /// <param name="MaxInt">大于等于 最大长度</param>
        /// <param name="IsChecked">是否判断汉字</param>
        /// <returns>符合?true:false</returns>
        public static bool CheckLength(string Object, int MinInt, int MaxInt, bool IsChecked)
        {
            int length = Base.GetLength(Object, IsChecked);
            return length >= MinInt && length <= MaxInt;
        }
        /// <summary>
        /// 获取当前字符串 所对应的规定的模的值
        /// </summary>
        /// <param name="Object">当前字符串</param>
        /// <param name="UseIndex">规定的字符</param>
        /// <param name="ModInt">规定的取模数</param>
        /// <returns>模值</returns>
        public static int GetModValue(string Object, int UseIndex, int ModInt)
        {
            Object = Base.Mid(Object, UseIndex - 1, 1);
            int num = Base.StringToInt(Object.ToLower());
            return num % ModInt;
        }
        /// <summary>
        /// 获取当前单个字符 所对应的规定的模的值
        /// 字母占26个数据表
        /// 其他字符(包含汉字)占24个数据表
        /// 一共50个用户总表
        /// </summary>
        /// <param name="Object">单个字符</param>
        /// <returns>int</returns>
        public static int GetMod(string Object)
        {
            if (Object.Length > 1)
            {
                Object = Base.Left(Object, 1);
            }
            int num = Base.StringToInt(Object.ToLower());
            int result;
            if (num >= 48 && num <= 57)
            {
                result = 1 + num % 48;
            }
            else
            {
                if (num >= 97 && num <= 122)
                {
                    result = 11 + num % 97;
                }
                else
                {
                    result = 37 + num % 14;
                }
            }
            return result;
        }
        /// <summary>
        /// 文本操作 执行HTML解码
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>字符</returns>
        public static string HtmlDecode(string Object)
        {
            string result;
            if (Base.IsNull(Object))
            {
                result = "";
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder(Object);
                stringBuilder.Replace("&amp;", "&");
                stringBuilder.Replace("&lt;", "<");
                stringBuilder.Replace("&gt;", ">");
                stringBuilder.Replace("&quot;", "\"");
                stringBuilder.Replace("&#39;", "'");
                stringBuilder.Replace("&nbsp; &nbsp; ", "\t");
                stringBuilder.Replace("<p></p>", "\r\n\r\n");
                stringBuilder.Replace("<br />", "\r\n");
                stringBuilder.Replace("<br>", "\r\n");
                result = stringBuilder.ToString();
            }
            return result;
        }
        /// <summary>
        /// 文本操作 执行HTML编码
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>字符</returns>
        public static string HtmlEncode(string Object)
        {
            string result;
            if (Base.IsNull(Object))
            {
                result = "";
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder(Object);
                stringBuilder.Replace("&", "&amp;");
                stringBuilder.Replace("<", "&lt;");
                stringBuilder.Replace(">", "&gt;");
                stringBuilder.Replace("\"", "&quot;");
                stringBuilder.Replace("'", "&#39;");
                stringBuilder.Replace("\t", "&nbsp; &nbsp; ");
                stringBuilder.Replace("\r\n\r\n", "<p></p>");
                stringBuilder.Replace("\r\n", "<br />");
                stringBuilder.Replace("\n", "<br />");
                result = stringBuilder.ToString();
            }
            return result;
        }
        /// <summary>
        /// 文本操作 执行HTML解码
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>字符</returns>
        public static string HtmlDecodeBr(string Object)
        {
            string result;
            if (Base.IsNull(Object))
            {
                result = "";
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder(Object);
                stringBuilder.Replace("<p></p>", "\r\n\r\n");
                stringBuilder.Replace("<br />", "\r\n");
                stringBuilder.Replace("<br>", "\r\n");
                result = stringBuilder.ToString();
            }
            return result;
        }
        /// <summary>
        /// 文本操作 执行HTML Br 编码
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>字符</returns>
        public static string HtmlEncodeBr(string Object)
        {
            string result;
            if (Base.IsNull(Object))
            {
                result = "";
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder(Object);
                stringBuilder.Replace("\r\n\r\n", "<p></p>");
                stringBuilder.Replace("\r\n", "<br />");
                stringBuilder.Replace("\n\n", "<br />");
                stringBuilder.Replace("\n\n", "<br />");
                stringBuilder.Replace("\n", "<br />");
                result = stringBuilder.ToString();
            }
            return result;
        }
        /// <summary>
        /// 文本操作 执行TEXT解码
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>字符</returns>
        public static string TextDecode(string Object)
        {
            string result;
            if (Base.IsNull(Object))
            {
                result = "";
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder(Object);
                stringBuilder.Replace("&amp;", "&");
                stringBuilder.Replace("&lt;", "<");
                stringBuilder.Replace("&gt;", ">");
                stringBuilder.Replace("&quot;", "\"");
                stringBuilder.Replace("&#39;", "'");
                result = stringBuilder.ToString();
            }
            return result;
        }
        /// <summary>
        /// 文本操作 执行TEXT编码
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>字符</returns>
        public static string TextEncode(string Object)
        {
            string result;
            if (Base.IsNull(Object))
            {
                result = "";
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder(Object);
                stringBuilder.Replace("&", "&amp;");
                stringBuilder.Replace("<", "&lt;");
                stringBuilder.Replace(">", "&gt;");
                stringBuilder.Replace("\"", "&quot;");
                stringBuilder.Replace("'", "&#39;");
                result = stringBuilder.ToString();
            }
            return result;
        }
        /// <summary>
        /// 对获取的字符进行唯一 ' 的替换
        /// </summary>
        /// <param name="Value">要替换的字符</param>
        /// <returns>string</returns>
        public static string DoSqlReplace(string Value)
        {
            string result;
            if (Base.IsNull(Value))
            {
                result = "";
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder(Value);
                stringBuilder = stringBuilder.Replace("'", "&#39;");
                result = stringBuilder.ToString();
            }
            return result;
        }
        /// <summary>
        /// 对获取的字符进行唯一 ' 的替换
        /// </summary>
        /// <param name="Value">要替换的字符</param>
        /// <returns>string</returns>
        public static string ToSqlReplace(string Value)
        {
            string result;
            if (Base.IsNull(Value))
            {
                result = "";
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder(Value);
                stringBuilder = stringBuilder.Replace("&#39;", "'");
                result = stringBuilder.ToString();
            }
            return result;
        }
        /// <summary>
        /// 获取对应的权限
        /// </summary>
        /// <param name="Powers">权限列表</param>
        /// <returns>char[] </returns>
        public static char[] GetPower(string Powers)
        {
            char[] result;
            if (Base.IsNull(Powers))
            {
                result = null;
            }
            else
            {
                result = Powers.ToCharArray();
            }
            return result;
        }
        /// <summary>
        /// 判断单个权限 
        /// </summary>
        /// <param name="Index">某单个权限的索引</param>
        /// <param name="Powers">权限列表</param>
        /// <returns>bool</returns>
        public static bool IsPower(int Index, char[] Powers)
        {
            Index--;
            bool result;
            if (Index < 0)
            {
                result = false;
            }
            else
            {
                if (Powers == null)
                {
                    result = false;
                }
                else
                {
                    int num = Powers.Length;
                    if (Index >= num)
                    {
                        result = false;
                    }
                    else
                    {
                        bool flag = Powers[Index] == '1';
                        result = flag;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 判断一系列权限是否同时满足
        /// </summary>
        /// <param name="Indexs">一系列权限</param>
        /// <param name="Powers">权限列表</param>
        /// <returns>bool</returns>
        public static bool IsPower(int[] Indexs, char[] Powers)
        {
            bool result;
            if (Indexs == null)
            {
                result = false;
            }
            else
            {
                if (Powers == null)
                {
                    result = false;
                }
                else
                {
                    int num = Powers.Length;
                    int num2 = Indexs.Length;
                    for (int i = 0; i < num2; i++)
                    {
                        int num3 = Indexs[i];
                        num3--;
                        if (num3 < 0 || num3 >= num)
                        {
                            result = false;
                            return result;
                        }
                        if (Powers[num3] != '1')
                        {
                            result = false;
                            return result;
                        }
                    }
                    result = false;
                }
            }
            return result;
        }
        /// <summary>
        /// 根据一系列权限索引 来获取对应的权限
        /// </summary>
        /// <param name="Indexs">一系列权限索引</param>
        /// <param name="Powers">权限列表</param>
        /// <returns> bool[]</returns>
        public static bool[] GetPower(int[] Indexs, char[] Powers)
        {
            bool[] result;
            if (Indexs == null)
            {
                result = null;
            }
            else
            {
                if (Powers == null)
                {
                    result = null;
                }
                else
                {
                    int num = Powers.Length;
                    int num2 = Indexs.Length;
                    bool[] array = new bool[num2];
                    for (int i = 0; i < num2; i++)
                    {
                        int num3 = Indexs[i];
                        num3--;
                        if (num3 < 0 || num3 >= num)
                        {
                            array[i] = false;
                        }
                        else
                        {
                            array[i] = (Powers[num3] == '1');
                        }
                    }
                    result = array;
                }
            }
            return result;
        }
        /// <summary>
        /// 通过和权限的对比,获得新的权限字符串
        /// </summary>
        /// <param name="ParentPowers">父级权限</param>
        /// <param name="NewPowsers">新的权限的ID列表</param>
        /// <param name="MaxPower">最大权限ID</param>
        /// <returns>string</returns>
        public static string GetPower(char[] ParentPowers, string NewPowsers, int MaxPower)
        {
            return Base.GetPower(ParentPowers, NewPowsers, MaxPower, false);
        }
        /// <summary>
        /// 通过和权限的对比,获得新的权限字符串
        /// </summary>
        /// <param name="ParentPowers">父级权限</param>
        /// <param name="NewPowsers">新的权限的ID列表</param>
        /// <param name="MaxPower">最大权限ID</param>
        /// <param name="IsAdministrator">超级管理员</param>
        /// <returns>string</returns>
        public static string GetPower(char[] ParentPowers, string NewPowsers, int MaxPower, bool IsAdministrator)
        {
            NewPowsers = "," + NewPowsers + ",";
            string text = "";
            int i = 1;
            while (i <= MaxPower)
            {
                if (!IsAdministrator && !Base.IsPower(i, ParentPowers))
                {
                    goto IL_66;
                }
                if (NewPowsers.IndexOf("," + i.ToString() + ",") < 0)
                {
                    goto IL_66;
                }
                text += "1";
            IL_73:
                i++;
                continue;
            IL_66:
                text += "0";
                goto IL_73;
            }
            return text;
        }
    }
}
