using System;
using System.Text.Json.Serialization;

namespace Waveboard.Common.Data
{
    public class UpdateSettings
    {
        [JsonPropertyName("file_name")] public string FileName { get; set; } = String.Empty;
#if DEBUG
        [JsonPropertyName("use_http")] public bool UseHttp { get; set; } = false;
#endif
        [JsonPropertyName("url_base")] public string UrlBase { get; set; } = "api.github.com";
        [JsonPropertyName("channel")] public string Channel { get; set; } = "stable";
        [JsonPropertyName("timeout")] public int Timeout { get; set; } = 5;

        public void Validate()
        {
            if (FileName.Length < 1)
            {
                FileName = PlatformUtil.IsWindows ? "WaveboardUpdate.exe" : "WaveboardUpdate";
            }
        }
    }
}