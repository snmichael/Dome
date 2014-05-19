using System.Collections.Generic;
using Demo.Services.Models;

namespace Demo.Redis
{
    public class RedisBase
    {
        public static void AddToList(User entity)
        {
            using (var client = RedisManager.GetClient())
            {
                var clientType = client.As<User>();
                entity.Id = (int)clientType.GetNextSequence();
                clientType.Store(entity);
            }
        }

        public static int GetNextId<T>(T entity)
        {
            using (var client = RedisManager.GetClient())
            {
                var clientType = client.As<T>();
                return (int)clientType.GetNextSequence();
            }
        }

        public static IList<T> GetList<T>()
        {
            using (var client = RedisManager.GetClient())
            {
                var clientType = client.As<T>();
                return clientType.GetAll();
            }
        }
        
    }
}
