using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Redis;

namespace Demo.Redis
{
    public class RedisHelper:IDisposable
    {
        protected RedisClient Client = new RedisClient("127.0.0.1", 6379);//redis服务IP和端口
        public void Add(string key, List<Shipper> entity)
        {
            Client.Set(key, entity);
            Client.Get<List<Shipper>>(key).First().CompanyName="王彤";
            var value = Client.Get<List<Shipper>>(key);
        }

        public List<T> GetList<T>(string[] keys)
        {
            return Client.GetAll<T>(keys).Select(m=>m.Value).ToList();
        }

        public T GetEntity<T>(string key)
        {
            return Client.Get<T>(key);
        }

        private bool _mDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_mDisposed)
            {
                if (disposing)
                {
                    Client.Dispose();
                }
                _mDisposed = true;
            }
        }

        ~RedisHelper()
        {
            Dispose(false);
        }
    }
}
