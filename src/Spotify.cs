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

namespace aplicacion_musica
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
            Log.Instance.ImprimirMensaje("Intentando conectar con Spotify asíncronamente", TipoMensaje.Info, "Spotify.iniciar()");
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
                    Log.Instance.ImprimirMensaje("Conectado sin errores", TipoMensaje.Correcto, crono);
                }
                else
                {
                    Program.HayInternet(false);
                    Log.Instance.ImprimirMensaje("Se ha conectado pero el token es nulo", TipoMensaje.Error, crono);
                }
            }
            catch (NullReferenceException)
            {
                Program.HayInternet(false);
                Log.Instance.ImprimirMensaje("No se ha podido conectar con Spotify", TipoMensaje.Error);
                System.Windows.Forms.MessageBox.Show(Program.LocalTexts.GetString("error_internet"));
            }
            catch (HttpRequestException)
            {
                Program.HayInternet(false);
                Log.Instance.ImprimirMensaje("No se ha podido conectar con Spotify", TipoMensaje.Error);
                System.Windows.Forms.MessageBox.Show(Program.LocalTexts.GetString("error_internet"));
            }
        }
        private void iniciarModoStream()
        {
            try
            {
                Log.Instance.ImprimirMensaje("Intentando conectar cuenta de Spotify", TipoMensaje.Info, "Spotify.iniciarModoStream()");
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
                        Log.Instance.ImprimirMensaje("Conectado sin errores como " + _spotify.GetPrivateProfile().DisplayName, TipoMensaje.Correcto, crono);
                    }
                    else
                    {
                        cuentaLista = false;
                        cuentaVinculada = false;
                        Log.Instance.ImprimirMensaje("Se ha conectado pero el token es nulo", TipoMensaje.Error, crono);
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
            Log.Instance.ImprimirMensaje("Refrescando Token...",TipoMensaje.Info);
            Token newToken = auth.RefreshToken(CodigoRefresco).Result;
            _spotify.AccessToken = newToken.AccessToken;
            _spotify.TokenType = newToken.TokenType;
            tokenActual = newToken;
            Log.Instance.ImprimirMensaje("Token refrescado!", TipoMensaje.Correcto);
        }
        public List<SimpleAlbum> SearchAlbums(string query)
        {
            Log.Instance.ImprimirMensaje("Búsqueda en Spotify", TipoMensaje.Info, "Spotify.buscarAlbum(string)");
            Stopwatch crono = Stopwatch.StartNew();
            try
            {
                List<SimpleAlbum> AlbumList = _spotify.SearchItems(query, SearchType.Album).Albums.Items;
                Log.Instance.ImprimirMensaje("Búsqueda en Spotify ha finalizado correctamente", TipoMensaje.Correcto, crono);
                return AlbumList;
            }
            catch (NullReferenceException e)
            {
                Log.Instance.ImprimirMensaje("Error buscando álbumes", TipoMensaje.Error);
                Log.Instance.ImprimirMensaje(e.InnerException.Message, TipoMensaje.Error);
                crono.Stop();
            }
            return null;
        }
        public SimpleAlbum DevolverAlbum(string a)
        {
            Log.Instance.ImprimirMensaje("Búsqueda en Spotify", TipoMensaje.Info, "Spotify.devolverAlbum(string)");
            Stopwatch crono = Stopwatch.StartNew();
            try
            {
                SimpleAlbum album = _spotify.SearchItems(a, SearchType.Album).Albums.Items[0];
                crono.Stop();
                Log.Instance.ImprimirMensaje("Búsqueda en Spotify ha finalizado correctamente", TipoMensaje.Correcto, crono);

                return album;
            }
            catch (ArgumentOutOfRangeException)
            {
                Log.Instance.ImprimirMensaje("Busqueda en Spotify no ha encontrado nada", TipoMensaje.Advertencia, crono);
                return null;
            }

        }
        public bool InsertarAlbumFromURI(string uri)
        {
            Log.Instance.ImprimirMensaje("Insertando álbum con URI "+uri, TipoMensaje.Info);
            Stopwatch crono = Stopwatch.StartNew();
            FullAlbum sa = _spotify.GetAlbum(uri);
            try
            {
                procesarAlbum(sa);

            }
            catch (Exception)
            {
                crono.Stop();
                Log.Instance.ImprimirMensaje("Repetido", TipoMensaje.Advertencia);
                return false;
            }
            crono.Stop();
            Log.Instance.ImprimirMensaje("Añadido",TipoMensaje.Correcto, crono);
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
                    Log.Instance.ImprimirMensaje("Excepción capturada System.Net.WebException", TipoMensaje.Advertencia);
                    System.Windows.Forms.MessageBox.Show(Program.LocalTexts.GetString("errorPortada"), "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    portada = "";
                }

            }
            AlbumData a = new AlbumData(album.Name, album.Artists[0].Name, Convert.ToInt16(parseFecha[0]), Environment.CurrentDirectory + "/covers/" + portada); //creamos A
            if (Program.Collection.IsInCollection(a))
            {
                Log.Instance.ImprimirMensaje("Intentando añadir duplicado, cancelando...", TipoMensaje.Advertencia);
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
                Log.Instance.ImprimirMensaje("Intentando añadir duplicado, cancelando...", TipoMensaje.Advertencia);
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
            Log.Instance.ImprimirMensaje("Reiniciando Spotify", TipoMensaje.Info);
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
            foreach(Song parte in cl.Partes)
            {
                uris.Add("spotify:track:"+DevolverCancionDelAlbum(uri, parte.Title));
            }
            return _spotify.ResumePlayback(uris: uris, offset: "", positionMs: 0);
        }
    }
}
