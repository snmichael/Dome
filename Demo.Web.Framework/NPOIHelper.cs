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
        /// DataTable������Excel�ļ�
        /// </summary>
        /// <param name="dtSource">ԴDataTable</param>
        /// <param name="strHeaderText">��ͷ�ı�</param>
        /// <param name="strFileName">����λ��</param>
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
        /// DataTable������Excel��MemoryStream
        /// </summary>
        /// <param name="dtSource">ԴDataTable</param>
        /// <param name="strHeaderText">��ͷ�ı�</param>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet();

            #region �һ��ļ� ������Ϣ
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "�ļ�������Ϣ"; //���xls�ļ�������Ϣ
                si.ApplicationName = "����������Ϣ"; //���xls�ļ�����������Ϣ
                si.LastAuthor = "��󱣴�����Ϣ"; //���xls�ļ���󱣴�����Ϣ
                si.Comments = "������Ϣ"; //���xls�ļ�������Ϣ
                si.Title = "������Ϣ"; //���xls�ļ�������Ϣ
                si.Subject = "������Ϣ";//����ļ�������Ϣ
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion

            HSSFCellStyle dateStyle = workbook.CreateCellStyle();
            HSSFDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //ȡ���п�
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
                #region �½�������ͷ�������ͷ����ʽ
                if (rowIndex == 10000 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        if (strHeaderText.Length > 0)
                            sheet = workbook.CreateSheet(strHeaderText);
                        else
                            sheet = workbook.CreateSheet();
                    }

                    #region ��ͷ����ʽ
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


                    #region ��ͷ����ʽ
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

                            //�����п�
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                        // headerRow.Dispose();
                    }
                    #endregion

                    rowIndex = 2;
                }
                #endregion


                #region �������
                HSSFRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    HSSFCell newCell = dataRow.CreateCell(column.Ordinal);

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://�ַ�������
                            drValue = drValue.Replace("&", "&");
                            drValue = drValue.Replace(">", ">");
                            drValue = drValue.Replace("<", "<");
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://��������
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//��ʽ����ʾ
                            break;
                        case "System.Boolean"://������
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://����
                        case "System.Int32"://����
                        case "System.Int64"://����
                        case "System.Int"://����
                            int invV = 0;
                            int.TryParse(drValue, out invV);
                            newCell.SetCellValue(invV);
                            break;

                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://������                           
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://��ֵ����
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
                //workbook.Dispose();//һ��ֻ��д��һ����OK�ˣ�����������ͷ�������Դ������ǰ�汾����������ֻ�ͷ�sheet
                return ms;
            }
        }

        /// <summary>
        /// ����Web����
        /// </summary>
        /// <param name="dtSource">ԴDataTable</param>
        /// <param name="strHeaderText">��ͷ�ı�</param>
        /// <param name="strFileName">�ļ���</param>
        public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName)
        {
            HttpContext curContext = HttpContext.Current;

            // ���ñ���͸�����ʽ
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
        /// <summary>��ȡexcel
        /// Ĭ�ϵ�һ��Ϊ��ͷ
        /// </summary>
        /// <param name="strFileName">excel�ĵ�·��</param>
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
        /// ��ȡSheet����Ϣ
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