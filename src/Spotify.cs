using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Net.Http;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;

namespace aplicacion_musica
{
    class Spotify
    {
        public SpotifyWebAPI _spotify;
        private AuthorizationCodeAuth auth;
        private CredentialsAuth authMetadatos;
        private readonly char[] CaracteresProhibidosWindows = { '\\', '/', '|', '?', '*', '"', ':', '>', '<' };
        private readonly String clavePublica = "f49317757dd64bb190576aec028f4efc";
        private readonly String clavePrivada = ClaveAPI.Spotify;
        public bool cuentaLista = false;
        public bool cuentaVinculada = false;
        Token tokenActual;
        public Spotify(bool v)
        {
            if(v == false)
                iniciar();
            else
                iniciarModoStream();
        }
        public async void SpotifyVinculado()
        {
            iniciarModoStream();
        }
        public bool TokenExpirado()
        {
            return tokenActual.IsExpired();
        }
        private async void iniciar()
        {
            Console.WriteLine("Intentando conectar a Spotify asíncronamente");
            Stopwatch crono = Stopwatch.StartNew();
            Programa.HayInternet(false);
            try
            {
                CredentialsAuth authMetadatos = new CredentialsAuth(clavePrivada, clavePrivada);
                Token token = await authMetadatos.GetToken();
                _spotify = new SpotifyWebAPI()
                {
                    TokenType = token.TokenType,
                    AccessToken = token.AccessToken
                };
                Programa.HayInternet(true);
            }
            catch (NullReferenceException)
            {
                Programa.HayInternet(false);
                Console.WriteLine("No tienes internet");
                System.Windows.Forms.MessageBox.Show(Programa.textosLocal.GetString("error_internet"));
            }
            catch (HttpRequestException)
            {
                Programa.HayInternet(false);
                Console.WriteLine("No tienes internet");
                System.Windows.Forms.MessageBox.Show(Programa.textosLocal.GetString("error_internet"));
            }
            crono.Stop();
            Console.WriteLine("Conectado sin errores en " + crono.ElapsedMilliseconds + "ms");
        }
        private async void iniciarModoStream()
        {
            //try
            {
                Console.WriteLine("Intentando conectar cuenta de Spotify");
                Programa.HayInternet(true);
                Stopwatch crono = Stopwatch.StartNew();
                auth = new AuthorizationCodeAuth(
                    clavePublica,
                    clavePrivada,
                    "http://localhost:4002/",
                    "http://localhost:4002/",
                    Scope.UserReadEmail | Scope.UserReadPrivate | Scope.Streaming | Scope.UserReadPlaybackState
                    );
                auth.AuthReceived += async (sender, payload) =>
                {
                    auth.Stop();
                    Token token = await auth.ExchangeCode(payload.Code);
                    tokenActual = token;
                    _spotify = new SpotifyWebAPI()
                    {
                        TokenType = token.TokenType,
                        AccessToken = token.AccessToken
                    };
                    crono.Stop();
                    cuentaLista = true;
                    cuentaVinculada = true;
                    Programa.config.AppSettings.Settings["VinculadoConSpotify"].Value = "true";
                    Console.WriteLine("Conectado sin errores en " + crono.ElapsedMilliseconds + "ms");
                };
                auth.Start();
                auth.OpenBrowser();
            }
            //catch (NullReferenceException)
            //{
            //    Programa.HayInternet(false);
            //    Console.WriteLine("No tienes internet");
            //    System.Windows.Forms.MessageBox.Show(Programa.textosLocal.GetString("error_internet"));
            //}
            //catch (HttpRequestException)
            //{
            //    Programa.HayInternet(false);
            //    Console.WriteLine("No tienes internet");
            //    System.Windows.Forms.MessageBox.Show(Programa.textosLocal.GetString("error_internet"));
            //}
        }
        public async void RefrescarToken()
        {
            Token newToken = await auth.RefreshToken(tokenActual.RefreshToken);
            _spotify.AccessToken = newToken.AccessToken;
            _spotify.TokenType = newToken.TokenType;
        }
        public void buscarAlbum(string a)
        {
            Console.WriteLine("Búsqueda en Spotify en Spotify::buscarAlbum(string a)");
            Stopwatch crono = Stopwatch.StartNew();
            List<SimpleAlbum> item = _spotify.SearchItems(a, SearchType.Album).Albums.Items;

            resultadoSpotify res = new resultadoSpotify(ref item);
            crono.Stop();
            Console.WriteLine("Búsqueda en Spotify en Spotify::buscarAlbum(string a) ha terminado en "+crono.ElapsedMilliseconds+"ms");
            res.ShowDialog();
            if (res.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                return;
        }
        public SimpleAlbum DevolverAlbum(string a)
        {
            Console.WriteLine("Búsqueda en Spotify en Spotify::buscarAlbum(string a)");
            Stopwatch crono = Stopwatch.StartNew();
            SimpleAlbum album = _spotify.SearchItems(a, SearchType.Album).Albums.Items[0];
            crono.Stop();
            Console.WriteLine("Búsqueda en Spotify en Spotify::buscarAlbum(string a) ha terminado en " + crono.ElapsedMilliseconds + "ms");
            return album;
        }
        public FullTrack cancion(string song)
        {
            var item = _spotify.SearchItems(song, SpotifyAPI.Web.Enums.SearchType.Track, 5, 0, "ES");
            FullTrack cancionQueBusco = item.Tracks.Items.First();
            return cancionQueBusco;
        }
        public void insertarAlbumFromURI(string uri)
        {
            Console.WriteLine("Insertando álbum con URI "+uri);
            Stopwatch crono = Stopwatch.StartNew();
            FullAlbum sa = _spotify.GetAlbum(uri);

            procesarAlbum(sa);
            crono.Stop();
            Console.WriteLine("Añadido en "+crono.ElapsedMilliseconds+"ms");
            Programa.refrescarVista();
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
                    Console.WriteLine("Excepción capturada System.Net.WebException");
                    System.Windows.Forms.MessageBox.Show("");
                    portada = "";
                }

            }
            Album a = new Album(album.Name, album.Artists[0].Name, Convert.ToInt16(parseFecha[0]), Convert.ToInt16(album.TotalTracks), Environment.CurrentDirectory + "/covers/" + portada); //creamos A
            List<Cancion> canciones = new List<Cancion>(a.numCanciones);
            List<SimpleTrack> c = _spotify.GetAlbumTracks(album.Id,a.numCanciones).Items;
            for (int i = 0; i < c.Count; i++)
            {
                canciones.Add(new Cancion(c[i].Name, new TimeSpan(0, 0, 0, 0, c[i].DurationMs), ref a));
                if(canciones[i].duracion.Milliseconds >500)
                    canciones[i].duracion += new TimeSpan(0, 0, 0, 0, 1000 - canciones[i].duracion.Milliseconds);
                else
                    canciones[i].duracion -= new TimeSpan(0, 0, 0, 0, canciones[i].duracion.Milliseconds);
                a.duracion += canciones[i].duracion;
            }
            a.canciones = canciones;
            Programa.miColeccion.agregarAlbum(ref a);
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
            Album a = new Album(album.Name, album.Artists[0].Name, Convert.ToInt16(parseFecha[0]), Convert.ToInt16(album.TotalTracks), Environment.CurrentDirectory + "/covers/" + portada); //creamos A
            List<Cancion> canciones = new List<Cancion>(a.numCanciones);
            List<SimpleTrack> c = _spotify.GetAlbumTracks(album.Id, a.numCanciones).Items;
            for (int i = 0; i < c.Count; i++)
            {
                canciones.Add(new Cancion(c[i].Name, new TimeSpan(0, 0, 0, 0, c[i].DurationMs), ref a));
                if (canciones[i].duracion.Milliseconds > 500)
                    canciones[i].duracion += new TimeSpan(0, 0, 0, 0, 1000 - canciones[i].duracion.Milliseconds);
                else
                    canciones[i].duracion -= new TimeSpan(0, 0, 0, 0, canciones[i].duracion.Milliseconds);
                a.duracion += canciones[i].duracion;
            }
            a.canciones = canciones;
            Programa.miColeccion.agregarAlbum(ref a);
        }
        public void Reiniciar()
        {
            authMetadatos = null;
        }
    }
}
