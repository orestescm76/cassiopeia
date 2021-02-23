using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Net.Http;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;
using System.Threading;

namespace Cassiopeia
{
    class Spotify
    {
        public SpotifyWebAPI _spotify;
        private AuthorizationCodeAuth auth;
        private readonly char[] CaracteresProhibidosWindows = { '\\', '/', '|', '?', '*', '"', ':', '>', '<' };
        private readonly String clavePublica = "f49317757dd64bb190576aec028f4efc";
        private readonly String clavePrivada = ClaveAPI.Spotify;
        public bool cuentaLista = false;
        public bool cuentaVinculada = false;
        private string CodigoRefresco;
        Token tokenActual;

        public Spotify(bool v)
        {
            if(!v)
                iniciar();
            else
                iniciarModoStream();
        }
        public void SpotifyVinculado()
        {
            iniciarModoStream();
        }
        public bool TokenExpirado()
        {
            return tokenActual.IsExpired();
        }
        private async void iniciar()
        {
            Log.Instance.PrintMessage("Intentando conectar con Spotify asíncronamente", MessageType.Info, "Spotify.iniciar()");
            Stopwatch crono = Stopwatch.StartNew();
            Program.HayInternet(false);
            try
            {
                CredentialsAuth authMetadatos = new CredentialsAuth(clavePublica, clavePrivada);
                Token token = await authMetadatos.GetToken();
                _spotify = new SpotifyWebAPI()
                {
                    TokenType = token.TokenType,
                    AccessToken = token.AccessToken
                };
                crono.Stop();
                if(_spotify.AccessToken != null)
                {
                    Program.HayInternet(true);
                    Log.Instance.PrintMessage("Conectado sin errores", MessageType.Correct, crono, TimeType.Miliseconds);
                }
                else
                {
                    Program.HayInternet(false);
                    Log.Instance.PrintMessage("Se ha conectado pero el token es nulo", MessageType.Error, crono, TimeType.Miliseconds);
                }
            }
            catch (NullReferenceException)
            {
                Program.HayInternet(false);
                Log.Instance.PrintMessage("No se ha podido conectar con Spotify", MessageType.Error);
                System.Windows.Forms.MessageBox.Show(Program.LocalTexts.GetString("error_internet"));
            }
            catch (HttpRequestException)
            {
                Program.HayInternet(false);
                Log.Instance.PrintMessage("No se ha podido conectar con Spotify", MessageType.Error);
                System.Windows.Forms.MessageBox.Show(Program.LocalTexts.GetString("error_internet"));
            }
        }
        private void iniciarModoStream()
        {
            try
            {
                Log.Instance.PrintMessage("Intentando conectar cuenta de Spotify", MessageType.Info, "Spotify.iniciarModoStream()");
                Program.HayInternet(true);
                Stopwatch crono = Stopwatch.StartNew();
                auth = new AuthorizationCodeAuth(
                    clavePublica,
                    clavePrivada,
                    "http://localhost:4002/",
                    "http://localhost:4002/",
                    Scope.UserReadEmail | Scope.UserReadPrivate | Scope.Streaming | Scope.UserReadPlaybackState
                    );
                auth.AuthReceived += (sender, payload) =>
                {
                    auth.Stop();
                    Token token = auth.ExchangeCode(payload.Code).Result;
                    tokenActual = token;
                    _spotify = new SpotifyWebAPI()
                    {
                        TokenType = token.TokenType,
                        AccessToken = token.AccessToken
                    };
                    crono.Stop();
                    if(_spotify.AccessToken != null)
                    {
                        cuentaLista = true;
                        cuentaVinculada = true;
                        Config.VinculadoConSpotify = true;
                        Program.ActivarReproduccionSpotify();
                        Log.Instance.PrintMessage("Conectado sin errores como " + _spotify.GetPrivateProfile().DisplayName, MessageType.Correct, crono, TimeType.Miliseconds);
                    }
                    else
                    {
                        cuentaLista = false;
                        cuentaVinculada = false;
                        Log.Instance.PrintMessage("Se ha conectado pero el token es nulo", MessageType.Error, crono, TimeType.Miliseconds);
                        Config.VinculadoConSpotify = false;
                    }
                    CodigoRefresco = token.RefreshToken;
                    Program.tareaRefrescoToken = new Thread(Program.RefreshSpotifyToken);
                    Program.tareaRefrescoToken.Start();
                };
                auth.Start();
                auth.OpenBrowser();
            }
            catch (NullReferenceException)
            {
                Program.HayInternet(false);
                Console.WriteLine("Algo fue mal");
                System.Windows.Forms.MessageBox.Show(Program.LocalTexts.GetString("error_internet"));
            }
            catch (HttpRequestException)
            {
                Program.HayInternet(false);
                Console.WriteLine("No tienes internet");
                System.Windows.Forms.MessageBox.Show(Program.LocalTexts.GetString("error_internet"));
            }
        }
        public void RefrescarToken()
        {
            Log.Instance.PrintMessage("Refrescando Token...",MessageType.Info);
            Token newToken = auth.RefreshToken(CodigoRefresco).Result;
            _spotify.AccessToken = newToken.AccessToken;
            _spotify.TokenType = newToken.TokenType;
            tokenActual = newToken;
            Log.Instance.PrintMessage("Token refrescado!", MessageType.Correct);
        }
        public List<SimpleAlbum> SearchAlbums(string query)
        {
            Log.Instance.PrintMessage("Búsqueda en Spotify", MessageType.Info, "Spotify.buscarAlbum(string)");
            Stopwatch crono = Stopwatch.StartNew();
            try
            {
                List<SimpleAlbum> AlbumList = _spotify.SearchItems(query, SearchType.Album).Albums.Items;
                Log.Instance.PrintMessage("Búsqueda en Spotify ha finalizado correctamente", MessageType.Correct, crono, TimeType.Miliseconds);
                return AlbumList;
            }
            catch (NullReferenceException e)
            {
                Log.Instance.PrintMessage("Error buscando álbumes", MessageType.Error);
                Log.Instance.PrintMessage(e.InnerException.Message, MessageType.Error);
                crono.Stop();
            }
            return null;
        }
        public SimpleAlbum DevolverAlbum(string a)
        {
            Log.Instance.PrintMessage("Búsqueda en Spotify", MessageType.Info, "Spotify.devolverAlbum(string)");
            Stopwatch crono = Stopwatch.StartNew();
            try
            {
                SimpleAlbum album = _spotify.SearchItems(a, SearchType.Album).Albums.Items[0];
                crono.Stop();
                Log.Instance.PrintMessage("Búsqueda en Spotify ha finalizado correctamente", MessageType.Correct, crono, TimeType.Miliseconds);

                return album;
            }
            catch (ArgumentOutOfRangeException)
            {
                Log.Instance.PrintMessage("Busqueda en Spotify no ha encontrado nada", MessageType.Warning, crono, TimeType.Miliseconds);
                return null;
            }

        }
        public bool InsertarAlbumFromURI(string uri)
        {
            Log.Instance.PrintMessage("Insertando álbum con URI "+uri, MessageType.Info);
            Stopwatch crono = Stopwatch.StartNew();
            FullAlbum sa = _spotify.GetAlbum(uri);
            try
            {
                procesarAlbum(sa);

            }
            catch (Exception)
            {
                crono.Stop();
                Log.Instance.PrintMessage("Repetido", MessageType.Warning);
                return false;
            }
            crono.Stop();
            Log.Instance.PrintMessage("Añadido",MessageType.Correct, crono, TimeType.Miliseconds);
            Program.ReloadView();
            return true;
        }
        public void procesarAlbum(SimpleAlbum album)
        {
            String[] parseFecha = album.ReleaseDate.Split('-');
            string portada = album.Name + "_" + album.Artists[0].Name + ".jpg";
            foreach (char ch in CaracteresProhibidosWindows)
            {
                if (portada.Contains(ch.ToString()))
                    portada = portada.Replace(ch.ToString(), string.Empty);
            }
            using (System.Net.WebClient cliente = new System.Net.WebClient())
            {
                try
                {
                    System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "/covers");
                    cliente.DownloadFile(new Uri(album.Images[0].Url), Environment.CurrentDirectory + "/covers/" + portada);
                }
                catch (System.Net.WebException)
                {
                    Log.Instance.PrintMessage("Excepción capturada System.Net.WebException", MessageType.Warning);
                    System.Windows.Forms.MessageBox.Show(Program.LocalTexts.GetString("errorPortada"), "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    portada = "";
                }

            }
            AlbumData a = new AlbumData(album.Name, album.Artists[0].Name, Convert.ToInt16(parseFecha[0]), Environment.CurrentDirectory + "/covers/" + portada); //creamos A
            if (Program.Collection.IsInCollection(a))
            {
                Log.Instance.PrintMessage("Intentando añadir duplicado, cancelando...", MessageType.Warning);
                return;
            }
            a.IdSpotify = album.Id;
            List<Song> canciones = new List<Song>(a.NumberOfSongs);
            List<SimpleTrack> c = _spotify.GetAlbumTracks(album.Id, 50).Items;
            for (int i = 0; i < c.Count; i++)
            {
                canciones.Add(new Song(c[i].Name, new TimeSpan(0, 0, 0, 0, c[i].DurationMs), ref a));
                if(canciones[i].Length.Milliseconds > 500)
                    canciones[i].Length += new TimeSpan(0, 0, 0, 0, 1000 - canciones[i].Length.Milliseconds);
                else
                    canciones[i].Length -= new TimeSpan(0, 0, 0, 0, canciones[i].Length.Milliseconds);
            }
            a.Songs = canciones;
            Program.Collection.AddAlbum(ref a);
        }
        public void procesarAlbum(FullAlbum album)
        {
            String[] parseFecha = album.ReleaseDate.Split('-');
            string portada = album.Name + "_" + album.Artists[0].Name + ".jpg";
            foreach (char ch in CaracteresProhibidosWindows)
            {
                if (portada.Contains(ch.ToString()))
                    portada = portada.Replace(ch.ToString(), string.Empty);
            }
            using (System.Net.WebClient cliente = new System.Net.WebClient())
            {
                try
                {
                    System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "/covers");
                    cliente.DownloadFile(new Uri(album.Images[0].Url), Environment.CurrentDirectory + "/covers/" + portada);
                }
                catch (System.Net.WebException)
                {
                    System.Windows.Forms.MessageBox.Show("");
                    portada = "";
                }

            }
            AlbumData a = new AlbumData(album.Name, album.Artists[0].Name, Convert.ToInt16(parseFecha[0]), Environment.CurrentDirectory + "/covers/" + portada); //creamos A
            if (Program.Collection.IsInCollection(a))
            {
                Log.Instance.PrintMessage("Intentando añadir duplicado, cancelando...", MessageType.Warning);
                throw new InvalidOperationException();
            }
            a.IdSpotify = album.Id;
            List<Song> canciones = new List<Song>(a.NumberOfSongs);
            List<SimpleTrack> c = _spotify.GetAlbumTracks(album.Id).Items;
            for (int i = 0; i < c.Count; i++)
            {
                canciones.Add(new Song(c[i].Name, new TimeSpan(0, 0, 0, 0, c[i].DurationMs), ref a));
                if (canciones[i].Length.Milliseconds > 500)
                    canciones[i].Length += new TimeSpan(0, 0, 0, 0, 1000 - canciones[i].Length.Milliseconds);
                else
                    canciones[i].Length -= new TimeSpan(0, 0, 0, 0, canciones[i].Length.Milliseconds);
            }
            a.Songs = canciones;
            a.CanBeRemoved = true;
            Program.Collection.AddAlbum(ref a);
        }

