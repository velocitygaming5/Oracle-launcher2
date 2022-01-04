﻿using Oracle_Launcher.Oracle;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Oracle_Launcher.FrontPages.MainPageControls.Childs
{
    /// <summary>
    /// Interaction logic for Article.xaml
    /// </summary>
    public partial class Article : UserControl
    {
        private string pImageUrl;
        private string pTitle;
        private string pDate;
        private string pURL;

        public Article(string _imageUrl, string _title, string _date, string _url)
        {
            InitializeComponent();
            pImageUrl = _imageUrl;
            pTitle = _title;
            pDate = _date;
            pURL = _url;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ToolHandler.SetImageSource(ArticleImage, pImageUrl, UriKind.Absolute);

                ArticleTitle.Text = pTitle;
                ArticleDate.Text = pDate;
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "Article.xaml.cs", "UserControl_Loaded");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(pURL);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "Article.xaml.cs", "Button_Click");
            }
        }
    }
}
