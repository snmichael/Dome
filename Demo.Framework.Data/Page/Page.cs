using System.Text;

namespace Demo.Framework.Data.Page
{
    public class Page
    {
        /// <summary>
        /// 样式名
        /// </summary>
        private string _className;

        /// <summary>
        /// 设置样式
        /// </summary>
        public string ClassName
        {
            set { _className = value; }
        }

        /// <summary>
        /// Div ID
        /// </summary>
        private string _divID;

        /// <summary>
        /// Div ID
        /// </summary>
        public string DivID
        {
            set { _divID = value; }
        }
        /// <summary>
        /// 输出页数
        /// </summary>
        /// <param name="index">当前页从1开始</param>
        /// <param name="count">一共页数</param>
        /// <param name="url">路径</param>
        /// <param name="state">参数 如：a=b&c=d</param>
        /// <param name="falg">是否要显示条</param>
        /// <returns></returns>
        public string PrintPage(int index, int count, int countnum, string method, string state, bool falg)
        {
            if (count <= 0)
            {
                return "";
            }

            if (index < 0)
            {
                index = 0;
            }
            else if (index > count)
            {
                index = count;
            }

            //if (state != null && state != "")
            //{
            //    state = string.Format("&{0}", state);
            //}
            int Num = 0;
            StringBuilder sb = new StringBuilder();
            int start = 0;
            if (count > 6)
            {
                if (index > 3)
                {
                    if (count - index > 2)
                    {
                        start = index - 2;
                    }
                    else
                    {
                        start = count - 5;
                    }
                }
                else
                {
                    start = 0;
                }
                Num = 5 + start;
            }
            else
            {
                start = 0;
                Num = count;
            }


            sb.AppendFormat("<div id=\"Pagination\"><div id=\"" + _divID + "\" class=\"pagination\">");
            if (index > 0)
            {
                
                //sb.AppendFormat("<a href=\"{0}?p=0{1}\">首&nbsp;页</a>", url, state);
                sb.AppendFormat("<a class=\"current prev\" tag=\"{0}\" rel=\"prev\">上一页</a>", index - 1);


                if (index > 3)
                {
                    sb.AppendFormat("<a tag=\"0\">1</a><span>...</span>"); //显示第一页    
                }
                
                //sb.AppendFormat("<a href=\"{0}?p={1}{2}\">上一页</a>", url, index - 1, state);
            }
            else
            {
                //sb.AppendFormat("<a>首&nbsp;页</a>");
                sb.AppendFormat("<a class=\"current prev\">上一页</a>");
            }

            for (int i = start; i < Num; i++)
            {
                if (i == index)
                {
                    sb.AppendFormat("<a class=\"current\">{0}</a>", i + 1);
                }
                else
                {
                    //sb.AppendFormat("<a href=\"{0}?p={1}{2}\">{3}</a>", url, i, state, i + 1);
                    sb.AppendFormat("<a tag=\"{0}\" >{1}</a>", i, i + 1);
                }
            }

            if (index < count - 1)
            {
                sb.AppendFormat("<span>...</span><a tag=\"{0}\" >" + count + "</a>", count - 1);
                sb.AppendFormat("<a class=\"next\" tag=\"{0}\">下一页</a>", ++index);
              
                //sb.AppendFormat("<a href=\"{0}?p={1}{2}\">下一页</a>", url, ++index, state);
                //sb.AppendFormat("<a href=\"{0}?p={1}{2}\">尾&nbsp;页</a>", url, count - 1, state);
            }
            else
            {
                sb.AppendFormat("<a class=\"next\">下一页</a>");
                //sb.AppendFormat("<a>尾&nbsp;页</a>");
            }
            if (falg)
            {
                //sb.AppendFormat("<span class=\"p\" tag=\"" + count + "\" >共&nbsp;{0}&nbsp;页&nbsp;总计&nbsp;{1}&nbsp;条</span>", count, countnum);
            }

            //sb.Append("&nbsp;<input type=\"text\" class=\"_pageindex\" style=\"width:30px;\">&nbsp;<a class=\"aJump\">跳转</a>");
            sb.Append("</div></div>");
            sb.Append("<script type=\"text/javascript\">$(function () {$(\"#" + _divID + " a\").click(function () {");
            if (method != "")
                sb.Append(method);
            else
                sb.Append("ReturnView");
            sb.Append("($(this).attr(\"tag\"),\"" + state + "\");});$(\"#" + _divID + " .aJump\").click(function () {");
            sb.Append("var totalPage = $(\"#" + _divID + " .p\").attr(\"tag\"); var jumpPage = $(\"#" + _divID + " ._pageindex\").val();if (parseInt(jumpPage) > parseInt(totalPage)) return;");
            sb.Append("if (jumpPage < 1) return;");
            if (method != "")
                sb.Append(method);
            else
                sb.Append("ReturnView");
            sb.Append("(jumpPage-1,\"" + state + "\");});});</script>");


            return sb.ToString();
        }
        /// <summary>
        /// 输出页数
        /// </summary>
        /// <param name="index">当前页从1开始</param>
        /// <param name="count">一共页数</param>
        /// <param name="url">路径</param>
        /// <param name="state">参数 如：a=b&c=d</param>
        /// <param name="falg">是否要显示条</param>
        /// <returns></returns>
        public string PrintPage(int index, int count, string url, string state)
        {
            if (count <= 0)
            {
                return "";
            }

            if (index < 0)
            {
                index = 0;
            }
            else if (index > count)
            {
                index = count;
            }

            if (state != null && state != "")
            {
                state = string.Format("&{0}", state);
            }
            int Num = 0;
            StringBuilder sb = new StringBuilder();
            int start = 0;
            if (count > 6)
            {
                if (index > 3)
                {
                    if (count - index > 2)
                    {
                        start = index - 2;
                    }
                    else
                    {
                        start = count - 5;
                    }
                }
                else
                {
                    start = 0;
                }
                Num = 5 + start;
            }
            else
            {
                start = 0;
                Num = count;
            }
            sb.AppendFormat("<div class=\"pager\">", _className);
            if (index > 0)
            {
                sb.AppendFormat("<a tag=\"{0}\" >首&nbsp;页</a>",state);
                sb.AppendFormat("<a tag=\"{0}\" >上一页</a>", index);
            }
            else
            {
                sb.AppendFormat("<a>首&nbsp;页</a>");
                sb.AppendFormat("<a>上一页</a>");
            }



            for (int i = start; i < Num; i++)
            {
                if (i == index)
                {
                    sb.AppendFormat("<a class=\"current\">{0}</a>", i + 1);
                }
                else
                {
                    sb.AppendFormat("<a tag=\"{0}\">{1}</a>", i, i + 1);
                }
            }

            if (index < count - 1)
            {
                sb.AppendFormat("<a tag=\"{0}\" >下一页</a>", ++index);
                sb.AppendFormat("<a tag=\"{0}\" >尾&nbsp;页</a>", count - 1);
            }
            else
            {
                sb.AppendFormat("<a>下一页</a>");
                sb.AppendFormat("<a>尾&nbsp;页</a>");
            }
            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// 新用
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="method"></param>
        /// <param name="divId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public string PrintPage(int index, int count, string method, string divId, string state)
        {
            if (count <= 0)
            {
                return "";
            }
            if (index < 0)
            {
                index = 1;
            }
            else if (index > count)
            {
                index = count;
            }
            int Num = 1;
            StringBuilder sb = new StringBuilder();
            int start = 1;
            if (count > 6)
            {
                if (index > 3)
                {
                    if (count - index > 2)
                    {
                        start = index - 2;
                    }
                    else
                    {
                        start = count - 5;
                    }
                }
                else
                {
                    start = 1;
                }
                Num = 5 + start;
            }
            else
            {
                start = 1;
                Num = count;
            }


            sb.AppendFormat("<div id=\"Pagination\"><div id=\"" + divId + "\" class=\"pagination\">");
            if (index > 1)
            {
                //sb.AppendFormat("<a tag=\"1\">首&nbsp;页</a>");
                //sb.AppendFormat("<a tag=\"{0}\" >上一页</a>", index - 1);
                sb.AppendFormat("<a class=\"current prev\" tag=\"{0}\" rel=\"prev\">上一页</a>", index - 1);
            }
            else
            {
                //sb.AppendFormat("<a>首&nbsp;页</a>");
                //sb.AppendFormat("<a>上一页</a>");
                sb.AppendFormat("<a class=\"current prev\">上一页</a>");
            }

            for (int i = start; i <= Num; i++)
            {
                if (i == index)
                {
                    sb.AppendFormat("<a class=\"current\">{0}</a>", i);
                }
                else
                {
                    if (i <= 100)
                        sb.AppendFormat("<a tag=\"{0}\" >{1}</a>", i, i);
                }
            }

            if (index < count)
            {
                if (index < 100)
                    //sb.AppendFormat("<a tag=\"{0}\">下一页</a>", ++index);
                    sb.AppendFormat("<a class=\"next\" tag=\"{0}\">下一页</a>", ++index);
                else
                    sb.AppendFormat("<a class=\"next\">下一页</a>");
                //int endCount = count;
                //if (endCount > 100) endCount = 100;
                // sb.AppendFormat("<a tag=\"{0}\" >尾&nbsp;页</a>", endCount);
            }
            else
            {
                sb.AppendFormat("<a class=\"next\">下一页</a>");
                //sb.AppendFormat("<a>尾&nbsp;页</a>");
            }
            //if (flag)
            //{
            //    int endCount = count;
            //    if (endCount > 100) endCount = 100;
            //    sb.AppendFormat("<span class=\"p\" tag=\"" + endCount + "\" >共&nbsp;{0}&nbsp;页</span>", endCount);
            //}

            //sb.Append("&nbsp;<input type=\"text\" class=\"_pageindex\" style=\"width:30px;\">&nbsp;<a class=\"aJump\">跳转</a>");
            sb.Append("</div></div>");

            sb.Append("<script type=\"text/javascript\">$(function () {$(\"#" + divId + " a\").click(function () {");
            if (method != "")
                sb.Append(method);
            else
                sb.Append("ReturnView");
            sb.Append("($(this).attr(\"tag\"),\"" + state + "\");});$(\"#" + divId + " .aJump\").click(function () {");
            sb.Append("var totalPage = $(\"#" + divId + " .p\").attr(\"tag\"); var jumpPage = $(\"#" + divId + " ._pageindex\").val();if (parseInt(jumpPage) > parseInt(totalPage)) return; if(parseInt(jumpPage)>100) return;");
            sb.Append("if (jumpPage < 1) return;");
            if (method != "")
                sb.Append(method);
            else
                sb.Append("ReturnView");
            sb.Append("(jumpPage,\"" + state + "\");});});</script>");
            return sb.ToString();
        }

        public int GetPageCount(int recordCount,int pageSizeCount)
        {
            if (pageSizeCount == 0) return 0;
            return recordCount % pageSizeCount == 0 ? recordCount / pageSizeCount : recordCount / pageSizeCount + 1;
        }
    }
}
