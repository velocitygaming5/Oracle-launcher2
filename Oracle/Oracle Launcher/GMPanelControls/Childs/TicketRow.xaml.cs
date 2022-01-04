using Oracle_Launcher.Oracle;
using Oracle_Launcher.OtherWindows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Oracle_Launcher.GMPanelControls.Childs
{
    /// <summary>
    /// Interaction logic for TicketRow.xaml
    /// </summary>
    public partial class TicketRow : UserControl
    {
        private GMPanel pGMPanel;
        public long pTicketId;
        public string pTicketName;
        public long pTicketOnline;
        public long pTicketPlayerRace;
        public long pTicketPlayerClass;
        public long pTicketPlayerGender;
        public long pTicketCreateTime;
        public long pTicketLastModified;
        public string pTicketAsignedTo;
        public string pTicketMessage;
        public string pTicketRealmName;
        public long pTicketRealmId;

        public TicketRow(GMPanel _gmPanel, long _ticketId, string _ticketName, long _ticketOnline,
            long _ticketPlayerRace, long _ticketPlayerClass, long _ticketPlayerGender, string _ticketMessage,
            long _ticketCreateTime, long _ticketLastModified, string _ticketAssignedTo, 
            string _ticketRealmName, long _ticketRealmId)
        {
            InitializeComponent();
            pGMPanel = _gmPanel;
            pTicketId = _ticketId;
            pTicketName = _ticketName;
            pTicketOnline = _ticketOnline;
            pTicketPlayerRace = _ticketPlayerRace;
            pTicketPlayerClass = _ticketPlayerClass;
            pTicketPlayerGender = _ticketPlayerGender;
            pTicketMessage = _ticketMessage;
            pTicketCreateTime = _ticketCreateTime;
            pTicketLastModified = _ticketLastModified;
            pTicketAsignedTo = _ticketAssignedTo;
            pTicketRealmName = _ticketRealmName;
            pTicketRealmId = _ticketRealmId;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TicketName.Text = pTicketName;

            if (pTicketOnline == 0)
            {
                TicketOnline.Text = "Offline";
                TicketOnline.Foreground = ToolHandler.GetColorFromHex("#FFFF0000");
            }
            else
            {
                TicketOnline.Text = "Online";
                TicketOnline.Foreground = ToolHandler.GetColorFromHex("#FF17FF00");
            }

            ToolHandler.SetRaceGenderImage(TicketCharRace, pTicketPlayerRace, pTicketPlayerGender);
            ToolHandler.SetClassImage(TicketCharClass, pTicketPlayerClass);

            TicketMessage.Text = pTicketMessage;

            var tCreateTime = ToolHandler.UnixTimeStampToDateTime(pTicketCreateTime);
            var tLastModified = ToolHandler.UnixTimeStampToDateTime(pTicketLastModified);
            TicketCreateTime.Text = tCreateTime.ToString("dd MMMM yyyy");
            TicketLastModified.Text = tLastModified.ToString("dd MMMM yyyy");

            TicketAsignedTo.Text = pTicketAsignedTo;
            TicketRealmName.Text = $"[{pTicketRealmId}] {pTicketRealmName}";
        }

        private async void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Close Ticket", $"Do you really want to close {pTicketName}'s ticket?", false, false, pGMPanel);
                confirmation.Owner = SystemTray.oracleLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pGMPanel.ShowActionMessage($"Closing player [{pTicketName}]'s ticket with ID {pTicketId}.");

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetTicketCloseJson
                        (
                            OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, pTicketId.ToString(), pTicketRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowTicketsPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "TicketRow.xaml.cs", "BtnClose_Click");
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Delete Ticket", $"Do you really want to delete {pTicketName}'s ticket? This will close and delete the ticket.", false, false, pGMPanel);
                confirmation.Owner = SystemTray.oracleLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pGMPanel.ShowActionMessage($"Closing and deleting player [{pTicketName}]'s ticket with ID {pTicketId}.");

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetTicketCloseJson
                        (
                            OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, pTicketId.ToString(), pTicketRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetTicketDeleteJson
                        (
                            OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, pTicketId.ToString(), pTicketRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowTicketsPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "TicketRow.xaml.cs", "BtnDelete_Click");
            }
        }

        private async void BtnAssign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Assign Ticket", $"Do you really want to assign {pTicketName}'s ticket? " +
                $"Type the GM's character name you would like to be assigned to.", true, false, pGMPanel);
                confirmation.Owner = SystemTray.oracleLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pGMPanel.ShowActionMessage($"Assigning player [{pTicketName}]'s ticket with ID {pTicketId} to GM [{confirmation.TextInserted}].");

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetTicketAssignJson
                        (
                            OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, pTicketId.ToString(), confirmation.TextInserted, pTicketRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowTicketsPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "TicketRow.xaml.cs", "BtnAssign_Click");
            }
        }

        private async void BtnUnAssign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Unassign Ticket", $"Do you really want to unassign {pTicketName}'s ticket?", false, false, pGMPanel);
                confirmation.Owner = SystemTray.oracleLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pGMPanel.ShowActionMessage($"Unassigning player [{pTicketName}]'s ticket with ID {pTicketId}.");

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetTicketUnassignJson
                        (
                            OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, pTicketId.ToString(), pTicketRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowTicketsPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "TicketRow.xaml.cs", "BtnUnAssign_Click");
            }
        }

        private async void BtnReply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Reply to Ticket", $"Do you wish to reply to {pTicketName}'s ticket? " +
                $"This action will also send a mailbox copy of the response and closes the ticket.", true, false, pGMPanel);
                confirmation.Owner = SystemTray.oracleLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pGMPanel.ShowActionMessage($"Closing and replying player [{pTicketName}]'s ticket with ID {pTicketId}.");

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetTicketCloseJson
                        (
                            OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, pTicketId.ToString(), pTicketRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetTicketReplyJson
                        (
                            OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, pTicketName, confirmation.TextInserted, pTicketRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowTicketsPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "TicketRow.xaml.cs", "BtnReply_Click");
            }
        }
    }
}
