using Autofac;
using Autofac.Integration.WebApi;
using GPPDatabase.Repository;
using GPPDatabase.RepositoryCommon;
using GPPDatabase.Service;
using GPPDatabase.ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace GPPDatabase.App_Start
{
    public class ContainerConfig
    {
        
    
        public static void ConfigureContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<PassengerService>().As<IPassengerService>();
            builder.RegisterType<PassengerRepository>().As<IPassengerRepository>();

            IContainer container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }

    }
}