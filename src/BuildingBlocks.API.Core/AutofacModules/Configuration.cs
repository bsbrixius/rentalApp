//using Autofac;
//using Autofac.Extensions.DependencyInjection;
//using MediatR.Extensions.Autofac.DependencyInjection.Builder;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;

//namespace BuildingBlocks.API.Core.AutofacModules
//{
//    public static class Configuration
//    {
//        public static IHostBuilder AddAutofacIoC(this IHostBuilder hostBuilder, IConfiguration configuration)
//        {
//            var mediatrConfiguration = MediatRConfigurationBuilder
//            .Create(typeof(RegisterMotorcycleCommand).Assembly)
//            .WithAllOpenGenericHandlerTypesRegistered()
//            .Build();

//            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//            hostBuilder.ConfigureContainer<ContainerBuilder>(
//               builder =>
//               {
//                   builder.RegisterModule(new AppModule(configuration));
//                   builder.RegisterMediatR(mediatrConfiguration);
//               });

//            return hostBuilder;
//        }
//    }
//}
