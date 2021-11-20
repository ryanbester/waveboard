using System.Text.Json.Serialization;

namespace Waveboard.Common.Data.Skin
{
    public class SplashConfig
    {
        [JsonPropertyName("title_color")] public int TitleColor { get; set; } = 16711422;
        [JsonPropertyName("version_color")] public int VersionColor { get; set; } = 16711422;
        [JsonPropertyName("copyright_color")] public int CopyrightColor { get; set; } = 16711422;
        [JsonPropertyName("status_color")] public int StatusColor { get; set; } = 16711422;

        [JsonPropertyName("progress_back_color")]
        public int ProgressBackColor { get; set; } = 255;

        [JsonPropertyName("progress_fore_color")]
        public int ProgressForeColor { get; set; } = 16711422;
    }
}