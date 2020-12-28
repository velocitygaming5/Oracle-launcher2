using Oracle_Launcher.Oracle;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Oracle_Launcher.Controls
{
    /// <summary>
    /// Interaction logic for Article.xaml
    /// </summary>
    public partial class Article : UserControl
    {
        private string ImageUrl;
        private string Title;
        private string Date;
        private string URL;

        public Article(string _imageUrl, string _title, string _date, string _url)
        {
            InitializeComponent();
            ImageUrl = _imageUrl;
            Title = _title;
            Date = _date;
            URL = _url;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ToolHandler.SetImageSource(ArticleImage, ImageUrl, UriKind.Absolute);
            ArticleTitle.Text = Title;
            ArticleDate.Text = Date;
        }

        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ArticleBorder.Background = ToolHandler.GetColorFromHex("#FF303440");
        }

        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();

            myLinearGradientBrush.StartPoint = new Point(0, 0);
            myLinearGradientBrush.EndPoint = new Point(1, 0);
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 40, 42, 53), 0.0));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 43, 44, 49), 1.0));

            ArticleBorder.Background = myLinearGradientBrush;
        }

        private void ArticleTitle_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start(URL);
        }
    }
}
