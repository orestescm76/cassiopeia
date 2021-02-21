using System;

namespace aplicacion_musica
{
    public enum MediaCondition
    {
        M,
        NMint,
        VGPlus,
        VG,
        GPlus,
        G,
        F,
        P
    }

    public class AlbumFisico
    {
        //no quiero guardarlo 2 veces, ni cargarlo 
        [Newtonsoft.Json.JsonIgnore] public AlbumData Album { get; protected set; }
        public MediaCondition EstadoExterior { get; set; }
        public short Year {get;set;}
        public string Country { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }

        public AlbumFisico()
        {

        }

        public AlbumFisico(string s, MediaCondition estado, short y = 0, String p = null)
        {
            Album = Program.Collection.GetAlbum(s);
            EstadoExterior = estado;
            Year = y;
            Country = p;
            Artist = Album.Artist;
            Title = Album.Title;
        }

        public AlbumFisico(string s)
        {
            Album = Program.Collection.GetAlbum(s);
        }
    }
}
