using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace Oracle_Login
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

        public static bool AnotherInstanceExists(string processName = null)
        {
            Process[] localAll = Process.GetProcesses();

            foreach (var process in localAll)
            {
                if (process.ProcessName.Contains(string.IsNullOrEmpty(processName) ? 
                    System.Reflection.Assembly.GetEntryAssembly().GetName().Name : processName))
                {
                    if (process.Id != Process.GetCurrentProcess().Id)
                        return true;
                }
            }

            return false;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length != 0)
                OracleLogin.LoggedOut = bool.Parse(e.Args[0]);

            //if (AnotherInstanceExists())
            //{
            //    ShowWindowOfRunningProcessName(System.Reflection.Assembly.GetEntryAssembly().GetName().Name);
            //    Current.Shutdown();
            //}
            //else
            //{
            //    //if (AnotherInstanceExists("Oracle Launcher"))
            //    //{
            //        ShowWindowOfRunningProcessName("Oracle Launcher");
            //        Current.Shutdown();
            //    //}
            //}
        }
    }
}
