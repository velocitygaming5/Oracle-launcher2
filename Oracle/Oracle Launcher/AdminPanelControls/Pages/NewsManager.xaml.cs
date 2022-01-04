﻿using Oracle_Launcher.AdminPanelControls.Childs;
using Oracle_Launcher.AdminPanelControls.Windows;
using Oracle_Launcher.Oracle;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using WebHandler;

namespace Oracle_Launcher.AdminPanelControls.Pages
{
    /// <summary>
    /// Interaction logic for NewsManager.xaml
    /// </summary>
    public partial class NewsManager : UserControl
    {
        public AdminPanel pAdminPanel;
        public int SelectedExpansion;

        public NewsManager(AdminPanel _adminPanel)
        {
            InitializeComponent();
            pAdminPanel = _adminPanel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadArticles();
        }

        public async void LoadArticles()
        {
            try // load expansions list
            {
                await Task.Delay(1000);
                var expansionsCollection = Documents.RemoteConfig.SelectNodes("OracleLauncher/Expansions/Expansion");
                SPExpansions.Children.Clear();

                foreach (XmlNode node in expansionsCollection)
                {
                    var nMR = new NewsExpansionRow(this, int.Parse(node.Attributes["id"].Value));
                    SPExpansions.Children.Add(nMR);
                    AnimHandler.MoveUpAndFadeIn300Ms(nMR);
                }

                if (SelectedExpansion == 0)
                {
                    var firstExpansionRow = SPExpansions.Children.OfType<NewsExpansionRow>().FirstOrDefault();
                    firstExpansionRow.LoadArticlesForThisExpansionID(firstExpansionRow.pExpansionId);
                }
                else // reload selected expansion articles
                {
                    foreach (NewsExpansionRow expRow in SPExpansions.Children.OfType<NewsExpansionRow>())
                    {
                        if (expRow.pExpansionId == SelectedExpansion)
                            expRow.LoadArticlesForThisExpansionID(SelectedExpansion);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "NewsManager.xaml.cs", "LoadArticles");
            }
        }

        private async void BtnNewArticle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ArticleEditor editor = new ArticleEditor(pAdminPanel);
                editor.Owner = pAdminPanel;
                if (editor.ShowDialog() == true)
                {
                    pAdminPanel.ShowActionMessage($"Creating article \"{editor.ArticleTitle.Text}\" for expansion \"{ToolHandler.ExpansionIdToName(SelectedExpansion)}\".");

                    var json = NewsClass.ArticleCreate.FromJson(await NewsClass.GetArticleCreateResponseJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, pAdminPanel.SecKey,
                        SelectedExpansion.ToString(), editor.ArticleTitle.Text, editor.ArticleUrl.Text, editor.ImageUrl.Text));

                    pAdminPanel.ShowActionMessage(json.ResponseMsg);

                    LoadArticles();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "NewsManager.xaml.cs", "BtnNewArticle_Click");
            }
        }
    }
}
