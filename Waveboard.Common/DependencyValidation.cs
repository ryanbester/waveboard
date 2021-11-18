using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Waveboard.Common
{
    public static class DependencyValidation
    {
        public static void ValidateDependencies(Dictionary<string, string> checksums)
        {
            foreach (var (filename, checksum) in checksums)
            {
                if (!File.Exists(filename))
                {
                    throw new Exception($"Required dependency {filename} does not exist");
                }

                using var sha256 = SHA256.Create();
                using FileStream fileStream = File.OpenRead(filename);
                var hash = Convert.ToHexString(sha256.ComputeHash(fileStream));
                if (!hash.Equals(checksum))
                {
                    throw new Exception($"Checksum validation failed for {filename}");
                }
            }
        }
    }
}