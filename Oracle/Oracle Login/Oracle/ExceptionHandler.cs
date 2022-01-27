using System;
using System.IO;
using System.Reflection;
using WebHandler;

namespace Oracle_Login.Oracle
{
    class ExceptionHandler
    {
        public static async void AskToReport(Exception ex, string fileName, string whereAt)
        {
            //MessageBoxResult mBoxResult = MessageBox.Show(ex.Message + "\r\n \r\n" + "Launcher file: " + fileName + "\r\n \r\n" + "Where at: " + whereAt,
            //    "Would you like to send this error?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (mBoxResult == MessageBoxResult.Yes)
            //{
            //}

            try
            {
                AddLogLine("Exception error in " + fileName + " at " + whereAt);

                await DiscordClass.SendNewIssueReport("ORACLE LOGIN",
                    Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                    $"\"{fileName}\" at ({whereAt})",
                    ex.Message);
            }
            catch
            {

            }
        }

        private static string LOG_FILE_PATH = @"logs\log.txt";

        public static void AddLogLine(string message)
        {
            var log = $"[{DateTime.Now}]: {message}";

            if (!File.Exists(LOG_FILE_PATH))
            {
                using (StreamWriter sw = File.CreateText(LOG_FILE_PATH))
                    sw.WriteLine(log);
            }
            else
                File.AppendAllLines(LOG_FILE_PATH, new[] { log });
        }

        public static void CreateStartupLogFile()
        {
            try
            {
                if (!Directory.Exists("logs"))
                    Directory.CreateDirectory("logs");

                LOG_FILE_PATH = $@"logs\{Assembly.GetExecutingAssembly().GetName()}_v" +
                    $"{Assembly.GetExecutingAssembly().GetName().Version}_log_{DateTime.Now}.txt";

                File.Create(LOG_FILE_PATH);
            }
            catch
            {

            }
        }
    }
}
