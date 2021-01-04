using Oracle_Launcher.Oracle;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;
using Oracle_Login;
using Oracle_Launcher.Controls;
using System;
using System.Windows.Threading;

namespace Oracle_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for UserPanel.xaml
    /// </summary>
    public partial class UserPanel : UserControl
    {
        public UserPanel()
        {
            InitializeComponent();
        }

        private void UserLogoutButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationWindow confirmation = new ConfirmationWindow("Logout", "Are you sure about that?");
            confirmation.Owner = SystemTray.oracleLauncher;
            if (confirmation.ShowDialog() == true)
            {
                Process.Start(typeof(OracleLogin).Assembly.GetName().Name, "True");
                Application.Current.Shutdown();
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (OracleLauncher.LoginUsername != null)
            {
                AccountName.Text = OracleLauncher.LoginUsername.ToUpper();
                
                // account rank
                foreach (var account in AccountRank.FromJson(await AuthClass.GetAccountRankJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword)))
                {
                    TbRankName.Foreground = ToolHandler.GetColorFromHex($"#FF{account.RankColor}");
                    TbRankName.Text = account.RankName;
                }

                // account state
                foreach (var account in WebHandler.AccountState.FromJson(await AuthClass.GetAccountStateJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword)))
                {
                    switch (account.State)
                    {
                        case "Banned":
                            AccountState.Foreground = ToolHandler.GetColorFromHex("#FFbc0000");
                            break;
                        case "OK":
                            AccountState.Foreground = ToolHandler.GetColorFromHex("#FF00FF00");
                            break;
                        default:
                            break;
                    }
                    AccountState.Text = account.State;
                    //AccountUnbanDate.Text = account.UnbanDate;
                }

                // account balance
                foreach (var balance in AccountBalance.FromJson(await WebClass.GetAccountBalanceJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword)))
                {
                    AccountDonatePoints.Text = balance.DP.ToString();
                    AccountVotePoints.Text = balance.VP.ToString();
                }

                // characters list
                try
                {
                    CharactersListPanel.Children.Clear();

                    int cCount = 0;

                    foreach (var realm in CharacterData.FromJson(await CharClass.GetCharactersListJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword)))
                    {
                        var realmNameRow = new CharRealmNameRow();
                        CharactersListPanel.Children.Add(realmNameRow);

                        foreach (var character in realm)
                        {
                            realmNameRow.SetRealmName(character.Realm);
                            CharactersListPanel.Children.Add(new CharRealmRow(character.Name, character.Level, character.Race, character.Class, character.Gender));
                            cCount++;
                        }
                    }

                    if (cCount == 0)
                        CharactersListPanel.Children.Add(new Label { 
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

            // Reload account side panel
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Start();
            timer.Tick += (_s, _e) =>
            {
                timer.Stop();
                UserControl_Loaded(sender, e);
            };
        }
    }
}
