using System.Data;

namespace Demo.Framework.Data.Page
{
    /// <summary>
    /// 分页数据集接口
    /// </summary>
    public interface IPaging
    {
        DataSet GetDataSet();

        DataTable GetDataTable();
    }
}
