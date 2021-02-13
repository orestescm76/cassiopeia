using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
namespace aplicacion_musica
{

    public enum EstadoMedio
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

        [Newtonsoft.Json.JsonIgnore] //no quiero guardarlo 2 veces, ni cargarlo
        public Album Album { get; protected set; }
        public EstadoMedio EstadoExterior { get; set; }
        public short YearRelease {get;set;}
        public String PaisPublicacion { get; set; }
        public String Artista { get; set; }
        public String Nombre { get; set; }
        public AlbumFisico() 
        {
        }
        public AlbumFisico(String s, EstadoMedio ee, short y = 0, String p = null)
        {
            Album = Programa.miColeccion.devolverAlbum(s);
            EstadoExterior = ee;
            YearRelease = y;
            PaisPublicacion = p;
            Artista = Album.Artist;
            Nombre = Album.Title;
        }
        public AlbumFisico(String s)
        {
            Album = Programa.miColeccion.devolverAlbum(s);
        }
    }
}
