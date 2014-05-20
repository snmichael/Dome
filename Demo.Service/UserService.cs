using System.Collections.Generic;
using Demo.Core;
using Demo.IService;
using Demo.Redis;
using Demo.Services.Models;

namespace Demo.Service
{
    public class UserService : IUserService
    {
        public void Add(User entity)
        {
            RedisBase.AddToList(entity);
        }

        public IList<User> Get()
        {
            return RedisBase.GetList<User>();
        }
    }
}