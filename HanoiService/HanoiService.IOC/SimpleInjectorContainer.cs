using HanoiService.Core;
using HanoiService.Core.Interfaces;
using HanoiService.Data.Context;
using HanoiService.Data.Repository;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HanoiService.IOC
{
    public class SimpleInjectorContainer
    {
        public static Container RegisterServices()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.Register<HanoiContext>(Lifestyle.Scoped);
            container.Register<IHanoiExecutionRepository, HanoiLogRepository>();
            container.Register<IHanoiManager, HanoiManager>();


            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();
            return container;
        }
    }
}
