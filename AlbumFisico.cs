using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
namespace aplicacion_musica
{
    /// <summary>
    /// Estado del medio según el estándar Goldmine
    /// </summary>
    public enum EstadoMedio
    {
        ///<summary>Mint</summary>
        M,
        ///<summary>Mint- o NearMint</summary>
        NMint,
        ///<summary>Very Good +</summary>
        VGPlus,
        ///<summary>Very Good</summary>
        VG,
        ///<summary>Good+</summary>
        GPlus,
        ///<summary>Good</summary>
        G,
        ///<summary>Fair</summary>
        F,
        ///<summary>Poor</summary>
        P
    }

    public class AlbumFisico
    {
        /// <summary>
        /// Cual es el álbum que tengo
        /// </summary>
        [Newtonsoft.Json.JsonIgnore] //no quiero guardarlo 2 veces, ni cargarlo
        public Album Album { get; protected set; }
        /// <summary>
        /// Estado de la portada y demás complementos según el estándar goldmine
        /// </summary>
        public EstadoMedio EstadoExterior { get; set; }
        /// <summary>
        /// Año cuando salió la copia
        /// </summary>
        public short YearRelease {get;set;}
        /// <summary>
        /// País de publicación del medio, puede ser desconocido
        /// </summary>
        public String PaisPublicacion { get; set; }
        /// <summary>
        /// Número de canciones del disco
        /// </summary>
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
            Artista = Album.artista;
            Nombre = Album.nombre;
        }
        public AlbumFisico(String s)
        {
            Album = Programa.miColeccion.devolverAlbum(s);
        }
    }
}
