﻿using Oracle_Launcher.FrontPages.VotePageControls.Childs;
using Oracle_Launcher.Oracle;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Oracle_Launcher.FrontPages.VotePageControls
{
    /// <summary>
    /// Interaction logic for VotePage.xaml
    /// </summary>
    public partial class VotePage : UserControl
    {
        public VotePage()
        {
            InitializeComponent();
        }

        private void BtnReturnHome_Click(object sender, RoutedEventArgs e)
        {
            SPVoteRows.Children.Clear();
            Visibility = Visibility.Hidden;
            AnimHandler.FadeIn(SystemTray.oracleLauncher.mainPage, 300);
        }

        public async void LoadVotePage()
        {
            SystemTray.oracleLauncher.mainPage.Visibility = Visibility.Hidden;
            AnimHandler.FadeIn(this, 300);

            SPVoteRows.Children.Clear();

            try
            {
                var voteSitesCollection = AuthClass.VoteSitesList.FromJson(await AuthClass.GetVoteSitesListJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword));

                if (voteSitesCollection != null)
                {
                    foreach (var voteSite in voteSitesCollection)
                    {
                        SPVoteRows.Children.Add(new VoteRow(voteSite.SiteId, voteSite.SiteName, voteSite.CooldownSecLeft, voteSite.ImageUrl, voteSite.VoteUrl, voteSite.Points));
                    }
                    AnimHandler.MoveUpAndFadeIn300Ms(SPVoteRows);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "VotePage.xaml.cs", "LoadVotePage");
            }
        }
    }
}
