using Oracle_Launcher.Oracle;
using System.Windows;

namespace Oracle_Launcher
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        public ConfirmationWindow(string _title, string _text)
        {
            InitializeComponent();

            WindowTitle.Text = _title;
            WindowText.Text = _text;
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e) => DialogResult = true;

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AnimHandler.FadeIn(SystemTray.oracleLauncher.OverlayBlur, 300);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AnimHandler.FadeOut(SystemTray.oracleLauncher.OverlayBlur, 300);
        }
    }
}
