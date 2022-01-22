using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace Oracle_Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static class WindowHelper
        {
            public static void BringProcessToFront(Process process)
            {
                IntPtr handle = process.MainWindowHandle;
                if (IsIconic(handle))
                {
                    ShowWindow(handle, SW_RESTORE);
                }

                SetForegroundWindow(handle);
            }

            const int SW_RESTORE = 9;

            [DllImport("User32.dll")]
            private static extern bool SetForegroundWindow(IntPtr handle);
            [DllImport("User32.dll")]
            private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
            [DllImport("User32.dll")]
            private static extern bool IsIconic(IntPtr handle);
        }

        private void ShowWindowOfRunningProcessName(string pname)
        {
            Process[] processRunning = Process.GetProcesses();
            foreach (Process pr in processRunning)
            {
                if (pr.ProcessName.Contains(pname))
                {
                    WindowHelper.BringProcessToFront(pr);
                }
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                OracleLauncher WindowParent = new OracleLauncher();
                WindowParent.SetArguments(e.Args);
            }
            catch
            {
                if (!OracleLauncher.AnotherInstanceExists("Oracle Launcher"))
                {
                    if (!OracleLauncher.AnotherInstanceExists("Oracle Login"))
                        Process.Start("Oracle Login.exe");
                    else
                        ShowWindowOfRunningProcessName("Oracle Login");
                }
                else
                    ShowWindowOfRunningProcessName("Oracle Launcher");
            }
            finally
            {
                Current.Shutdown();
            }
        }
    }
}
