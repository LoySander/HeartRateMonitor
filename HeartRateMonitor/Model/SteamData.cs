using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.Serialization.Json;
using System.Collections.ObjectModel;

namespace HeartRateMonitor.Model
{
    class SteamData
    {
        private static SteamData instance;
        string apiURL = "http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=" + Key + "&steamid=" + ID +
               "&format=json&include_appinfo=1";
        private HttpWebRequest _httpWebRequest;
        private HttpWebResponse _httpWebResponse;
        private ICollection<Game> gamesList = new List<Game>();

        public static string Key { get; set; } = null;
        public static string ID { get; set; } = null;

        public static SteamData getInstance()
        {
            if (instance == null)
                instance = new SteamData();
            return instance;
        }

        public GameResponse GetGames()
        {
            string response;
            _httpWebRequest = (HttpWebRequest)WebRequest.Create(apiURL);
            _httpWebResponse = (HttpWebResponse)_httpWebRequest.GetResponse();
            response = string.Empty;
            using (StreamReader streamReader = new StreamReader(_httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
            var results = JsonConvert.DeserializeObject<GameResponse>(response);    
            GamesList = results.Response.Games;
            return results;
        }

        public ICollection<Game> GamesList
        {
            get => gamesList;
            set => gamesList = value;
        }

        
    }

        #region вспомогательные классы
        public class GameInfo
        {
            public int appid { get; set; }
            public string name { get; set; }
        }

        public class RootObjectApp
        {
            public Applist applist { get; set; }
        }

        public class Applist
        {
            public List<GameInfo> apps { get; set; }
        }

        public partial class GameResponse
        {
            [JsonProperty("response")]
            public Response Response { get; set; }
        }

        public partial class Response
        {
            [JsonProperty("game_count")]
            public long GameCount { get; set; }

            [JsonProperty("games")]
            public Game[] Games { get; set; }
        }

        public partial class Game
        {
            [JsonProperty("appid")]
            public long Appid { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("playtime_2weeks", NullValueHandling = NullValueHandling.Ignore)]
            public long? Playtime2Weeks { get; set; }

            [JsonProperty("playtime_forever")]
            public long PlaytimeForever { get; set; }

            [JsonProperty("img_icon_url")]
            public string ImgIconUrl { get; set; }

            [JsonProperty("img_logo_url")]
            public string ImgLogoUrl { get; set; }

            [JsonProperty("has_community_visible_stats")]
            public bool HasCommunityVisibleStats { get; set; }

            [JsonProperty("playtime_windows_forever")]
            public long PlaytimeWindowsForever { get; set; }

            [JsonProperty("playtime_mac_forever")]
            public long PlaytimeMacForever { get; set; }

            [JsonProperty("playtime_linux_forever")]
            public long PlaytimeLinuxForever { get; set; }
        }
        #endregion
    
}

