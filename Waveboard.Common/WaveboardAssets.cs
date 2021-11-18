using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Waveboard.Common
{
    public static class WaveboardAssets
    {
        public static Image<Rgba32> GetBitmap(string resourcePath, string assetPath)
        {
            Image<Rgba32> img;

            try
            {
                img = Image.Load<Rgba32>(assetPath);
            }
            catch (Exception)
            {
                try
                {
                    img = Image.Load<Rgba32>(
                        Resources.WaveboardResources.ResourceAssembly.GetManifestResourceStream(resourcePath));
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return img;
        }
    }
}