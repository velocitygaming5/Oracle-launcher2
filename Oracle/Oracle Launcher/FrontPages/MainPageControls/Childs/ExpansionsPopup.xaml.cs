using Oracle_Launcher.Oracle;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Oracle_Launcher.FrontPages.MainPageControls.Childs
{
    /// <summary>
    /// Interaction logic for ExpansionsPopup.xaml
    /// </summary>
    public partial class ExpansionsPopup : UserControl
    {
        private MainPage mainPage;

        public ExpansionsPopup(MainPage _mainPage)
        {
            InitializeComponent();
            mainPage = _mainPage;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Documents.RemoteConfig.ChildNodes.Count != 0)
                {
                    try
                    {
                        foreach (XmlNode node in Documents.RemoteConfig.SelectNodes("OracleLauncher/Expansions/Expansion"))
                            ExpansionsPanel.Children.Add(new ExpansionPopupRow(mainPage, int.Parse(node.Attributes["id"].Value)));
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "ExpansionsPopup.xaml.cs", "UserControl_Loaded");
            }
        }
    }
}
