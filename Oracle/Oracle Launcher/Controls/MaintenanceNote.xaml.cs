using Oracle_Launcher.Oracle;
using System.Windows.Controls;
using System.Windows.Input;

namespace Oracle_Launcher.Controls
{
    /// <summary>
    /// Interaction logic for MaintenanceNote.xaml
    /// </summary>
    public partial class MaintenanceNote : UserControl
    {
        public MaintenanceNote()
        {
            InitializeComponent();
        }

        public void SetText(string _text)
        {
            Note.Text = _text;
            Note.Height = 40;
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Note.Height != 40)
                AnimHandler.ToSpecificHeight(Note, 40);
            else
                AnimHandler.ToSpecificHeight(Note, 190);
        }
    }
}
