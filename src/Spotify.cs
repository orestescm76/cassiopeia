using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Net.Http;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System.Threading.Tasks;
//NEED TO UPDATE SPOTIFY API TO NEW VERSION!
namespace Cassiopeia
{
    class Spotify
    {
        public SpotifyClient SpotifyClient;
        private SpotifyClientConfig SpotifyConfig;
        private readonly char[] WindowsForbiddenChars = { '\\', '/', '|', '?', '*', '"', ':', '>', '<' };
        //should change this..
        private readonly String PublicKey = "f49317757dd64bb190576aec028f4efc";
        private readonly String PrivateKey = ClaveAPI.Spotify;
        public bool AccountReady = false;
        public bool AccountLinked = false;
        private AuthorizationCodeTokenResponse Token;
        private AuthorizationCodeRefreshResponse TokenRefresh;

        public Spotify(bool v)
        {
            if(!v)
                Start();
            else
                StartStreamMode();
        }
        public void LinkSpotify()
        {
            StartStreamMode();
        }
        public bool IsTokenExpired()
        {
            if (Token is not null)
                return Token.IsExpired;
            else if (TokenRefresh is not null)
                return TokenRefresh.IsExpired;
            return false;
        }
        private void Start()
        {
            Log.Instance.PrintMessage("Trying to connect to Spotify...", MessageType.Info, "Spotify.Start()");
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
                    Kernel.InitTask();
                    Log.Instance.PrintMessage("Conectado sin errores", MessageType.Correct, crono, TimeType.Milliseconds);
                }
                else
                {
                    Kernel.InternetAvaliable(false);
                    Log.Instance.PrintMessage("Se ha conectado pero el token es nulo", MessageType.Error, crono, TimeType.Milliseconds);
                }

            }
            catch (NullReferenceException)
            {
                Kernel.InternetAvaliable(false);
                Log.Instance.PrintMessage("No se ha podido conectar con Spotify", MessageType.Error);
                System.Windows.Forms.MessageBox.Show(Kernel.LocalTexts.GetString("error_internet"));
            }
            catch (HttpRequestException)
            {
                Kernel.InternetAvaliable(false);
                Log.Instance.PrintMessage("No se ha podido conectar con Spotify", MessageType.Error);
                System.Windows.Forms.MessageBox.Show(Kernel.LocalTexts.GetString("error_internet"));
            }
        }
        private void StartStreamMode()
        {
            try
            {
                Log.Instance.PrintMessage("Trying to connect Spotify account", MessageType.Info, "Spotify.StartStreamMode()");
                Kernel.InternetAvaliable(false);
                Stopwatch crono = Stopwatch.StartNew();
                SpotifyConfig = SpotifyClientConfig.CreateDefault();
                var server = new EmbedIOAuthServer(new Uri("http://localhost:4002/"), 4002);
                server.AuthorizationCodeReceived += async (sender, response) =>
                {
                    await server.Stop();
                    Token = await new OAuthClient(SpotifyConfig).RequestToken(new AuthorizationCodeTokenRequest(PublicKey, PrivateKey, response.Code, server.BaseUri));
                    SpotifyClient = new SpotifyClient(SpotifyConfig.WithToken(Token.AccessToken));

                    AccountReady = true;
                    AccountLinked = true;
                    Config.LinkedWithSpotify = true;
                    Kernel.ActivarReproduccionSpotify();
                    Kernel.InternetAvaliable(true);
                    Kernel.BringMainFormFront();
                    Log.Instance.PrintMessage("Conectado sin errores como " + SpotifyClient.UserProfile.Current().Result.Email, MessageType.Correct, crono, TimeType.Milliseconds);
                    Kernel.InitTask();
                    crono.Stop();
                };
                server.Start();
                var login = new LoginRequest(server.BaseUri, PublicKey, LoginRequest.ResponseType.Code)
                {
                    Scope = new[] { Scopes.UserReadEmail, Scopes.UserReadPrivate, Scopes.Streaming, Scopes.PlaylistReadPrivate, Scopes.UserReadPlaybackState }
                };
                BrowserUtil.Open(login.ToUri());
            }
            catch (NullReferenceException)
            {
                Kernel.InternetAvaliable(false);
                Console.WriteLine("Algo fue mal");
                System.Windows.Forms.MessageBox.Show(Kernel.LocalTexts.GetString("error_internet"));
            }
            catch (HttpRequestException)
            {
                Kernel.InternetAvaliable(false);
                Console.WriteLine("No tienes internet");
                System.Windows.Forms.MessageBox.Show(Kernel.LocalTexts.GetString("error_internet"));
            }
        }

        public async Task RefreshTokenAsync()
        {
            Log.Instance.PrintMessage("Refreshing Token...", MessageType.Info);
            TokenRefresh = await new OAuthClient(SpotifyConfig).RequestToken(new AuthorizationCodeRefreshRequest(PublicKey, PrivateKey, Token.RefreshToken));
            SpotifyClient = new SpotifyClient(SpotifyConfig.WithToken(TokenRefresh.AccessToken));
            Token = null;
            Log.Instance.PrintMessage("Token refreshed!", MessageType.Correct);
        }
        //Returns a list of albums based on a query.
        public List<SimpleAlbum> SearchAlbums(string query)
        {
            Log.Instance.PrintMessage("Album search started", MessageType.Info, "Spotify.SearchAlbums(string)");
            Stopwatch crono = Stopwatch.StartNew();
            try
            {
                SearchRequest request = new SearchRequest(SearchRequest.Types.Album, query);
                request.Limit = 50;
                List<SimpleAlbum> AlbumList = SpotifyClient.Search.Item(request).Result.Albums.Items;
                Log.Instance.PrintMessage("Album search completed", MessageType.Correct, crono, TimeType.Milliseconds);
                return AlbumList;
            }
            catch (APIException e)
            {
                Log.Instance.PrintMessage("Cannot search albums...", MessageType.Error);
                Log.Instance.PrintMessage(e.InnerException.Message, MessageType.Error);
                crono.Stop();
            }
            //If the search fails, return null.
            return null;
        }
        public SimpleAlbum ReturnAlbum(string a)
        {
            Log.Instance.PrintMessage("Búsqueda en Spotify", MessageType.Info, "Spotify.devolverAlbum(string)");
            Stopwatch crono = Stopwatch.StartNew();
            try
            {
                SimpleAlbum album = SearchAlbums(a).First();
                crono.Stop();
                Log.Instance.PrintMessage("Búsqueda en Spotify ha finalizado correctamente", MessageType.Correct, crono, TimeType.Milliseconds);

                return album;
            }
            catch (ArgumentOutOfRangeException)
            {
                Log.Instance.PrintMessage("Busqueda en Spotify no ha encontrado nada", MessageType.Warning, crono, TimeType.Milliseconds);
                return null;
            }

        }
        public bool InsertAlbumFromURI(string uri)
        {
            Log.Instance.PrintMessage("Inserting album with URI " + uri, MessageType.Info);
            Stopwatch crono = Stopwatch.StartNew();
            //FullAlbum sa = SpotifyClient.GetAlbum(uri);
            try
            {
                FullAlbum album = SpotifyClient.Albums.Get(uri).Result;
                ProcessAlbum(album);

            }
            catch (APIException e)
            {
                crono.Stop();
                Log.Instance.PrintMessage("Album was not inserted...", MessageType.Warning);
                Log.Instance.PrintMessage(e.Message, MessageType.Warning);
                return false;
            }
            crono.Stop();
            Log.Instance.PrintMessage("Añadido", MessageType.Correct, crono, TimeType.Milliseconds);
            Kernel.ReloadView();
            return true;
        }
        public void ProcessAlbum(FullAlbum album)
        {
            String[] parseFecha = album.ReleaseDate.Split('-');
            string cover = album.Name + "_" + album.Artists[0].Name + ".jpg";
            foreach (char ch in WindowsForbiddenChars)
            {
                if (cover.Contains(ch.ToString()))
                    cover = cover.Replace(ch.ToString(), string.Empty);
            }
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
                    System.Windows.Forms.MessageBox.Show(Kernel.LocalTexts.GetString("errorPortada"), "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    cover = "";
                }
            }
            AlbumData a = new AlbumData(album.Name, album.Artists[0].Name, Convert.ToInt16(parseFecha[0]), Environment.CurrentDirectory + "/covers/" + cover); //creamos A
            if (Kernel.Collection.IsInCollection(a))
            {
                Log.Instance.PrintMessage("Adding duplicate album", MessageType.Warning);
                return;
            }
            a.IdSpotify = album.Id;
            List<Song> songs = new List<Song>(a.NumberOfSongs);
            List<SimpleTrack> albumSongs = album.Tracks.Items;
            for (int i = 0; i < albumSongs.Count; i++)
            {
                songs.Add(new Song(albumSongs[i].Name, new TimeSpan(0, 0, 0, 0, albumSongs[i].DurationMs), ref a));
                if (songs[i].Length.Milliseconds > 500)
                    songs[i].Length += new TimeSpan(0, 0, 0, 0, 1000 - songs[i].Length.Milliseconds);
                else
                    songs[i].Length -= new TimeSpan(0, 0, 0, 0, songs[i].Length.Milliseconds);
            }
            a.Songs = songs;
            a.CanBeRemoved = true;
            Kernel.Collection.AddAlbum(ref a);
        }
        public void ProcessAlbum(SimpleAlbum album)
        {
            FullAlbum fullAlbum = SpotifyClient.Albums.Get(album.Id).Result;
            ProcessAlbum(fullAlbum);
        }
        public bool UserIsPremium()
        {
            return SpotifyClient.UserProfile.Current().Result.Product == "premium" ? true : false;
        }
        public void PlayAlbum(string uri)
        {
            PlayerResumePlaybackRequest request = new PlayerResumePlaybackRequest()
            {
                ContextUri = "spotify:album:" + uri, PositionMs = 0
            };
            SpotifyClient.Player.ResumePlayback(request);
            //return _spotify.ResumePlayback(contextUri: "spotify:album:" + uri, offset: "", positionMs: 0);
        }

        public void PlaySongFromAlbum(string uri, int cual) //reproduce una cancion de un album
        {
//            FullAlbum album = _spotify.GetAlbum(uri);
//            string uricancion = "";
//            if (cual != 0)
//            {
//                for (int i = 0; i <= cual; i++)
//                    uricancion = album.Tracks.Items[i].Id;
//            }
//            else
//                uricancion = album.Tracks.Items.First().Id;
//;            string temp = uricancion;
//            uricancion = "";
//            uricancion += "spotify:track:" + temp;
//            List<string> uris = new List<string>();
//            uris.Add(uricancion);
//            return _spotify.ResumePlayback(uris: uris, offset: "", positionMs: 0);
        }
        public string DevolverCancionDelAlbum(string uri, string cancion)
        {
            //FullAlbum album = _spotify.GetAlbum(uri);
            //foreach (SimpleTrack track in album.Tracks.Items)
            //{
            //    if (track.Name == cancion)
            //        return track.Id;
            //}
            return string.Empty;
        }
        public void PlaySong(string uri, LongSong cl)
        {
            //FullAlbum album = _spotify.GetAlbum(uri);
            //List<string> uris = new List<string>();
            //foreach(Song parte in cl.Parts)
            //{
            //    uris.Add("spotify:track:"+DevolverCancionDelAlbum(uri, parte.Title));
            //}
            //return _spotify.ResumePlayback(uris: uris, offset: "", positionMs: 0);
        }
    }
}
