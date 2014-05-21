using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Services.Models;
using Dome.DataBase;

namespace Demo.Component
{
    public static class MappingExtensions
    {

        public static Sys_Users ToEntity(this UserDto dto)
        {
            return Mapper.Map<UserDto, Sys_Users>(dto);
        }

        public static UserDto ToDto(this Sys_Users entity)
        {
            return Mapper.Map<Sys_Users, UserDto>(entity);
        }

        public static IEnumerable<Sys_Users> ToEntity(this IEnumerable<UserDto> dtoLst)
        {
            return Mapper.Map<IEnumerable<UserDto>, IEnumerable<Sys_Users>>(dtoLst);
        }

        public static IEnumerable<UserDto> ToDto(this IEnumerable<Sys_Users> entityLst)
        {
            return Mapper.Map<IEnumerable<Sys_Users>, IEnumerable<UserDto>>(entityLst);
        }
    }
}
