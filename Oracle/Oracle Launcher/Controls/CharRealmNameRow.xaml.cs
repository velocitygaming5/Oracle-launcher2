using System.Windows.Controls;

namespace Oracle_Launcher.Controls
{
    /// <summary>
    /// Interaction logic for CharRealmNameRow.xaml
    /// </summary>
    public partial class CharRealmNameRow : UserControl
    {
        public CharRealmNameRow()
        {
            InitializeComponent();
        }

        public void SetRealmName(string name) => RealmName.Text = name;
    }
}
