using Swashbuckle.AspNetCore.SwaggerGen;

namespace BuildingBlocks.API.Core.Swagger
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(Microsoft.OpenApi.Models.OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(name => schema.Enum.Add(new Microsoft.OpenApi.Any.OpenApiString(name)));
            }
        }
    }
}
