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
    /// Interaction logic for NotificationsManager.xaml
    /// </summary>
    public partial class NotificationsManager : UserControl
    {
        public AdminPanel pAdminPanel;
        public int SelectedExpansion;

        public NotificationsManager(AdminPanel _adminPanel)
        {
            InitializeComponent();
            pAdminPanel = _adminPanel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadNotifications();
        }

        public async void LoadNotifications()
        {
            try
            {

                var notificationsCollection = NotificationsClass.NotificationsListAsAdmin.FromJson(
                    await NotificationsClass.GetNotificationsListAsAdminJson(OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, pAdminPanel.SecKey));
                WPNotifications.Children.Clear();
                if (notificationsCollection != null)
                {
                    foreach (var notification in notificationsCollection)
                    {
                        WPNotifications.Children.Add(new NotificationRow(pAdminPanel, this, notification.Id, notification.Mention, notification.Subject, notification.Message));   
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "NotificationsManager.xaml.cs", "LoadNotifications");
            }
        }

        private async void BtnNewNotification_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NotificationEditor editor = new NotificationEditor(pAdminPanel);
                editor.Owner = pAdminPanel;
                if (editor.ShowDialog() == true)
                {
                    pAdminPanel.ShowActionMessage($"Creating notification \"{editor.Subject.Text}\".");

                    var json = NotificationsClass.NotificationCreate.FromJson(await NotificationsClass.GetNotificationCreateResponseJson(
                        OracleLauncher.LoginUsername, OracleLauncher.LoginPassword, pAdminPanel.SecKey,
                        editor.Subject.Text, editor.Message.Text, editor.ImageUrl.Text, editor.RedirectUrl.Text, editor.AccountID.Text));

                    pAdminPanel.ShowActionMessage(json.ResponseMsg);

                    LoadNotifications();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "NotificationsManager.xaml.cs", "BtnNewNotification_Click");
            }
        }
    }
}
