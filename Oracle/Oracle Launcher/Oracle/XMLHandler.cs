using System.Threading.Tasks;

namespace Oracle_Launcher.Oracle
{
    class XMLHandler
    {
        public static async Task LoadXMLRemoteConfigAsync()
        {
            try
            {
                await Task.Run(() => Documents.RemoteConfig.Load(Properties.Settings.Default.XMLDocumentUrl));
            }
            catch
            {

            }
        }
    }
}
