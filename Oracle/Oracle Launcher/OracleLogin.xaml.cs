using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using WebHandler;
using System;
using Oracle_Launcher.Oracle;
using System.Threading.Tasks;

namespace Oracle_Launcher
{
    /// <summary>
    /// Interaction logic for OracleLogin.xaml
    /// </summary>
    public partial class OracleLogin : Window
    {
        public bool LoggedOut;

        public OracleLogin()
        {
            InitializeComponent();
            ExceptionHandler.CreateStartupLogFile();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginUsernameBox.Focus();

            if (LoggedOut)
            {
                ResetLoginInfo();
                ClearLoginFields();
            }
            else
            {
                if (bool.Parse(XMLHelper.GetSettingValue("remember_me")))
                {
                    if (!string.IsNullOrEmpty(XMLHelper.GetSettingValue("login_user")) && !string.IsNullOrEmpty(XMLHelper.GetSettingValue("login_pass")))
                    {
                        LoginUsernameBox.Text = ToolHandler.Base64Decode(XMLHelper.GetSettingValue("login_user"));
                        LoginPasswordBox.Password = ToolHandler.Base64Decode(XMLHelper.GetSettingValue("login_pass"));
                        CheckBoxSaveLogin.IsChecked = true;
                        AttemptToLogin();
                    }
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

        private void CheckBoxSaveLogin_Checked(object sender, RoutedEventArgs e) => SaveLoginInfo();

        private void CheckBoxSaveLogin_Unchecked(object sender, RoutedEventArgs e) => ResetLoginInfo();

        private void SaveLoginInfo()
        {
            if (!string.IsNullOrEmpty(LoginUsernameBox.Text) && !string.IsNullOrEmpty(LoginPasswordBox.Password))
            {
                XMLHelper.UpdateSettingValue("login_user", ToolHandler.Base64Encode(LoginUsernameBox.Text));
                XMLHelper.UpdateSettingValue("login_pass", ToolHandler.Base64Encode(LoginPasswordBox.Password));
                XMLHelper.UpdateSettingValue("remember_me", "true");
            }
            else
                ResetLoginInfo();
        }

        private void ResetLoginInfo()
        {
            XMLHelper.UpdateSettingValue("login_user", "");
            XMLHelper.UpdateSettingValue("login_pass", "");
            XMLHelper.UpdateSettingValue("remember_me", "false");
        }

        public void ClearLoginFields()
        {
            LoginUsernameBox.Text = "Username";
            LoginPasswordBox.Password= "Password";
        }

        private void Button_Click(object sender, RoutedEventArgs e) => AttemptToLogin();

        private async void AttemptToLogin()
        {
            StartLoginAnimation();

            try
            {
                var loginResponse = AuthClass.LoginResponse.FromJson(await AuthClass.GetLoginReponseJson(
                    ToolHandler.Base64Encode(LoginUsernameBox.Text), ToolHandler.Base64Encode(LoginPasswordBox.Password)));

                if (loginResponse != null)
                {
                    if (!string.IsNullOrEmpty(loginResponse.Username) && loginResponse.Logged)
                    {
                        if (CheckBoxSaveLogin.IsChecked ?? true)
                            SaveLoginInfo();

                        await Task.Delay(1000);

                        Hide();
                        OracleLauncher oracleLauncher = new OracleLauncher();
                        OracleLauncher.LoginUsername = ToolHandler.Base64Encode(loginResponse.Username);
                        OracleLauncher.LoginPassword = ToolHandler.Base64Encode(LoginPasswordBox.Password);
                        oracleLauncher.Show();
                    }
                    else
                        ErrorBlock.Text = loginResponse.Response;
                }
                else
                    ErrorBlock.Text = "Could not get a response, check api configs!";
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "OracleLogin.xaml.cs", "AttemptToLogin");
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

        private void BtnNewAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Properties.Settings.Default.RegisterAccountUrl);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "OracleLogin.xaml.cs", "BtnNewAccount_Click");
            }
        }

        private void BtnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Properties.Settings.Default.ResetPasswordUrl);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "OracleLogin.xaml.cs", "BtnResetPassword_Click");
            }
        }

        private void LoginPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckBoxSaveLogin.IsChecked = false;
        }
    }
}
