using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack.Redis;

namespace Demo.Redis
{
    partial class RedisBase : IDisposable
    {
        public const string RedisPath = "127.0.0.1:6379";

        public RedisBase()
        {
        }

        public static PooledRedisClientManager ClientManager;
        private static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            // 支持读写分离，均衡负载 
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                MaxWritePoolSize = 5, // “写”链接池链接数 
                MaxReadPoolSize = 5, // “读”链接池链接数 
                AutoStart = true,
            });
        }

        public IRedisClient GetInstance()
        {
           
            if (ClientManager == null) 
            {
                ClientManager = CreateManager(new string[] { RedisPath }, new string[] { RedisPath });
            }
            return ClientManager.GetClient();
        }

        private bool _mDisposed = false;

        public void Dispose()
        {
            //Dispose(true);
            GC.SuppressFinalize(this);
        }

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!_mDisposed)
        //    {
        //        if (disposing)
        //        {
        //            _redis.Dispose();
        //        }
        //        _mDisposed = true;
        //    }
        //}

        ~RedisBase()
        {
            Dispose();
        }
    }
}