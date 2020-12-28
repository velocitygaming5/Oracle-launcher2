using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebHandler
{
    /*------------------------------------------------------------------------------------------------
     ---------------------------------------- AUTH CLASS ---------------------------------------------
     ------------------------------------------------------------------------------------------------*/
    public class AuthClass
    {
        public enum Response { INVALID, OK, ERROR }

        public static async Task<int> GetLoginReponse(string username, string password)
        {
            int.TryParse(await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "login_response" },
                { "user", username },
                { "pass", password }
            }), out int response);
            return response;
        }

        public static async Task<string> GetAccountStateJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "account_state" },
                { "user", username },
                { "pass", password }
            });
        }
    }

    public partial class AccountState
    {
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("unbanDate")]
        public long UnbanDate { get; set; }
    }

    public partial class AccountState
    {
        public static List<AccountState> FromJson(string json) => JsonConvert.DeserializeObject<List<AccountState>>(json, Converter.Settings);
    }

    /*------------------------------------------------------------------------------------------------
     ---------------------------------------- CHAR CLASS ---------------------------------------------
     ------------------------------------------------------------------------------------------------*/
    public class CharClass
    {
        public static async Task<string> GetCharactersListJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "characters_list" },
                { "user", username },
                { "pass", password }
            });
        }
    }

    public partial class CharacterData
    {
        [JsonProperty("realm")]
        public string Realm { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("gender")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Gender { get; set; }

        [JsonProperty("level")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Level { get; set; }

        [JsonProperty("race")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Race { get; set; }

        [JsonProperty("class")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Class { get; set; }
    }

    public partial class CharacterData
    {
        public static List<List<CharacterData>> FromJson(string json) => JsonConvert.DeserializeObject<List<List<CharacterData>>>(json, Converter.Settings);
    }

    /*------------------------------------------------------------------------------------------------
     ----------------------------------------- WEB CLASS ---------------------------------------------
     ------------------------------------------------------------------------------------------------*/
    public class WebClass
    {
        public static async Task<string> GetAccountBalanceJson(string username, string password)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "account_balance" },
                { "user", username },
                { "pass", password }
            });
        }
    }

    public partial class AccountBalance
    {
        [JsonProperty("dp")]
        public long DP { get; set; }

        [JsonProperty("vp")]
        public long VP { get; set; }
    }

    public partial class AccountBalance
    {
        public static List<AccountBalance> FromJson(string json) => JsonConvert.DeserializeObject<List<AccountBalance>>(json, Converter.Settings);
    }

    /*------------------------------------------------------------------------------------------------
    ------------------------------------------- WEB TOOL ---------------------------------------------
    ------------------------------------------------------------------------------------------------*/
    public class WebTool
    {
        public static async Task<string> GetStringFromPOST(string URL, Dictionary<string, string> values)
        {
            try
            {
                var content = new FormUrlEncodedContent(values);

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(URL, content);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    /*------------------------------------------------------------------------------------------------
    ------------------------------------------- FILES LIST -------------------------------------------
    ------------------------------------------------------------------------------------------------*/
    public class FilesListClass
    {
        public static async Task<string> GetFilesListJson(int _expansionID)
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "files_list" },
                { "expansion", _expansionID.ToString() }
            });
        }
    }

    public partial class FilesList
    {
        [JsonProperty("filePath")]
        public string FilePath { get; set; }

        [JsonProperty("fileSize")]
        public long FileSize { get; set; }

        [JsonProperty("modifiedTime")]
        public long ModifiedTime { get; set; }
    }

    public partial class FilesList
    {
        public static List<List<FilesList>> FromJson(string json) => JsonConvert.DeserializeObject<List<List<FilesList>>>(json, Converter.Settings);
    }

    /* EXAMPLE
        foreach (var file in FilesList.FromJson(await FilesListClass.GetFilesListJson(_expansionID)))
        {
            foreach (var data in file)
            {
                MessageBox.Show(data.FileUrl + "\r\n" + data.Md5Hash);
            }
        }
    */
}
