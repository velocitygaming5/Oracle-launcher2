using Oracle_Launcher.Oracle;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Oracle_Launcher.Controls
{
    /// <summary>
    /// Interaction logic for ExpansionMenuRow.xaml
    /// </summary>
    public partial class ExpansionMenuRealmRow : UserControl
    {
        private string RealmName;
        private string Realmlist;
        private int Port;

        public ExpansionMenuRealmRow(string _realmName, string _realmlist, int _port)
        {
            InitializeComponent();
            RealmName = _realmName;
            Realmlist = _realmlist;
            Port = _port;
        }

        private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RealmNameTextBlock.Text = RealmName;

            var pendingAnim = AnimHandler.SpinForever(RealmStatusIcon);

            await SetRealmstatusIcon();

            pendingAnim.Stop();
        }

        private async Task SetRealmstatusIcon()
        {
            if (await Task.Run(() => RealmHandler.GetRealmStatus(Realmlist, Port, 2500)))
                ToolHandler.SetImageSource(RealmStatusIcon, "../Assets/Menu Icons/realm_up.png", UriKind.Relative);
            else
                ToolHandler.SetImageSource(RealmStatusIcon, "../Assets/Menu Icons/realm_down.png", UriKind.Relative);
        }
    }
}
