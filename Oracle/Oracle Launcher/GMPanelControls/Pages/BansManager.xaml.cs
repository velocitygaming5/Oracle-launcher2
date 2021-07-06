using Oracle_Launcher.GMPanelControls.Childs;
using Oracle_Launcher.Oracle;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Oracle_Launcher.GMPanelControls.Pages
{
    /// <summary>
    /// Interaction logic for BansManager.xaml
    /// </summary>
    public partial class BansManager : UserControl
    {
        private GMPanel pGMPanel;

        public BansManager(GMPanel _gmPanel)
        {
            InitializeComponent();
            pGMPanel = _gmPanel;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            int bCount = 0;

            try
            {
                var banTypesCollection = GameMasterClass.AllBansList.FromJson(await GameMasterClass.GetAllBansListJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword));
                SPBans.Children.Clear();

                if (banTypesCollection != null)
                {
                    foreach (var banType in banTypesCollection)
                    {
                        if (banType != null)
                        {
                            foreach (var ban in banType)
                            {
                                SPBans.Children.Add(new BanRow(
                                    pGMPanel,
                                    ban.BanType,
                                    ban.AccOrCharName,
                                    ban.BanDate,
                                    ban.UnbanDate,
                                    ban.BannedBy,
                                    ban.BanReason,
                                    ban.RealmName,
                                    ban.RealmId.GetValueOrDefault(0x0)
                                    ));
                                bCount++;
                            }
                        }
                    }
                }

                AnimHandler.MoveUpAndFadeIn(SPBans);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        public void ReloadBansList(object sender, RoutedEventArgs e) => UserControl_Loaded(sender, e);

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text == "Search By")
                return;

            try
            {
                foreach (BanRow ban in SPBans.Children.OfType<BanRow>())
                {
                    string searchText = SearchBox.Text.ToLower();

                    if (string.IsNullOrEmpty(searchText) || string.IsNullOrWhiteSpace(searchText))
                    {
                        ban.Visibility = Visibility.Visible;
                        continue;
                    }

                    switch (CBSearchOptions.SelectedIndex)
                    {
                        case 0: // account or character name
                            if (!ban.pAccOrCharName.ToLower().Contains(searchText))
                                ban.Visibility = Visibility.Collapsed;
                            break;
                        case 1: // realm name
                            {
                                if (ban.pRealmName != null)
                                {
                                    if (!ban.pRealmName.ToLower().Contains(searchText))
                                        ban.Visibility = Visibility.Collapsed;
                                }
                                else
                                    ban.Visibility = Visibility.Collapsed;
                                break;
                            }
                        case 2: // ban reason
                            if (!ban.pBanReason.ToLower().Contains(searchText))
                                ban.Visibility = Visibility.Collapsed;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void BtnResetSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
            CBSearchOptions.SelectedIndex = 0;
        }

        private async void BtnNewBan_Click(object sender, RoutedEventArgs e)
        {
            AnimHandler.FadeIn(pGMPanel.OverlayBlur, 300);

            try
            {
                NewBan ban = new NewBan() { Owner = SystemTray.oracleLauncher };
                if (ban.ShowDialog() == true)
                {
                    if (ban.pBanType == 0)
                    {
                        pGMPanel.ShowActionMessage($"Banning account [{ban.pAccOrCharacterName}] on realm [{ban.pRealmName}] for {ban.pBanTime} reason: {ban.pBanReason}.");
                        pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                        (
                            await GameMasterClass.GetBanAccountJson
                            (
                                OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, ban.pAccOrCharacterName, ban.pBanTime, ban.pBanReason, ban.pRealmId.ToString())
                            ).ResponseMsg
                        );

                        pGMPanel.ShowBansPage();
                    }
                    else
                    {
                        pGMPanel.ShowActionMessage($"Banning character [{ban.pAccOrCharacterName}] on realm [{ban.pRealmName}] for {ban.pBanTime} reason: {ban.pBanReason}.");
                        pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                        (
                            await GameMasterClass.GetBanCharacterJson
                            (
                                OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, ban.pAccOrCharacterName, ban.pBanTime, ban.pBanReason, ban.pRealmId.ToString())
                            ).ResponseMsg
                        );

                        pGMPanel.ShowBansPage();
                    }

                    pGMPanel.ShowBansPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }

            AnimHandler.FadeOut(pGMPanel.OverlayBlur, 300);
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Search By")
                textBox.Text = string.Empty;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Search By";
        }

        private void SPBans_LayoutUpdated(object sender, EventArgs e)
        {
            int count = 0;
            foreach (BanRow banRow in SPBans.Children.OfType<BanRow>())
                if (banRow.Visibility == Visibility.Visible)
                    count++;

            BansCount.Text = $"{count} Bans";
        }
    }
}
