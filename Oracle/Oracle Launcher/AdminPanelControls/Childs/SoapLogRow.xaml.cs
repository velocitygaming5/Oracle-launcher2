﻿using Oracle_Launcher.Oracle;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Oracle_Launcher.AdminPanelControls.Childs
{
    /// <summary>
    /// Interaction logic for SoapLogRow.xaml
    /// </summary>
    public partial class SoapLogRow : UserControl
    {
        public string pAccountName;
        public string pDateTime;
        public string pRealmName;
        public string pCommand;

        public SoapLogRow(string _accountName, string _dateTime, string _realmName, string _command)
        {
            InitializeComponent();
            pAccountName = _accountName;
            pDateTime = _dateTime;
            pRealmName = _realmName;
            pCommand = _command;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                AccountName.Text = pAccountName;
                DateAndTime.Text = pDateTime;
                RealmName.Text = pRealmName;
                Command.Text = pCommand;
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "SoapLogRow.xaml.cs", "UserControl_Loaded");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ToolHandler.CopyTextBlockTextToClipboard(Command);
        }
    }
}
