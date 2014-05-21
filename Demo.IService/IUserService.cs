using System.Collections.Generic;
using Demo.Services.Models;

namespace Demo.IService
{
    public interface IUserService 
    {
        void Add(UserDto entity);
        IList<UserDto> Get();
    }
}
