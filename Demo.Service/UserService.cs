using System;
using System.Collections.Generic;
using Demo.Framework.Data;
using Demo.IService;
using System.Linq;
using Demo.Services.Models;
using Dome.DataBase;

namespace Demo.Service
{
    public class UserService : BaseService, IUserService
    {
        private readonly IRepository<Sys_Users> _sysUsers;

        public UserService(IRepository<Sys_Users> sysUsers)
        {
            _sysUsers = sysUsers;
        }

        public void Add(UserDto entity)
        {
            var model = new Sys_Users();
            model.CreateTime = DateTime.Now;
            model.Department = 1;
            model.Name = entity.Name;
            model.Position = 1;
            _sysUsers.Add(model);
        }

        public IList<UserDto> Get()
        {
            //return RedisBase.GetList<UserDto>();
            var lst = _sysUsers.Table.ToList();
            IList<UserDto> dtoLst = new List<UserDto>();
            foreach (var item in lst)
            {
                var dto = new UserDto();
                dto.UID = item.UID;
                dto.CreateTime = dto.CreateTime;
                dto.Department = dto.Department;
                dto.Name = dto.Name;
                dto.Position = dto.Position;
                dtoLst.Add(dto);
            }
            return dtoLst;

        }
    }
}