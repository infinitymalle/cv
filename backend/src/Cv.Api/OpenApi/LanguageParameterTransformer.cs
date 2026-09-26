using System.Text.Json.Nodes;
using Cv.Domain.Common;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Cv.Api.OpenApi;

/// <summary>Documents "lang" parameters as the supported codes ("en" | "sv") instead of any string.</summary>
internal sealed class LanguageParameterTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var languageParameters = context.Description.ParameterDescriptions
            .Where(p => (Nullable.GetUnderlyingType(p.Type) ?? p.Type) == typeof(Language))
            .Select(p => p.Name)
            .ToHashSet();

        foreach (var parameter in operation.Parameters ?? [])
        {
            if (parameter.Name is not null && languageParameters.Contains(parameter.Name)
                && parameter is OpenApiParameter { Schema: OpenApiSchema schema } editable)
            {
                schema.Enum = Language.Supported.Select(l => (JsonNode)JsonValue.Create(l.Code)).ToList();
                editable.Description = $"Content language. Defaults to \"{Language.Default.Code}\".";
            }
        }

        return Task.CompletedTask;
    }
}
