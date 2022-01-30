using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Oracle_Launcher.Oracle
{
    class XMLHandler
    {
        public static async Task LoadXMLRemoteConfigAsync()
        {
            try
            {
                await Task.Run(() => Documents.RemoteConfig.Load(Properties.Settings.Default.XMLConfigUrl));
                PeriodicallyCheckLauncherVersion();
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "XMLHandler.cs", "LoadXMLRemoteConfigAsync");
            }
        }

        private static async void PeriodicallyCheckLauncherVersion()
        {
            try
            {
                var LV = WebHandler.FilesListClass.LVersionResponse.FromJson(await WebHandler.FilesListClass.GetLauncherVersionResponseJson());

                if (LV.Version != System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() && 
                    Properties.Settings.Default.CheckForLauncherUpdates)
                {
                    AnimHandler.FadeIn(SystemTray.oracleLauncher.OracleUpdate, 300);
                }

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(60);
                timer.Start();
                timer.Tick += (_s, _e) =>
                {
                    timer.Stop();
                    PeriodicallyCheckLauncherVersion();
                };
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "XMLHandler.cs", "PeriodicallyCheckLauncherVersion");
            }
        }
    }
}
