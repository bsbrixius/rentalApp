using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace BuildingBlocks.API.Core.Swagger
{
    public static class SwaggerHelper
    {
        public class ControllerInfo
        {
            public Type ControllerType { get; set; }
            public AreaAttribute? Area { get; set; }
        }
        public static IEnumerable<ControllerInfo> GetControllerAreas()
        {
            var assembly = Assembly.GetEntryAssembly(); // Or specify another assembly
            var controllerTypes = assembly.GetTypes()
                                          .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract)
                                          .ToList();

            var controllerInfos = new List<ControllerInfo>();

            foreach (var type in controllerTypes)
            {
                var areaAttribute = type.GetCustomAttribute<AreaAttribute>();

                controllerInfos.Add(new ControllerInfo
                {
                    ControllerType = type,
                    Area = areaAttribute
                });
            }

            return controllerInfos;
        }

        public static void SwaggerDocByArea(this SwaggerGenOptions options)
        {
            var controllerInfos = GetControllerAreas();
            foreach (var controllerInfo in controllerInfos)
            {
                if (controllerInfo.Area != null)
                {
                    options.SwaggerDoc(controllerInfo.Area.RouteValue, new OpenApiInfo
                    {
                        Title = controllerInfo.Area.RouteValue,
                        Version = "v1",//TODO add versioning
                        Description = $"{controllerInfo.Area.RouteValue} - Section"
                    });
                }
            }

            options.DocInclusionPredicate((docName, apiDesc) =>
            {
                var area = apiDesc.ActionDescriptor.RouteValues["area"];
                return docName == area || area is null;
            });
        }

        public static void SwaggerEndpointByArea(this SwaggerUIOptions options)
        {
            var controllerInfos = GetControllerAreas();
            var urls = new List<UrlDescriptor>(options.ConfigObject.Urls ?? Enumerable.Empty<UrlDescriptor>());
            foreach (var controllerInfo in controllerInfos)
            {
                if (controllerInfo.Area != null)
                {
                    urls.Add(new UrlDescriptor { Url = $"{controllerInfo.Area.RouteValue}/swagger.json", Name = controllerInfo.Area.RouteValue });
                }
            }
            options.ConfigObject.Urls = urls;
        }
    }
}
