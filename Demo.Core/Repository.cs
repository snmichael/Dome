using System.Web;

namespace Demo.Core
{
    //Repository泛型接口的泛型实现：
    public class Repository<T> : IRepository<T> where T : class
    {
        public Repository()
        {
        }

        #region IRepository<T> Members

        public void Insert(T entity)
        {
            HttpContext.Current.Response.Write("您添加了一个："
                                               + entity.GetType().FullName);
        }

        public void Update(T entity)
        {

            HttpContext.Current.Response.Write("您更新一个:"
                                               + entity.GetType().FullName);
        }

        public void Delete(T entity)
        {
            HttpContext.Current.Response.Write("您删除了一个："
                                               + entity.GetType().FullName);
        }

        #endregion
    }
}