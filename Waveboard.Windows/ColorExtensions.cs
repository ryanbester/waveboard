using MColor = System.Windows.Media.Color;
using DColor = System.Drawing.Color;

namespace Waveboard
{
    public static class ColorExtensions
    {
        public static MColor ToMediaColor(this DColor color)
        {
            return MColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static MColor ToMediaColor(this DColor color, byte alpha)
        {
            return MColor.FromArgb(alpha, color.R, color.G, color.B);
        }

        public static MColor FromInteger(int color)
        {
            var c = DColor.FromArgb(color);
            return c.ToMediaColor();
        }

        public static MColor FromInteger(int color, byte alpha)
        {
            var c = DColor.FromArgb(color);
            return c.ToMediaColor(alpha);
        }
    }
}