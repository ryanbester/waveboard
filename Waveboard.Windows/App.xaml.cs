using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Waveboard.Common;
using SplashScreen = Waveboard.UI.SplashScreen;

namespace Waveboard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            if (e.Args.Contains("--generate-checksums"))
            {
                // Generate dependency checksums
                try
                {
                    DependencyValidation.GenerateChecksums(new[]
                    {
                        "Microsoft.Win32.SystemEvents.dll",
                        "System.Drawing.Common.dll",
                        "Waveboard.Common.dll",
                        "SixLabors.ImageSharp.dll",
                        "Waveboard.Resources.dll",
                    });

                    MessageBox.Show("Successfully generated checksums file");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating checksums file: " + ex.Message);
                }

                Current.Shutdown();
            }

            SplashScreen splash = new SplashScreen();
            splash.ShowDialog();
        }
    }
}