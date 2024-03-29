﻿/*
 * SPOTIFY API WRAPPER
 * CODENAME BETRAYAL
 * MADE BY ORESTESCM76
 */

using Cassiopeia.src.Classes;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public Spotify()
        {
            /*
            if (!linked)
                Start();
            else
                StartStreamMode();
            */
        }
        public void InitNormalMode()
        {
            Start();
        }
        public async Task InitStreamMode()
        {
            await StartStreamMode();
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
                if (SpotifyClient is not null) //??
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
            catch (Exception ex)
            {
                Kernel.InternetAvaliable(false);
                Log.Instance.PrintMessage(ex.Message, MessageType.Error);
                MessageBox.Show(Kernel.GetText("error_internet"));
            }
        }
        private async Task StartStreamMode()
        {
            try
            {
                Log.Instance.PrintMessage("Trying to connect Spotify account", MessageType.Info, "Spotify.StartStreamMode()");
                //Kernel.InternetAvaliable(false);
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
                        Scope = new List<string> { Scopes.UserReadEmail, Scopes.UserReadPrivate, Scopes.Streaming, Scopes.UserReadPlaybackState, Scopes.UserLibraryRead }
                    };
                    BrowserUtil.Open(login.ToUri());
                }
                else
                    await StartLoginSpotify(crono);
            }
            catch (Exception e)
            {
                Kernel.InternetAvaliable(false);
                Log.Instance.PrintMessage(e.Message, MessageType.Error);
                if (e.InnerException.Message == "invalid_grant")
                {
                    Log.Instance.PrintMessage("App was delinked from Spotify, relaunching", MessageType.Warning);
                    Kernel.ResetSpotifyLink();
                    Config.LinkedWithSpotify = false;
                    File.Delete(AuthPath);
                    Start();
                }
                else
                    MessageBox.Show(Kernel.GetText("error_internet"));
            }
        }
        public bool IsSpotifyReady()
        {
            return AccountReady;
        }
        private async Task StartLoginSpotify(Stopwatch crono)
        {
            Log.Instance.PrintMessage("Logging to Spotify", MessageType.Info);
            //Leer token existente
            var json = await File.ReadAllTextAsync(AuthPath);
            //Pasar a objeto token
            var token = JsonConvert.DeserializeObject<PKCETokenResponse>(json);
            //Crear autenticador
            var auth = new PKCEAuthenticator(PublicKey, token);
            //Definir el evento para refrescar el token automáticamente
            auth.TokenRefreshed += (sender, token) => File.WriteAllText(AuthPath, JsonConvert.SerializeObject(token));
            //Crear los objetos Spotify
            SpotifyConfig = SpotifyClientConfig.CreateDefault().WithAuthenticator(auth);
            SpotifyClient = new SpotifyClient(SpotifyConfig);
            AccountReady = true;
            User = SpotifyClient.UserProfile.Current().Result;
            Log.Instance.PrintMessage("Connected as " + User.Email, MessageType.Correct, crono, TimeType.Seconds);
            Config.LinkedWithSpotify = true;
            AccountLinked = true;
            if (!Kernel.MetadataStream)
                SignalLogin();
            crono.Stop();
        }
        private async void SignalLogin()
        {
            await Task.Run(() =>
            {
                Kernel.ActivarReproduccionSpotify();
                Kernel.InternetAvaliable(true);
                Kernel.MainForm.RemoveLink();
                Kernel.BringMainFormFront();

            });
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
                SimpleAlbum album = SearchAlbums(a, 1).First();
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

            bool res;
            try
            {
                FullAlbum album = SpotifyClient.Albums.Get(uri).Result;
                res = ProcessAlbum(album);

            }
            catch (APIException e)
            {
                Log.Instance.PrintMessage("Album was not inserted...", MessageType.Warning);
                Log.Instance.PrintMessage(e.Message, MessageType.Warning);
                return false;
            }
            Kernel.ReloadView();
            return res;
        }
        public bool ProcessAlbum(FullAlbum album, bool downloadCover = true)
        {
            String[] parseFecha = album.ReleaseDate.Split('-');
            string cover = album.Name + "_" + album.Artists[0].Name + ".jpg";
            //Remove Windows forbidden characters so we can save the album cover.
            foreach (char ch in ForbiddenChars)
            {
                if (cover.Contains(ch.ToString()))
                    cover = cover.Replace(ch.ToString(), string.Empty);
            }
            AlbumData a = new AlbumData(album.Name.Replace(";", ""), album.Artists[0].Name.Replace(";", ""), Convert.ToInt16(parseFecha[0]), ""); //creamos A
            //if (Kernel.Collection.IsInCollection(a))
            //{
            //    Log.Instance.PrintMessage("Adding duplicate album", MessageType.Warning);
            //    Log.Instance.PrintMessage(a.ToString(), MessageType.Info);
            //    return false;
            //}
            if (downloadCover)
            {
                DownloadCover(album, cover);
                a.CoverPath = Environment.CurrentDirectory + "/covers/" + cover;
            }
            else
                a.CoverPath = "";

            a.IdSpotify = album.Id;
            List<Song> songs = new List<Song>(a.NumberOfSongs);
            List<SimpleTrack> albumSongs = album.Tracks.Items;
            for (int i = 0; i < albumSongs.Count; i++)
            {
                songs.Add(new Song(albumSongs[i].Name.Replace(";", ""), new TimeSpan(0, 0, 0, 0, albumSongs[i].DurationMs), ref a));
                if (songs[i].Length.Milliseconds > 500)
                    songs[i].Length += new TimeSpan(0, 0, 0, 0, 1000 - songs[i].Length.Milliseconds);
                else
                    songs[i].Length -= new TimeSpan(0, 0, 0, 0, songs[i].Length.Milliseconds);
            }
            a.Songs = songs;
            a.CanBeRemoved = true;
            Kernel.SetSaveMark();
            return Kernel.Collection.AddAlbum(ref a);
        }
        private async void DownloadCover(FullAlbum album, string file_name)
        {
            using (HttpClient httpClient = new())
            {
                try
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "/covers");
                    var respuesta = await httpClient.GetAsync(new Uri(album.Images[0].Url));
                    respuesta.EnsureSuccessStatusCode();
                    await using var ms = await respuesta.Content.ReadAsStreamAsync();
                    await using var fs = File.Create(Environment.CurrentDirectory + "/covers/" + file_name);
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.CopyTo(fs);
                }
                catch (HttpRequestException e)
                {
                    Log.Instance.PrintMessage("Exception captured System.Net.HttpRequestException", MessageType.Warning);
                    MessageBox.Show(Kernel.GetText("errorPortada"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    file_name = "";
                }
                catch (Exception e)
                {
                    Log.Instance.PrintMessage("No album image!! Uri: " + album.Uri, MessageType.Warning);
                    file_name = "";
                }
            }
        }
        public bool ProcessAlbum(SimpleAlbum album)
        {
            FullAlbum fullAlbum = SpotifyClient.Albums.Get(album.Id).Result;
            return ProcessAlbum(fullAlbum);
        }
        public bool UserIsPremium()
        {
            try
            {
                return SpotifyClient.UserProfile.Current().Result.Product == "premium" ? true : false;
            }
            catch (Exception ex)
            {
                Log.Instance.PrintMessage("Couldn't retrieve user type", MessageType.Warning);
                Log.Instance.PrintMessage(ex.Message, MessageType.Warning);
                throw;
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
                temp.Add("spotify:track:" + uri);
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
                List<FullAlbum> albums = new();
                try
                {
                    //Get albums
                    var savedAlbums = await SpotifyClient.Library.GetAlbums();
                    int point = 0;
                    int limit = (int)savedAlbums.Total;
                    bool covers = true;
                    if (limit > 100)
                    {
                        DialogResult dr = Kernel.Warn(Kernel.GetText("importSpotifyWarning"));
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
                            try
                            {
                                if (a is not null)
                                    ProcessAlbum(a.Album, covers);
                                loadBar.Progreso();
                            }
                            catch (Exception)
                            {

                                Log.Instance.PrintMessage("GetUserAlbums() - Album could not be added", MessageType.Warning);
                                Log.Instance.PrintMessage(a.Album.Uri, MessageType.Warning);
                                continue;
                            }

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
                catch (Exception ex)
                {
                    Log.Instance.PrintMessage("GetUserAlbums() - Something happened", MessageType.Warning);
                    Log.Instance.PrintMessage(ex.Message, MessageType.Warning);
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
                throw ex;
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
        public void PlayResume(bool playing)
        {
            try
            {
                //Device = SpotifyClient.Player.GetAvailableDevices().Result;
                if (playing)
                    SpotifyClient.Player.PausePlayback();
                else
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
