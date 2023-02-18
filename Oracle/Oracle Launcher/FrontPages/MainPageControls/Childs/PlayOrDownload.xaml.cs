using Oracle_Launcher.Oracle;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

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

        public async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DownloadGrid.Visibility = Visibility.Hidden;

                bool m_accountIsActive = true;

                // custom check if account is activated
                // if we use the custom feature: account activation
                if (Properties.Settings.Default.AccountActivation)
                {
                    try
                    {
                        var account = AuthClass.AccountIsActivated.FromJson(await AuthClass.IsAccountActivatedJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword));
                        if (account != null)
                            m_accountIsActive = account.IsActivated;
                        else
                            m_accountIsActive = false;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.AskToReport(ex, "PlayOrDownload.xaml.cs", "IsAccountActivated");
                        m_accountIsActive = false;
                    }
                }

                if (!m_accountIsActive)
                {
                    // if account is not activated
                    PlayOrDownloadButtonSettings.IsEnabled = false;
                    PlayOrDownloadButton.IsEnabled = false;
                    PlayOrDownloadButton.Content = "PLEASE ACTIVATE";
                    GAME_STATE = (int)STATE_ENUM.INVALID_PATH;
                    InfoBlock.Foreground = ToolHandler.GetColorFromHex("#FFFFFFFF");
                    InfoBlock.Text = "Please activate your account first!";

                    // add a timer to check every 30 seconds if the account is activated

                    return;
                }

                //check if path to wow folder is set
                if (!ClientHandler.IsValidExpansionPath(ExpansionID, ClientHandler.GetExpansionPath(ExpansionID)))
                {
                    PlayOrDownloadButtonSettings.IsEnabled = true;
                    PlayOrDownloadButton.IsEnabled = false;
                    PlayOrDownloadButton.Content = "PATH NOT SET";
                    InfoBlock.Foreground = ToolHandler.GetColorFromHex("#FFFD8383");
                    InfoBlock.Text = "Invalid expansion path selected!";
                    GAME_STATE = (int)STATE_ENUM.INVALID_PATH;

                    return; // skip anything below
                }

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
                InfoBlock.Text = "Please update your game installation!";
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "PlayOrDownload.xaml.cs", "UserControl_Loaded");
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
                            // Fix Read Only Permissions On WoW Folder
                            Process cmd = new Process();
                            cmd.StartInfo.FileName = "cmd.exe";
                            cmd.StartInfo.RedirectStandardInput = true;
                            cmd.StartInfo.RedirectStandardOutput = true;
                            cmd.StartInfo.CreateNoWindow = true;
                            cmd.StartInfo.UseShellExecute = false;
                            cmd.Start();
                            cmd.StandardInput.WriteLine("taskkill /im wow.exe");
                            cmd.StandardInput.WriteLine("cd /d " + @ClientHandler.GetExpansionPath(ExpansionID));
                            cmd.StandardInput.WriteLine("attrib -r * /s");
                            cmd.StandardInput.Flush();
                            cmd.StandardInput.Close();
                            cmd.WaitForExit();
                            // move non whitelisted mpq patches to backup folder
                            ClientHandler.MoveNonWhitelistedPatchesToBackupFolder(ExpansionID);
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
                ExceptionHandler.AskToReport(ex, "PlayOrDownload.xaml.cs", "PlayOrDownloadButton_Click");
            }
        }

        private void PlayOrDownloadButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    fbd.SelectedPath = ClientHandler.GetExpansionPath(ExpansionID);

                    fbd.Description = $"Create or Select your World of Warcraft { ClientHandler.GetExpansionName(ExpansionID).ToUpper() } folder";

                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                    if (result.ToString() == "OK" && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        ClientHandler.SaveExpansionPath(ExpansionID, fbd.SelectedPath);
                        ClientHandler.SetLocalUpdateVersion(ExpansionID, 0);
                        UserControl_Loaded(sender, e);
                        // Fix Read Only Permissions On WoW Folder
                        Process cmd = new Process();
                        cmd.StartInfo.FileName = "cmd.exe";
                        cmd.StartInfo.RedirectStandardInput = true;
                        cmd.StartInfo.RedirectStandardOutput = true;
                        cmd.StartInfo.CreateNoWindow = true;
                        cmd.StartInfo.UseShellExecute = false; 
                        cmd.Start();
                        cmd.StandardInput.WriteLine("taskkill /im wow.exe");
                        cmd.StandardInput.WriteLine("cd /d " + @fbd.SelectedPath);
                        cmd.StandardInput.WriteLine("attrib -r * /s");
                        cmd.StandardInput.Flush();
                        cmd.StandardInput.Close();
                        cmd.WaitForExit();
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "PlayOrDownload.xaml.cs", "PlayOrDownloadButtonSettings_Click");
            }
        }
    }
}
