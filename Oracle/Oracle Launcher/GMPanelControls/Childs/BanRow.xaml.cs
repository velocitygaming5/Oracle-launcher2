using Oracle_Launcher.Oracle;
using Oracle_Launcher.OtherWindows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Oracle_Launcher.GMPanelControls.Childs
{
    /// <summary>
    /// Interaction logic for BanRow.xaml
    /// </summary>
    public partial class BanRow : UserControl
    {
        private GMPanel pGMPanel;
        public long pBanType;
        public string pAccOrCharName;
        public long pBanDate;
        public long pUnbanDate;
        public string pBannedBy;
        public string pBanReason;
        public string pRealmName;
        public long? pRealmId;

        public BanRow(GMPanel _gmPanel, long _banType, string _accOrCharName, long _banDate, long _unbanDate, string _bannedBy, string _banReason, string _realmName, long? _realmId = 0)
        {
            InitializeComponent();
            pGMPanel = _gmPanel;
            pBanType = _banType;
            pAccOrCharName = _accOrCharName;
            pBanDate = _banDate;
            pUnbanDate = _unbanDate;
            pBannedBy = _bannedBy;
            pBanReason = _banReason;
            pRealmName = _realmName;
            pRealmId = _realmId;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SPAccount.Visibility = Visibility.Collapsed;
            SPCharacter.Visibility = Visibility.Collapsed;
            SPRealm.Visibility = Visibility.Collapsed;

            BanType.Text = pBanType == 1 ? "Account" : "Character";

            if (pBanType == 1)
            {
                SPAccount.Visibility = Visibility.Visible;
                AccName.Text = pAccOrCharName;
            }
            else
            {
                SPCharacter.Visibility = Visibility.Visible;
                CharName.Text = pAccOrCharName;
            }

            var tBanDate = ToolHandler.UnixTimeStampToDateTime(pBanDate);
            BanDate.Text = tBanDate.ToString("dd MMMM yyyy");

            var tUnbanDate = ToolHandler.UnixTimeStampToDateTime(pUnbanDate);
            UnbanDate.Text = tUnbanDate != tBanDate ? tUnbanDate.ToString("dd MMMM yyyy") : "PERMANENT";

            BannedBy.Text = pBannedBy;
            BanReason.Text = pBanReason;

            if (pRealmName != null)
            {
                RealmName.Text = pRealmId != 0 ? $"[{pRealmId}] {pRealmName}" : pRealmName;
                SPRealm.Visibility = Visibility.Visible;
            }
        }

        private async void BtnUnban_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pBanType == 1)
                {
                    ConfirmationWindow confirmation = new ConfirmationWindow("Unban Account", $"Do you wish to lift [{pAccOrCharName}]'s account ban?", false, false, pGMPanel);
                    confirmation.Owner = SystemTray.oracleLauncher;
                    if (confirmation.ShowDialog() == true)
                    {
                        pGMPanel.ShowActionMessage($"Unbaning account [{pAccOrCharName}].");
                        await GameMasterClass.UnbanAccount(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, pAccOrCharName);
                        pGMPanel.ShowBansPage();
                    }
                }
                else
                {
                    ConfirmationWindow confirmation = new ConfirmationWindow("Unban Character", $"Do you wish to lift [{pAccOrCharName}]'s character ban?", false, false, pGMPanel);
                    confirmation.Owner = SystemTray.oracleLauncher;
                    if (confirmation.ShowDialog() == true)
                    {
                        pGMPanel.ShowActionMessage($"Unbaning character [{pAccOrCharName}].");

                        pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                        (
                            await GameMasterClass.GetUnbanCharacterJson
                            (
                                OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, pAccOrCharName, pRealmId.ToString())
                            ).ResponseMsg
                        );

                        pGMPanel.ShowBansPage();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "BanRow.xaml.cs", "BtnUnban_Click");
            }
        }
    }
}
