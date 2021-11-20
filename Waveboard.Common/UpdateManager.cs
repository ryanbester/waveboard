using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using Waveboard.Common.Data;

namespace Waveboard.Common
{
    public static class UpdateManager
    {
        private const string UrlPath = "/repos/ryanbester/waveboard/releases";

        public static GithubRelease CheckForUpdates(UpdateSettings updateSettings)
        {
            updateSettings.Validate();

            var url = "https://" + updateSettings.UrlBase;
#if DEBUG
            if (updateSettings.UseHttp) url = "http://" + updateSettings.UrlBase;
#endif

            using var client = new HttpClient() { Timeout = TimeSpan.FromSeconds(updateSettings.Timeout) };
            var req = new HttpRequestMessage()
            {
                RequestUri = new Uri(Path.Join(url, UrlPath)),
                Method = HttpMethod.Get
            };
            req.Headers.UserAgent.Add(new ProductInfoHeaderValue("waveboard", "1"));

            try
            {
                var sendTask = client.SendAsync(req);
                sendTask.Wait();
                var res = sendTask.Result;
                var jsonTask = res.Content.ReadFromJsonAsync<List<GithubRelease>>();
                jsonTask.Wait();

                var json = jsonTask.Result;
                if (json == null || json.Count < 1)
                {
                    return null;
                }

                foreach (var release in json)
                {
                    // Remove v from version string
                    var verStr = release.TagName.Trim('v');
                    var version = Version.Parse(verStr);

                    if (release.Draft) continue;

                    if (version <= Assembly.GetExecutingAssembly().GetName().Version)
                    {
                        // Version is less, so no updates
                        return null;
                    }

                    if (updateSettings.Channel != "prerelease" && release.Prerelease)
                    {
                        continue;
                    }

                    // Update found
                    return release;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}