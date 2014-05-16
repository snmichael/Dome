using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Demo.EfService;
using Demo.IService;

namespace Dome.Web.Utility
{
    public class Class1
    {
        public void Load()
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(Dal<>)).As(typeof(IDal<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            //上面的那些类如果在单独的工程里,如生成的程序集为AutofacUnitTest，就可以使用
            //Assembly.Load("AutofacUnitTest")获得响应的程序集。如果所有的文件在一个控制台程序里，
            //可以通过Assembly.GetExecutingAssembly();　直接获得相应的程序集。
            Assembly dataAccess = Assembly.Load("AutofacUnitTest");
            builder.RegisterAssemblyTypes(dataAccess)
                    .Where(t => typeof(IDependency).IsAssignableFrom(t) && t.Name.EndsWith("Bll"));
            //RegisterAssemblyTypes方法将实现IDependency接口并已Bll结尾的类都注册了，语法非常的简单。
        }
    }
}
