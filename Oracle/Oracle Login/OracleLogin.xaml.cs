using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Oracle_Login.Oracle;
using System.Windows.Media.Animation;
using WebHandler;
using System;

namespace Oracle_Login
{
    /// <summary>
    /// Interaction logic for OracleLogin.xaml
    /// </summary>
    public partial class OracleLogin : Window
    {
        public static bool LoggedOut;

        public OracleLogin()
        {
            if (AnotherInstanceExists())
            {
                //MessageBox.Show("You cannot run more than one instance of this application.");
                Application.Current.Shutdown();
            }

            InitializeComponent();
        }

        public static bool AnotherInstanceExists()
        {
            Process currentRunningProcess = Process.GetCurrentProcess();
            Process[] listOfProcs = Process.GetProcessesByName(currentRunningProcess.ProcessName);

            foreach (Process proc in listOfProcs)
            {
                if ((proc.MainModule.FileName == currentRunningProcess.MainModule.FileName) && (proc.Id != currentRunningProcess.Id))
                    return true;
            }
            return false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginUsernameBox.Focus();

            if (LoggedOut)
            {
                LogoutSavedLogin();
            }
            else
            {
                if (Properties.Settings.Default.RememberLogin)
                {
                    LoginUsernameBox.Text = Properties.Settings.Default.SavedUsername;
                    LoginPasswordBox.Password = Properties.Settings.Default.SavedPassword;
                    CheckBoxSaveLogin.IsChecked = true;

                    AttemptToLogin();
                }
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void LoginUsernameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginUsernameBox.Text == "Username")
                LoginUsernameBox.Text = "";
        }

        private void LoginUsernameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginUsernameBox.Text))
                LoginUsernameBox.Text = "Username";
        }

        private void LoginPasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginPasswordBox.Password == "Password")
                LoginPasswordBox.Password = "";
        }

        private void LoginPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginPasswordBox.Password))
                LoginPasswordBox.Password = "Password";
        }

        private void CheckBoxSaveLogin_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.RememberLogin = true;
            Properties.Settings.Default.SavedUsername = LoginUsernameBox.Text;
            Properties.Settings.Default.SavedPassword = LoginPasswordBox.Password;
            Properties.Settings.Default.Save();
        }

        private void CheckBoxSaveLogin_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.RememberLogin = false;
            Properties.Settings.Default.SavedUsername = string.Empty;
            Properties.Settings.Default.SavedPassword = string.Empty;
            Properties.Settings.Default.Save();
        }

        private void LogoutSavedLogin()
        {
            Properties.Settings.Default.RememberLogin = false;
            Properties.Settings.Default.SavedUsername = string.Empty;
            Properties.Settings.Default.SavedPassword = string.Empty;
            Properties.Settings.Default.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e) => AttemptToLogin();

        private async void AttemptToLogin()
        {
            StartLoginAnimation();

            try
            {
                var loginResponse = AuthClass.LoginResponse.FromJson(await AuthClass.GetLoginReponseJson(LoginUsernameBox.Text, LoginPasswordBox.Password));
                if (loginResponse != null)
                {
                    if (!string.IsNullOrEmpty(loginResponse.Username) && loginResponse.Logged)
                    {
                        Process.Start($"Oracle Launcher.exe", $"\"{ LoginUsernameBox.Text }\" \"{ LoginPasswordBox.Password }\"");
                        Application.Current.Shutdown();
                    }
                    else
                        ErrorBlock.Text = loginResponse.Response;
                }
                else
                    ErrorBlock.Text = "Could not get a response!";
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBoxResult mBoxResult = MessageBox.Show(ex.Message, "Report this error to our developers?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mBoxResult == MessageBoxResult.Yes)
                {
                    await DiscordClass.SendNewIssueReport(LoginUsernameBox.Text,
                        System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                        $"\"{new StackTrace(true).GetFrame(0).GetFileName()}\" at line ({new StackTrace(ex, true).GetFrame(0).GetFileLineNumber()})",
                        ex.Message);
                }
#endif
            }

            StopLoginAnimation();
        }

        Storyboard OracleLogoSB;

        private void StartLoginAnimation()
        {
            OracleLogoSB = AnimHandler.SpinForever(OracleLogo);

            LoginUsernameBox.IsEnabled = false;
            LoginPasswordBox.IsEnabled = false;
            CheckBoxSaveLogin.IsEnabled = false;
            LoginButton.IsEnabled = false;

            ErrorBlock.Text = "";
        }

        private void StopLoginAnimation()
        {
            OracleLogoSB.Stop();

            LoginUsernameBox.IsEnabled = true;
            LoginPasswordBox.IsEnabled = true;
            CheckBoxSaveLogin.IsEnabled = true;
            LoginButton.IsEnabled = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (OracleLogoSB != null)
                {
                    if (OracleLogoSB.GetCurrentState() != ClockState.Active)
                        AttemptToLogin();
                }
                else
                    AttemptToLogin();
            }
        }

        private async void BtnNewAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Properties.Settings.Default.RegisterAccountUrl);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBoxResult mBoxResult = MessageBox.Show(ex.Message, "Report this error to our developers?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mBoxResult == MessageBoxResult.Yes)
                {
                    await DiscordClass.SendNewIssueReport(LoginUsernameBox.Text,
                        System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                        $"\"{new StackTrace(true).GetFrame(0).GetFileName()}\" at line ({new StackTrace(ex, true).GetFrame(0).GetFileLineNumber()})",
                        ex.Message);
                }
#endif
            }
        }

        private async void BtnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Properties.Settings.Default.ResetPasswordUrl);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBoxResult mBoxResult = MessageBox.Show(ex.Message, "Report this error to our developers?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mBoxResult == MessageBoxResult.Yes)
                {
                    await DiscordClass.SendNewIssueReport(LoginUsernameBox.Text,
                        System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                        $"\"{new StackTrace(true).GetFrame(0).GetFileName()}\" at line ({new StackTrace(ex, true).GetFrame(0).GetFileLineNumber()})",
                        ex.Message);
                }
#endif
            }
        }

        private void LoginPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckBoxSaveLogin.IsChecked = false;
        }
    }
}
