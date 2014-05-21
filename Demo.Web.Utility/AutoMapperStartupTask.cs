using AutoMapper;
using Demo.Framework.Core.Infrastructure;
using Demo.Services.Models;
using Dome.DataBase;

namespace Demo.Web.Utility
{
    public class AutoMapperStartupTask : IStartupTask
    {
        public void Execute()
        {
            //Mapper.CreateMap<ROLE_GROUP, RoleGroupDto>();
            ////CodeMaster
            //Mapper.CreateMap<MemberStatisticsDTO, MemberStatistics>();
            //Mapper.CreateMap<CODE_MASTER, CodeMaster>();

            ////Activity
            //Mapper.CreateMap<Activity, ActivityDTO>();
            //Mapper.CreateMap<ActivityDTO, Activity>();

            //Users
            Mapper.CreateMap<UserDto, Sys_Users>();

        }



        public int Order
        {
            get { return 0; }
        }
    }
}
