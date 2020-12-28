using System.Windows;

namespace Oracle_Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            OracleLauncher WindowParent = new OracleLauncher();
            WindowParent.SetArguments(e.Args);
        }
    }
}
