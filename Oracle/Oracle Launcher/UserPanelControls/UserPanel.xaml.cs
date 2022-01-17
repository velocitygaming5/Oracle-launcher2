﻿using Oracle_Launcher.Oracle;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;
using Oracle_Login;
using System.Windows.Threading;
using System;
using System.Threading.Tasks;
using Oracle_Launcher.GMPanelControls;
using Oracle_Launcher.OtherWindows;
using Oracle_Launcher.UserPanelControls.Childs;
using Oracle_Launcher.AdminPanelControls;
using Oracle_Launcher.AccountStandingBarControls.Windows;

namespace Oracle_Launcher.UserPanelControls
{
    /// <summary>
    /// Interaction logic for UserPanel.xaml
    /// </summary>
    public partial class UserPanel : UserControl
    {
        public UserPanel()
        {
            InitializeComponent();
            SPGMpanel.Visibility = Visibility.Collapsed;
            SPAdminPanel.Visibility = Visibility.Collapsed;
            PeriodicSessionStatusUpdate();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (OracleLauncher.LoginUsername != null)
            {
                AccountName.Text = OracleLauncher.LoginUsername.ToUpper();
                UpdateAccountAvatar();
                UpdateAccountRankName();
                UpdateGMPanelVisibility();
                UpdateAdminPanelVisibility();
                UpdateAccountStanding();
                UpdateAccountBalance();
                UpdateCharactersList();
            }

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Start();
            timer.Tick += (_s, _e) =>
            {
                timer.Stop();
                UserControl_Loaded(sender, e);
            };
        }

        public void PeriodicSessionStatusUpdate()
        {
            AppHandler.PingMeAlive();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(30);
            timer.Start();
            timer.Tick += (_s, _e) =>
            {
                timer.Stop();
                PeriodicSessionStatusUpdate();
            };
        }

        public async void UpdateAccountAvatar()
        {
            try
            {
                var avatarResponse = AuthClass.SelfAvatarUrl.FromJson(await AuthClass.GetSelfAvatarUrlJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword));

                if (avatarResponse.AvatarUrl.StartsWith("http") || avatarResponse.AvatarUrl.StartsWith("https"))
                {
                    if (await ToolHandler.ImageExistsAtUrl(avatarResponse.AvatarUrl))
                        ToolHandler.SetImageSource(Avatar, avatarResponse.AvatarUrl, UriKind.Absolute);
                }
                else
                {
                    string pathToRelativeRes = avatarResponse.AvatarUrl.Replace("/Oracle Launcher;component/", "");
                    pathToRelativeRes = pathToRelativeRes.Replace(" ", "%20");

                    if (ToolHandler.RelativeResourceExists(pathToRelativeRes))
                        ToolHandler.SetImageSource(Avatar, avatarResponse.AvatarUrl, UriKind.Relative);
                }
            }
            catch
            {
                ExceptionHandler.AskToReport("Could not update account avatar!");
            }
        }

