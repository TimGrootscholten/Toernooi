using System.Reflection;
using Dtos;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tournaments.Helpers;

public class SwaggerFilters : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var calls in context.SchemaRepository.Schemas)
        {
            if (calls.Key == null && calls.Value == null) continue;
            var classType = Type.GetType($"Dtos.{calls.Key}, Dtos");
            if (classType?.BaseType?.Name != "BaseDto") continue;
            calls.Value?.Properties.Remove(calls.Value?.Properties.Keys.FirstOrDefault(x => x == "created")!);
            if (calls.Value == null) continue;
            calls.Value.Properties.Remove(calls.Value?.Properties.Keys.FirstOrDefault(x => x == "updated")!);
        }
    }
}