using Oracle_Launcher.Oracle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Oracle_Launcher.FrontPages.CharactersMarketControls.Childs
{
    /// <summary>
    /// Interaction logic for ProfessionRow.xaml
    /// </summary>
    public partial class ProfessionRow : UserControl
    {
        int pProfSkill;
        int pProfMinSkill;
        int pProfMaxSkill;

        public ProfessionRow(int _profSkill, int _profMinSkill, int _profMaxSkill)
        {
            InitializeComponent();

            pProfSkill = _profSkill;
            pProfMinSkill = _profMinSkill;
            pProfMaxSkill = _profMaxSkill;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //ProfBar.Minimum = pProfMinSkill;
            ProfBar.Maximum = pProfMaxSkill;
            ProfBar.Value = pProfMinSkill;

            ToolHandler.SetProfessionImage(ProfImage, pProfSkill);

            ProfName.Text = ToolHandler.ProfesionSkillIdToName(pProfSkill);
            ProfMinSkill.Text = pProfMinSkill.ToString();
            ProfMaxSkill.Text = pProfMaxSkill.ToString();
        }
    }
}
