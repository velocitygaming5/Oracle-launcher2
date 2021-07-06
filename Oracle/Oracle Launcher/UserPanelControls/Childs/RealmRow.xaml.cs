using System.Windows.Controls;

namespace Oracle_Launcher.UserPanelControls.Childs
{
    /// <summary>
    /// Interaction logic for CharRealmNameRow.xaml
    /// </summary>
    public partial class RealmRow : UserControl
    {
        public RealmRow()
        {
            InitializeComponent();
        }

        public void SetRealmName(string name) => RealmName.Text = name;
    }
}
