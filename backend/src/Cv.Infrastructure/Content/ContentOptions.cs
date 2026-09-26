using System.ComponentModel.DataAnnotations;

namespace Cv.Infrastructure.Content;

/// <summary>Bound from the "Content" configuration section (appsettings.json or env var Content__RootPath).</summary>
public sealed class ContentOptions
{
    public const string SectionName = "Content";

    /// <summary>Folder containing the CV content files (projects.json, ...).</summary>
    [Required]
    public string RootPath { get; set; } = string.Empty;
}
