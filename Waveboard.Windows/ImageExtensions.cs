using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Waveboard
{
    public static class ImageExtensions
    {
        public static WriteableBitmap ToWriteableBitmap(this Image<Rgba32> img)
        {
            var bmp = new WriteableBitmap(img.Width, img.Height, img.Metadata.HorizontalResolution,
                img.Metadata.VerticalResolution, PixelFormats.Bgra32, null);
            bmp.Lock();

            try
            {
                var backBuf = bmp.BackBuffer;
                for (var y = 0; y < img.Height; y++)
                {
                    var buf = img.GetPixelRowSpan(y);

                    for (var x = 0; x < img.Width; x++)
                    {
                        var backBufPos = backBuf + (y * img.Width + x) * 4;
                        var rgba = buf[x];
                        var color = rgba.A << 24 | rgba.R << 16 | rgba.G << 8 | rgba.B;

                        Marshal.WriteInt32(backBufPos, color);
                    }
                }

                bmp.AddDirtyRect(new Int32Rect(0, 0, img.Width, img.Height));
            }
            finally
            {
                bmp.Unlock();
            }

            return bmp;
        }
    }
}