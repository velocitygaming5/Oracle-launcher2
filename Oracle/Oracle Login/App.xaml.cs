using System.Windows;

namespace Oracle_Login
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length != 0)
                OracleLogin.LoggedOut = bool.Parse(e.Args[0]);
        }
    }
}
