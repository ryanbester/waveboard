using System;
using System.IO;
using System.Text.Json;

namespace Waveboard.Common.Data
{
    public static class ConfigFile<T> where T : new()
    {
        public static T ReadConfigFile(string path)
        {
            if (!File.Exists(path)) return new T();

            var json = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<T>(json);
            return data ?? new T();
        }

        public static T ReadOrCreateConfigFile(string path, T config)
        {
            throw new NotImplementedException();
        }
    }
}