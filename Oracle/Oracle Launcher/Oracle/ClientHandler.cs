using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using WebHandler;

namespace Oracle_Launcher.Oracle
{
    class ClientHandler
    {
        private static string CONFIG_FILE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            System.Reflection.Assembly.GetEntryAssembly().GetName().Name, "Paths.xml");

        public static string GetExpansionPath(int _expansionID)
        {
            string _path = "";

            try
            {
                XmlDocument pathsDocument = Properties.Settings.Default.ClientPathsDefault;

                if (File.Exists(CONFIG_FILE_PATH))
                    pathsDocument.Load(CONFIG_FILE_PATH);

                foreach (XmlNode node in pathsDocument.SelectNodes("ClientPaths/Expansion"))
                {
                    int.TryParse(node.Attributes["id"].Value, out int ExpansionID);

                    if (ExpansionID == _expansionID)
                        _path = node.InnerText;
                }

                return _path;
            }
            catch
            {
                return _path;
            }
        }

        public static void SaveExpansionPath(int _expansionID, string _path)
        {
            try
            {
                XmlDocument pathsDocument = Properties.Settings.Default.ClientPathsDefault;

                if (File.Exists(CONFIG_FILE_PATH))
                    pathsDocument.Load(CONFIG_FILE_PATH);

                foreach (XmlNode node in pathsDocument.SelectNodes("ClientPaths/Expansion"))
                {
                    int.TryParse(node.Attributes["id"].Value, out int ExpansionID);

                    if (ExpansionID == _expansionID)
                        node.InnerText = _path;
                }

                if (!Directory.Exists(Path.GetDirectoryName(CONFIG_FILE_PATH)))
                    Directory.CreateDirectory(Path.GetDirectoryName(CONFIG_FILE_PATH));

                pathsDocument.Save(CONFIG_FILE_PATH);
            }
            catch
            {

            }
        }

