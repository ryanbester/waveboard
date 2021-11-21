using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Waveboard.Common
{
    public static class DependencyValidation
    {
        public static void GenerateChecksums(IEnumerable<string> files)
        {
            var checksums = new Dictionary<string, string>();
            foreach (var file in files)
            {
                if (!File.Exists(file))
                {
                    throw new Exception("$Required dependency {file} does not exist");
                }

                using var sha256 = SHA256.Create();
                using var fileStream = File.OpenRead(file);
                var hash = Convert.ToHexString(sha256.ComputeHash(fileStream));
                checksums.Add(file, hash);
            }

            var json = JsonSerializer.Serialize(checksums);
            var password = Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText("password.txt")));
            var signedData = Crypto.SignChecksums(json, password);
            File.WriteAllLines("checksums", new[]
            {
                Convert.ToBase64String(Encoding.UTF8.GetBytes(json)),
                signedData
            });
        }

        public static Dictionary<string, string> GetChecksums()
        {
            var fileData = File.ReadAllLines("checksums");

            if (!Crypto.VerifyChecksums(fileData[0], fileData[1]))
            {
                throw new Exception("Failed to verify checksums");
            }

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(fileData[0]));
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }

        public static void ValidateDependencies(Dictionary<string, string> checksums)
        {
            foreach (var (filename, checksum) in checksums)
            {
                if (!File.Exists(filename))
                {
                    throw new Exception($"Required dependency {filename} does not exist");
                }

                using var sha256 = SHA256.Create();
                using var fileStream = File.OpenRead(filename);
                var hash = Convert.ToHexString(sha256.ComputeHash(fileStream));
                if (!hash.Equals(checksum))
                {
                    throw new Exception($"Checksum validation failed for {filename}");
                }
            }
        }

        private static class Crypto
        {
            private const string PubKey =
                "MIIBCgKCAQEA0+leW7KN/NPHKsQeKeLgw2HDvQVppp2psGA2/ugK4TcaXMWBjCkBXaOCUwnyStXSlAJZJIPErJoXlWEpSNZUQtPIHPxcIVi1s2NuIt0sPMGNfVrZito/jPWQ5zjukr6GG1ReVYFdSi0zCt8mE19ZMBncXoo2gmJKoObxdI4mZndT9uLM4pO5h/3OEWoEIwRRttm7x3Mps7MN787PFUNjnJ7OaNx8vhuXAe43d8Vme/NMTIrWa/r1JIcVWG/dQ2ezV9ifEsgy+7794V7DGvZ4gHJnMNcUCYXhwKys5JEx2esYteNQVkHzjAijl96NI4d04SUBFOfDV8rtymeOxkCqfQIDAQAB";

            public static string SignChecksums(string checksums, string password)
            {
                using var rsa = RSA.Create();

                var data = Encoding.UTF8.GetBytes(checksums);

                var priv = Convert.FromBase64String(File.ReadAllText("private.key"));
                rsa.ImportEncryptedPkcs8PrivateKey(password, priv, out _);

                return Convert.ToBase64String(rsa.SignData(data, HashAlgorithmName.SHA512,
                    RSASignaturePadding.Pkcs1));
            }

            public static bool VerifyChecksums(string checksums, string signature)
            {
                using var rsa = RSA.Create();

                var toVerify = Convert.FromBase64String(checksums);

                var pub = Convert.FromBase64String(PubKey);
                rsa.ImportRSAPublicKey(pub, out _);

                return rsa.VerifyData(toVerify, Convert.FromBase64String(signature), HashAlgorithmName.SHA512,
                    RSASignaturePadding.Pkcs1);
            }
        }
    }
}