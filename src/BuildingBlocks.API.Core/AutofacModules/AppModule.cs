using Autofac;
using MediatR;
using System.Reflection;

namespace BuildingBlocks.API.Core.AutofacModules
{
    public class AppModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            //Repositories
            var repositories = Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(x =>
                x.FullName.EndsWith($"Repository") &&
                x.IsInterface == false)
                .ToArray();
            builder.RegisterTypes(repositories).AsImplementedInterfaces();

            //Queries
            var queries = Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(x =>
                x.FullName.EndsWith($"Queries") &&
                x.IsInterface == false)
                .ToArray();
            builder.RegisterTypes(queries).AsImplementedInterfaces();

            //Commands
            var commandHandlers = Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(x =>
                x.FullName.EndsWith($"CommandHandler") &&
                x.IsInterface == false)
                .ToArray();
            builder.RegisterTypes(commandHandlers).AsImplementedInterfaces();

            //DomainEventHandlers
            var domainEventHandlers = Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(x =>
                x.FullName.EndsWith($"DomainEventHandler") &&
                x.IsInterface == false)
                .ToArray();
            builder.RegisterTypes(domainEventHandlers).AsImplementedInterfaces();

            //Validators
            var validators = Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(x => x.Name.EndsWith("Validator")).ToArray();
            builder.RegisterTypes(validators).AsImplementedInterfaces();
        }
    }
}
