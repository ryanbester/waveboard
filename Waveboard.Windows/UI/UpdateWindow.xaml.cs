using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Waveboard.Common;
using Waveboard.Common.Data;

namespace Waveboard.UI
{
    public partial class UpdateWindow : Window
    {
        public UpdateWindow()
        {
            InitializeComponent();
        }


        private void UpdateWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                var updateSettings = ConfigFile<UpdateSettings>.ReadConfigFile("update.json");
                var updateDetails = UpdateManager.CheckForUpdates(updateSettings);
                if (updateDetails != null)
                {
                    Dispatcher.BeginInvoke(() => { Text.Text = updateDetails.Body; }, DispatcherPriority.Background);
                }
            });
        }
    }
}