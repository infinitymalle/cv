using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Cv.Api.Tests;

/// <summary>Starts the real API in memory, pointed at the test content folder instead of the real one.</summary>
public sealed class CvApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Content:RootPath", Path.Combine(AppContext.BaseDirectory, "TestContent"));
    }
}
