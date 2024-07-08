﻿using Autofac;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace BuildingBlocks.API.Core.AutofacModules
{
    public class AppModule : Autofac.Module
    {
        public readonly string _assemblyPrefixName;
        public AppModule(IConfiguration configuration)
        {
            _assemblyPrefixName = configuration.GetValue<string>("AssemblyPrefixName");
        }


        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            var assemblyPrefixName = _assemblyPrefixName ?? Assembly.GetEntryAssembly().GetName().Name.Split('.').FirstOrDefault();
            ArgumentNullException.ThrowIfNull(assemblyPrefixName, nameof(assemblyPrefixName));
            var currentDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.StartsWith(assemblyPrefixName)).ToArray();

            builder.RegisterAssemblyTypes(currentDomainAssemblies)
                .Where(x => x.IsInterface == false)
                .AsImplementedInterfaces();
        }
    }
}
