using Oracle_Launcher.Oracle;
using Oracle_Launcher.Pages;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Oracle_Launcher.Controls
{
    /// <summary>
    /// Interaction logic for NavbarButton.xaml
    /// </summary>
    public partial class NavbarButton : UserControl
    {
        public MainPage mainPage;
        public int ExpansionID = 1;

        public NavbarButton(MainPage _mainPage, int _expansionID)
        {
            InitializeComponent();

            mainPage = _mainPage;
            ExpansionID = _expansionID;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetToolTip();
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            OnExpansionSelected();
        }

        public async void OnExpansionSelected()
        {
            UnhoverSiblings();
            SetHoverState();
            SetWoWLogo();
            mainPage.SetExpansionBackground(ExpansionID);
            // set expansion background

            // clear panel
            mainPage.ExpansionMenuPanel.Children.Clear();

            try // menu items
            {
                foreach (XmlNode node in Documents.RemoteConfig.SelectNodes("OracleLauncher/Expansions/Expansion"))
                {
                    if (int.Parse(node.Attributes["id"].Value) == ExpansionID)
                    {
                        foreach (XmlNode childnode in node.SelectNodes("Menu/Item"))
                        {
                            string menu_icon_name;
                            switch (int.Parse(childnode.Attributes["icon"].Value))
                            {
                                case 1:
                                    menu_icon_name = "chat_bubble";
                                    break;
                                case 2:
                                    menu_icon_name = "patch_notes";
                                    break;
                                case 3:
                                    menu_icon_name = "shopping_cart";
                                    break;
                                case 4:
                                    menu_icon_name = "download_icon";
                                    break;
                                default:
                                    menu_icon_name = "chat_buble";
                                    break;
                            }

                            mainPage.ExpansionMenuPanel.Children.Add(new ExpansionMenuRow(menu_icon_name, childnode.InnerText, childnode.Attributes["url"].Value));
                        }
                    }
                }
            }
            catch
            {

            }

            try // menu realms
            {
                foreach (XmlNode node in Documents.RemoteConfig.SelectNodes("OracleLauncher/Expansions/Expansion"))
                {
                    if (int.Parse(node.Attributes["id"].Value) == ExpansionID)
                    {
                        var realmlist = node.SelectSingleNode("Realms").Attributes["realmlist"].Value;
                        foreach (XmlNode childnode in node.SelectNodes("Realms/Realm"))
                        {
                            mainPage.ExpansionMenuPanel.Children.Add(new ExpansionMenuRealmRow(
                                childnode.InnerText,
                                realmlist, 
                                int.Parse(childnode.Attributes["port"].Value)));
                        }
                    }
                }
            }
            catch
            {

            }

            AnimHandler.MoveUpAndFadeIn(mainPage.ExpansionMenuScrollViewer);

            try // articles
            {
                mainPage.ArticlesPanel.Children.Clear();

                await Task.Delay(300);

                foreach (XmlNode node in Documents.RemoteConfig.SelectNodes("OracleLauncher/Expansions/Expansion"))
                {
                    if (int.Parse(node.Attributes["id"].Value) == ExpansionID)
                    {
                        foreach (XmlNode childnode in node.SelectNodes("Articles/Article"))
                        {
                            var article = new Article(
                                childnode.SelectSingleNode("Image").InnerText,
                                childnode.SelectSingleNode("Title").InnerText,
                                childnode.SelectSingleNode("Date").InnerText,
                                childnode.SelectSingleNode("Url").InnerText);

                            mainPage.ArticlesPanel.Children.Add(article);

                            AnimHandler.MoveUpAndFadeIn(article);
                        }
                    }
                }
            }
            catch
            {

            }

            // play or download buttons
            foreach (var item in mainPage.playOrDownloadGrid.Children.OfType<PlayOrDownload>())
            {
                if (item.ExpansionID == ExpansionID)
                {
                    item.Visibility = Visibility.Visible;
                    AnimHandler.FadeIn(item, 1000);
                }
                else
                {
                    item.Visibility = Visibility.Hidden;
                }
            }
        }

        private void UnhoverSiblings()
        {
            foreach (var btn in mainPage.NavbarPanel.Children.OfType<NavbarButton>())
                if (btn is FrameworkElement)
                    btn.NavButton.IsEnabled = true;
        }

        private void SetHoverState()
        {
            NavButton.IsEnabled = false;
        }

        private void SetToolTip()
        {
            switch (ExpansionID)
            {
                case 1:
                    ToolTip = "World of Warcraft Classic";
                    break;
                case 2:
                    ToolTip = "World of Warcraft Burning Crusade";
                    break;
                case 3:
                    ToolTip = "World of Warcraft Wrath of the Lich King";
                    break;
                case 4:
                    ToolTip = "World of Warcraft Cataclysm";
                    break;
                case 5:
                    ToolTip = "World of Warcraft Mists of Pandaria";
                    break;
                case 6:
                    ToolTip = "World of Warcraft Warlords of Draenor";
                    break;
                case 7:
                    ToolTip = "World of Warcraft Legion";
                    break;
                case 8:
                    ToolTip = "World of Warcraft Battle for Azeroth";
                    break;
                case 9:
                    ToolTip = "World of Warcraft Shadowlands";
                    break;
                default:
                    ToolTip = "World of Warcraft";
                    break;
            }
        }

        private void SetWoWLogo()
        {
            switch (ExpansionID)
            {
                case 1: // classic
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "../Assets/Logos/wow_classic_logo.png", UriKind.Relative);
                    break;
                }
                case 2: // tbc
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "../Assets/Logos/wow_tbc_logo.png", UriKind.Relative);
                    break;
                }
                case 3: // wotlk
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "../Assets/Logos/wow_wotlk_logo.png", UriKind.Relative);
                    break;
                }
                case 4: // cata
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "../Assets/Logos/wow_cata_logo.png", UriKind.Relative);
                    break;
                }
                case 5: // mop
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "../Assets/Logos/wow_mop_logo.png", UriKind.Relative);
                    break;
                }
                case 6: // wod
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "../Assets/Logos/wow_wod_logo.png", UriKind.Relative);
                    break;
                }
                case 7: // legion
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "../Assets/Logos/wow_legion_logo.png", UriKind.Relative);
                    break;
                }
                case 8: // bfa
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "../Assets/Logos/wow_bfa_logo.png", UriKind.Relative);
                    break;
                }
                case 9: // shadowlands
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "../Assets/Logos/wow_sl_logo.png", UriKind.Relative);
                    break;
                }
                default:
                    break;
            }

            AnimHandler.ScaleIn(mainPage.WoWLogo);
        }
    }
}
