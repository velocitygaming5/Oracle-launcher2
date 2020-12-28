using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Oracle_Launcher.Oracle
{
    class ToolHandler
    {
        public static SolidColorBrush GetColorFromHex(string hexColor)
        {
            return new SolidColorBrush(
                Color.FromArgb(
                    Convert.ToByte(hexColor.Substring(1, 2), 16),
                    Convert.ToByte(hexColor.Substring(3, 2), 16),
                    Convert.ToByte(hexColor.Substring(5, 2), 16),
                    Convert.ToByte(hexColor.Substring(7, 2), 16)
                )
            );
        }

        public static void SetImageSource(Image _image, string _uri ,UriKind _uriKind)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(_uri, _uriKind);
            bitmapImage.EndInit();

            _image.Source = bitmapImage;
        }

        public static string StringBeautify(string str)
        {
            string new_string;

            // Remove empty lines
            new_string = Regex.Replace(str, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            // Remove empty spaces
            new_string = Regex.Replace(new_string, " {2,}", " ");

            // Remove begining and ending of string space
            new_string = new_string.Trim();

            // remove new lines
            new_string = Regex.Replace(new_string, @"\t|\n|\r", "");

            return new_string;
        }

        public static async Task<bool> CheckForInternetConnectionAsync()
        {
            try
            {
                using (var client = new WebClient())
                using (await client.OpenReadTaskAsync(new Uri("http://google.com/generate_204", UriKind.Absolute)))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsFileDifferent(long _rFileSize, long _rModifiedTime, string _localFilePath)
        {
            try
            {
                FileInfo file = new FileInfo(_localFilePath);
                if (file.Length != _rFileSize)
                    return true;

                long localModifiedTime = ((DateTimeOffset)file.LastWriteTime).ToUnixTimeSeconds();

                if (localModifiedTime != _rModifiedTime)
                    return true;
            }
            catch
            {
                return true;
            }

            return false;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
