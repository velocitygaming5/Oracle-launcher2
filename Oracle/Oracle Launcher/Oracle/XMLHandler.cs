﻿using System;
using System.Diagnostics;
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
                await Task.Run(() => Documents.RemoteConfig.Load(Properties.Settings.Default.XMLDocumentUrl));
                PeriodicallyCheckLauncherVersion();
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private static void PeriodicallyCheckLauncherVersion()
        {
            if (Documents.RemoteConfig.SelectSingleNode("OracleLauncher").Attributes["assembly_version"].Value != System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())
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
    }
}
