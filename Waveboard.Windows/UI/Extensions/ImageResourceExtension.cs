using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Waveboard.UI.Extensions
{
    [ContentProperty (nameof(Source))]
    public class ImageResourceExtension : MarkupExtension
    {
        public ImageResourceExtension(string source)
        {
            Source = source;
        }

        public string Source { get; set;}

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            var bmp = new Bitmap(Waveboard.Resources.WaveboardResources.ResourceAssembly.GetManifestResourceStream(Source));

            return Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}