using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

namespace Demo.Framework.Core
{
    public class NPOIHelper
    {
        /// <summary>
        /// DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置</param>
        public static void Export(DataTable dtSource, string strHeaderText, string strFileName)
        {
            using (MemoryStream ms = Export(dtSource, strHeaderText))
            {
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet();

            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "文件作者信息"; //填加xls文件作者信息
                si.ApplicationName = "创建程序信息"; //填加xls文件创建程序信息
                si.LastAuthor = "最后保存者信息"; //填加xls文件最后保存者信息
                si.Comments = "作者信息"; //填加xls文件作者信息
                si.Title = "标题信息"; //填加xls文件标题信息
                si.Subject = "主题信息";//填加文件主题信息
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion

            HSSFCellStyle dateStyle = workbook.CreateCellStyle();
            HSSFDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 10000 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        if (strHeaderText.Length > 0)
                            sheet = workbook.CreateSheet(strHeaderText);
                        else
                            sheet = workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {
                        if (strHeaderText.Length > 0)
                        {
                            HSSFRow headerRow = sheet.CreateRow(0);
                            headerRow.HeightInPoints = 25;
                            headerRow.CreateCell(0).SetCellValue(strHeaderText);

                            HSSFCellStyle headStyle = workbook.CreateCellStyle();
                            headStyle.Alignment = NPOI.HSSF.UserModel.HSSFCellStyle.ALIGN_CENTER;
                            HSSFFont font = workbook.CreateFont();
                            font.FontHeightInPoints = 20;
                            font.Boldweight = 700;
                            headStyle.SetFont(font);
                            headerRow.GetCell(0).CellStyle = headStyle;
                            sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
                        }
                        //headerRow.Dispose();
                    }
                    #endregion


                    #region 列头及样式
                    {
                        HSSFRow headerRow = sheet.CreateRow(1);
                        HSSFCellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = NPOI.HSSF.UserModel.HSSFCellStyle.ALIGN_CENTER;
                        HSSFFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                        // headerRow.Dispose();
                    }
                    #endregion

                    rowIndex = 2;
                }
                #endregion


                #region 填充内容
                HSSFRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    HSSFCell newCell = dataRow.CreateCell(column.Ordinal);

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            drValue = drValue.Replace("&", "&");
                            drValue = drValue.Replace(">", ">");
                            drValue = drValue.Replace("<", "<");
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32"://整型
                        case "System.Int64"://整型
                        case "System.Int"://整型
                            int invV = 0;
                            int.TryParse(drValue, out invV);
                            newCell.SetCellValue(invV);
                            break;

                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型                           
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }
                #endregion

                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

                //sheet.Dispose();
                //workbook.Dispose();//一般只用写这一个就OK了，他会遍历并释放所有资源，但当前版本有问题所以只释放sheet
                return ms;
            }
        }

        /// <summary>
        /// 用于Web导出
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">文件名</param>
        public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName)
        {
            HttpContext curContext = HttpContext.Current;

            // 设置编码和附件格式
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));

            curContext.Response.BinaryWrite(Export(dtSource, strHeaderText).GetBuffer());
            curContext.Response.End();
        }

        //public static DataTable Import(string strFileName, int sheetindex)
        //{
        //    return Import(strFileName, sheetindex);
        //}
        /// <summary>读取excel
        /// 默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable Import(string strFileName, int sheetindex, int recordcount, int topcount)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            HSSFSheet sheet = hssfworkbook.GetSheetAt(sheetindex);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            if (topcount == 0)
                topcount = 1;
            HSSFRow headerRow = sheet.GetRow(0 + topcount - 1);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                HSSFCell cell = headerRow.GetCell(j);
                if (cell == null)
                    continue;
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + topcount); i <= sheet.LastRowNum; i++)
            {
                if (recordcount > 0)
                {
                    if (i > (recordcount + 2))
                        break;
                }
                HSSFRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if(dt.Columns.Count>j)
                           dataRow[j] = row.GetCell(j).ToString();
                    }
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 获取Sheet列信息
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="sheetindex"></param>
        /// <param name="topcount"></param>
        /// <returns></returns>
        public static System.Collections.Generic.List<string> GetSheetColumns(string strFileName, int sheetindex, int topcount)
        {

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            HSSFSheet sheet = hssfworkbook.GetSheetAt(sheetindex);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            if (topcount < 1)
                topcount = 1;
            HSSFRow headerRow = sheet.GetRow(0 + topcount - 1);
            int cellCount = headerRow.LastCellNum;
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            for (int j = 0; j < cellCount; j++)
            {
                HSSFCell cell = headerRow.GetCell(j);
                if (cell == null)
                    continue;
                if (cell.ToString().Trim().Length > 0)
                {
                    list.Add(cell.ToString());
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static System.Collections.Generic.Dictionary<int, string> GetSheetList(string strFileName)
        {
            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            System.Collections.Generic.Dictionary<int, string> lists = new System.Collections.Generic.Dictionary<int, string>();

            for (int i = 0; i < hssfworkbook.NumberOfSheets; i++)
            {
                lists.Add(i, hssfworkbook.GetSheetName(i));
            }
            return lists;
        }
    }
}