        public static bool IsValidExpansionPath(int _expansionID, string _path, bool _fullClient = true)
        {
            // check if path exists
            if (!Directory.Exists(_path))
                return false;

            if (_fullClient && Properties.Settings.Default.CheckIfValidExpansionPath)
            {
                // check if wow.exe exists
                if (!File.Exists($@"{ _path }\Wow.exe"))
                    return false;

                // check if wow exe version starts with the regarding expansion id number
                try
                {
                    if (!FileVersionInfo.GetVersionInfo($@"{ _path }\Wow.exe").FileVersion.StartsWith(_expansionID.ToString()))
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        public static string GetExpansionName(int _expansionID)
        {
            switch (_expansionID)
            {
                case 1:
                    return "Classic";
                case 2:
                    return "Burning Crusade";
                case 3:
                    return "Wrath of the Lich King";
                case 4:
                    return "Cataclysm";
                case 5:
                    return "Mists of Pandaria";
                case 6:
                    return "Warlords of Draenor";
                case 7:
                    return "Legion";
                case 8:
                    return "Battle for Azeroth";
                case 9:
                    return "Shadowlands";
                default:
                    return "Unknown";
            }
        }
        public static int GetLocalUpdateVersion(int _expansionID)
        {
            int updateVersion = 0;

            try
            {
                XmlDocument pathsDocument = Properties.Settings.Default.ClientPathsDefault;

                if (File.Exists(CONFIG_FILE_PATH))
                    pathsDocument.Load(CONFIG_FILE_PATH);

                foreach (XmlNode node in pathsDocument.SelectNodes("ClientPaths/Expansion"))
                {
                    int.TryParse(node.Attributes["id"].Value, out int ExpansionID);

                    if (ExpansionID == _expansionID)
                        updateVersion = int.Parse(node.Attributes["update_version"].Value);
                }

                return updateVersion;
            }
            catch
            {
                return updateVersion;
            }
        }

        public static void SetLocalUpdateVersion(int _expansionID, int _version)
        {
            try
            {
                XmlDocument pathsDocument = Properties.Settings.Default.ClientPathsDefault;

                if (File.Exists(CONFIG_FILE_PATH))
                    pathsDocument.Load(CONFIG_FILE_PATH);

                foreach (XmlNode node in pathsDocument.SelectNodes("ClientPaths/Expansion"))
                {
                    int.TryParse(node.Attributes["id"].Value, out int ExpansionID);

                    if (ExpansionID == _expansionID)
                        node.Attributes["update_version"].Value = _version.ToString();
                }

                if (!Directory.Exists(Path.GetDirectoryName(CONFIG_FILE_PATH)))
                    Directory.CreateDirectory(Path.GetDirectoryName(CONFIG_FILE_PATH));

                pathsDocument.Save(CONFIG_FILE_PATH);
            }
            catch
            {

            }
        }

        public static int GetRemoteUpdateVersion(int _expansionID)
        {
            try
            {
                foreach (XmlNode node in Documents.RemoteConfig.SelectNodes("OracleLauncher/Expansions/Expansion"))
                {
                    int.TryParse(node.Attributes["id"].Value, out int ExpansionID);

                    if (ExpansionID == _expansionID)
                        return int.Parse(node.Attributes["update_version"].Value);
                }
            }
            catch
            {

            }

            return 0;
        }

        private static void DeleteCache(string _clientPath, bool _delCache = true)
        {
            try
            {
                if (Directory.Exists($"{ _clientPath }\\Cache"))
                {
                    var dir = new DirectoryInfo($"{ _clientPath }\\Cache");
                    dir.Delete(true); // true => recursive delete
                }
            }
            catch
            {
                
            }
        }

        private static string GetExpansionRealmlist(int _expansionID)
        {
            try
            {
                foreach (XmlNode node in Documents.RemoteConfig.SelectNodes("OracleLauncher/Expansions/Expansion"))
                {
                    if (int.Parse(node.Attributes["id"].Value) == _expansionID)
                    {
                        return node.SelectSingleNode("Realms").Attributes["realmlist"].Value;
                    }
                }
            }
            catch
            {
            }

            return "localhost";
        }

        private static void SetRealmlistPerLocale(int expansionID)
        {
            try
            {
                string[] locales = new string[] { "enUS", "esMX", "ptBR", "deDE", "enGB", "esES", "frFR", "itIT", "ruRU", "koKR", "zhTW", "zhCN" };

                foreach (var d in Directory.GetDirectories($@"{GetExpansionPath(expansionID)}\data"))
                {
                    var dir = new DirectoryInfo(d);
                    var dirName = dir.Name;

                    if (locales.Contains(dirName))
                    {
                        string configWTFPath = $@"{ GetExpansionPath(expansionID) }\data\{ dirName }\Realmlist.wtf";

                        if (File.Exists(configWTFPath))
                        {
                            var oldLines = File.ReadAllLines(configWTFPath);

                            // reads all lines except the lines that contains SET portal
                            var newLines = oldLines.Where(line => !line.ToLower().Contains("set realmlist"));

                            File.WriteAllLines(configWTFPath, newLines);

                            using (var outputFile = new StreamWriter(configWTFPath, true))
                                outputFile.WriteLine($"SET realmList \"{ GetExpansionRealmlist(expansionID) }\"");
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private static void SetRealmlist(int expansionID)
        {
            try
            {
                string configWTFPath = $@"{ GetExpansionPath(expansionID) }\WTF\Config.wtf";

                // Also set realmlist per locale if client supports that
                SetRealmlistPerLocale(expansionID);

                if (File.Exists(configWTFPath))
                {
                    var oldLines = File.ReadAllLines(configWTFPath);

                    // reads all lines except the lines that contains SET portal
                    var newLines = oldLines.Where(line => !line.ToLower().Contains("set realmlist"));

                    File.WriteAllLines(configWTFPath, newLines);

                    using (var outputFile = new StreamWriter(configWTFPath, true))
                        outputFile.WriteLine($"SET realmList \"{ GetExpansionRealmlist(expansionID) }\"");
                }
            }
            catch
            {

            }
        }

        public static async void MoveNonWhitelistedPatchesToBackupFolder(int expansionID)
        {
            try
            {
                var remoteWhiteList = FilesListClass.PatchesWhitelist.FromJson(await FilesListClass.GetPatchesWhitelistJSON());

                List<string> localMPQs = Directory.GetFiles(GetExpansionPath(expansionID), "*.*", SearchOption.AllDirectories).Where(file => file.ToLower().EndsWith("mpq")).ToList();
                localMPQs = localMPQs.ConvertAll(d => d.ToLower());

                List<string> whiteList = new List<string>();

                if (remoteWhiteList != null)
                {
                    foreach (var patch in remoteWhiteList)
                    {
                        string path = $@"{GetExpansionPath(expansionID)}\{patch.Path}";
                        whiteList.Add(path.ToLower());
                    }
                }

                if (localMPQs != null && whiteList != null)
                {
                    var nonWhiteListedPatches = localMPQs.Except(whiteList).ToList();
                    if (nonWhiteListedPatches != null)
                    {
                        foreach (var nonWhiteListedPatch in nonWhiteListedPatches)
                        {
                            string FROM = nonWhiteListedPatch;
                            string TO = $@"{GetExpansionPath(expansionID).ToLower()}\Backup{nonWhiteListedPatch.Replace(GetExpansionPath(expansionID).ToLower(), "")}";

                            if (!Directory.Exists(Path.GetDirectoryName(TO)))
                                Directory.CreateDirectory(Path.GetDirectoryName(TO));

                            File.Move(FROM, TO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, "ClientHandler.cs", "BackupNonWhitelistedPatches");
            }
        }

        public static void StartWoWClient(int _expansionID)
        {
            string WowExePath = $@"{ GetExpansionPath(_expansionID) }\Wow.exe";
            if (File.Exists(WowExePath))
            {
                try
                {
                    DeleteCache(GetExpansionPath(_expansionID));

                    SetRealmlist(_expansionID);

                    Process.Start(WowExePath);
                }
                catch
                {

                }
            }
        }
    }
}
