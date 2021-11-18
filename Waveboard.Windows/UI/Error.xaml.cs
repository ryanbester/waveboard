using System;
using System.Windows;

namespace Waveboard.UI
{
    /// <summary>
    /// Interaction logic for Error.xaml
    /// </summary>
    public partial class Error : Window
    {
        public Error()
        {
            InitializeComponent();
        }

        public Error(Exception ex) : this()
        {
            OverviewTxt.Text = ex.Message;
            DetailsTxt.Text = ex.Message + Environment.NewLine + ex.StackTrace ?? "";
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}