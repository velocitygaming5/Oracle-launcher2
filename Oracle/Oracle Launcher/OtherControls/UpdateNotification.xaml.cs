using Oracle_Launcher.Oracle;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Oracle_Launcher.OtherControls
{
    /// <summary>
    /// Interaction logic for UpdateNotification.xaml
    /// </summary>
    public partial class UpdateNotification : UserControl
    {
        public UpdateNotification()
        {
            InitializeComponent();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("Oracle Updater.exe");

            AppHandler.Shutdown();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            AppHandler.Shutdown();
        }
    }
}
