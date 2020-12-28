using System;
using System.Windows;
using System.Windows.Forms;
using WinForms = System.Windows.Forms;

namespace Oracle_Launcher.Oracle
{
    class SystemTray
    {
        public static NotifyIcon notifier = new WinForms.NotifyIcon();

        public static OracleLauncher oracleLauncher;

        public SystemTray(OracleLauncher _oracleLauncher)
        {
            oracleLauncher = _oracleLauncher;
        }
    }
}
