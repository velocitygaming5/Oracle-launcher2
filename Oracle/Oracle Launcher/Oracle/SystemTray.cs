using System.Windows.Forms;

namespace Oracle_Launcher.Oracle
{
    class SystemTray
    {
        public static NotifyIcon notifier = new NotifyIcon();

        public static OracleLauncher oracleLauncher;

        public SystemTray(OracleLauncher _oracleLauncher)
        {
            oracleLauncher = _oracleLauncher;
        }
    }
}
