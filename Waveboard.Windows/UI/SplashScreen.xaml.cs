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
using Waveboard.Common.Data;
using Waveboard.Common.Data.Skin;

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
                "0F66F68D0874C5D1742B17497F0D5B92AAA48E04D2D57FC2075DDF04883EECC5"
            },
            {
                "SixLabors.ImageSharp.dll",
                "91C9C5706BE3F7756E13BE121185F163F0FEA51E0F3C8B5A46FA7AEBAC1D26EC"
            },
            {
                "Waveboard.Resources.dll",
                "E2BD162A9D63717B8D71D60F8A61BFAB58F79F97D304A5E0B8DFCA266A63E5F2"
            },
        };

        public SplashScreen()
        {
            InitializeComponent();

            var splashConfig = ConfigFile<SplashConfig>.ReadConfigFile("Assets/splash.json");
            TitleText.Foreground = new SolidColorBrush(ColorExtensions.FromInteger(splashConfig.TitleColor, 255));
            VersionTxt.Foreground =
                new SolidColorBrush(ColorExtensions.FromInteger(splashConfig.VersionColor, 255));
            CopyrightTxt.Foreground =
                new SolidColorBrush(ColorExtensions.FromInteger(splashConfig.CopyrightColor, 255));
            StatusTxt.Foreground = new SolidColorBrush(ColorExtensions.FromInteger(splashConfig.StatusColor, 255));
            ProgressBar.Resources["ProgressForeColor"] =
                ColorExtensions.FromInteger(splashConfig.ProgressForeColor, 255);
            ProgressBar.Resources["ProgressBackColor"] =
                ColorExtensions.FromInteger(splashConfig.ProgressBackColor, 255);

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
                        Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
                }, DispatcherPriority.Background);

#if !DEBUG
                if (!ValidateFiles())
                {
                    Dispatcher.BeginInvoke(Close, DispatcherPriority.Background);
                    return;
                }
#endif

                // Part 2: Checking for updates
                Dispatcher.BeginInvoke(() => { StatusTxt.Text = Waveboard.Resources.Resources.LoadingStatus2; },
                    DispatcherPriority.Background);

                var updateSettings = ConfigFile<UpdateSettings>.ReadConfigFile("update.json");
                if (UpdateManager.CheckForUpdates(updateSettings) != null)
                {
                    MessageBoxResult updateRes = MessageBoxResult.No;
                    Dispatcher.BeginInvoke(() =>
                    {
                        updateRes = MessageBox.Show(this, Waveboard.Resources.Resources.UpdateAvailableMessage,
                            Waveboard.Resources.Resources.UpdateAvailableTitle, MessageBoxButton.YesNo,
                            MessageBoxImage.Asterisk);
                    }, DispatcherPriority.Background).Wait();
                    if (updateRes == MessageBoxResult.Yes)
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            var updateWin = new UpdateWindow();
                            updateWin.Show();
                            Close();
                        }, DispatcherPriority.Background);
                        return;
                    }
                }

                // Part 3: Loading settings
                Dispatcher.BeginInvoke(() => { StatusTxt.Text = Waveboard.Resources.Resources.LoadingStatus3; },
                    DispatcherPriority.Background);
                // TODO: Load settings

                // Part 4: Initialising sound engine
                Dispatcher.BeginInvoke(() => { StatusTxt.Text = Waveboard.Resources.Resources.LoadingStatus4; },
                    DispatcherPriority.Background);
                // TODO: Sound engine initialisation

                // Part 5: Loading sound packs
                Dispatcher.BeginInvoke(() => { StatusTxt.Text = Waveboard.Resources.Resources.LoadingStatus5; },
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