        public void Reiniciar()
        {
            Log.Instance.PrintMessage("Reiniciando Spotify", MessageType.Info);
        }

        public ErrorResponse ReproducirAlbum(string uri)
        {
            return _spotify.ResumePlayback(contextUri: "spotify:album:" + uri, offset: "", positionMs: 0);
        }

        public ErrorResponse ReproducirCancion(string uri, int cual) //reproduce una cancion de un album
        {
            FullAlbum album = _spotify.GetAlbum(uri);
            string uricancion = "";
            if (cual != 0)
            {
                for (int i = 0; i <= cual; i++)
                    uricancion = album.Tracks.Items[i].Id;
            }
            else
                uricancion = album.Tracks.Items.First().Id;
;            string temp = uricancion;
            uricancion = "";
            uricancion += "spotify:track:" + temp;
            List<string> uris = new List<string>();
            uris.Add(uricancion);
            return _spotify.ResumePlayback(uris: uris, offset: "", positionMs: 0);
        }
        public string DevolverCancionDelAlbum(string uri, string cancion)
        {
            FullAlbum album = _spotify.GetAlbum(uri);
            foreach (SimpleTrack track in album.Tracks.Items)
            {
                if (track.Name == cancion)
                    return track.Id;
            }
            return string.Empty;
        }
        public ErrorResponse ReproducirCancion(string uri, LongSong cl)
        {
            FullAlbum album = _spotify.GetAlbum(uri);
            List<string> uris = new List<string>();
            foreach(Song parte in cl.Parts)
            {
                uris.Add("spotify:track:"+DevolverCancionDelAlbum(uri, parte.Title));
            }
            return _spotify.ResumePlayback(uris: uris, offset: "", positionMs: 0);
        }
    }
}
