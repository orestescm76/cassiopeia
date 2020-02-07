using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
namespace aplicacion_musica
{
    class Disco : Album
    {
        /// <summary>
        /// Estado del medio según el estado Goldmine
        /// </summary>
        public enum Estado
        {
            ///<summary>Mint</summary>
            M,
            ///<summary>Near Mint</summary>
            NMinus,
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
        public short YearRelease {get;set;}
        /// <summary>
        /// País de publicación del medio, puede ser desconocido
        /// </summary>
        public RegionInfo PaisPublicacion { get; set; }
    }
}
