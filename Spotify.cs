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
        public FullAlbum buscarAlbum(string a)
        {
            var item = _spotify.SearchItems(a, SpotifyAPI.Web.Enums.SearchType.Album,5,0,"ES"); //busco 5 albumes
            List<SimpleAlbum> busqueda = item.Albums.Items;
            string info = "";
            for (int i = 0; i < busqueda.Count; i++)
            {
                info += i + 1 + ". " + busqueda[i].Artists.First().Name + " - " + busqueda[i].Name +" " + busqueda[i].ReleaseDate + Environment.NewLine;
            }
            System.Windows.Forms.MessageBox.Show(info);
            return null; //test
        }
        public FullTrack cancion(string song)
        {
            var item = _spotify.SearchItems(song, SpotifyAPI.Web.Enums.SearchType.Track, 5, 0, "ES");
            FullTrack cancionQueBusco = item.Tracks.Items.First();
            return cancionQueBusco;
        }
    }
}
