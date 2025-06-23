using System.Text.Json.Serialization;

namespace Anduin.AnduinOSHome.Models;

public class VersionInfo
{
    [JsonPropertyName("isVisible")]
    public required bool IsVisible { get; set; }

    [JsonPropertyName("largeCard")]
    public required bool LargeCard { get; set; }

    [JsonPropertyName("version")]
    public required string Version { get; set; }

    [JsonPropertyName("latest")]
    public required string Latest { get; set; }

    [JsonPropertyName("codename")]
    public required string Codename { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("size")]
    public required string Size { get; set; }

    [JsonPropertyName("support")]
    public required string Support { get; set; }

    [JsonPropertyName("gnome")]
    public required string Gnome { get; set; }

    [JsonPropertyName("packages")]
    public required string Packages { get; set; }

    [JsonPropertyName("kernel")]
    public required string Kernel { get; set; }

    [JsonPropertyName("releaseDate")]
    public required string ReleaseDate { get; set; }

    [JsonPropertyName("additionalText")]
    public string? AdditionalText { get; set; }

    [JsonPropertyName("recommended")]
    public required bool Recommended { get; set; }

    [JsonPropertyName("notRecommended")]
    public required bool NotRecommended { get; set; }
}
