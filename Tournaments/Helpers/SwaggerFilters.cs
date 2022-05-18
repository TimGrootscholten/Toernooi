using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tournaments.Helpers;

public abstract class SwaggerFilters : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (context.SchemaRepository.Schemas == null) return;
        foreach (var (_, value) in from calls in context.SchemaRepository.Schemas
                 where calls.Key != null || calls.Value != null
                 let classType = Type.GetType($"Dtos.{calls.Key}, Dtos")
                 where classType?.BaseType?.Name == "BaseDto"
                 select calls)
        {
            value?.Properties.Remove(value.Properties.Keys.FirstOrDefault(x => x == "created")!);
            value?.Properties.Remove(value.Properties.Keys.FirstOrDefault(x => x == "updated")!);
        }
    }
}