using System;
using System.Reflection;
using System.Windows;
using WebHandler;

namespace Oracle_Launcher.Oracle
{
    class ExceptionHandler
    {
        public static async void AskToReport(Exception ex, string fileName, string whereAt)
        {
            MessageBoxResult mBoxResult = MessageBox.Show(ex.Message + "\r\n \r\n" + "Launcher file: " + fileName + "\r\n \r\n" + "Where at: " + whereAt,
                "Would you like to send this error?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mBoxResult == MessageBoxResult.Yes)
            {
                await DiscordClass.SendNewIssueReport(OracleLauncher.LoginUsername,
                    Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                    $"\"{fileName}\" at ({whereAt})",
                    ex.Message);
            }
        }
    }
}
