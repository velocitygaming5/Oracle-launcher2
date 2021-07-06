using Oracle_Launcher.GMPanelControls.Pages;
using Oracle_Launcher.Oracle;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Oracle_Launcher.GMPanelControls
{
    /// <summary>
    /// Interaction logic for GMPanel.xaml
    /// </summary>
    public partial class GMPanel : Window
    {
        public GMPanel()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AnimHandler.FadeIn(SystemTray.oracleLauncher.OverlayBlur, 300);
            AnimHandler.FadeIn(this, 1000);

            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new TicketsManager(this));
            ShowActionMessage($"Welcome {OracleLauncher.LoginUsername} buddy, you are a part of the staff. What are you up to?");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AnimHandler.FadeOut(SystemTray.oracleLauncher.OverlayBlur, 300);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnTickets_Click(object sender, RoutedEventArgs e)
        {
            ShowTicketsPage();
            ShowActionMessage("Showing tickets..");
        }

        private void BtnBans_Click(object sender, RoutedEventArgs e)
        {
            ShowBansPage();
            ShowActionMessage("Showing bans..");
        }

        private void BtnMuteLogs_Click(object sender, RoutedEventArgs e)
        {
            ShowMutesPage();
            ShowActionMessage("Showing mute logs..");
        }

        private void BtnPInfo_Click(object sender, RoutedEventArgs e)
        {
            ShowPlayerinfoPage();
            ShowActionMessage("Showing player info tab..");
        }

        public void ShowTicketsPage()
        {
            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new TicketsManager(this));
        }

        public void ShowBansPage()
        {
            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new BansManager(this));
        }

        public void ShowMutesPage()
        {
            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new Pages.MuteLogs(this));
        }

        public void ShowPlayerinfoPage()
        {
            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new Pages.PlayerInfo(this));
        }

        public async void ShowActionMessage(string message)
        {
            try
            {
                SPActionSentMessage.Children.Clear();
                TextBlock labelMessage = new TextBlock()
                {
                    Text = message,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 0, 0),
                    Background = null,
                    Foreground = ToolHandler.GetColorFromHex("#FFD3D697"),
                    FontWeight = FontWeights.Bold,
                    FontFamily = new System.Windows.Media.FontFamily("Open Sans"),
                    TextTrimming = TextTrimming.CharacterEllipsis,
                };
                SPActionSentMessage.Children.Add(labelMessage);
                await AnimHandler.MoveUpAndFadeInThenFadeOut(labelMessage, 3500);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }
    }
}
