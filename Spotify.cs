using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Net.Http;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;

namespace aplicacion_musica
{
    class Spotify
    {
        public SpotifyWebAPI _spotify;
        private CredentialsAuth _auth;
        private readonly char[] CaracteresProhibidosWindows = { '\\', '/', '|', '?', '*', '"', ':', '>', '<' };
        private readonly String clavePublica = "f49317757dd64bb190576aec028f4efc";
        private readonly String clavePrivada = ClaveAPI.Spotify;
        public Spotify()
        {
            _auth = new CredentialsAuth(clavePublica, clavePrivada);
            iniciar();
        }
        private async void iniciar()
        {
            try
            {
                Console.WriteLine("Intentando conectar a Spotify asíncronamente");
                Programa.HayInternet(false);
                Stopwatch crono = Stopwatch.StartNew();
                Token token = await _auth.GetToken();
                _spotify = new SpotifyWebAPI()
                {
                    AccessToken = token.AccessToken,
                    TokenType = token.TokenType
                };
                Programa.HayInternet(true);
                crono.Stop();
                Console.WriteLine("Conectado sin errores en "+crono.ElapsedMilliseconds+"ms");
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

        }

        public void buscarAlbum(string a)
        {
            Console.WriteLine("Búsqueda en Spotify en Spotify::buscarAlbum(string a)");
            Stopwatch crono = Stopwatch.StartNew();
            List<SimpleAlbum> item = _spotify.SearchItems(a, SpotifyAPI.Web.Enums.SearchType.Album).Albums.Items;

            resultadoSpotify res = new resultadoSpotify(ref item);
            crono.Stop();
            Console.WriteLine("Búsqueda en Spotify en Spotify::buscarAlbum(string a) ha terminado en "+crono.ElapsedMilliseconds+"ms");
            res.ShowDialog();
            if (res.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                return;
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
    }
}
