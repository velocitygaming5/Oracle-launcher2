using Oracle_Launcher.GMPanelControls.Subchilds;
using Oracle_Launcher.Oracle;
using Oracle_Launcher.OtherWindows;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Oracle_Launcher.GMPanelControls.Childs
{
    /// <summary>
    /// Interaction logic for TicketRow.xaml
    /// </summary>
    public partial class PlayerInfoRow : UserControl
    {
        private GMPanel pGMPanel;
        public string pAccountName;
        public long pAccountId;
        public string pAccountRankColor;
        public string pAccountRankName;
        public string pPlayerName;
        public long pPlayerRace;
        public long pPlayerClass;
        public long pPlayerGender;
        public long pPlayerLevel;
        public string pGuildName;
        public string pRealmName;
        public long pRealmId;
        public string pTotalPlayedTime;
        public long pOnline;
        public string pLastLogout;
        public long pMoney;
        public long pArenaPoints;
        public long pHonorPoints;
        public long pTotalKills;
        public long pTodayKills;
        public long pYesterdayKills;
        public List<GameMasterClass.PInfoAccBanLog> pPInfoAccBanLogs;
        public List<GameMasterClass.PInfoCharBanLog> pPInfoCharBanLogs;
        public List<GameMasterClass.PInfoMuteLog> pPInfoMuteLogs;
        public List<GameMasterClass.VpDp> pInfoBalance;



        public PlayerInfoRow(GMPanel _gmPanel, string _accountName, long _accountId, string _accountRankColor, string _accountRankName, string _playerName,
            long _playerRace, long _playerClass, long _playerGender, long _playerLevel,
            string _guildName, string _realmName, long _realmId, string _totalPlayedTime, long _online, string _lastLogout,
            long _money, long _arenaPoints, long _honorPoints, long _totalKills, long _todayKills,
            long _yesterdayKills, List<GameMasterClass.PInfoAccBanLog> _accBanLogs, List<GameMasterClass.PInfoCharBanLog> _charBanLogs, List<GameMasterClass.PInfoMuteLog> _muteLogs, List<GameMasterClass.VpDp> _vpDp)
        {
            InitializeComponent();
            pGMPanel = _gmPanel;
            pAccountName = _accountName;
            pAccountId = _accountId;
            pAccountRankColor = _accountRankColor;
            pAccountRankName = _accountRankName;
            pPlayerName = _playerName;
            pPlayerRace = _playerRace;
            pPlayerClass = _playerClass;
            pPlayerGender = _playerGender;
            pPlayerLevel = _playerLevel;
            pGuildName = _guildName;
            pRealmName = _realmName;
            pRealmId = _realmId;
            pTotalPlayedTime = _totalPlayedTime;
            pOnline = _online;
            pLastLogout = _lastLogout;
            pMoney = _money;
            pArenaPoints = _arenaPoints;
            pHonorPoints = _honorPoints;
            pTotalKills = _totalKills;
            pTodayKills = _todayKills;
            pYesterdayKills = _yesterdayKills;
            pPInfoAccBanLogs = _accBanLogs;
            pPInfoCharBanLogs = _charBanLogs;
            pPInfoMuteLogs = _muteLogs;
            pInfoBalance = _vpDp;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ToolHandler.IsAlliance(pPlayerRace))
                    ToolHandler.SetImageSource(FactionBackground, "/Oracle Launcher;component/Assets/pinfo/pinfo_alliance_bg.jpg", UriKind.Relative);
                else
                    ToolHandler.SetImageSource(FactionBackground, "/Oracle Launcher;component/Assets/pinfo/pinfo_horde_bg.jpg", UriKind.Relative);

                AccountName.Content = pAccountName;

                AccountID.Content = pAccountId;

                ToolHandler.SetRaceGenderImage(RaceAvatar, pPlayerRace, pPlayerGender);
                ToolHandler.SetClassImage(ClassIcon, pPlayerClass);

                AccountRank.Text = $"[{pAccountRankName}]";
                AccountRank.Foreground = ToolHandler.GetColorFromHex($"#FF{pAccountRankColor}");

                PlayerName.Text = pPlayerName;
                PlayerName.Foreground = ToolHandler.GetPlayerClassColorBrush(pPlayerClass);

                PlayerLevel.Text = pPlayerLevel.ToString();

                if (string.IsNullOrEmpty(pGuildName))
                    GuildName.Visibility = Visibility.Collapsed;
                else
                    GuildName.Text = $"<{pGuildName}>";

                RealmName.Text = pRealmName;
                TotalPlayedTime.Text = pTotalPlayedTime;

                if (pOnline != 0)
                {
                    OnlineStatus.Text = "(Online)";
                    OnlineStatus.Foreground = ToolHandler.GetColorFromHex("#FF23FF00");
                }

                LastTimeLogout.Text = pLastLogout;

                int gold = (int)(pMoney / 10000);
                int silver = (gold / 100) % 100;
                int copper = gold % 100;

                MoneyGold.Content = gold;
                MoneySilver.Content = silver;
                MoneyCopper.Content = copper;

                ArenaPoints.Content = pArenaPoints;
                HonorPoints.Content = pHonorPoints;
                TotalKills.Content = pTotalKills;
                TodayKills.Content = pTodayKills;
                YesterdayKills.Content = pYesterdayKills;

                SPBanMuteLogs.Children.Clear();

                // loop acc ban logs
                foreach (var accBanLog in pPInfoAccBanLogs)
                    SPBanMuteLogs.Children.Add(new PInfoBanLogRow(pGMPanel, "Account", accBanLog.BanDate, accBanLog.Duration, accBanLog.UnbanDate, accBanLog.BannedBy, accBanLog.BanReason));

                // loop char ban logs
                foreach (var charBanLog in pPInfoCharBanLogs)
                    SPBanMuteLogs.Children.Add(new PInfoBanLogRow(pGMPanel, "Character", charBanLog.BanDate, charBanLog.Duration, charBanLog.UnbanDate, charBanLog.BannedBy, charBanLog.BanReason));

                // loop mute logs
                foreach (var muteLog in pPInfoMuteLogs)
                    SPBanMuteLogs.Children.Add(new PInfoMuteLogRow(pGMPanel, muteLog.MuteDate, muteLog.MuteTime, muteLog.MutedBy, muteLog.MuteReason));

                // loop vp dp - not the ideal way
                foreach (var balance in pInfoBalance)
                {
                    VotePoints.Content = balance.VP;
                    DonatePoints.Content = balance.DP;
                }

                AnimHandler.MoveUpAndFadeIn(SPBanMuteLogs);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "PlayerInfoRow.xaml.cs", "UserControl_Loaded");
            }
        }

        private async void BtnKick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Kick player", $"Do you really want to kick {pPlayerName}? Player must be online in order to work.", false, false, pGMPanel);
                confirmation.Owner = SystemTray.oracleLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pGMPanel.ShowActionMessage($"Kicking player [{pPlayerName}] on realm [{pRealmName}].");

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetKickPlayerJson
                        (
                            OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, pPlayerName, pRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowPlayerinfoPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "PlayerInfoRow.xaml.cs", "BtnKick_Click");
            }
        }

        private async void BtnUnstuck_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Unstuck player", $"Do you really want to unstuck {pPlayerName}? Player must be online in order to work.", false, false, pGMPanel);
                confirmation.Owner = SystemTray.oracleLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pGMPanel.ShowActionMessage($"Unstucking [{pPlayerName}] on realm [{pRealmName}].");

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetUnstuckPlayerJson
                        (
                            OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, pPlayerName, pRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowPlayerinfoPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "PlayerInfoRow.xaml.cs", "BtnUnstuck_Click");
            }
        }
    }
}
