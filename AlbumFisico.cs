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
    /// <summary>
    /// Año cuando salió la copia
    /// </summary>
    public class AlbumFisico
    {
        public Album Album { get; protected set; }
        /// <summary>
        /// Estado de la portada y demás complementos según el estándar goldmine
        /// </summary>
        public EstadoMedio EstadoExterior { get; set; }
        public short YearRelease {get;set;}
        /// <summary>
        /// País de publicación del medio, puede ser desconocido
        /// </summary>
        public RegionInfo PaisPublicacion { get; set; }
		/// <summary>
        /// Número de canciones del disco
        /// </summary>
		///
        public AlbumFisico(ref Album a, EstadoMedio ee, short y = 0, RegionInfo r = null)
        {
            Album = a;
            EstadoExterior = ee;
            YearRelease = y;
            PaisPublicacion = r;
        }
        public AlbumFisico(ref Album a)
        {
            Album = a;
        }
    }
}
