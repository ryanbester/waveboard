using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Waveboard.Common;

namespace Waveboard.UI
{
    public partial class SplashScreen : Window
    {
        // Dependency validation
        private readonly Dictionary<String, String> _dependencies = new()
        {
            {
                "Microsoft.Win32.SystemEvents.dll",
                "2283A220F5CFE9E78BA7105FCB039178BEB79B53B3A9FD9555C5FB72576A0ACE"
            },
            {
                "System.Drawing.Common.dll",
                "07AE3C66A7131303A8C7088C755D4A52DEB6D6FF9A2A437896952960F46FB00C"
            },
            {
                "Waveboard.Common.dll",
                "CF38EED4D72FB62D84B88ED85804223D58BFFF47C0894FB7C463D08965D13BA1"
            },
            {
                "SixLabors.ImageSharp.dll",
                "91C9C5706BE3F7756E13BE121185F163F0FEA51E0F3C8B5A46FA7AEBAC1D26EC"
            },
            {
                "Waveboard.Resources.dll",
                "9EAD5C85907FC0EDE08D6EE052D3013E5FC16D180DC36CDA7A3E0027EFE3F54C"
            },
        };

        public SplashScreen()
        {
            InitializeComponent();

            Image<Rgba32> img =
                WaveboardAssets.GetBitmap("Waveboard.Resources.Assets.splash.png", "Assets/splash.png");

            var bmp = img.ToWriteableBitmap();
            double ratio = (double)img.Width / img.Height;

            var br = new ImageBrush
            {
                ImageSource = bmp
            };
            Width = Height * ratio;
            Background = br;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Animate logo
            IconImg.Opacity = 0;
            TitleText.Opacity = 0;
            DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(1));
            IconImg.BeginAnimation(OpacityProperty, ani);
            TitleText.BeginAnimation(OpacityProperty, ani);

            // Set debug text
#if DEBUG
            DebugTxt.Text = "Local debug build";
#endif

            Task.Run(() =>
            {
                // Part 1: File validation
                Dispatcher.BeginInvoke(() =>
                {
                    VersionTxt.Text = String.Format(Waveboard.Resources.Resources.LoadingVersion,
                        Assembly.GetExecutingAssembly().GetName().Version);
                }, DispatcherPriority.Background);

#if !DEBUG
                if (!ValidateFiles())
                {
                    Dispatcher.BeginInvoke(Close, DispatcherPriority.Background);
                    return;
                }
#endif

                // Part 2: Checking for updates
                Dispatcher.BeginInvoke(() => { StatusText.Text = Waveboard.Resources.Resources.LoadingStatus2; },
                    DispatcherPriority.Background);
                // TODO: Connect to update server

                // Part 3: Loading settings
                Dispatcher.BeginInvoke(() => { StatusText.Text = Waveboard.Resources.Resources.LoadingStatus3; },
                    DispatcherPriority.Background);
                // TODO: Load settings

                // Part 4: Initialising sound engine
                Dispatcher.BeginInvoke(() => { StatusText.Text = Waveboard.Resources.Resources.LoadingStatus4; },
                    DispatcherPriority.Background);
                // TODO: Sound engine initialisation

                // Part 5: Loading sound packs
                Dispatcher.BeginInvoke(() => { StatusText.Text = Waveboard.Resources.Resources.LoadingStatus5; },
                    DispatcherPriority.Background);
                // TODO: Load sound packs

                Dispatcher.BeginInvoke(() =>
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }, DispatcherPriority.Background);
            });
        }

        private bool ValidateFiles()
        {
            try
            {
                DependencyValidation.ValidateDependencies(_dependencies);
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    var errWin = new Error(ex);
                    errWin.Show();
                }, DispatcherPriority.Background);
                return false;
            }

            return true;
        }
    }
}