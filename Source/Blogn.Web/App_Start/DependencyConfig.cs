﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Blogn.Infrastructure;

namespace Blogn
{
    [ExcludeFromCodeCoverage]
    public static class DependencyConfig
    {
        public static IContainer RegisterDependencyResolvers()
        {
            ContainerBuilder builder = new ContainerBuilder();
            RegisterDependencyMappingDefaults(builder);
            RegisterDependencyMappingOverrides(builder);
            IContainer container = builder.Build();
            // Set Up MVC Dependency Resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            // Set Up WebAPI Resolver
            GlobalConfiguration.Configuration.DependencyResolver=new AutofacWebApiDependencyResolver(container);
            return container;
        }

        private static void RegisterDependencyMappingDefaults(ContainerBuilder builder)
        {
            Assembly coreAssembly = Assembly.GetAssembly(typeof (IStateManager));
            Assembly webAssembly = Assembly.GetAssembly(typeof (MvcApplication));

            builder.RegisterAssemblyTypes(coreAssembly).AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(webAssembly).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterControllers(webAssembly);
            builder.RegisterModule(new AutofacWebTypesModule());
        }

        private static void RegisterDependencyMappingOverrides(ContainerBuilder builder)
        {
            builder.RegisterType<WebSettingManager>().AsImplementedInterfaces().SingleInstance();
        }
    }
}