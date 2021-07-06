using Oracle_Launcher.Oracle;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Oracle_Launcher.FrontPages.MainPageControls.Childs
{
    /// <summary>
    /// Interaction logic for PlayOrDownload.xaml
    /// </summary>
    public partial class PlayOrDownload : UserControl
    {
        public int GAME_STATE;

        public enum STATE_ENUM
        {
            READY,
            NEEDS_UPDATE,
            INVALID_PATH,
            UPDATING
        };

        public int ExpansionID;

        public PlayOrDownload(int _expansionID)
        {
            InitializeComponent();
            DataContext = this;
            ExpansionID = _expansionID;
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DownloadGrid.Visibility = Visibility.Hidden;

                // check if path to wow folder is set
                //if (!ClientHandler.IsValidExpansionPath(ExpansionID, ClientHandler.GetExpansionPath(ExpansionID)))
                //{
                //    PlayOrDownloadButtonSettings.IsEnabled = true;
                //    PlayOrDownloadButton.IsEnabled = false;
                //    PlayOrDownloadButton.Content = "PATH NOT SET";
                //    InfoBlock.Foreground = ToolHandler.GetColorFromHex("#FFFD8383");
                //    InfoBlock.Text = "Invalid expansion path selected!";
                //    GAME_STATE = (int)STATE_ENUM.INVALID_PATH;

                //    return; // skip anything below
                //}

                // check if local and remote update versions match
                if (ClientHandler.GetLocalUpdateVersion(ExpansionID) == ClientHandler.GetRemoteUpdateVersion(ExpansionID))
                {
                    PlayOrDownloadButtonSettings.IsEnabled = true;
                    PlayOrDownloadButton.IsEnabled = true;
                    PlayOrDownloadButton.Content = "PLAY";
                    InfoBlock.Foreground = ToolHandler.GetColorFromHex("#FFA4A4A4");
                    InfoBlock.Text = "Up to date, ready to play";
                    GAME_STATE = (int)STATE_ENUM.READY;

                    return; // skip anything below
                }

                // if local and remote update versions are different
                PlayOrDownloadButtonSettings.IsEnabled = true;
                PlayOrDownloadButton.IsEnabled = true;
                PlayOrDownloadButton.Content = "UPDATE";
                GAME_STATE = (int)STATE_ENUM.NEEDS_UPDATE;
                InfoBlock.Foreground = ToolHandler.GetColorFromHex("#FFFFFFFF");
                InfoBlock.Text = "Please update your game!";
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private async void PlayOrDownloadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (GAME_STATE)
                {
                    case (int)STATE_ENUM.READY:
                        {
                            ClientHandler.StartWoWClient(ExpansionID);

                            switch (Properties.Settings.Default.PlayButtonSettingId)
                            {
                                case 0:
                                    // keep launcher window open
                                    break;
                                case 1: // minimize to taskbar
                                    SystemTray.oracleLauncher.WindowState = WindowState.Minimized;
                                    break;
                                case 2: // minimize to system tray
                                    SystemTray.oracleLauncher.Hide();
                                    SystemTray.notifier.Visible = true;
                                    break;
                                case 3: // shutdown launcher
                                    AppHandler.Shutdown();
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }
                    case (int)STATE_ENUM.NEEDS_UPDATE:
                        {
                            // start update when press the button
                            PlayOrDownloadButtonSettings.IsEnabled = false;
                            PlayOrDownloadButton.IsEnabled = false;
                            InfoBlock.Text = string.Empty;
                            PlayOrDownloadButton.Content = "UPDATING";

                            Downloader downloader = new Downloader(this);
                            await downloader.CreateDownloadList();
                            downloader.DownloadUpdates();
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void PlayOrDownloadButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    fbd.SelectedPath = ClientHandler.GetExpansionPath(ExpansionID);

                    fbd.Description = $"Select your World of Warcraft { ClientHandler.GetExpansionName(ExpansionID).ToUpper() } folder";

                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                    if (result.ToString() == "OK" && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        ClientHandler.SaveExpansionPath(ExpansionID, fbd.SelectedPath);
                        ClientHandler.SetLocalUpdateVersion(ExpansionID, 0);
                        UserControl_Loaded(sender, e);
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }
    }
}
