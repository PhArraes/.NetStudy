[assembly: WebActivator.PostApplicationStartMethod(typeof(HanoiService.Web.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace HanoiService.Web.App_Start
{
    using System.Web.Http;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
    using Data.Context;
    using Core.Interfaces;
    using Data.Repository;
    using Core;
    using Core.Interfaces.Repositories;
    using Core.Interfaces.Services;
    using Core.Services;

    public static class SimpleInjectorWebApiInitializer
    {
        /// <summary>Initialize the container and register it as Web API Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            
            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
       
            container.Verify();
            
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
     
        private static void InitializeContainer(Container container)
        {
            container.Register<HanoiContext>(Lifestyle.Singleton);
            container.Register<IHanoiExecutionRepository, HanoiLogRepository>(Lifestyle.Singleton);
            container.Register<IHanoiHistoryRepository, HanoiLogRepository>(Lifestyle.Singleton);
            container.Register<IHanoiHistoryService, HanoiHistoryService>(Lifestyle.Singleton);
            container.Register<IHanoiManager, HanoiManager>(Lifestyle.Singleton);


            // For instance:
            // container.Register<IUserRepository, SqlUserRepository>(Lifestyle.Scoped);
        }
    }
}