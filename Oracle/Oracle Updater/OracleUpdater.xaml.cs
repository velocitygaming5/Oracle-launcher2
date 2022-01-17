﻿using Oracle_Updater.Oracle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Oracle_Updater
{
    /// <summary>
    /// Interaction logic for OracleUpdater.xaml
    /// </summary>
    public partial class OracleUpdater : Window
    {
        private WebClient webClient = new WebClient();

        private Stopwatch SWSpeed = new Stopwatch();

        List<RemoteFiles> DownloadsList;

        public int TotalFilesCount { get; private set; }
        public int CurrentFileIndex { get; private set; }
        public string CurrentFileName { get; private set; }
        public DateTime WstartTime { get; private set; }

        public OracleUpdater()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SPProgressStatus.Visibility = Visibility.Collapsed;

            try
            {
                DownloadsList = RemoteFiles.FromJson(await Updater.GetRemoteFilesListJson());
                SPPlaceholder.Visibility = Visibility.Collapsed;
                SPProgressStatus.Visibility = Visibility.Visible;
                TotalFilesCount = DownloadsList.Count;

                if (DownloadsList != null)
                {
                    StartUpdating();
                }
                else
                {
                    Process.Start("Oracle Login.exe");
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to cancel the current update? Some files may remain incompleted", "Cancel update", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void CloseAnyOracleRunningProcesses()
        {
            // Get all processes running on the local computer.
            Process[] localAll = Process.GetProcesses();
            
            foreach (var process in localAll)
            {
                if (process.ProcessName.Contains("Oracle Login") || process.ProcessName.Contains("Oracle Launcher"))
                {
                    process.Kill(); // bye
                }
            }
        }

        #region UPDATER
        private async void ApplyUpdatesAndStartLauncher()
        {
            SPProgressStatus.Visibility = Visibility.Collapsed;
            SPPlaceholder.Visibility = Visibility.Visible;
            PlaceHorderText.Text = "Applying updates..";

            // Closes any running oracle login or oracle launcher
            CloseAnyOracleRunningProcesses();

            await Task.Delay(3000);

            try
            {
                string SourcePath = "temp_downloads";
                string DestinationPath = Directory.GetCurrentDirectory();

                // Creates all of the directories
                foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

                // Copies all the files and replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);

                PlaceHorderText.Text = "Successfully applied updates, starting the launcher..";

                await Task.Delay(1500);

                Directory.Delete("temp_downloads", true);

                Process.Start("Oracle Login.exe");
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }
        #endregion

        #region DOWNLOADER
        public void StartUpdating()
        {
            try
            {
                pbStatus.Value = 0;

                // Download first file from the collection
                foreach (var rFile in DownloadsList)
                {
                    string localPath = "temp_downloads\\" + rFile.LocalPath;

                    CurrentFileIndex++;
                    CurrentFileName = rFile.FileName;

                    if (!Directory.Exists(Path.GetDirectoryName(localPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(localPath));
                    }

                    Download_File(rFile.RemoteUrl, localPath);

                    break; // skip others
                }
            }
            catch
            {

            }
        }

        private void Download_File(string urlAddress, string destination)
        {
            WstartTime = DateTime.Now;

            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);

                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                Uri downloadURL = new Uri(urlAddress);

                SWSpeed.Start(); // Start the stopwatch which we will be using to calculate the download speed

                try
                {
                    webClient.DownloadFileAsync(downloadURL, destination);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            WstartTime = DateTime.Now; // timer used for download speed
            pbStatus.Value = e.ProgressPercentage;
            FileFirstIndex.Text = CurrentFileIndex.ToString();
            FileSecondIndex.Text = TotalFilesCount.ToString();
            DownloadingFileName.Text = CurrentFileName;
            DownloadSpeed.Text = $"{e.BytesReceived / 1024d / 1024d / SWSpeed.Elapsed.TotalSeconds:0.00} MB/s";
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            SWSpeed.Reset(); // timer used for download speed

            if (e.Cancelled == true)
            {
                MessageBox.Show("Downloads cancelled, closing..");
                Application.Current.Shutdown();
            }
            else
            {
                if (CurrentFileIndex >= TotalFilesCount) // if all downloads completed
                {
                    pbStatus.Value = 100;
                    ApplyUpdatesAndStartLauncher();

                }
                else // continue download list left files
                {

                    pbStatus.Value = 100;

                    DownloadsList.RemoveAt(0); // remove downloaded file from the list

                    StartUpdating(); // continue with the next file from the download list
                }
            }
        }
        #endregion
    }
}
