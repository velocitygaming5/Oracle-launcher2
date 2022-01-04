using Oracle_Launcher.GMPanelControls.Childs;
using Oracle_Launcher.Oracle;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Oracle_Launcher.GMPanelControls.Pages
{
    /// <summary>
    /// Interaction logic for TicketsPage.xaml
    /// </summary>
    public partial class TicketsManager : UserControl
    {
        GMPanel pGmPanel;

        public TicketsManager(GMPanel _gmPanel)
        {
            InitializeComponent();
            pGmPanel = _gmPanel;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var ticketsCollection = GameMasterClass.Tickets.FromJson(await GameMasterClass.GetTicketsListJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword));
                SPTickets.Children.Clear();

                if (ticketsCollection != null)
                {
                    foreach (var ticket in ticketsCollection)
                    {
                        SPTickets.Children.Add(new TicketRow(
                            pGmPanel,
                            ticket.TicketId,
                            ticket.TicketName,
                            ticket.TicketOnline,
                            ticket.TicketRace,
                            ticket.TicketClass,
                            ticket.TicketGender,
                            ticket.TicketMessage,
                            ticket.TicketCreateTime,
                            ticket.TicketLastModified,
                            ticket.TicketAssignedTo,
                            ticket.TicketRealmName,
                            ticket.TicketRealmId));
                    }
                }
                AnimHandler.MoveUpAndFadeIn(SPTickets);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "TicketsManager.xaml.cs", "UserControl_Loaded");
            }

            if (SPTickets.Children.Count == 0)
            {
                SPTickets.Children.Add(new Label() { 
                    Content = "No tickets to read..",
                    Foreground = ToolHandler.GetColorFromHex("#FFFF0000"),
                    FontSize = 14,
                    FontWeight = FontWeights.SemiBold,
                    FontFamily = new System.Windows.Media.FontFamily("Open Sans"),
                    Margin = new Thickness(0, 0, 0, 0)
                });
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text == "Search By")
                return;

            try
            {
                foreach (TicketRow ticket in SPTickets.Children.OfType<TicketRow>())
                {
                    string searchText = SearchBox.Text.ToLower();

                    if (string.IsNullOrEmpty(searchText) || string.IsNullOrWhiteSpace(searchText))
                    {
                        ticket.Visibility = Visibility.Visible;
                        continue;
                    }

                    switch (CBSearchOptions.SelectedIndex)
                    {
                        case 0: // character name
                            if (!ticket.pTicketName.ToLower().Contains(searchText))
                                ticket.Visibility = Visibility.Collapsed;
                            break;
                        case 1: // assigned gm name
                            if (!ticket.pTicketAsignedTo.ToLower().Contains(searchText))
                                ticket.Visibility = Visibility.Collapsed;
                            break;
                        case 2: // realm name
                            if (!ticket.pTicketRealmName.ToLower().Contains(searchText))
                                ticket.Visibility = Visibility.Collapsed;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "TicketsManager.xaml.cs", "SearchBox_TextChanged");
            }
        }

        private void BtnResetSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
            CBSearchOptions.SelectedIndex = 0;
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Search By")
                textBox.Text = string.Empty;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Search By";
        }

        private void SPTickets_LayoutUpdated(object sender, EventArgs e)
        {
            int count = 0;
            foreach (TicketRow ticketRow in SPTickets.Children.OfType<TicketRow>())
                if (ticketRow.Visibility == Visibility.Visible)
                    count++;

            TicketsCount.Text = $"{count} Open Tickets";
        }
    }
}
