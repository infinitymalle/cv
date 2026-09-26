using System.Text.Json;
using System.Text.Json.Serialization;
using Cv.Api.Endpoints;
using Cv.Api.OpenApi;
using Cv.Api.Security;
using Cv.Application;
using Cv.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(kestrel =>
{
    kestrel.AddServerHeader = false;                 // don't advertise what we run
    kestrel.Limits.MaxRequestBodySize = 16 * 1024;   // read-only API: no big uploads
});

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration, builder.Environment.ContentRootPath)
    .AddApiSecurity(builder.Configuration);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    // Enums as "advanced" rather than 1, and numbers as plain JSON numbers (keeps the OpenAPI types exact).
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
});

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi(options => options.AddOperationTransformer<LanguageParameterTransformer>());
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseExceptionHandler(new ExceptionHandlerOptions
{
    // Bad input (e.g. ?lang=xx) is the client's fault: keep its 400 instead of turning it into a 500.
    StatusCodeSelector = ex => ex is BadHttpRequestException bad ? bad.StatusCode : StatusCodes.Status500InternalServerError,
});
app.UseStatusCodePages();
app.UseApiSecurity();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // /openapi/v1.json
}

app.MapHealthChecks("/health");

var api = app.MapGroup("/api/v1");
api.MapProfileEndpoints();
api.MapProjectEndpoints();
api.MapTimelineEndpoints();
api.MapCourseEndpoints();

app.Run();
