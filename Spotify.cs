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
        public Spotify()
        {
            _auth = new CredentialsAuth("f49317757dd64bb190576aec028f4efc", "6cd805517def403e96ff1866ae67143e");
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
        public FullTrack cancion(string song)
        {
            var item = _spotify.SearchItems(song, SpotifyAPI.Web.Enums.SearchType.Track, 5, 0, "ES");
            FullTrack cancionQueBusco = item.Tracks.Items.First();
            return cancionQueBusco;
        }
    }
}
