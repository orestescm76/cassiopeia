using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Token token = await _auth.GetToken();
            _spotify = new SpotifyWebAPI()
            {
                AccessToken = token.AccessToken,
                TokenType = token.TokenType
            };
        }
        public void buscarAlbum(string a)
        {
            var item = _spotify.SearchItems(a, SpotifyAPI.Web.Enums.SearchType.Album);
            resultadoSpotify res = new resultadoSpotify(item.Albums.Items);
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
                    System.Windows.Forms.MessageBox.Show("Error descargando la imagen");
                    portada = "";
                }

            }
            Album a = new Album(album.Name, album.Artists[0].Name, Convert.ToInt16(parseFecha[0]), Convert.ToInt16(album.TotalTracks), Environment.CurrentDirectory + "/covers/" + portada); //creamos A
            Cancion[] canciones = new Cancion[a.numCanciones];
            List<SimpleTrack> c = _spotify.GetAlbumTracks(album.Id).Items;
            for (int i = 0; i < c.Count; i++)
            {
                canciones[i] = new Cancion(c[i].Name, new TimeSpan(0, 0, 0, 0, c[i].DurationMs), ref a);
                if(canciones[i].duracion.Milliseconds >500)
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
