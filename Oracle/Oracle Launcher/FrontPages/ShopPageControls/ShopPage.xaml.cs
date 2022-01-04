﻿using Oracle_Launcher.FrontPages.ShopPageControls.Childs;
using Oracle_Launcher.Oracle;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Oracle_Launcher.FrontPages.ShopPageControls
{
    /// <summary>
    /// Interaction logic for ShopPage.xaml
    /// </summary>
    public partial class ShopPage : UserControl
    {
        public ShopPage()
        {
            InitializeComponent();
        }

        private void BtnReturnHome_Click(object sender, RoutedEventArgs e)
        {
            SPShopRows.Children.Clear();
            Visibility = Visibility.Hidden;
            AnimHandler.FadeIn(SystemTray.oracleLauncher.mainPage, 300);
        }

        public async void LoadShopPage()
        {
            SystemTray.oracleLauncher.mainPage.Visibility = Visibility.Hidden;
            AnimHandler.FadeIn(this, 300);

            try
            {
                // Retrieve realms list first
                var realms = GameMasterClass.RealmsList.FromJson(await GameMasterClass.GetRealmsListJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword));

                ComboBox1_ab.Items.Clear();

                foreach (GameMasterClass.RealmsList realm in realms)
                {
                    ComboBox1_ab.Items.Add(new ComboBoxItem()
                    {
                        Content = realm.RealmName,
                        Tag = realm.RealmId
                    });
                }

                if (ComboBox1_ab.Items.Count != 0)
                {
                    ComboBox1_ab.SelectedIndex = 0;
                    ComboBox1_ab.IsEnabled = true;
                }

                // Retrieve shop list based on realm id
                var shopList = AuthClass.ShopList.FromJson(await AuthClass.GetShopListJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword));

                SPShopRows.Children.Clear();

                if (shopList != null)
                {
                    foreach (var shopItem in shopList)
                    {
                        var shopRow = new ShopRow(shopItem.Id, shopItem.Title, shopItem.Description, shopItem.ImgUrl, shopItem.PriceDp, shopItem.PriceVp, shopItem.Category, shopItem.RealmId);
                        SPShopRows.Children.Add(shopRow);
                        AnimHandler.ScaleIn(shopRow);
                    }

                    SimulateRealmSelection();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "ShopPage.xaml.cs", "LoadShopPage");
            }
        }

        private void BtnBuyDP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Properties.Settings.Default.BuyDPUrl);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "ShopPage.xaml.cs", "BtnBuyDP_Click");
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Search")
                textBox.Text = string.Empty;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Search";
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text == "Search")
                return;

            try
            {
                string searchText = SearchBox.Text.ToLower();

                int.TryParse(((ComboBoxItem)ComboBox1_ab.SelectedItem).Tag.ToString(), out int _realmId);

                if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrWhiteSpace(searchText))
                {
                    foreach (ShopRow shopRow in SPShopRows.Children.OfType<ShopRow>())
                    {
                        if (!shopRow.pTitle.ToLower().Contains(searchText))
                        {
                            shopRow.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            if (shopRow.pRealmId != _realmId)
                                shopRow.Visibility = Visibility.Collapsed;
                            else
                                shopRow.Visibility = Visibility.Visible;
                        }
                    }
                }
                else
                {
                    foreach (ShopRow shopRow in SPShopRows.Children.OfType<ShopRow>())
                    {
                        if (shopRow.pRealmId != _realmId)
                            shopRow.Visibility = Visibility.Collapsed;
                        else
                            shopRow.Visibility = Visibility.Visible;
                    }
                }
            }
            catch
            {

            }
        }

        private void ComboBox1_aa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int.TryParse(((ComboBoxItem)ComboBox1_ab.SelectedItem).Tag.ToString(), out int _realmId);

                foreach (ShopRow shopRow in SPShopRows.Children.OfType<ShopRow>())
                    shopRow.Visibility = Visibility.Visible;

                foreach (ShopRow shopRow in SPShopRows.Children.OfType<ShopRow>())
                {
                    switch (ComboBox1_aa.SelectedIndex)
                    {
                        case 0: // show all
                        {
                            if (shopRow.pRealmId != _realmId)
                                shopRow.Visibility = Visibility.Collapsed;
                            else
                                shopRow.Visibility = Visibility.Visible;

                            break;
                        }
                        default:
                        {
                            if (shopRow.pRealmId != _realmId)
                            {
                                shopRow.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                if (shopRow.pCategory != ComboBox1_aa.SelectedIndex)
                                {
                                    shopRow.Visibility = Visibility.Collapsed;
                                }
                            }

                            break;
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void ComboBox1_ab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SimulateRealmSelection();
        }

        private void SimulateRealmSelection()
        {
            try
            {
                foreach (ShopRow shopRow in SPShopRows.Children.OfType<ShopRow>())
                    shopRow.Visibility = Visibility.Collapsed;

                foreach (ShopRow shopRow in SPShopRows.Children.OfType<ShopRow>())
                {
                    int.TryParse(((ComboBoxItem)ComboBox1_ab.SelectedItem).Tag.ToString(), out int _realmId);

                    if (shopRow.pRealmId == _realmId)
                    {
                        shopRow.Visibility = Visibility.Visible;
                    }
                }
            }
            catch
            {

            }
        }
    }
}
