using Oracle_Launcher.Oracle;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Oracle_Launcher.GMPanelControls.Subchilds
{
    /// <summary>
    /// Interaction logic for BanRow.xaml
    /// </summary>
    public partial class PInfoBanLogRow : UserControl
    {
        private GMPanel pGMPanel;
        public string pBanType;
        public string pBanDate;
        public string pBanDuration;
        public string pUnbanDate;
        public string pBannedBy;
        public string pBanReason;

        public PInfoBanLogRow(GMPanel _gmPanel, string _banType, string _banDate, string _banDuration, string _unbanDate, string _bannedBy, string _banReason)
        {
            InitializeComponent();
            pGMPanel = _gmPanel;
            pBanType = _banType;
            pBanDate = _banDate;
            pBanDuration = _banDuration;
            pUnbanDate = _unbanDate;
            pBannedBy = _bannedBy;
            pBanReason = _banReason;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                BanType.Text = pBanType;
                BanDate.Text = pBanDate;
                BanDuration.Text = pBanDuration;
                UnbanDate.Text = pUnbanDate;
                BannedBy.Text = pBannedBy;
                BanReason.Text = pBanReason;
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "PInfoBanLogRow.xaml.cs", "UserControl_Loaded");
            }
        }
    }
}
