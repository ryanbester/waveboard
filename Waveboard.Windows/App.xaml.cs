using System.Windows;
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
            SplashScreen splash = new SplashScreen();
            splash.ShowDialog();
        }
    }
}