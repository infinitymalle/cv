using Cv.Api.Endpoints;
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

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseApiSecurity();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // /openapi/v1.json
}

app.MapHealthChecks("/health");
app.MapGroup("/api/v1").MapProjectEndpoints();

app.Run();
