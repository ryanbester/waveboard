using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Waveboard.Common.Data.Skin
{
    public class SplashConfig
    {
        [JsonPropertyName("title_color")] public int TitleColor { get; set; } = 16711422;
        [JsonPropertyName("version_color")] public int VersionColor { get; set; } = 16711422;
        [JsonPropertyName("copyright_color")] public int CopyrightColor { get; set; } = 16711422;
        [JsonPropertyName("status_color")] public int StatusColor { get; set; } = 16711422;

        [JsonPropertyName("progress_bar_color")]
        public int ProgressBarColor { get; set; } = 255;
    }

    public static class SplashConfigFile
    {
        public static SplashConfig LoadConfigFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    var data = JsonSerializer.Deserialize<SplashConfig>(json);
                    return data;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }
    }
}