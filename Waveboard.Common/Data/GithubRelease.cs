using System.Text.Json.Serialization;

namespace Waveboard.Common.Data
{
    public class GithubRelease
    {
        [JsonPropertyName("assets_url")] public string AssetsUrl { get; set; }
        [JsonPropertyName("html_url")] public string HtmlUrl { get; set; }
        [JsonPropertyName("tag_name")] public string TagName { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("draft")] public bool Draft { get; set; }
        [JsonPropertyName("prerelease")] public bool Prerelease { get; set; }
        [JsonPropertyName("published_at")] public string PublishedAt { get; set; }
        [JsonPropertyName("body")] public string Body { get; set; }
    }
}