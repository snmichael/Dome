using System.Collections.Generic;
using Demo.Core;
using Demo.Services.Models;

namespace Demo.IService
{
    public interface IUserService : IDependency
    {
        void Add(User entity);
        IList<User> Get();
    }
}
