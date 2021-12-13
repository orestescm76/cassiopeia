/*
 * CASSIOPEIA 2.0.225.30
 * SPOTIFY API WRAPPER
 * CODENAME STORM
 * MADE BY ORESTESCM76
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Cassiopeia.src.Classes;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace Cassiopeia
{
    public class Spotify
    {
        private SpotifyClient SpotifyClient;
        private SpotifyClientConfig SpotifyConfig;
        private readonly char[] ForbiddenChars = { '\\', '/', '|', '?', '*', '"', ':', '>', '<', ';' };
        //should change this..
        private readonly String PublicKey = "f49317757dd64bb190576aec028f4efc";
        private readonly String PrivateKey = ClaveAPI.Spotify;
        public bool AccountReady = false;
        public bool AccountLinked = false;

        public DeviceResponse Device;
        private PrivateUser User;
        private static string AuthPath = "spotifyLogin.json";
        public Spotify(bool linked)
        {
            if (!linked)
                Start();
            else
                StartStreamMode();
        }
        public async Task LinkSpotify()
        {
            StartStreamMode();
        }
        private void Start()
        {
            Log.Instance.PrintMessage("Trying to connect to Spotify...", MessageType.Info, "Spotify.Start()");
            User = null;
            Stopwatch crono = Stopwatch.StartNew();
            Kernel.InternetAvaliable(false);
            try
            {
                SpotifyConfig = SpotifyClientConfig.CreateDefault().WithAuthenticator(new ClientCredentialsAuthenticator(PublicKey, PrivateKey));
                SpotifyClient = new SpotifyClient(SpotifyConfig);
                crono.Stop();
                if(SpotifyConfig is not null) //??
                {
                    Kernel.InternetAvaliable(true);
                    Log.Instance.PrintMessage("Connected!", MessageType.Correct, crono, TimeType.Milliseconds);
                }
                else //yo  creoque esto nunca se ejecuta...
                {
                    Kernel.InternetAvaliable(false);
                    Log.Instance.PrintMessage("Token is null", MessageType.Error, crono, TimeType.Milliseconds);
                }

            }
            catch (APIException ex)
            {
                Kernel.InternetAvaliable(false);
                Log.Instance.PrintMessage(ex.Message, MessageType.Error);
                MessageBox.Show(Kernel.LocalTexts.GetString("error_internet"));
            }
        }
        private async void StartStreamMode()
        {
            try
            {
                Log.Instance.PrintMessage("Trying to connect Spotify account", MessageType.Info, "Spotify.StartStreamMode()");
                Kernel.InternetAvaliable(false);
                Stopwatch crono = Stopwatch.StartNew();
                if (!File.Exists(AuthPath))
                {
                    var (verifier, challenge) = PKCEUtil.GenerateCodes();
                    var server = new EmbedIOAuthServer(new Uri("http://localhost:4002/callback"), 4002);
                    await server.Start();
                    server.AuthorizationCodeReceived += async (sender, response) =>
                    {
                        await server.Stop();
                        PKCETokenResponse token = await new OAuthClient().RequestToken(new PKCETokenRequest(PublicKey, response.Code, server.BaseUri, verifier));
                        await File.WriteAllTextAsync(AuthPath, JsonConvert.SerializeObject(token));
                        await StartLoginSpotify(crono);
                        server.Dispose();
                    };
                    var login = new LoginRequest(server.BaseUri, PublicKey, LoginRequest.ResponseType.Code)
                    {
                        CodeChallenge = challenge,
                        CodeChallengeMethod = "S256",
                        Scope = new List<string> { Scopes.UserReadEmail, Scopes.UserReadPrivate, Scopes.Streaming, Scopes.PlaylistReadPrivate, Scopes.UserReadPlaybackState, Scopes.UserLibraryRead }
                    };
                    BrowserUtil.Open(login.ToUri());
                }
                else
                    await StartLoginSpotify(crono);
            }
            catch (APIException e)
            {
                Kernel.InternetAvaliable(false);
                Log.Instance.PrintMessage(e.Message, MessageType.Error);
                System.Windows.Forms.MessageBox.Show(Kernel.LocalTexts.GetString("error_internet"));
            }
        }
        public bool IsSpotifyReady()
        {
            return AccountReady;
        }
        private async Task StartLoginSpotify(Stopwatch crono)
        {
            Log.Instance.PrintMessage("Logging to Spotify", MessageType.Info);
            var json = await File.ReadAllTextAsync(AuthPath);
            var token = JsonConvert.DeserializeObject<PKCETokenResponse>(json);
            var auth = new PKCEAuthenticator(PublicKey, token);
            auth.TokenRefreshed += (sender, token) => File.WriteAllText(AuthPath, JsonConvert.SerializeObject(token));
            SpotifyConfig = SpotifyClientConfig.CreateDefault().WithAuthenticator(auth);
            SpotifyClient = new SpotifyClient(SpotifyConfig);
            AccountReady = true;
            User = SpotifyClient.UserProfile.Current().Result;
            Log.Instance.PrintMessage("Connected as " + User.Email, MessageType.Correct, crono, TimeType.Seconds);
            Config.LinkedWithSpotify = true;
            AccountLinked = true;
            Kernel.ActivarReproduccionSpotify();
            Kernel.InternetAvaliable(true);
            Kernel.BringMainFormFront();
            crono.Stop();
        }

        //Returns a list of albums based on a query.
        public List<SimpleAlbum> SearchAlbums(string query, int limit)
        {
            Log.Instance.PrintMessage("Album search started", MessageType.Info, "Spotify.SearchAlbums(string)");
            Stopwatch crono = Stopwatch.StartNew();
            try
            {
                SearchRequest request = new SearchRequest(SearchRequest.Types.Album, query)
                {
                    Limit = limit
                };
                List<SimpleAlbum> AlbumList = SpotifyClient.Search.Item(request).Result.Albums.Items;
                Log.Instance.PrintMessage("Album search completed", MessageType.Correct, crono, TimeType.Milliseconds);
                return AlbumList;
            }
            catch (APIException e)
            {
                Log.Instance.PrintMessage("Cannot search albums...", MessageType.Error);
                Log.Instance.PrintMessage(e.InnerException.Message, MessageType.Error);
                crono.Stop();
                throw e;
            }
        }
        public SimpleAlbum ReturnAlbum(string a)
        {
            Log.Instance.PrintMessage("Búsqueda en Spotify", MessageType.Info, "Spotify.devolverAlbum(string)");
            Stopwatch crono = Stopwatch.StartNew();
            try
            {
                SimpleAlbum album = SearchAlbums(a,1).First();
                crono.Stop();
                Log.Instance.PrintMessage("Búsqueda en Spotify ha finalizado correctamente", MessageType.Correct, crono, TimeType.Milliseconds);

                return album;
            }
            catch (APIException e)
            {
                Log.Instance.PrintMessage("Spotify search failed", MessageType.Warning, crono, TimeType.Milliseconds);
                throw e;
            }

        }
        public bool InsertAlbumFromURI(string uri)
        {
            Log.Instance.PrintMessage("Inserting album with URI " + uri, MessageType.Info);
            Stopwatch crono = Stopwatch.StartNew();
            bool res;
            try
            {
                FullAlbum album = SpotifyClient.Albums.Get(uri).Result;
                res = ProcessAlbum(album);

            }
            catch (APIException e)
            {
                crono.Stop();
                Log.Instance.PrintMessage("Album was not inserted...", MessageType.Warning);
                Log.Instance.PrintMessage(e.Message, MessageType.Warning);
                return false;
            }
            crono.Stop();
            Log.Instance.PrintMessage("Added", MessageType.Correct, crono, TimeType.Milliseconds);
            Kernel.ReloadView();
            return res;
        }
        public bool ProcessAlbum(FullAlbum album, bool downloadCover = true)
        {
            String[] parseFecha = album.ReleaseDate.Split('-');
            string cover = album.Name + "_" + album.Artists[0].Name + ".jpg";

            AlbumData a = new AlbumData(album.Name.Replace(";",""), album.Artists[0].Name.Replace(";", ""), Convert.ToInt16(parseFecha[0]), Environment.CurrentDirectory + "/covers/" + cover); //creamos A
            if (Kernel.Collection.IsInCollection(a))
            {
                Log.Instance.PrintMessage("Adding duplicate album", MessageType.Warning);
                Log.Instance.PrintMessage(a.ToString(), MessageType.Info);
                return false;
            }

            //Remove Windows forbidden characters so we can save the album cover.
            foreach (char ch in ForbiddenChars)
            {
                if (cover.Contains(ch.ToString()))
                    cover = cover.Replace(ch.ToString(), string.Empty);
            }
            if (downloadCover)
            {
                using (System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "/covers");
                        webClient.DownloadFile(new Uri(album.Images[0].Url), Environment.CurrentDirectory + "/covers/" + cover);
                    }
                    catch (System.Net.WebException e)
                    {
                        Log.Instance.PrintMessage("Exception captured System.Net.WebException", MessageType.Warning);
                        MessageBox.Show(Kernel.LocalTexts.GetString("errorPortada"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cover = "";
                    }
                }
            }
            else
                a.CoverPath = "";

            a.IdSpotify = album.Id;
            List<Song> songs = new List<Song>(a.NumberOfSongs);
            List<SimpleTrack> albumSongs = album.Tracks.Items;
            for (int i = 0; i < albumSongs.Count; i++)
            {
                songs.Add(new Song(albumSongs[i].Name.Replace(";",""), new TimeSpan(0, 0, 0, 0, albumSongs[i].DurationMs), ref a));
                if (songs[i].Length.Milliseconds > 500)
                    songs[i].Length += new TimeSpan(0, 0, 0, 0, 1000 - songs[i].Length.Milliseconds);
                else
                    songs[i].Length -= new TimeSpan(0, 0, 0, 0, songs[i].Length.Milliseconds);
            }
            a.Songs = songs;
            a.CanBeRemoved = true;
            Kernel.Collection.AddAlbum(ref a);
            Kernel.SetSaveMark();
            return true;
        }
        public void ProcessAlbum(SimpleAlbum album)
        {
            FullAlbum fullAlbum = SpotifyClient.Albums.Get(album.Id).Result;
            ProcessAlbum(fullAlbum);
        }
        public bool UserIsPremium()
        {
            try
            {
                return SpotifyClient.UserProfile.Current().Result.Product == "premium" ? true : false;
            }
            catch (APIException ex)
            {
                Log.Instance.PrintMessage("Couldn't retrieve user type", MessageType.Warning);
                Log.Instance.PrintMessage(ex.Message, MessageType.Warning);
                throw ex;
            }
        }
        public void PlayAlbum(string uri)
        {
            try
            {
                Device = SpotifyClient.Player.GetAvailableDevices().Result;
                PlayerResumePlaybackRequest request = new PlayerResumePlaybackRequest()
                {
                    ContextUri = "spotify:album:" + uri,
                    PositionMs = 0,
                    DeviceId = Device.Devices.First().Id
                };
                SpotifyClient.Player.ResumePlayback(request);
            }
            catch (APIException e)
            {

                throw e;
            }
        }

        public void PlaySongFromAlbum(string uri, int cual) //reproduce una cancion de un album
        {
            try
            {
                FullAlbum album = SpotifyClient.Albums.Get(uri).Result;
                string uricancion = "";
                if (cual != 0)
                {
                    for (int i = 1; i <= cual; i++)
                        uricancion = album.Tracks.Items[i].Id;
                }
                else
                    uricancion = album.Tracks.Items.First().Id;
                PlaySong(uricancion);
            }
            catch (APIException ex)
            {
                throw ex;
            }

        }

        public void PlaySong(string uri)
        {
            try
            {
                Device = SpotifyClient.Player.GetAvailableDevices().Result;
                List<string> temp = new List<string>();
                temp.Add("spotify:track:"+uri);
                PlayerResumePlaybackRequest request = new PlayerResumePlaybackRequest()
                {
                    Uris = temp,
                    PositionMs = 0,
                    DeviceId = Device.Devices.First().Id
                };
                SpotifyClient.Player.ResumePlayback(request);
            }
            catch (APIException e)
            {
                throw e;
            }
        }
        public void PlaySong(List<string> uris)
        {
            try
            {
                Device = SpotifyClient.Player.GetAvailableDevices().Result;
                PlayerResumePlaybackRequest request = new PlayerResumePlaybackRequest()
                {
                    Uris = uris,
                    PositionMs = 0,
                    DeviceId = Device.Devices.First().Id
                };
                SpotifyClient.Player.ResumePlayback(request);
            }
            catch (APIException e)
            {
                throw e;
            }
        }
        public void PlaySong(string uri, LongSong cl)
        {
            try
            {
                FullAlbum album = SpotifyClient.Albums.Get(uri).Result;
                List<string> uris = new List<string>();
                foreach (Song parte in cl.Parts)
                {
                    uris.Add("spotify:track:" + ReturnSongFromAlbum(album, parte.Title));
                }
                PlaySong(uris);
            }
            catch (APIException ex)
            {
                throw ex;
            }

        }
        public string ReturnSongFromAlbum(FullAlbum album, string cancion)
        {
            foreach (SimpleTrack track in album.Tracks.Items)
            {
                if (track.Name == cancion)
                    return track.Id;
            }
            return string.Empty;
        }

        public async void GetUserAlbums()
        {
            if (User is not null)
            {
                List<FullAlbum> albums = new List<FullAlbum>();
                try
                {
                    //Get albums
                    var savedAlbums = await SpotifyClient.Library.GetAlbums();
                    int point = 0;
                    int limit = (int)savedAlbums.Total;
                    bool covers = true;
                    if (limit > 100)
                    {
                        DialogResult dr = Kernel.Warn(Kernel.LocalTexts.GetString("importSpotifyWarning"));
                        if (dr == DialogResult.Cancel)
                            return;
                        if (dr == DialogResult.No)
                            covers = false;
                    }
                    Cassiopeia.src.Forms.LoadBar loadBar = new src.Forms.LoadBar(limit, "Downloading albums");
                    Log.Instance.PrintMessage("Downloading and adding albums", MessageType.Info);
                    Stopwatch crono = Stopwatch.StartNew();
                    loadBar.Show();
                    do
                    {
                        //Add the albums
                        foreach (var a in savedAlbums.Items)
                        {
                            if(a is not null)
                                ProcessAlbum(a.Album, covers);
                            loadBar.Progreso();
                        }
                        point += 20;
                        LibraryAlbumsRequest request = new LibraryAlbumsRequest()
                        {
                            Offset = point
                        };
                        savedAlbums = SpotifyClient.Library.GetAlbums(request).Result;
                    } while (point < limit);
                    crono.Stop();
                    loadBar.Dispose();
                    Log.Instance.PrintMessage("Done!", MessageType.Correct, crono, TimeType.Seconds);
                    albums.Clear();
                    Kernel.ReloadView();
                }
                catch (APIException)
                {
                    Log.Instance.PrintMessage("Failed to save user's library", MessageType.Error);
                }

            }

        }
        #region Spotify Commands
        public void SetVolume(int vol)
        {
            SpotifyClient.Player.SetVolume(new PlayerVolumeRequest(vol));
        }

        public void SetShuffle(bool state)
        {
            SpotifyClient.Player.SetShuffle(new PlayerShuffleRequest(state));
        }

        public void SeekTo(long pos)
        {
            SpotifyClient.Player.SeekTo(new PlayerSeekToRequest(pos));
        }
        public Task<CurrentlyPlayingContext> GetPlayingContextAsync()
        {
            try
            {
                return SpotifyClient.Player.GetCurrentPlayback();
            }
            catch (APIException ex)
            {
                Log.Instance.PrintMessage(ex.Message, MessageType.Warning);
                return null;
            }
        }
        public CurrentlyPlayingContext GetPlayingContext()
        {
            try
            {
                return SpotifyClient.Player.GetCurrentPlayback().Result;
            }
            catch (APIException ex)
            {

                Log.Instance.PrintMessage(ex.Message, MessageType.Warning);
                return null;
            }
        }
        //Plays or resumes playback on the first device
        public void PlayResume()
        {
            try
            {
                //Device = SpotifyClient.Player.GetAvailableDevices().Result;
                SpotifyClient.Player.ResumePlayback();
            }
            catch (APIException ex)
            {
                Log.Instance.PrintMessage(ex.Message, MessageType.Error);
                throw ex;
            }
            catch (NullReferenceException)
            {
                Log.Instance.PrintMessage("No playback devices found!", MessageType.Warning);
            }
        }
        public void SkipNext()
        {
            SpotifyClient.Player.SkipNext();
        }
        public void SkipPrevious()
        {
            SpotifyClient.Player.SkipPrevious();
        }
        public PrivateUser GetPrivateUser()
        {
            return SpotifyClient.UserProfile.Current().Result;
        }
        #endregion
    }
}
