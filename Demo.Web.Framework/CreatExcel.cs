using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace Demo.Framework.Core
{
    /// <summary>
    /// 生成Excel类
    /// </summary>
    public class CreatExcel
    {
        public static ResultExcel BuildingExcel(HttpContextBase page, DataSet ds, string titles)
        {
            return BuildingExcel(page, DateTime.Now.ToString(), ds, titles);
        }

        /// <summary>
        /// 构建Excel
        /// </summary>
        /// <param name="page">当前页面</param>
        /// <param name="Type">文件类型 (0：.xls,1：.csv)</param>
        /// <param name="FileName">Excel文件名</param>
        /// <param name="Content">Excel内容</param>
        /// <returns></returns>
        public static ResultExcel BuildingExcel(HttpContextBase page, string FileName, DataSet ds, string titles)
        {
            StringBuilder Content = new StringBuilder();
            Content.Append(titles);

            if (ds != null)
            {
                int RankCount = ds.Tables[0].Columns.Count - 1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    for (int i = 0; i < RankCount; i++)
                    {
                        Content.Append(GetStr(row[i].ToString().Trim()));
                        if (i != RankCount - 1)
                        {
                            Content.Append("\t");
                        }
                    }
                    Content.Append("\r\n");
                }
            }

            return BuildingExcel(page, 0, FileName, Content.ToString());
        }



        /// <summary>
        /// 判断字符串大小
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected static string GetStr(string str)
        {
            if (str.Length >= 60)
            {
                return str.Substring(0, 60) + "...";
            }
            return str;
        }

        /// <summary>
        /// 构建Excel
        /// </summary>
        /// <param name="page">当前页面</param>
        /// <param name="Type">文件类型 (0：.xls,1：.csv)</param>
        /// <param name="FileName">Excel文件名</param>
        /// <param name="Content">Excel内容</param>
        /// <returns></returns>
        public static ResultExcel BuildingExcel(HttpContextBase page, int Type, string FileName, string Content)
        {
            ResultExcel execl = new ResultExcel();
            if (FileName == null || FileName.Trim() == "")
            {
                execl.State = false;
                execl.Result = "生成Excel文件名不能为空！";
                return execl;
            }

            StringWriter write = null;
            try
            {
                page.Response.Charset = "gb2312";
                //page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                ///设置输出格式 
                page.Response.AddHeader("Content-Disposition",
                                        string.Format("attachment;filename={0}{1}", FileName, GetSuffix(Type)));
                ///保存数据的格式为Excel 
                page.Response.ContentType = "application/ms-excel";
                write = new StringWriter();
                write.Write(Content);
                ///设置数据编码 
                page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                ///输出数据 
                page.Response.Write(write);
                page.Response.End();
            }
            catch (Exception ex)
            {

                execl.State = false;
                execl.Result = "生成Excel失败！";
                return execl;
            }
            finally
            {
                if (write != null)
                {
                    write.Close();
                }
            }

            execl.State = true;
            execl.Result = "生成Excel成功！";
            return execl;
        }

        /// <summary>
        /// 获取后最名
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        private static string GetSuffix(int Type)
        {
            switch (Type)
            {
                case 0: return ".xls";
                case 1: return ".csv";
                default: return ".xls";
            }
        }
    }


    public class ResultExcel
    {
        private bool _state;
        private string _result;

        /// <summary>
        /// 状态
        /// </summary>
        public bool State
        {
            set { _state = value; }
            get { return _state; }
        }

        /// <summary>
        /// 结果
        /// </summary>
        public string Result
        {
            set { _result = value; }
            get { return _result; }
        }
    }
}

