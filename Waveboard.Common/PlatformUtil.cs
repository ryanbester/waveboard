using System.Runtime.InteropServices;

namespace Waveboard.Common
{
    public static class PlatformUtil
    {
        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }
}