        public async void UpdateAccountRankName()
        {
            try
            {
                var accountRankName = AuthClass.AccountRankName.FromJson(await AuthClass.GetAccountRankNameJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword));
                if (accountRankName != null)
                {
                    TbRankName.Foreground = ToolHandler.GetColorFromHex($"#FF{accountRankName.RankColor}");
                    TbRankName.Text = accountRankName.RankName;
                }
            }
            catch
            {
                ExceptionHandler.AskToReport("Could not update account rank name!");
            }
        }

        public async void UpdateGMPanelVisibility()
        {
            try
            {
                var gmPanelVisibility = AuthClass.GMPanelAccess.FromJson(await AuthClass.GetGMPanelAccessJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword));
                if (gmPanelVisibility.CanAccess)
                {
                    SPGMpanel.Visibility = Visibility.Visible;
                    AnimHandler.MoveUpAndFadeIn(SPGMpanel);
                }
            }
            catch
            {
                ExceptionHandler.AskToReport("Could not update gm panel access!");
            }
        }

        public async void UpdateAdminPanelVisibility()
        {
            try
            {
                var adminPanelVisibility = AuthClass.AdminPanelAccess.FromJson(await AuthClass.GetAdminPanelAccessJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword));
                if (adminPanelVisibility.CanAccess)
                {
                    SPAdminPanel.Visibility = Visibility.Visible;
                    AnimHandler.MoveUpAndFadeIn(SPAdminPanel);
                }
            }
            catch
            {
                ExceptionHandler.AskToReport("Could not update admin panel access!");
            }
        }

        public async void UpdateAccountStanding()
        {
            try
            {
                var account = AuthClass.AccountStanding.FromJson(await AuthClass.GetAccountStandingJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword));
                if (account != null)
                {
                    switch (account.Standing)
                    {
                        case "Suspended":
                            AccountState.Foreground = ToolHandler.GetColorFromHex("#FFC55F5F");
                            RtbBanTimeLeft.Visibility = Visibility.Visible;
                            AnimHandler.ScaleIn(RtbBanTimeLeft);
                            BanTimeLeft.Text = account.BanTineLeft;
                            break;
                        default:
                            AccountState.Foreground = ToolHandler.GetColorFromHex("#FF00FF00");
                            RtbBanTimeLeft.Visibility = Visibility.Collapsed;
                            break;
                    }

                    AccountState.Text = account.Standing;
                    AnimHandler.ScaleIn(AccountState);
                }
            }
            catch
            {
                ExceptionHandler.AskToReport("Could not update account standing!");
            }
        }

        public async void UpdateAccountBalance()
        {
            try
            {
                foreach (var balance in AuthClass.AccountBalance.FromJson(await AuthClass.GetAccountBalanceJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword)))
                {
                    AccountDonatePoints.Text = balance.DP.ToString();
                    AccountVotePoints.Text = balance.VP.ToString();
                    AnimHandler.MoveUpAndFadeIn(AccountDonatePoints);
                    AnimHandler.MoveUpAndFadeIn(AccountVotePoints);
                }
            }
            catch
            {
                //ExceptionHandler.AskToReport("Could not update account balance!");
                // Logs out user if can't update account balance
                Process.Start(typeof(OracleLogin).Assembly.GetName().Name, "True");
                Application.Current.Shutdown();
            }
        }

        public async void UpdateCharactersList()
        {
            try
            {
                var realmsCollection = CharClass.CharacterData.FromJson(await CharClass.GetCharactersListJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword));

                CharactersListPanel.Children.Clear();

                int cCount = 0;

                if (realmsCollection != null)
                {
                    foreach (var realm in realmsCollection)
                    {
                        var realmRow = new RealmRow();
                        CharactersListPanel.Children.Add(realmRow);
                        AnimHandler.FadeIn(realmRow, 500);

                        await Task.Delay(100);

                        foreach (var character in realm)
                        {
                            realmRow.SetRealmName(character.Realm);
                            var characterRow = new CharacterRow(character.Name, character.Level, character.Race, character.Class, character.Gender);

                            CharactersListPanel.Children.Add(characterRow);
                            AnimHandler.MoveUpAndFadeIn300Ms(characterRow);

                            await Task.Delay(100);
                            cCount++;
                        }
                    }
                }

                if (cCount == 0)
                    CharactersListPanel.Children.Add(new Label
                    {
                        Content = "No characters",
                        Foreground = ToolHandler.GetColorFromHex("#FF7A7A7A"),
                        Background = null,
                        FontFamily = new System.Windows.Media.FontFamily("Open Sans"),
                        FontSize = 14,
                        FontWeight = FontWeights.Bold
                    });
            }
            catch
            {
                ExceptionHandler.AskToReport("Could not update characters list!");

                CharactersListPanel.Children.Add(new Label
                {
                    Content = "No characters",
                    Foreground = ToolHandler.GetColorFromHex("#FF7A7A7A"),
                    Background = null,
                    FontFamily = new System.Windows.Media.FontFamily("Open Sans"),
                    FontSize = 14,
                    FontWeight = FontWeights.Bold
                });
            }
        }

        private void UserLogoutButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationWindow confirmation = new ConfirmationWindow("Logout", "Are you sure about that?", false);
            confirmation.Owner = SystemTray.oracleLauncher;
            if (confirmation.ShowDialog() == true)
            {
                Process.Start(typeof(OracleLogin).Assembly.GetName().Name, "True");
                Application.Current.Shutdown();
            }
        }

        private void GMPanelButton_Click(object sender, RoutedEventArgs e)
        {
            GMPanel gMPanel = new GMPanel();
            gMPanel.Owner = SystemTray.oracleLauncher;
            gMPanel.ShowDialog();
        }

        private void AdminPanelButton_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Owner = SystemTray.oracleLauncher;
            adminPanel.ShowDialog();
        }

        private async void Avatar_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                AvatarSelector avatarSelector = new AvatarSelector();
                avatarSelector.Owner = SystemTray.oracleLauncher;
                if (avatarSelector.ShowDialog() == true)
                {
                    ToolHandler.SetImageSource(Avatar, avatarSelector.pAvatarPath, avatarSelector.pAvatarUriKind);
                    await AuthClass.SetSelfAvatar(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, avatarSelector.pAvatarPath);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "UserPanel.xaml.cs", "Avatar_MouseLeftButtonUp");
            }
        }

        private void BtnAccountStanding_Click(object sender, RoutedEventArgs e)
        {
            SinsHistory sinsHistory = new SinsHistory();
            sinsHistory.Owner = SystemTray.oracleLauncher;
            sinsHistory.ShowDialog();
        }
    }
}
