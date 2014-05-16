using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Demo.Based
{
    public class XmlToData
    {
        /// <summary>
        /// 将Xml内容字符串转换成DataSet对象
        /// </summary>
        /// <param name="XmlString">Xml内容字符串</param>
        /// <returns>DataSet对象</returns>
        public static DataSet XmlToDataSet(string XmlString)
        {
            DataSet result;
            if (Base.IsNull(XmlString))
            {
                result = null;
            }
            else
            {
                try
                {
                    DataSet dataSet = new DataSet();
                    StringReader stringReader = new StringReader(XmlString);
                    XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
                    dataSet.ReadXml(xmlTextReader);
                    xmlTextReader.Close();
                    stringReader.Close();
                    stringReader.Dispose();
                    result = dataSet;
                }
                catch
                {
                    result = null;
                }
            }
            return result;
        }
        /// <summary>
        /// 将Xml字符串转换成DataTable对象
        /// 指定DataTable索引
        /// </summary>
        /// <param name="XmlString">Xml字符串</param>
        /// <param name="TableIndex">Table表索引</param>
        /// <returns>DataTable对象</returns>
        public static DataTable XmlToDatatTable(string XmlString, int TableIndex)
        {
            DataSet dataSet = XmlToData.XmlToDataSet(XmlString);
            DataTable result;
            if (dataSet == null)
            {
                result = null;
            }
            else
            {
                DataTable dataTable = dataSet.Tables[TableIndex];
                dataSet.Dispose();
                result = dataTable;
            }
            return result;
        }
        /// <summary>
        /// 将Xml字符串转换成DataTable对象
        /// DataTable索引为0
        /// </summary>
        /// <param name="XmlString">Xml字符串</param>
        /// <returns>DataTable对象</returns>
        public static DataTable XmlToDatatTable(string XmlString)
        {
            return XmlToData.XmlToDatatTable(XmlString, 0);
        }
        /// <summary>
        /// 读取Xml文件信息,并转换成DataSet对象
        /// </summary>
        /// <param name="XmlFile">Xml文件地址</param>
        /// <returns>DataSet对象</returns>
        public static DataSet XmlFileToDataSet(string XmlFile)
        {
            DataSet result;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(XmlFile);
                result = XmlToData.XmlToDataSet(xmlDocument.InnerXml);
            }
            catch
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// 读取Xml文件信息,并转换成DataTable对象
        /// </summary>
        /// <param name="XmlFile">Xml文件路径</param>
        /// <param name="TableIndex">Table索引</param>
        /// <returns>DataTable对象</returns>
        public static DataTable XmlFileToDataTable(string XmlFile, int TableIndex)
        {
            DataSet dataSet = XmlToData.XmlFileToDataSet(XmlFile);
            DataTable result;
            if (dataSet == null)
            {
                result = null;
            }
            else
            {
                DataTable dataTable = dataSet.Tables[TableIndex];
                dataSet.Dispose();
                result = dataTable;
            }
            return result;
        }
        /// <summary>
        /// 读取Xml文件信息,并转换成DataTable对象
        /// </summary>
        /// <param name="XmlFile">Xml文件路径</param>
        /// <returns>DataTable对象</returns>
        public static DataTable XmlFileToDataTable(string XmlFile)
        {
            return XmlToData.XmlFileToDataTable(XmlFile, 0);
        }
    }
}
