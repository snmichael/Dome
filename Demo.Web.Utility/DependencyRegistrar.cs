
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Demo.Component.Entity;
using Demo.Component;
using Demo.Data;
using Demo.Define;
using Demo.Framework.Core;
using Demo.Framework.Core.Caching;
using Demo.Framework.Core.Infrastructure;
using Demo.Framework.Data;
using Demo.Framework.Web.Fakes;
using Demo.Service;
namespace Demo.Web.Utility
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            if (HttpContext.Current != null)
            {
                DependencyResolver.SetResolver(new AutofacDependencyResolver(BaseEngine.ApplicationContainer));
            }
            else
            {
                var lifetimeScopeProvider = new StubLifetimeScopeProvider(BaseEngine.ApplicationContainer);
                var resolver = new AutofacDependencyResolver(BaseEngine.ApplicationContainer, lifetimeScopeProvider);
                DependencyResolver.SetResolver(resolver);
            }

            //HTTP context and other related stuff
            builder.Register(c =>
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerHttpRequest();

            //web helper
            //builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerHttpRequest();

            //Register Model Binders
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            //controllers
            //builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            var contextList = typeFinder.FindClassesOfType<BaseDbContext>(true);
            foreach (var type in contextList)
            {
                var dbname = type.Name.Replace("Entities", "");
                if (AppSettings.ConnenctionString(dbname) == null)
                {
                    continue;

                }
                RegisterDB(builder, type, typeFinder);
            }


            //cache manager
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("cache_static").SingleInstance();
            //builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("sdf_cache_per_request").InstancePerHttpRequest();

            ////work context
            //builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerHttpRequest();

            //data
            var data = typeFinder.FindClassesOfType<CrmDbDataAccessBase>().ToList();
            data.ForEach(s => builder.RegisterType(s).AsImplementedInterfaces().InstancePerHttpRequest().PropertiesAutowired());

            //component
            var component = typeFinder.FindClassesOfType<BaseComponent>().ToList();
            component.ForEach(s => builder.RegisterType(s).AsImplementedInterfaces().InstancePerHttpRequest().PropertiesAutowired());

            //services
            var services = typeFinder.FindClassesOfType<BaseService>().ToList();
            services.ForEach(s => builder.RegisterType(s).AsImplementedInterfaces().InstancePerHttpRequest().PropertiesAutowired()
                //.OnActivated(o =>
                //    {
                //        var currentUser = DependencyResolver.Current.GetService<HttpSessionStateBase>()[Consts.SessionKey.CurrentUser] as CurrentUser ;
                //        (o.Instance as BaseService).SetCurrentUser(currentUser);
                //    })
                );

            builder.Register(c =>
            {
                var session = c.Resolve<HttpSessionStateBase>();
                if (session[Consts.SessionKey.CurrentUser] != null)
                {
                    return session[Consts.SessionKey.CurrentUser] as CurrentUser;
                }
                return null;
            }).InstancePerHttpRequest();

            //Server
            //var servers = typeFinder.FindClassesOfType<IContract>(true).ToList();
            //servers.ForEach(s => builder.RegisterType(s).AsImplementedInterfaces().InstancePerHttpRequest().PropertiesAutowired());


            builder.RegisterFilterProvider();

            // builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerHttpRequest();


            ////Register event consumers
            //var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            //foreach (var consumer in consumers)
            //{
            //    builder.RegisterType(consumer)
            //        .As(consumer.FindInterfaces((type, criteria) =>
            //        {
            //            var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
            //            return isMatch;
            //        }, typeof(IConsumer<>)))
            //        .InstancePerHttpRequest();
            //}
            //builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            //builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();

        }

        private void RegisterDB(ContainerBuilder builder, Type type, ITypeFinder typeFinder)
        {
            var entityAssembly = Assembly.Load(type.Namespace);
            builder.RegisterType(type).As(type).Named<DbContext>(type.FullName).InstancePerHttpRequest();
            var a = new Assembly[] { entityAssembly };

            var typeList = typeFinder.FindClassesOfType<BaseEntity>(a, true);
            var tRepository = typeof(EfRepository<>);
            foreach (Type t in typeList)
            {
                var t2 = tRepository.MakeGenericType(t);
                builder.RegisterType(t2).AsImplementedInterfaces().WithParameter(ResolvedParameter.ForNamed<DbContext>(type.FullName)).InstancePerHttpRequest();
            }
        }


        public int Order
        {
            get { return 0; }
        }
    }


    //public class SettingsSource : IRegistrationSource
    //{
    //    static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
    //        "BuildRegistration",
    //        BindingFlags.Static | BindingFlags.NonPublic);

    //    public IEnumerable<IComponentRegistration> RegistrationsFor(
    //            Service service,
    //            Func<Service, IEnumerable<IComponentRegistration>> registrations)
    //    {
    //        var ts = service as TypedService;
    //        if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
    //        {
    //            var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
    //            yield return (IComponentRegistration)buildMethod.Invoke(null, null);
    //        }
    //    }

    //    static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
    //    {
    //        return RegistrationBuilder
    //            .ForDelegate((c, p) => c.Resolve<IConfigurationProvider<TSettings>>().Settings)
    //            .InstancePerHttpRequest()
    //            .CreateRegistration();
    //    }

    //    public bool IsAdapterForIndividualComponents { get { return false; } }
    //}

}
