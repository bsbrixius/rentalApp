using Autofac;
using MediatR;
using System.Reflection;

namespace BuildingBlocks.API.Core.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            //Commands
            var commands = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x =>
                x.FullName.EndsWith($"Command") &&
                x.IsInterface == false)
                .ToArray();
            builder.RegisterTypes(commands).AsImplementedInterfaces();

            //DomainEventHandlers
            var domainEventHandlers = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x =>
                x.FullName.EndsWith($"DomainEventHandler") &&
                x.IsInterface == false)
                .ToArray();
            builder.RegisterTypes(domainEventHandlers).AsImplementedInterfaces();

            //Validators
            var validators = System.Reflection.Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.Name.EndsWith("Validator")).ToArray();
            builder.RegisterTypes(validators).AsImplementedInterfaces();
        }
    }
}
