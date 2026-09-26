using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Cv.Api.Tests;

/// <summary>Starts the real API in memory, pointed at the test content folder instead of the real one.</summary>
public class CvApiFactory : WebApplicationFactory<Program>
{
    protected virtual string ContentPath => Path.Combine(AppContext.BaseDirectory, "TestContent");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Content:RootPath", ContentPath);
    }
}

/// <summary>Starts the API against the repository's real content/ folder.</summary>
public sealed class RealContentApiFactory : CvApiFactory
{
    public static string RealContentPath { get; } = FindRealContentFolder();

    protected override string ContentPath => RealContentPath;

    private static string FindRealContentFolder()
    {
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, "content");
            if (File.Exists(Path.Combine(candidate, "profile.json")))
            {
                return candidate;
            }
        }

        throw new DirectoryNotFoundException("Could not find the repository's content/ folder.");
    }
}
